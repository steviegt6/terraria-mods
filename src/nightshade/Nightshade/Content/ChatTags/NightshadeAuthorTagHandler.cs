using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Features;

using ReLogic.Graphics;

using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Nightshade.Content.ChatTags;

internal sealed class NightshadeAuthorTagHandler : ILoadableTagHandler<NightshadeAuthorTagHandler>
{
    private sealed class Snippet(string authorName) : TextSnippet
    {
        public override bool UniqueDraw(
            bool        justCheckingString,
            out Vector2 size,
            SpriteBatch spriteBatch,
            Vector2     position = default,
            Color       color    = default,
            float       scale    = 1
        )
        {
            if (!justCheckingString && color is { R: > 0, G: > 0, B: > 0 })
            {
                var mod      = ModContent.GetInstance<ModImpl>();
                var iconName = $"Assets/Images/UI/ModIcon/{authorName}";
                if (mod.RequestAssetIfExists<Texture2D>(iconName, out var icon))
                {
                    spriteBatch.Draw(icon.Value, new Rectangle((int)position.X, (int)position.Y - 2, 26, 26), Color.White);
                }
            }

            size = new Vector2(26f);
            return true;
        }

        public override float GetStringLength(DynamicSpriteFont font)
        {
            return 26f;
        }

        public override Color GetVisibleColor()
        {
            return Color.White;
        }
    }

    public string[] TagNames { get; } = ["nsa"];

    TextSnippet ITagHandler.Parse(string text, Color baseColor, string? options)
    {
        return new Snippet(text);
    }
}