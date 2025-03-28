using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

using Hjson;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using Newtonsoft.Json.Linq;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

[Generator]
public sealed class LocalizationReferenceGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context.AdditionalTextsProvider.Where(x => x.Path.EndsWith(".hjson"));

        context.RegisterSourceOutput(
            context.CompilationProvider
                   .Combine(files.Collect())
                   .Combine(context.AnalyzerConfigOptionsProvider),
            static (x, ctx) =>
            {
                x.AddSource("LocalizationReferences.g.cs", Generate(x, ctx.Left.Right, ctx.Right));
                x.AddSource("LocalizationCommon.g.cs",     GenerateCommonLocalization(x, ctx.Right));
            }
        );
    }

    private static string Generate(
        SourceProductionContext        ctx,
        ImmutableArray<AdditionalText> files,
        AnalyzerConfigOptionsProvider  options
    )
    {
        if (GeneratorUtil.GetRootNamespaceOrRaiseDiagnostic(ctx, options.GlobalOptions) is not { } rootNamespace)
        {
            return "#error Failed to find root namespace";
        }

        return GenerateLocalization(files.ToList(), rootNamespace);
    }

    private static string GenerateLocalization(List<AdditionalText> hjsonFiles, string rootNamespace)
    {
        var keys = new HashSet<string>();

        foreach (var file in hjsonFiles)
        foreach (var key in GetKeysFromFile(file))
        {
            keys.Add(key);
        }

        var root = new LocalizationNode("", [], []);

        foreach (var key in keys)
        {
            var parts   = key.Split('.');
            var current = root;

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                if (i == parts.Length - 1)
                {
                    current.Keys.Add(key);
                }
                else
                {
                    if (!current.Nodes.TryGetValue(part, out var node))
                    {
                        node = new LocalizationNode(part, [], []);
                        current.Nodes.Add(part, node);
                    }

                    current = node;
                }
            }
        }

        var sb = new StringBuilder();

        sb.AppendLine($"using {rootNamespace}.Common.Localization;");
        sb.AppendLine();
        sb.AppendLine($"namespace {rootNamespace}.Core;");
        sb.AppendLine();
        sb.AppendLine($"internal static class LocalizationReferences");
        sb.AppendLine("{");

        foreach (var node in root.Nodes.Values)
        {
            sb.AppendLine(GenerateTextFromLocalizationNode(node, 1));
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateTextFromLocalizationNode(LocalizationNode node, int depth = 0)
    {
        var sb     = new StringBuilder();
        var indent = new string(' ', depth * 4);

        sb.AppendLine($"{indent}public static class {node.Name} {{");
        foreach (var key in node.Keys)
        {
            sb.AppendLine($"{indent}    public static readonly LocalizableText {key.Split('.').Last()} = LocalizableText.FromKey(\"{key}\");");
        }

        foreach (var child in node.Nodes.Values)
        {
            sb.Append(GenerateTextFromLocalizationNode(child, depth + 1));
        }

        sb.AppendLine($"{indent}}}");

        return sb.ToString();
    }

    private static List<string> GetKeysFromFile(AdditionalText file)
    {
        var keys       = new List<string>();
        var prefix     = GetPrefixFromPath(file.Path);
        var text       = file.GetText()!.ToString();
        var json       = HjsonValue.Parse(text).ToString();
        var jsonObject = JObject.Parse(json);

        foreach (var t in jsonObject.SelectTokens("$..*"))
        {
            if (t.HasValues)
            {
                continue;
            }

            if (t is JObject { Count: 0 })
            {
                continue;
            }

            var path    = "";
            var current = t;

            for (var parent = t.Parent; parent is not null; parent = parent.Parent)
            {
                path = parent switch
                {
                    JProperty property => property.Name          + (path == string.Empty ? string.Empty : '.' + path),
                    JArray array       => array.IndexOf(current) + (path == string.Empty ? string.Empty : '.' + path),
                    _                  => path,
                };
                current = parent;
            }

            path = path.Replace(".$parentVal", "");
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                path = prefix + '.' + path;
            }

            keys.Add(path);
        }

        return keys;
    }

    private static string? GetPrefixFromPath(string path)
    {
        path = Path.GetFileNameWithoutExtension(path);
        var splitByUnderscore = path.Split('_');

        return splitByUnderscore.Length switch
        {
            0 => null,
            1 => splitByUnderscore[0],
            2 => splitByUnderscore[1],
            _ => throw new ArgumentException("Invalid path format", nameof(path))
        };
    }

    private static string GenerateCommonLocalization(
        SourceProductionContext       ctx,
        AnalyzerConfigOptionsProvider options
    )
    {
        if (GeneratorUtil.GetRootNamespaceOrRaiseDiagnostic(ctx, options.GlobalOptions) is not { } rootNamespace)
        {
            return "#error Failed to find root namespace";
        }

        return $@"
#nullable enable

using Terraria.Localization;

namespace {rootNamespace}.Common.Localization;

internal interface ILocalizableTextProvider
{{
    string GetText(object?[] arguments);

    bool IsLoaded();
}}

internal sealed class KeyLocalizableTextProvider(string key) : ILocalizableTextProvider
{{
    public string GetText(object?[] arguments)
    {{
        return Language.GetTextValue(key, arguments);
    }}

    public bool IsLoaded()
    {{
        return Language.Exists(key);
    }}
}}

internal sealed class LiteralLocalizableTextProvider(string text) : ILocalizableTextProvider
{{
    public string GetText(object?[] arguments)
    {{
        return string.Format(text, arguments);
    }}

    public bool IsLoaded()
    {{
        return true;
    }}
}}


internal sealed class LocalizedTextLocalizableTextProvider(LocalizedText text) : ILocalizableTextProvider
{{
    // Honestly, there should never be a case where the instance isn't already
    // formatted... but better safe than sorry.
    public string GetText(object?[] arguments)
    {{
        return Language.GetTextValue(text.Key, arguments);
    }}

    // Nothing stopping you from getting an instance that doesn't exist... so
    // take this.
    public bool IsLoaded()
    {{
        return Language.Exists(text.Key);
    }}
}}


internal readonly struct LocalizableText
{{
    private readonly ILocalizableTextProvider provider;

    public object[] Arguments {{ get; }}

    private LocalizableText(ILocalizableTextProvider provider, params object[] arguments)
    {{
        this.provider = provider;
        Arguments = arguments;
    }}

    public LocalizableText WithArguments(params object[] arguments)
    {{
        return new LocalizableText(provider, arguments);
    }}

    public override string ToString()
    {{
        return MaskWhenNotYetLoaded(provider, Arguments);
    }}

    private static string MaskWhenNotYetLoaded(ILocalizableTextProvider provider, object?[] arguments)
    {{
        const string not_yet_loaded_msg = ""Localization not yet loaded..."";
        return !provider.IsLoaded() ? not_yet_loaded_msg : provider.GetText(arguments);
    }}

    public static LocalizableText FromLiteral(string text, params object[] args)
    {{
        return new LocalizableText(new LiteralLocalizableTextProvider(text), args);
    }}

    public static LocalizableText FromKey(string key, params object[] args)
    {{
        return new LocalizableText(new KeyLocalizableTextProvider(key), args);
    }}

    public static LocalizableText FromLocalizedText(LocalizedText text, params object[] args)
    {{
        return new LocalizableText(new LocalizedTextLocalizableTextProvider(text), args);
    }}
}}
".Trim();
    }
}