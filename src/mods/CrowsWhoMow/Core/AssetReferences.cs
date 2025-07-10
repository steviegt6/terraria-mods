#pragma warning disable CS8981

namespace Tomat.TML.Mod.CrowsWhoMow.Core;

// ReSharper disable InconsistentNaming
internal static class AssetReferences
{
    public static class CrowThatMows
    {
        public const string KEY = "CrowsWhoMow/CrowThatMows";

        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
    }

    public static class icon
    {
        public const string KEY = "CrowsWhoMow/icon";

        public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;

        private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));
    }
}