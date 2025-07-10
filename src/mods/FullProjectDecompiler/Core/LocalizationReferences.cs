using Terraria.Localization;

namespace Tomat.TML.Mod.FullProjectDecompiler.Core;

// ReSharper disable MemberHidesStaticFromOuterClass
internal static class LocalizationReferences
{
    public static class tModLoader
    {
        public const string KEY = "tModLoader";

        public static LocalizedText GetChildText(string childKey)
        {
            return Language.GetText(KEY + '.' + childKey);
        }

        public static string GetChildTextValue(string childKey, params object?[] values)
        {
            return Language.GetTextValue(KEY + '.' + childKey, values);
        }

        public static class ModInfoExtract
        {
            public const string KEY = "tModLoader.ModInfoExtract";
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