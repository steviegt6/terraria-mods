using Terraria.Localization;

namespace Nightshade.Core;

// ReSharper disable MemberHidesStaticFromOuterClass
internal static class LocalizationReferences
{
    public static class ItemName
    {
        public const string KEY = "ItemName";

        public static LocalizedText GetChildText(string childKey)
        {
            return Language.GetText(KEY + '.' + childKey);
        }

        public static string GetChildTextValue(string childKey, params object?[] values)
        {
            return Language.GetTextValue(KEY + '.' + childKey, values);
        }

        public static class FirstFractal
        {
            public const string KEY = "ItemName.FirstFractal";
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

        public static class Nightshade
        {
            public const string KEY = "Mods.Nightshade";

            public static LocalizedText GetChildText(string childKey)
            {
                return Language.GetText(KEY + '.' + childKey);
            }

            public static string GetChildTextValue(string childKey, params object?[] values)
            {
                return Language.GetTextValue(KEY + '.' + childKey, values);
            }

            public static class UI
            {
                public const string KEY = "Mods.Nightshade.UI";

                public static LocalizedText GetChildText(string childKey)
                {
                    return Language.GetText(KEY + '.' + childKey);
                }

                public static string GetChildTextValue(string childKey, params object?[] values)
                {
                    return Language.GetTextValue(KEY + '.' + childKey, values);
                }

                public static class VanityCursor
                {
                    public const string KEY = "Mods.Nightshade.UI.VanityCursor";
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

                public static class VanityCursorTrail
                {
                    public const string KEY = "Mods.Nightshade.UI.VanityCursorTrail";
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

                public static class ModIcon
                {
                    public const string KEY = "Mods.Nightshade.UI.ModIcon";

                    public static LocalizedText GetChildText(string childKey)
                    {
                        return Language.GetText(KEY + '.' + childKey);
                    }

                    public static string GetChildTextValue(string childKey, params object?[] values)
                    {
                        return Language.GetTextValue(KEY + '.' + childKey, values);
                    }

                    public static class ModName
                    {
                        public const string KEY = "Mods.Nightshade.UI.ModIcon.ModName";
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

                    public static class AuthorHeader
                    {
                        public const string KEY = "Mods.Nightshade.UI.ModIcon.AuthorHeader";
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

                    public static class AprilFools
                    {
                        public const string KEY = "Mods.Nightshade.UI.ModIcon.AprilFools";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class ModName
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.AprilFools.ModName";
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

                    public static class Authors
                    {
                        public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors";

                        public static LocalizedText GetChildText(string childKey)
                        {
                            return Language.GetText(KEY + '.' + childKey);
                        }

                        public static string GetChildTextValue(string childKey, params object?[] values)
                        {
                            return Language.GetTextValue(KEY + '.' + childKey, values);
                        }

                        public static class Tomat
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Tomat";
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

                        public static class Triangle
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Triangle";
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

                        public static class Math2
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Math2";
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

                        public static class OneThree
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.OneThree";
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

                        public static class Tyeski
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Tyeski";
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

                        public static class Wymsical
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Wymsical";
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

                        public static class Taco
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Taco";
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

                        public static class Sixtydegrees
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Sixtydegrees";
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

                        public static class DylanDoe21
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.DylanDoe21";
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

                        public static class Ebonfly
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Ebonfly";
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

                        public static class Citrus
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.Citrus";
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

                        public static class BabyBlueSheep
                        {
                            public const string KEY = "Mods.Nightshade.UI.ModIcon.Authors.BabyBlueSheep";
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

                public static class VanityCursorAppliesTo
                {
                    public const string KEY = "Mods.Nightshade.UI.VanityCursorAppliesTo";

                    public static LocalizedText GetChildText(string childKey)
                    {
                        return Language.GetText(KEY + '.' + childKey);
                    }

                    public static string GetChildTextValue(string childKey, params object?[] values)
                    {
                        return Language.GetTextValue(KEY + '.' + childKey, values);
                    }

