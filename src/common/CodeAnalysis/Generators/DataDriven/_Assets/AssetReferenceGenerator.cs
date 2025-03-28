using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

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

        context.RegisterSourceOutput(
            context.CompilationProvider
                   .Combine(fileNames.Collect())
                   .Combine(context.AnalyzerConfigOptionsProvider),
            static (x, ctx) =>
            {
                x.AddSource("AssetReferences.g.cs", Generate(x, ctx.Left.Left, ctx.Left.Right, ctx.Right));
            }
        );
    }

    private static string Generate(SourceProductionContext ctx, Compilation compilation, ImmutableArray<string> fileNames, AnalyzerConfigOptionsProvider options)
    {
        if (GeneratorUtil.GetRootNamespaceOrRaiseDiagnostic(ctx, options.GlobalOptions) is not { } rootNamespace)
        {
            return "#error Failed to find root namespace";
        }

        var refs  = default_references.ToDictionary(x => x.Extension, x => x);
        var files = fileNames.Where(x => refs.ContainsKey(Path.GetExtension(x))).ToArray();

        return GenerateAssetReferences(refs, files, rootNamespace, GeneratorUtil.GetAssemblyName(compilation));
    }

    private static string GenerateAssetReferences(
        Dictionary<string, IAssetReference> referencesByExtension,
        string[]                            filePaths,
        string                              rootNamespace,
        string                              assemblyName
    )
    {
        var sb = new StringBuilder();

        var root = CreateAssetTree(filePaths, referencesByExtension, assemblyName);

        sb.AppendLine("using System;");
        sb.AppendLine("using ReLogic.Content;");
        sb.AppendLine("using Terraria.ModLoader;");
        sb.AppendLine();
        sb.AppendLine($"namespace {rootNamespace}.Core;");
        sb.AppendLine();
        sb.AppendLine("internal static class AssetReferences");
        sb.AppendLine("{");
        {
            // TODO: My code is terrible, should only ever be one node in the root.
            foreach (var node in root.Nodes.Values)
            {
                sb.Append(GenerateTextFromPathNode(node));
            }
        }
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateTextFromPathNode(PathNode pathNode, int depth = 0)
    {
        var sb = new StringBuilder();

        var indent = new string(' ', depth * 4);

        if (depth != 0)
        {
            sb.AppendLine($"{indent}public static class {pathNode.Name}");
            sb.AppendLine($"{indent}{{");
        }

        foreach (var file in pathNode.Files)
        {
            sb.AppendLine($"{indent}    public static class {file.Name}");
            sb.AppendLine($"{indent}    {{");
            sb.AppendLine($"{indent}        public const string NAME = \"{file.Path.Replace('\\', '/')}\";");
            sb.AppendLine($"{indent}        private static readonly Lazy<Asset<{file.Reference.QualifiedType}>> lazy = new(() => ModContent.Request<{file.Reference.QualifiedType}>(NAME));");
            sb.AppendLine($"{indent}        public static Asset<{file.Reference.QualifiedType}> Asset => lazy.Value;");
            sb.AppendLine($"{indent}    }}");

            sb.AppendLine();
        }

        foreach (var node in pathNode.Nodes.Values)
        {
            sb.AppendLine(GenerateTextFromPathNode(node, depth + 1));
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
}