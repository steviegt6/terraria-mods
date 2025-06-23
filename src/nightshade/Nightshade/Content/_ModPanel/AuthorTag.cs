using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.Localization;
using Terraria.ModLoader;

namespace Nightshade.Content;

/// <summary>
///     An author tag to be automatically loaded and displayed in the Mods List
///     UI.
/// </summary>
public abstract class AuthorTag : ModTexturedType, ILocalizedModType
{
    public string LocalizationCategory => "AuthorTags";

    public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

    protected sealed override void Register()
    {
        ModTypeLookup<AuthorTag>.Register(this);
    }

    public sealed override void SetupContent()
    {
        base.SetupContent();

        SetStaticDefaults();
    }

    public virtual void DrawIcon(SpriteBatch sb, Vector2 position)
    {
        if (!ModContent.RequestIfExists<Texture2D>(Texture, out var icon))
        {
            return;
        }

        sb.Draw(
            icon.Value,
            new Rectangle((int)position.X, (int)position.Y - 2, 26, 26),
            Color.White
        );
    }
}