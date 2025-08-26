using Terraria.Localization;

namespace Tomat.TML.Mod.PrefixGrammar.Core;

// ReSharper disable MemberHidesStaticFromOuterClass
internal static class LocalizationReferences
{
    public static class Mods
    {
        public const string KEY = "Mods";

        public static LocalizedText GetChildText(string childKey)
        {
            return Language.GetText(KEY + '.' + childKey);
        }

        public static string GetChildTextValue(string childKey, params object?[] values)
        {
            return Language.GetTextValue(KEY + '.' + childKey, values);
        }

        public static class PrefixGrammar
        {
            public const string KEY = "Mods.PrefixGrammar";

            public static LocalizedText GetChildText(string childKey)
            {
                return Language.GetText(KEY + '.' + childKey);
            }

            public static string GetChildTextValue(string childKey, params object?[] values)
            {
                return Language.GetTextValue(KEY + '.' + childKey, values);
            }

            public static class Configs
            {
                public const string KEY = "Mods.PrefixGrammar.Configs";

                public static LocalizedText GetChildText(string childKey)
                {
                    return Language.GetText(KEY + '.' + childKey);
                }

                public static string GetChildTextValue(string childKey, params object?[] values)
                {
                    return Language.GetTextValue(KEY + '.' + childKey, values);
                }

                public static class GrammarConfig
                {
                    public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig";

                    public static LocalizedText GetChildText(string childKey)
                    {
                        return Language.GetText(KEY + '.' + childKey);
                    }

                    public static string GetChildTextValue(string childKey, params object?[] values)
                    {
                        return Language.GetTextValue(KEY + '.' + childKey, values);
                    }

                    public static class DisplayName
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.DisplayName";
                        public const int ARG_COUNT = 0;

                        public static LocalizedText GetText()
                        {
                            return Language.GetText(KEY);
                        }

                        public static string GetTextValue()
                        {
                            return Language.GetTextValue(KEY);
                        }

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }
                    }

                    public static class PrefixFormatting
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.PrefixFormatting";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.PrefixFormatting.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }

                        public static class Tooltip
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.PrefixFormatting.Tooltip";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }

                    public static class DoubleFormatting
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.DoubleFormatting";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.DoubleFormatting.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }

                        public static class Tooltip
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.DoubleFormatting.Tooltip";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }

                    public static class InsertComma
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.InsertComma";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.InsertComma.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }

                        public static class Tooltip
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.GrammarConfig.InsertComma.Tooltip";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }
                }

                public static class PrefixOptions
                {
                    public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions";

                    public static LocalizedText GetChildText(string childKey)
                    {
                        return Language.GetText(KEY + '.' + childKey);
                    }

                    public static string GetChildTextValue(string childKey, params object?[] values)
                    {
                        return Language.GetTextValue(KEY + '.' + childKey, values);
                    }

                    public static class Tooltip
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.Tooltip";
                        public const int ARG_COUNT = 0;

                        public static LocalizedText GetText()
                        {
                            return Language.GetText(KEY);
                        }

                        public static string GetTextValue()
                        {
                            return Language.GetTextValue(KEY);
                        }

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }
                    }

                    public static class Default
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.Default";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.Default.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }

                    public static class Replace
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.Replace";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.Replace.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }

                    public static class After
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.After";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.PrefixOptions.After.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }
                }

                public static class DoubleOptions
                {
                    public const string KEY = "Mods.PrefixGrammar.Configs.DoubleOptions";

                    public static LocalizedText GetChildText(string childKey)
                    {
                        return Language.GetText(KEY + '.' + childKey);
                    }

                    public static string GetChildTextValue(string childKey, params object?[] values)
                    {
                        return Language.GetTextValue(KEY + '.' + childKey, values);
                    }

                    public static class Tooltip
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.DoubleOptions.Tooltip";
                        public const int ARG_COUNT = 0;

                        public static LocalizedText GetText()
                        {
                            return Language.GetText(KEY);
                        }

                        public static string GetTextValue()
                        {
                            return Language.GetTextValue(KEY);
                        }

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }
                    }

                    public static class Default
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.DoubleOptions.Default";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.DoubleOptions.Default.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }

                    public static class Doubly
                    {
                        public const string KEY = "Mods.PrefixGrammar.Configs.DoubleOptions.Doubly";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Label
                        {
                            public const string KEY = "Mods.PrefixGrammar.Configs.DoubleOptions.Doubly.Label";
                            public const int ARG_COUNT = 0;

                            public static LocalizedText GetText()
                            {
                                return Language.GetText(KEY);
                            }

                            public static string GetTextValue()
                            {
                                return Language.GetTextValue(KEY);
                            }

                            public static LocalizedText GetChildText(string childKey)
                            {
                                return Language.GetText(KEY + '.' + childKey);
                            }

                            public static string GetChildTextValue(string childKey, params object?[] values)
                            {
                                return Language.GetTextValue(KEY + '.' + childKey, values);
                            }
                        }
                    }
                }
            }
        }
    }
}