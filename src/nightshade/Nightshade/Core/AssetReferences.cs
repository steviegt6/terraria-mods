#pragma warning disable CS8981

namespace Nightshade.Core;

// ReSharper disable InconsistentNaming
internal static class AssetReferences
{
    public static class icon
    {
        public const string KEY = "Nightshade/icon";

        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
    }

    public static class icon_small
    {
        public const string KEY = "Nightshade/icon_small";

        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
    }

    public static class Assets
    {
        public static class Images
        {
            public static class Achievement1
            {
                public const string KEY = "Nightshade/Assets/Images/Achievement1";

                public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
            }

            public static class Achievement2
            {
                public const string KEY = "Nightshade/Assets/Images/Achievement2";

                public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
            }

            public static class Achievement3
            {
                public const string KEY = "Nightshade/Assets/Images/Achievement3";

                public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
            }

            public static class Categories
            {
                public const string KEY = "Nightshade/Assets/Images/Categories";

                public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
            }

            public static class Dusts
            {
                public static class DotDropletDust
                {
                    public const string KEY = "Nightshade/Assets/Images/Dusts/DotDropletDust";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }
            }

            public static class NPCs
            {
                public static class RainbowSlime
                {
                    public const string KEY = "Nightshade/Assets/Images/NPCs/RainbowSlime";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }
            }

            public static class Particles
            {
                public static class LiquidSplashParticle
                {
                    public const string KEY = "Nightshade/Assets/Images/Particles/LiquidSplashParticle";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }

                public static class RingGlow
                {
                    public const string KEY = "Nightshade/Assets/Images/Particles/RingGlow";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }
            }

            public static class Projectiles
            {
                public static class PreDigesterHeldProjSheet
                {
                    public const string KEY = "Nightshade/Assets/Images/Projectiles/PreDigesterHeldProjSheet";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }

                public static class StarFragmentSpawner
                {
                    public const string KEY = "Nightshade/Assets/Images/Projectiles/StarFragmentSpawner";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }
            }

            public static class QueenSlimePalettes
            {
                public static class RainbowSlime
                {
                    public const string KEY = "Nightshade/Assets/Images/QueenSlimePalettes/RainbowSlime";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }
            }

            public static class UI
            {
                public static class CursorTrail_Slot
                {
                    public const string KEY = "Nightshade/Assets/Images/UI/CursorTrail_Slot";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }

                public static class Cursor_Slot
                {
                    public const string KEY = "Nightshade/Assets/Images/UI/Cursor_Slot";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }

                public static class Cursor_Visibility
                {
                    public const string KEY = "Nightshade/Assets/Images/UI/Cursor_Visibility";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                }

                public static class ModIcon
                {
                    public static class BabyBlueSheep
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/BabyBlueSheep";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Citrus
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Citrus";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Ebonfly
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Ebonfly";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Icon
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Icon";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Icon_Dots
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Icon_Dots";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Math2
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Math2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class OneThree
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/OneThree";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Sixtydegrees
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Sixtydegrees";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Taco
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Taco";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Tomat
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Tomat";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Triangle
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Triangle";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Tyeski
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Tyeski";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Wymsical
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModIcon/Wymsical";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }

                public static class ModLoader
                {
                    public static class ButtonDeps
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModLoader/ButtonDeps";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class ButtonModConfig
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModLoader/ButtonModConfig";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class ButtonModConfig_ConciseModsList
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModLoader/ButtonModConfig_ConciseModsList";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class ButtonModInfo
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModLoader/ButtonModInfo";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class InnerPanelBackground
                    {
                        public const string KEY = "Nightshade/Assets/Images/UI/ModLoader/InnerPanelBackground";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }
            }

            public static class Gores
            {
                public static class Misc
                {
                    public static class CactusPotGore1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Gores/Misc/CactusPotGore1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CactusPotGore2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Gores/Misc/CactusPotGore2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }
            }

