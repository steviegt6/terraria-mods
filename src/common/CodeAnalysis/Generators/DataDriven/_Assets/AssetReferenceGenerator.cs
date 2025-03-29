using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using Newtonsoft.Json;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

[Generator]
public sealed class AssetReferenceGenerator : IIncrementalGenerator
{
    private static readonly IAssetReference[] default_references =
    [
        new Texture2DAssetReference(".png"),
        new Texture2DAssetReference(".rawimg"),
    ];

    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        var fileNames = context.AdditionalTextsProvider.Select((a, _) => a.Path);

        var effectFiles = context.AdditionalTextsProvider.Where(x => x.Path.EndsWith(".effect.json")).Select((a, c) => (a.Path, a.GetText(c)!.ToString()));

        context.RegisterSourceOutput(
            context.CompilationProvider
                   .Combine(fileNames.Collect())
                   .Combine(context.AnalyzerConfigOptionsProvider)
                   .Combine(effectFiles.Collect()),
            static (x, ctx) =>
            {
                x.AddSource("AssetReferences.g.cs", Generate(x, ctx.Left.Left.Left, ctx.Left.Left.Right, ctx.Left.Right, ctx.Right));
                x.AddSource("AssetCommon.g.cs",     GenerateCommon(x, ctx.Left.Right));
            }
        );
    }

    private static string GenerateCommon(
        SourceProductionContext       ctx,
        AnalyzerConfigOptionsProvider options
    )
    {
        if (GeneratorUtil.GetRootNamespaceOrRaiseDiagnostic(ctx, options.GlobalOptions) is not { } rootNamespace)
        {
            return "#error Failed to find root namespace";
        }

        return $@"
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;

namespace {rootNamespace}.Core;

internal interface IShaderParameters
{{
    void Apply(EffectParameterCollection parameters);
}}

internal sealed class WrapperShaderData<TParameters> : ShaderData
    where TParameters : IShaderParameters, new()
{{
    public TParameters Parameters {{ get; }} = new();

    // public WrapperShaderData(Ref<Effect> shader, string passName) : base(shader, passName) {{ }}

    public WrapperShaderData(Asset<Effect> shader, string passName) : base(shader, passName) {{ }}

    public override void Apply()
    {{
        Parameters.Apply(Shader.Parameters);

        base.Apply();
    }}
}}
".Trim();
    }

    private static string Generate(
        SourceProductionContext                        ctx,
        Compilation                                    compilation,
        ImmutableArray<string>                         fileNames,
        AnalyzerConfigOptionsProvider                  options,
        ImmutableArray<(string path, string contents)> effectFiles
    )
    {
        if (GeneratorUtil.GetRootNamespaceOrRaiseDiagnostic(ctx, options.GlobalOptions) is not { } rootNamespace)
        {
            return "#error Failed to find root namespace";
        }

        var refs  = default_references.ToDictionary(x => x.Extension, x => x);
        var files = fileNames.Where(x => refs.ContainsKey(Path.GetExtension(x))).Concat(effectFiles.Select(x => x.path)).ToArray();

        var assemblyName = GeneratorUtil.GetAssemblyName(compilation);

        var theEffectFiles = effectFiles.ToDictionary(x => x.path[x.path.IndexOf(assemblyName, StringComparison.InvariantCulture)..], x => x.contents);

        return GenerateAssetReferences(
            refs,
            files,
            rootNamespace,
            assemblyName,
            theEffectFiles
        );
    }

    private static string GenerateAssetReferences(
        Dictionary<string, IAssetReference> referencesByExtension,
        string[]                            filePaths,
        string                              rootNamespace,
        string                              assemblyName,
        Dictionary<string, string>          effectFiles
    )
    {
        var sb = new StringBuilder();

        var root = CreateAssetTree(filePaths, referencesByExtension, assemblyName);

        sb.AppendLine("#pragma warning disable CS8981");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using ReLogic.Content;");
        sb.AppendLine("using Terraria.ModLoader;");
        sb.AppendLine();
        sb.AppendLine($"namespace {rootNamespace}.Core;");
        sb.AppendLine();
        sb.AppendLine("internal static partial class AssetReferences");
        sb.AppendLine("{");
        {
            // TODO: My code is terrible, should only ever be one node in the root.
            foreach (var node in root.Nodes.Values)
            {
                sb.Append(GenerateTextFromPathNode(node, effectFiles));
            }
        }
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateTextFromPathNode(PathNode pathNode, Dictionary<string, string> effectFiles, int depth = 0)
    {
        var sb = new StringBuilder();

        var indent = new string(' ', depth * 4);

        if (depth != 0)
        {
            sb.AppendLine($"{indent}public static partial class {pathNode.Name}");
            sb.AppendLine($"{indent}{{");
        }

        foreach (var file in pathNode.Files)
        {
            sb.AppendLine($"{indent}    public static partial class {file.Name}");
            sb.AppendLine($"{indent}    {{");

            if (file.Reference is not EffectAssetReference)
            {
                sb.AppendLine($"{indent}        public const string KEY = \"{Path.ChangeExtension(file.Path.Replace('\\', '/'), null)}\";");
                sb.AppendLine($"{indent}        private static readonly Lazy<Asset<{file.Reference.QualifiedType}>> lazy = new(() => ModContent.Request<{file.Reference.QualifiedType}>(KEY));");
                sb.AppendLine($"{indent}        public static Asset<{file.Reference.QualifiedType}> Asset => lazy.Value;");
            }
            else
            {
                var actualName = Path.ChangeExtension(file.Path.Replace('\\', '/'), null);
                if (actualName.EndsWith(".effect"))
                {
                    actualName = actualName[..^".effect".Length];
                }

                if (!effectFiles.TryGetValue(file.Path, out var effectContents))
                {
                    sb.AppendLine($"#error Could not find file: {file.Path}");
                }
                else
                {
                    sb.AppendLine($"{indent}        public const string KEY = \"{actualName}\";");

                    var effectData = JsonConvert.DeserializeObject<EffectFile>(effectContents);
                    if (effectData is null)
                    {
                        sb.AppendLine($"#error Failed to parse effect file: {file.Path}");
                    }
                    else
                    {
                        const string type = "global::Microsoft.Xna.Framework.Graphics.Effect";

                        sb.AppendLine($"{indent}        public sealed class Parameters : IShaderParameters");
                        sb.AppendLine($"{indent}        {{");

                        sb.AppendLine();
                        foreach (var sampler in effectData.Samplers)
                        {
                            // hardcoded to just support textures for now :/
                            sb.AppendLine($"{indent}            public global::Microsoft.Xna.Framework.Graphics.Texture2D {sampler.Key} {{ get; set; }}");
                        }
                        sb.AppendLine();

                        sb.AppendLine();
                        foreach (var uniform in effectData.Uniforms)
                        {
                            var uniformType = GetUniformType(uniform.Value);

                            sb.AppendLine($"{indent}            public {uniformType} {uniform.Key} {{ get; set; }}");
                        }
                        sb.AppendLine();

                        sb.AppendLine($"{indent}            public void Apply(global::Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)");
                        sb.AppendLine($"{indent}            {{");
                        foreach (var name in effectData.Samplers.Select(x => x.Key).Concat(effectData.Uniforms.Select(x => x.Key)))
                        {
                            sb.AppendLine($"{indent}                parameters[\"{name}\"]?.SetValue({name});");
                        }
                        
                        // special case for uTime
                        sb.AppendLine($"{indent}                parameters[\"uTime\"]?.SetValue(global::Terraria.Main.GlobalTimeWrappedHourly);");
                        sb.AppendLine($"{indent}            }}");

                        sb.AppendLine($"{indent}        }}");
                        sb.AppendLine();
                        sb.AppendLine($"{indent}        private static readonly Lazy<Asset<{type}>> lazy = new(() => ModContent.Request<{type}>(KEY));");
                        sb.AppendLine($"{indent}        public static Asset<{type}> Asset => lazy.Value;");
                        sb.AppendLine();

                        foreach (var passName in effectData.Passes.Keys)
                        {
                            sb.AppendLine($"{indent}        public static WrapperShaderData<Parameters> Create{passName}()");
                            sb.AppendLine($"{indent}        {{");
                            sb.AppendLine($"{indent}            return new WrapperShaderData<Parameters>(Asset, \"{passName}\");");
                            sb.AppendLine($"{indent}        }}");
                        }
                    }
                }
            }

            sb.AppendLine($"{indent}    }}");

            sb.AppendLine();
        }

        foreach (var node in pathNode.Nodes.Values)
        {
            sb.AppendLine(GenerateTextFromPathNode(node, effectFiles, depth + 1));
        }

        if (depth != 0)
        {
            sb.AppendLine($"{indent}}}");
        }

        return sb.ToString().TrimEnd();
    }

    private static PathNode CreateAssetTree(IEnumerable<string> filePaths, Dictionary<string, IAssetReference> referencesByExtension, string assemblyName)
    {
        // Root node representing the root of all assets.
        var rootNode = new PathNode("Root", [], []);

        foreach (var filePath in filePaths)
        {
            if (filePath.EndsWith(".effect.json", StringComparison.InvariantCultureIgnoreCase))
            {
                var path = filePath[filePath.IndexOf(assemblyName, StringComparison.InvariantCulture)..];

                var pathNodes = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                var fileName = Path.GetFileName(path[..^".effect.json".Length]);

                var currentNode = rootNode;
                for (var i = 0; i < pathNodes.Length - 1; i++) // -1 to exclude the file name
                {
                    var directoryName = pathNodes[i];

                    if (!currentNode.Nodes.ContainsKey(directoryName))
                    {
                        currentNode.Nodes[directoryName] = new PathNode(directoryName, [], []);
                    }

                    currentNode = currentNode.Nodes[directoryName];
                }

                var assetFile = new AssetFile(fileName, path, new EffectAssetReference());

                currentNode.Files.Add(assetFile);
            }
            else
            {
                var extension = Path.GetExtension(filePath).ToLower();
                if (!referencesByExtension.ContainsKey(extension))
                {
                    continue;
                }

                var path = filePath[filePath.IndexOf(assemblyName, StringComparison.InvariantCulture)..];

                var pathNodes = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                var fileName = Path.GetFileNameWithoutExtension(path);

                var currentNode = rootNode;
                for (var i = 0; i < pathNodes.Length - 1; i++) // -1 to exclude the file name
                {
                    var directoryName = pathNodes[i];

                    if (!currentNode.Nodes.ContainsKey(directoryName))
                    {
                        currentNode.Nodes[directoryName] = new PathNode(directoryName, [], []);
                    }

                    currentNode = currentNode.Nodes[directoryName];
                }

                var assetReference = referencesByExtension[extension];
                var assetFile      = new AssetFile(fileName, path, assetReference);

                currentNode.Files.Add(assetFile);
            }
        }

        return rootNode;
    }

    /*private static void PrintNodes(PathNode node, StringBuilder sb, int depth = 0)
    {
        var indent = new string(' ', depth * 4);
        sb.AppendLine($"{indent}- {node.Name} (Files: {node.Files.Count})");

        foreach (var file in node.Files)
        {
            sb.AppendLine($"{indent}  * {file.Name} ({file.Path})");
        }

        foreach (var child in node.Nodes.Values)
        {
            PrintNodes(child, sb, depth + 1);
        }
    }*/

    private static string GetUniformType(string uniformType)
    {
        return uniformType switch
        {
            "float4" => "global::Microsoft.Xna.Framework.Vector4",
            "float3" => "global::Microsoft.Xna.Framework.Vector3",
            "float2" => "global::Microsoft.Xna.Framework.Vector2",
            "float"  => "float",
            _        => throw new InvalidOperationException(),
        };
    }
}