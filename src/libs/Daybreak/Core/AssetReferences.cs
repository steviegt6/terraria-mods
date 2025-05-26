#pragma warning disable CS8981

namespace Daybreak.Core;

// ReSharper disable InconsistentNaming

internal static partial class AssetReferences
{
    public static partial class icon
    {
        public const string KEY = "icon";

        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<global::Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));

    }

    public static partial class Assets
    {
        public static partial class Shaders
        {
            public static partial class UI
            {
                public static partial class ModPanelShader
                {
                    // TODO: ModPanelShader
                }

                public static partial class ModPanelShaderSampler
                {
                    // TODO: ModPanelShaderSampler
                }

                public static partial class PowerfulSunIcon
                {
                    // TODO: PowerfulSunIcon
                }
            }
        }
    }
}