                    public static class None
                    {
                        public const string KEY = "Mods.Nightshade.UI.VanityCursorAppliesTo.None";
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

                    public static class Cursor
                    {
                        public const string KEY = "Mods.Nightshade.UI.VanityCursorAppliesTo.Cursor";
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

                    public static class Outline
                    {
                        public const string KEY = "Mods.Nightshade.UI.VanityCursorAppliesTo.Outline";
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

                    public static class Both
                    {
                        public const string KEY = "Mods.Nightshade.UI.VanityCursorAppliesTo.Both";
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

            public static class Items
            {
                public const string KEY = "Mods.Nightshade.Items";

                public static LocalizedText GetChildText(string childKey)
                {
                    return Language.GetText(KEY + '.' + childKey);
                }

                public static string GetChildTextValue(string childKey, params object?[] values)
                {
                    return Language.GetTextValue(KEY + '.' + childKey, values);
                }

                public static class StarTalismanItem
                {
                    public const string KEY = "Mods.Nightshade.Items.StarTalismanItem";

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
                        public const string KEY = "Mods.Nightshade.Items.StarTalismanItem.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.StarTalismanItem.Tooltip";
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

                public static class RejuvenationAmuletItem
                {
                    public const string KEY = "Mods.Nightshade.Items.RejuvenationAmuletItem";

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
                        public const string KEY = "Mods.Nightshade.Items.RejuvenationAmuletItem.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.RejuvenationAmuletItem.Tooltip";
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

                public static class MechanicalBeetleItem
                {
                    public const string KEY = "Mods.Nightshade.Items.MechanicalBeetleItem";

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
                        public const string KEY = "Mods.Nightshade.Items.MechanicalBeetleItem.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.MechanicalBeetleItem.Tooltip";
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

                public static class ImpactBullet
                {
                    public const string KEY = "Mods.Nightshade.Items.ImpactBullet";

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
                        public const string KEY = "Mods.Nightshade.Items.ImpactBullet.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.ImpactBullet.Tooltip";
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

                public static class RiptideArrow
                {
                    public const string KEY = "Mods.Nightshade.Items.RiptideArrow";

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
                        public const string KEY = "Mods.Nightshade.Items.RiptideArrow.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.RiptideArrow.Tooltip";
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

                public static class BarbaricCuffsItem
                {
                    public const string KEY = "Mods.Nightshade.Items.BarbaricCuffsItem";

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
                        public const string KEY = "Mods.Nightshade.Items.BarbaricCuffsItem.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.BarbaricCuffsItem.Tooltip";
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

                public static class SpikedCuffsItem
                {
                    public const string KEY = "Mods.Nightshade.Items.SpikedCuffsItem";

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
                        public const string KEY = "Mods.Nightshade.Items.SpikedCuffsItem.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.SpikedCuffsItem.Tooltip";
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

                public static class StarFragmentItem
                {
                    public const string KEY = "Mods.Nightshade.Items.StarFragmentItem";

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
                        public const string KEY = "Mods.Nightshade.Items.StarFragmentItem.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.StarFragmentItem.Tooltip";
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

                public static class LivingCactusBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingCactusBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusBlock.Tooltip";
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

                public static class LivingCactusWand
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingCactusWand";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWand.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWand.Tooltip";
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

                public static class LivingCactusWoodBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodBlock.Tooltip";
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

                public static class LivingPalmLeafBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingPalmLeafBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingPalmLeafBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingPalmLeafBlock.Tooltip";
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

                public static class LivingPalmWoodBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingPalmWoodBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingPalmWoodBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingPalmWoodBlock.Tooltip";
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

                public static class LivingCactusPotPlacer
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingCactusPotPlacer";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusPotPlacer.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusPotPlacer.Tooltip";
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

                public static class CactusWoodBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.CactusWoodBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.CactusWoodBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.CactusWoodBlock.Tooltip";
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

