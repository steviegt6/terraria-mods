using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Build.Pre.Util;
using Build.Shared;

using Hjson;

using Newtonsoft.Json.Linq;

namespace Build.Pre.Features.Localization;

internal sealed partial class LocalizationGenerator : BuildTask
{
    // TODO: Support reference regex too?  Sucks!
    // https://github.com/tModLoader/tModLoader/blob/eaf63ce340c09e3361a40c0e8fe8073a6cdd1b3d/patches/tModLoader/Terraria/Localization/LanguageManager.tML.cs#L43
    private static readonly Regex arg_remapping_regex = ArgRemappingRegex();

    public override void Run(ProjectContext ctx)
    {
        ctx.WriteFile(
            "Core/LocalizationReferences.cs",
            GenerateLocalization(ctx, ctx.EnumerateGroup("localization"))
        );
    }

    private static string GenerateLocalization(ProjectContext ctx, IEnumerable<ProjectFile> files)
    {
        var keys = new HashSet<(string key, string value)>();

        foreach (var file in files)
        {
            if (!HjsonValidator.ValidateHjsonFile(file.FullPath))
            {
                continue;
            }

            foreach (var key in GetKeysFromFile(file))
            {
                keys.Add(key);
            }
        }

        var root = new LocalizationNode("", [], []);

        foreach (var key in keys)
        {
            var parts = key.key.Split('.');
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

        foreach (var node in root.Nodes.Values)
        {
            sb.Append(GenerateTextFromLocalizationNode(node, "", 1));
        }

        return $$"""
                 using Terraria.Localization;

                 namespace {{ctx.ProjectNamespace}}.Core;

                 // ReSharper disable MemberHidesStaticFromOuterClass
                 internal static class LocalizationReferences
                 {
                 {{sb.ToString().TrimEnd()}}
                 }
                 """;

        static string GenerateTextFromLocalizationNode(LocalizationNode node, string parentKey, int depth = 0)
        {
            var ourKey = (parentKey + '.' + node.Name).TrimStart('.');

            var sb = new StringBuilder();
            var indent = new string(' ', depth * 4);

            if (depth > 1)
            {
                sb.AppendLine();
            }

            sb.AppendLine($"{indent}public static class {node.Name}");
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

            for (var i = 0; i < node.Keys.Count; i++)
            {
                sb.AppendLine();

                var (key, value) = node.Keys[i];
                var name = key.Split('.').Last();
                var args = GetArgumentCount(value);

                sb.AppendLine($"{indent}    public static class {name}");
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
                    for (var j = 0; j < args; j++)
                    {
                        argNames.Add($"arg{j}");
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
    }

    private static List<(string key, string value)> GetKeysFromFile(ProjectFile file)
    {
        var keys = new List<(string key, string value)>();
        var prefix = GetPrefixFromPath(file.RelativePath);
        var text = File.ReadAllText(file.FullPath);
        var json = HjsonValue.Parse(text).ToString();
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

            var path = "";
            var current = t;

            for (var parent = t.Parent; parent is not null; parent = parent.Parent)
            {
                path = parent switch
                {
                    JProperty property => property.Name + (path == string.Empty ? string.Empty : '.' + path),
                    JArray array => array.IndexOf(current) + (path == string.Empty ? string.Empty : '.' + path),
                    _ => path,
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
                JTokenType.String => t.Value<string>() ?? "",
                JTokenType.Integer => t.Value<int>().ToString(),
                JTokenType.Boolean => t.Value<bool>().ToString(),
                JTokenType.Float => t.Value<float>().ToString(CultureInfo.InvariantCulture),
                _ => t.ToString(),
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

    [GeneratedRegex(@"(?<={\^?)(\d+)(?=(?::[^\r\n]+?)?})", RegexOptions.Compiled)]
    private static partial Regex ArgRemappingRegex();
}