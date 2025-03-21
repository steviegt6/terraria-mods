using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;
using ReLogic.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

using Tomat.TML.Mod.ChitterChatter.Common.Loading;

namespace Tomat.TML.Mod.ChitterChatter.Content.Features.TagHandlers;

internal sealed class ModIconTagHandler : ILoadableTagHandler<ModIconTagHandler>
{
    private readonly record struct ModCache(
        Asset<Texture2D>? Icon,
        string?           DisplayName
    );

    private static class ModIconCache
    {
        private static readonly Dictionary<string, ModCache> cache = [];

        public static ModCache GetModCache(string modName)
        {
            if (cache.TryGetValue(modName, out var modCache))
            {
                return modCache;
            }

            var icon = InnerGetModIcon(modName, out var displayName);
            return cache[modName] = new ModCache(icon, displayName);

            static Asset<Texture2D>? InnerGetModIcon(string modName, out string? displayName)
            {
                if (!ModLoader.TryGetMod(modName, out var mod))
                {
                    displayName = null;
                    return null;
                }

                displayName = mod.DisplayName;

                if (mod.FileExists("icon_small.rawimg"))
                {
                    return mod.Assets.Request<Texture2D>("icon_small", AssetRequestMode.ImmediateLoad);
                }

                if (mod.FileExists("icon.png"))
                {
                    return mod.Assets.Request<Texture2D>("icon", AssetRequestMode.ImmediateLoad);
                }

                return null;
            }
        }
    }

    private sealed class ModIconTextSnippet(string modName) : TextSnippet
    {
        private readonly ModCache modCache = ModIconCache.GetModCache(modName);

        public override bool UniqueDraw(
            bool        justCheckingString,
            out Vector2 size,
            SpriteBatch spriteBatch,
            Vector2     position = default,
            Color       color    = default,
            float       scale    = 1
        )
        {
            if (!justCheckingString && color != Color.Black && modCache.Icon is { } icon)
            {
                // spriteBatch.Draw(icon.Value, position, null, Color.White, 0f, Vector2.Zero, modCache.Scale * scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(icon.Value, new Rectangle((int)position.X, (int)position.Y - 2, (int)icon_size, (int)icon_size), Color.White);
            }

            size = new Vector2(icon_size);
            return true;
        }

        public override void OnHover()
        {
            base.OnHover();

            Main.instance.MouseText(modCache.DisplayName ?? modName);
        }

        public override float GetStringLength(DynamicSpriteFont font)
        {
            return icon_size;
        }

        public override Color GetVisibleColor()
        {
            return Color.White;
        }
    }

    public string[] TagNames { get; } = ["mi", "modicon"];

    private const float icon_size = 26f;

    TextSnippet ITagHandler.Parse(string text, Color baseColor, string? options)
    {
        return new ModIconTextSnippet(text);
    }
}