            public static class Items
            {
                public static class Accessories
                {
                    public static class BarbaricCuffs
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/BarbaricCuffs";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class BarbaricCuffs_HandsOn
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/BarbaricCuffs_HandsOn";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class DriftersBoots
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/DriftersBoots";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FourLeafClover
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/FourLeafClover";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Godspeed
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Godspeed";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class HallowedCharm
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/HallowedCharm";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class HandOfCreationStool
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/HandOfCreationStool";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class MechanicalBeetle
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/MechanicalBeetle";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RabbitsFoot
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/RabbitsFoot";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RaBoots
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/RaBoots";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RainbowHorseshoe
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/RainbowHorseshoe";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RejuvenationAmulet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/RejuvenationAmulet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RejuvenationAmulet_Neck
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/RejuvenationAmulet_Neck";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class SpikedCuffs
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/SpikedCuffs";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class SpikedCuffs_HandsOn
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/SpikedCuffs_HandsOn";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarTalisman
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/StarTalisman";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarTalisman_Neck
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/StarTalisman_Neck";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class SunGodEye
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/SunGodEye";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class TemporalVestige
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Accessories/TemporalVestige";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Cursors
                    {
                        public static class BiomeCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/BiomeCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class DepthCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/DepthCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class LifeCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/LifeCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class ManaCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/ManaCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class MoneyCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/MoneyCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class PartyCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/PartyCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class SpeedCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/SpeedCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }

                        public static class TimeCursor
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Accessories/Cursors/TimeCursor";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }
                    }
                }

                public static class Ammo
                {
                    public static class ImpactBullet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Ammo/ImpactBullet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class ImpactBulletProjectile
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Ammo/ImpactBulletProjectile";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RiptideArrow
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Ammo/RiptideArrow";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RiptideArrowProjectile
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Ammo/RiptideArrowProjectile";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }

                public static class Furniture
                {
                    public static class CoconutChest
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Furniture/CoconutChest";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }

                public static class Misc
                {
                    public static class CactusSplashJug
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/CactusSplashJug";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CactusWoodBlock
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/CactusWoodBlock";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingBorealLeafWand
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingBorealLeafWand";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingBorealWoodWand
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingBorealWoodWand";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingCactusWand
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingCactusWand";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingCactusWoodWallBlock
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingCactusWoodWallBlock";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingCactusWoodWand
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingCactusWoodWand";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingPalmLeafWand
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingPalmLeafWand";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingPalmWoodWand
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/LivingPalmWoodWand";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class PreDigester
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/PreDigester";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class PreDigesterSheet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/PreDigesterSheet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarFragment_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/StarFragment_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarFragment_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/StarFragment_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarFragment_3
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/StarFragment_3";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarFragment_4
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/StarFragment_4";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class StarFragment_5
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Misc/StarFragment_5";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CactusWoodFurniture
                    {
                        public static class CactusWoodPlatformBlock
                        {
                            public const string KEY = "Nightshade/Assets/Images/Items/Misc/CactusWoodFurniture/CactusWoodPlatformBlock";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }
                    }
                }

