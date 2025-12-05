using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;

namespace PrefixGrammar;

public abstract class LanguageRules
{
    private sealed class English : LanguageRules
    {
        private const string doubly_prefix = "Doubly-";

        private const string separating_punctuation = ",";

        // TODO: Handle indefinites ('a', 'an'), which may require detecting the
        //       correct form for prefixes (we can perhaps hardcode a list of
        //       correct forms for known prefixes).
        private static readonly string[] articles = ["The"];

        public override string FormatString(
            string text,
            GrammarConfig.PrefixOptions prefixFormatting,
            GrammarConfig.DoubleOptions doubleFormatting,
            bool insertPunctuation
        )
        {
            var parts = text.Split(' ').ToList();

            // Don't need to do anything for a single word.
            if (parts.Count < 2)
            {
                return text;
            }

            var realIdx = 0;
            foreach (var article in articles)
            {
                if (parts[1] != article)
                {
                    continue;
                }

                realIdx = 1;
                (parts[0], parts[1]) = (parts[1], parts[0]);
                break;
            }

            // Safety because the index can be bumped by 1, meaning we can
            // directly access element 3.  If the item is two words or less then
            // these conditions will probably never apply, anyway.
            if (parts.Count < 3)
            {
                return string.Join(' ', parts);
            }

            // We can add ordinal-ignore-case comparison later.  Needs thinking.
            var isDouble = parts[realIdx] == parts[realIdx + 1] && doubleFormatting != GrammarConfig.DoubleOptions.Default;

            if (isDouble)
            {
                switch (doubleFormatting)
                {
                    case GrammarConfig.DoubleOptions.Default:
                        break;

                    case GrammarConfig.DoubleOptions.Doubly:
                        var prefix = parts[realIdx];
                        parts = parts[(realIdx + 2)..];
                        parts.Insert(0, doubly_prefix + prefix);
                        return string.Join(' ', parts);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(doubleFormatting), doubleFormatting, null);
                }
            }

            // TODO: Is this adequate?
            var startsWithPrefix = Lang.prefix.Any(x => x.Value == parts[realIdx + 1]);
            if (!startsWithPrefix)
            {
                return string.Join(' ', parts);
            }

            switch (prefixFormatting)
            {
                case GrammarConfig.PrefixOptions.Default:
                    if (insertPunctuation)
                    {
                        parts[realIdx] += separating_punctuation;
                    }

                    break;

                case GrammarConfig.PrefixOptions.Replace:
                    parts.RemoveAt(realIdx + 1);
                    break;

                case GrammarConfig.PrefixOptions.After:
                    (parts[realIdx], parts[realIdx + 1]) = (parts[realIdx + 1], parts[realIdx]);

                    if (insertPunctuation)
                    {
                        parts[realIdx] += separating_punctuation;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(prefixFormatting), prefixFormatting, null);
            }

            return string.Join(' ', parts);
        }
    }

    private const string english_default_code = "en-US";

    private static readonly Dictionary<string, LanguageRules> rules_by_language_code = new()
    {
        [english_default_code] = new English(),
    };

    private static readonly LanguageRules default_rule = rules_by_language_code[english_default_code];

    public static LanguageRules GetRulesForLanguage(GameCulture culture)
    {
        return rules_by_language_code.GetValueOrDefault(culture.CultureInfo.Name, default_rule);
    }

    public abstract string FormatString(
        string text,
        GrammarConfig.PrefixOptions prefixFormatting,
        GrammarConfig.DoubleOptions doubleFormatting,
        bool insertPunctuation
    );
}