                public static class CactusWoodPlatformBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.CactusWoodPlatformBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.CactusWoodPlatformBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.CactusWoodPlatformBlock.Tooltip";
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

                public static class LivingCactusWoodWand
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodWand";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodWand.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodWand.Tooltip";
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

                public static class PreDigester
                {
                    public const string KEY = "Mods.Nightshade.Items.PreDigester";

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
                        public const string KEY = "Mods.Nightshade.Items.PreDigester.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.PreDigester.Tooltip";
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

                public static class CoconutChest
                {
                    public const string KEY = "Mods.Nightshade.Items.CoconutChest";

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
                        public const string KEY = "Mods.Nightshade.Items.CoconutChest.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.CoconutChest.Tooltip";
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

                public static class FourLeafClover
                {
                    public const string KEY = "Mods.Nightshade.Items.FourLeafClover";

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
                        public const string KEY = "Mods.Nightshade.Items.FourLeafClover.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.FourLeafClover.Tooltip";
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

                public static class RainbowHorseshoe
                {
                    public const string KEY = "Mods.Nightshade.Items.RainbowHorseshoe";

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
                        public const string KEY = "Mods.Nightshade.Items.RainbowHorseshoe.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.RainbowHorseshoe.Tooltip";
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

                public static class HallowedCharm
                {
                    public const string KEY = "Mods.Nightshade.Items.HallowedCharm";

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
                        public const string KEY = "Mods.Nightshade.Items.HallowedCharm.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.HallowedCharm.Tooltip";
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

                public static class RabbitsFoot
                {
                    public const string KEY = "Mods.Nightshade.Items.RabbitsFoot";

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
                        public const string KEY = "Mods.Nightshade.Items.RabbitsFoot.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.RabbitsFoot.Tooltip";
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

                public static class TemporalVestige
                {
                    public const string KEY = "Mods.Nightshade.Items.TemporalVestige";

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
                        public const string KEY = "Mods.Nightshade.Items.TemporalVestige.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.TemporalVestige.Tooltip";
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

                public static class DriftersBoots
                {
                    public const string KEY = "Mods.Nightshade.Items.DriftersBoots";

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
                        public const string KEY = "Mods.Nightshade.Items.DriftersBoots.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.DriftersBoots.Tooltip";
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

                public static class SunGodEye
                {
                    public const string KEY = "Mods.Nightshade.Items.SunGodEye";

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
                        public const string KEY = "Mods.Nightshade.Items.SunGodEye.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.SunGodEye.Tooltip";
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

                public static class RaBoots
                {
                    public const string KEY = "Mods.Nightshade.Items.RaBoots";

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
                        public const string KEY = "Mods.Nightshade.Items.RaBoots.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.RaBoots.Tooltip";
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

                public static class Godspeed
                {
                    public const string KEY = "Mods.Nightshade.Items.Godspeed";

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
                        public const string KEY = "Mods.Nightshade.Items.Godspeed.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.Godspeed.Tooltip";
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

                public static class CactusSplashJug
                {
                    public const string KEY = "Mods.Nightshade.Items.CactusSplashJug";

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
                        public const string KEY = "Mods.Nightshade.Items.CactusSplashJug.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.CactusSplashJug.Tooltip";
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

                public static class LivingCactusWoodWallBlock
                {
                    public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodWallBlock";

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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodWallBlock.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LivingCactusWoodWallBlock.Tooltip";
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

                public static class BiomeCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.BiomeCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.BiomeCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.BiomeCursor.Tooltip";
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

                public static class DepthCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.DepthCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.DepthCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.DepthCursor.Tooltip";
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

                public static class LifeCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.LifeCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.LifeCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.LifeCursor.Tooltip";
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

                public static class ManaCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.ManaCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.ManaCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.ManaCursor.Tooltip";
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

                public static class MoneyCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.MoneyCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.MoneyCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.MoneyCursor.Tooltip";
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

                public static class PartyCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.PartyCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.PartyCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.PartyCursor.Tooltip";
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