                public static class Variants
                {
                    public static class AntlionMandible_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/AntlionMandible_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class AntlionMandible_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/AntlionMandible_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class AshGrassSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/AshGrassSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class AshGrassSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/AshGrassSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class BlinkrootSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/BlinkrootSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class BlinkrootSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/BlinkrootSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Bone_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Bone_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Bone_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Bone_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Bone_3
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Bone_3";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CorruptSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CorruptSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CorruptSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CorruptSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CrimsonSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CrimsonSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CrimsonSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CrimsonSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CrystalShard_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CrystalShard_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CrystalShard_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CrystalShard_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CursedFlame_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CursedFlame_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CursedFlame_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/CursedFlame_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class DaybloomSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/DaybloomSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class DaybloomSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/DaybloomSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class DeathweedSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/DeathweedSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class DeathweedSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/DeathweedSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Ectoplasm_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Ectoplasm_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Ectoplasm_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Ectoplasm_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Ectoplasm_3
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Ectoplasm_3";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Feather_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Feather_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Feather_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Feather_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Feather_3
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Feather_3";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FireblossomSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FireblossomSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FireblossomSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FireblossomSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentNebula_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentNebula_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentNebula_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentNebula_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentSolar_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentSolar_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentSolar_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentSolar_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentStardust_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentStardust_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentStardust_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentStardust_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentVortex_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentVortex_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class FragmentVortex_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/FragmentVortex_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class GrassSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/GrassSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class GrassSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/GrassSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class HallowedSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/HallowedSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class HallowedSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/HallowedSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Ichor_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Ichor_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Ichor_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Ichor_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class JungleGrassSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/JungleGrassSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class JungleGrassSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/JungleGrassSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class JungleSpores_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/JungleSpores_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class JungleSpores_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/JungleSpores_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Lens_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Lens_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Lens_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Lens_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Lens_CataractEye
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Lens_CataractEye";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Lens_CataractEye_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Lens_CataractEye_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Lens_CataractEye_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Lens_CataractEye_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class MoonglowSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/MoonglowSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class MoonglowSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/MoonglowSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class MushroomGrassSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/MushroomGrassSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class MushroomGrassSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/MushroomGrassSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RottenChunk_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/RottenChunk_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class RottenChunk_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/RottenChunk_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class ShiverthornSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/ShiverthornSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class ShiverthornSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/ShiverthornSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_BigMossHornet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_BigMossHornet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_GiantMossHornet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_GiantMossHornet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_HornetHoney
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_HornetHoney";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_HornetLeafy
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_HornetLeafy";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_HornetSpikey
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_HornetSpikey";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_HornetStingy
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_HornetStingy";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_LittleMossHornet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_LittleMossHornet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_MossHornet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_MossHornet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_SpikedJungleSlime
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_SpikedJungleSlime";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Stinger_TinyMossHornet
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Stinger_TinyMossHornet";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class TatteredCloth_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/TatteredCloth_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class TatteredCloth_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/TatteredCloth_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Vertebrae_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Vertebrae_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Vertebrae_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Vertebrae_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Vine_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Vine_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class Vine_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/Vine_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class WaterleafSeeds_1
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/WaterleafSeeds_1";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class WaterleafSeeds_2
                    {
                        public const string KEY = "Nightshade/Assets/Images/Items/Variants/WaterleafSeeds_2";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }
            }

            public static class Tiles
            {
                public static class Furniture
                {
                    public static class CoconutChestTile
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Furniture/CoconutChestTile";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CoconutChestTile_Highlight
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Furniture/CoconutChestTile_Highlight";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }

                public static class Misc
                {
                    public static class CactusWood
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/CactusWood";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingCactus
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/LivingCactus";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingCactusPot
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/LivingCactusPot";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingCactusWood
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/LivingCactusWood";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingPalmLeaf
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/LivingPalmLeaf";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class LivingPalmWood
                    {
                        public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/LivingPalmWood";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }

                    public static class CactusWoodFurniture
                    {
                        public static class CactusWoodPlatform
                        {
                            public const string KEY = "Nightshade/Assets/Images/Tiles/Misc/CactusWoodFurniture/CactusWoodPlatform";

                            public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                            private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                        }
                    }
                }
            }

            public static class Walls
            {
                public static class Misc
                {
                    public static class LivingCactusWoodWall
                    {
                        public const string KEY = "Nightshade/Assets/Images/Walls/Misc/LivingCactusWoodWall";

                        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
                    }
                }
            }
        }

        public static class Music
        {
            public static class JUNO_6
            {
                // TODO: JUNO-6
            }

            public static class NIGHTSHADE___track_0
            {
                // TODO: NIGHTSHADE - track 0
            }
        }

        public static class Shaders
        {
            public static class Misc
            {
                public static class BasicPixelizationShader
                {
                    public sealed class Parameters : IShaderParameters
                    {
                        public float uPixel { get; set; }

                        public Microsoft.Xna.Framework.Vector2 uSize { get; set; }

                        public Microsoft.Xna.Framework.Graphics.Texture2D? uImage0 { get; set; }

                        public void Apply(Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)
                        {
                            parameters["uPixel"]?.SetValue(uPixel);
                            parameters["uSize"]?.SetValue(uSize);
                            parameters["uImage0"]?.SetValue(uImage0);
                        }
                    }

                    public const string KEY = "Nightshade/Assets/Shaders/Misc/BasicPixelizationShader";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Effect>(KEY));

