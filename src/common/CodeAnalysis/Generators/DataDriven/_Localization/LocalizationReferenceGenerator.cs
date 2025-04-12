using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Hjson;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using Newtonsoft.Json.Linq;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

[Generator]
public sealed class LocalizationReferenceGenerator : IIncrementalGenerator
{
    // TODO: Support reference regex too?  Sucks!
    // https://github.com/tModLoader/tModLoader/blob/eaf63ce340c09e3361a40c0e8fe8073a6cdd1b3d/patches/tModLoader/Terraria/Localization/LanguageManager.tML.cs#L43
    private static readonly Regex arg_remapping_regex = new(@"(?<={\^?)(\d+)(?=(?::[^\r\n]+?)?})", RegexOptions.Compiled);

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
        var keys = new HashSet<(string key, string value)>();

        foreach (var file in hjsonFiles)
        foreach (var key in GetKeysFromFile(file))
        {
            keys.Add(key);
        }

        var root = new LocalizationNode("", [], []);

        foreach (var key in keys)
        {
            var parts   = key.key.Split('.');
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

        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using Terraria.Localization;");
        sb.AppendLine();
        sb.AppendLine($"namespace {rootNamespace}.Core;");
        sb.AppendLine();
        sb.AppendLine("internal static class LocalizationReferences");
        sb.AppendLine("{");

        foreach (var node in root.Nodes.Values)
        {
            sb.AppendLine(GenerateTextFromLocalizationNode(node, "", 1));
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GenerateTextFromLocalizationNode(LocalizationNode node, string parentKey, int depth = 0)
    {
        var ourKey = (parentKey + '.' + node.Name).TrimStart('.');

        var sb     = new StringBuilder();
        var indent = new string(' ', depth * 4);

        sb.AppendLine($"{indent}public static partial class {node.Name}");
        sb.AppendLine($"{indent}{{");

        sb.AppendLine($"{indent}    public const string KEY = \"{ourKey}\";");
        sb.AppendLine();
        sb.AppendLine($"{indent}    public static LocalizedText GetChildText(string childKey)");
        sb.AppendLine($"{indent}    {{");
        sb.AppendLine($"{indent}        return Language.GetText(KEY + '.' + childKey);");
        sb.AppendLine($"{indent}    }}");
        sb.AppendLine();
        sb.AppendLine($"{indent}    public static string GetChildTextValue(string childKey, params object?[] values)");
        sb.AppendLine($"{indent}    {{");
        sb.AppendLine($"{indent}        return Language.GetTextValue(KEY + '.' + childKey, values);");
        sb.AppendLine($"{indent}    }}");

        foreach (var (key, value) in node.Keys)
        {
            var name = key.Split('.').Last();
            var args = GetArgumentCount(value);

            sb.AppendLine($"{indent}    public static partial class {name}");
            sb.AppendLine($"{indent}    {{");
            sb.AppendLine($"{indent}        public const string KEY = \"{key}\";");
            sb.AppendLine($"{indent}        public const int ARG_COUNT = {args};");
            sb.AppendLine();
            sb.AppendLine($"{indent}        public static LocalizedText GetText()");
            sb.AppendLine($"{indent}        {{");
            sb.AppendLine($"{indent}            return Language.GetText(KEY);");
            sb.AppendLine($"{indent}        }}");
            sb.AppendLine();

            if (args == 0)
            {
                sb.AppendLine($"{indent}        public static string GetTextValue()");
                sb.AppendLine($"{indent}        {{");
                sb.AppendLine($"{indent}            return Language.GetTextValue(KEY);");
                sb.AppendLine($"{indent}        }}");
            }
            else
            {
                var argNames = new List<string>();
                for (var i = 0; i < args; i++)
                {
                    argNames.Add($"arg{i}");
                }

                sb.AppendLine($"{indent}        public static string GetTextValue({string.Join(", ", argNames.Select(x => $"object? {x}"))})");
                sb.AppendLine($"{indent}        {{");
                sb.AppendLine($"{indent}            return Language.GetTextValue(KEY, {string.Join(", ", argNames)});");
                sb.AppendLine($"{indent}        }}");
            }

            sb.AppendLine();
            sb.AppendLine($"{indent}        public static LocalizedText GetChildText(string childKey)");
            sb.AppendLine($"{indent}        {{");
            sb.AppendLine($"{indent}            return Language.GetText(KEY + '.' + childKey);");
            sb.AppendLine($"{indent}        }}");
            sb.AppendLine();
            sb.AppendLine($"{indent}        public static string GetChildTextValue(string childKey, params object?[] values)");
            sb.AppendLine($"{indent}        {{");
            sb.AppendLine($"{indent}            return Language.GetTextValue(KEY + '.' + childKey, values);");
            sb.AppendLine($"{indent}        }}");

            sb.AppendLine($"{indent}    }}");
        }

        foreach (var child in node.Nodes.Values)
        {
            sb.Append(GenerateTextFromLocalizationNode(child, ourKey, depth + 1));
        }

        sb.AppendLine($"{indent}}}");

        return sb.ToString();
    }

    private static List<(string key, string value)> GetKeysFromFile(AdditionalText file)
    {
        var keys       = new List<(string key, string value)>();
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

            var value = t.Type switch
            {
                JTokenType.String  => t.Value<string>() ?? "",
                JTokenType.Integer => t.Value<int>().ToString(),
                JTokenType.Boolean => t.Value<bool>().ToString(),
                JTokenType.Float   => t.Value<float>().ToString(CultureInfo.InvariantCulture),
                _                  => t.ToString(),
            };

            keys.Add((path, value));
        }

        return keys;
    }

    private static string? GetPrefixFromPath(string path)
    {
        path = Path.GetFileNameWithoutExtension(path);
        var splitByUnderscore = path.Split('_');

        return splitByUnderscore.Length == 2 ? splitByUnderscore[1] : null;
    }

    private static int GetArgumentCount(string value)
    {
        return arg_remapping_regex.Matches(value).Count;
    }
}