                public static class SpeedCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.SpeedCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.SpeedCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.SpeedCursor.Tooltip";
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

                public static class TimeCursor
                {
                    public const string KEY = "Mods.Nightshade.Items.TimeCursor";

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
                        public const string KEY = "Mods.Nightshade.Items.TimeCursor.DisplayName";
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
                        public const string KEY = "Mods.Nightshade.Items.TimeCursor.Tooltip";
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

            public static class Projectiles
            {
                public const string KEY = "Mods.Nightshade.Projectiles";

                public static LocalizedText GetChildText(string childKey)
                {
                    return Language.GetText(KEY + '.' + childKey);
                }

                public static string GetChildTextValue(string childKey, params object?[] values)
                {
                    return Language.GetTextValue(KEY + '.' + childKey, values);
                }

                public static class ImpactBulletProjectile
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.ImpactBulletProjectile";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.ImpactBulletProjectile.DisplayName";
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

                public static class RiptideArrowProjectile
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.RiptideArrowProjectile";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.RiptideArrowProjectile.DisplayName";
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

                public static class StarFragmentProj1
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj1";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj1.DisplayName";
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

                public static class StarFragmentProj2
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj2";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj2.DisplayName";
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

                public static class StarFragmentProj3
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj3";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj3.DisplayName";
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

                public static class StarFragmentProj4
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj4";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj4.DisplayName";
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

                public static class StarFragmentProj5
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj5";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProj5.DisplayName";
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

                public static class StarFragmentProjSpawner
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProjSpawner";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.StarFragmentProjSpawner.DisplayName";
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

                public static class PreDigesterHeldProj
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.PreDigesterHeldProj";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.PreDigesterHeldProj.DisplayName";
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

                public static class CactusSplashJugThrown
                {
                    public const string KEY = "Mods.Nightshade.Projectiles.CactusSplashJugThrown";

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
                        public const string KEY = "Mods.Nightshade.Projectiles.CactusSplashJugThrown.DisplayName";
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

            public static class Configs
            {
                public const string KEY = "Mods.Nightshade.Configs";

                public static LocalizedText GetChildText(string childKey)
                {
                    return Language.GetText(KEY + '.' + childKey);
                }

                public static string GetChildTextValue(string childKey, params object?[] values)
                {
                    return Language.GetTextValue(KEY + '.' + childKey, values);
                }

                public static class DummyConfig
                {
                    public const string KEY = "Mods.Nightshade.Configs.DummyConfig";

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
                        public const string KEY = "Mods.Nightshade.Configs.DummyConfig.DisplayName";
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

            public static class Achievements
            {
                public const string KEY = "Mods.Nightshade.Achievements";

                public static LocalizedText GetChildText(string childKey)
                {
                    return Language.GetText(KEY + '.' + childKey);
                }

                public static string GetChildTextValue(string childKey, params object?[] values)
                {
                    return Language.GetTextValue(KEY + '.' + childKey, values);
                }

                public static class Achievement1
                {
                    public const string KEY = "Mods.Nightshade.Achievements.Achievement1";

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
                        public const string KEY = "Mods.Nightshade.Achievements.Achievement1.DisplayName";
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

                    public static class Description
                    {
                        public const string KEY = "Mods.Nightshade.Achievements.Achievement1.Description";
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

                public static class Achievement2
                {
                    public const string KEY = "Mods.Nightshade.Achievements.Achievement2";

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
                        public const string KEY = "Mods.Nightshade.Achievements.Achievement2.DisplayName";
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

                    public static class Description
                    {
                        public const string KEY = "Mods.Nightshade.Achievements.Achievement2.Description";
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

                public static class Achievement3
                {
                    public const string KEY = "Mods.Nightshade.Achievements.Achievement3";

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
                        public const string KEY = "Mods.Nightshade.Achievements.Achievement3.DisplayName";
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

                    public static class Description
                    {
                        public const string KEY = "Mods.Nightshade.Achievements.Achievement3.Description";
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