                    public static WrapperShaderData<Parameters> CreateStripShader()
                    {
                        return new WrapperShaderData<Parameters>(Asset, "StripShader");
                    }
                }

                public static class VanillaVertexStripShader
                {
                    public sealed class Parameters : IShaderParameters
                    {
                        public float uPixel { get; set; }

                        public float uColorResolution { get; set; }

                        public Microsoft.Xna.Framework.Vector2 uSize { get; set; }

                        public Microsoft.Xna.Framework.Graphics.Texture2D? uImage0 { get; set; }

                        public void Apply(Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)
                        {
                            parameters["uPixel"]?.SetValue(uPixel);
                            parameters["uColorResolution"]?.SetValue(uColorResolution);
                            parameters["uSize"]?.SetValue(uSize);
                            parameters["uImage0"]?.SetValue(uImage0);
                        }
                    }

                    public const string KEY = "Nightshade/Assets/Shaders/Misc/VanillaVertexStripShader";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Effect>(KEY));

                    public static WrapperShaderData<Parameters> CreateStripShader()
                    {
                        return new WrapperShaderData<Parameters>(Asset, "StripShader");
                    }
                }
            }

            public static class UI
            {
                public static class CoolFlowerShader
                {
                    public sealed class Parameters : IShaderParameters
                    {
                        public Microsoft.Xna.Framework.Graphics.Texture2D? uImage0 { get; set; }

                        public Microsoft.Xna.Framework.Vector4 uSource { get; set; }

                        public float uTime { get; set; }

                        public float uPixel { get; set; }

                        public void Apply(Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)
                        {
                            parameters["uImage0"]?.SetValue(uImage0);
                            parameters["uSource"]?.SetValue(uSource);
                            parameters["uTime"]?.SetValue(Terraria.Main.GlobalTimeWrappedHourly);
                            parameters["uPixel"]?.SetValue(uPixel);
                        }
                    }

                    public const string KEY = "Nightshade/Assets/Shaders/UI/CoolFlowerShader";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Effect>(KEY));

                    public static WrapperShaderData<Parameters> CreateFlowerShader()
                    {
                        return new WrapperShaderData<Parameters>(Asset, "FlowerShader");
                    }
                }

                public static class ModPanelShader
                {
                    public sealed class Parameters : IShaderParameters
                    {
                        public Microsoft.Xna.Framework.Graphics.Texture2D? uImage0 { get; set; }

                        public float uTime { get; set; }

                        public Microsoft.Xna.Framework.Vector4 uSource { get; set; }

                        public float uHoverIntensity { get; set; }

                        public float uPixel { get; set; }

                        public float uColorResolution { get; set; }

                        public float uGrayness { get; set; }

                        public Microsoft.Xna.Framework.Vector3 uInColor { get; set; }

                        public float uSpeed { get; set; }

                        public void Apply(Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)
                        {
                            parameters["uImage0"]?.SetValue(uImage0);
                            parameters["uTime"]?.SetValue(Terraria.Main.GlobalTimeWrappedHourly);
                            parameters["uSource"]?.SetValue(uSource);
                            parameters["uHoverIntensity"]?.SetValue(uHoverIntensity);
                            parameters["uPixel"]?.SetValue(uPixel);
                            parameters["uColorResolution"]?.SetValue(uColorResolution);
                            parameters["uGrayness"]?.SetValue(uGrayness);
                            parameters["uInColor"]?.SetValue(uInColor);
                            parameters["uSpeed"]?.SetValue(uSpeed);
                        }
                    }

                    public const string KEY = "Nightshade/Assets/Shaders/UI/ModPanelShader";

                    public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect> Asset => lazy.Value;

                    private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Effect>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Effect>(KEY));

                    public static WrapperShaderData<Parameters> CreatePanelShader()
                    {
                        return new WrapperShaderData<Parameters>(Asset, "PanelShader");
                    }
                }
            }
        }
    }

    public static class Content
    {
        public static class Dusts
        {
            public static class ItemTextureDust
            {
                public const string KEY = "Nightshade/Content/Dusts/ItemTextureDust";

                public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

                private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
            }
        }
    }
}