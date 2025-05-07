using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;

namespace Daybreak.Common.Features.ModPanel;

/// <summary>
///     Facilitates the customization of <see cref="UIModItem"/>s through
///     various hooks.
/// </summary>
/// <remarks>
///     This style may be applied to any mod, technically, so references to your
///     mod instance should be explicit and not assumed.
/// </remarks>
public abstract class ModPanelStyle
{
    /// <summary>
    ///     Optionally overrides the "ModInfo" texture.
    /// </summary>
    public virtual Asset<Texture2D>? ModInfoTexture => null;

    /// <summary>
    ///     Optionally overrides the "ModConfig" texture.
    /// </summary>
    public virtual Asset<Texture2D>? ModConfigTexture => null;

    /// <summary>
    ///     Optionally overrides the "InnerPanel" texture.
    /// </summary>
    public virtual Asset<Texture2D>? InnerPanelTexture => null;

    // I guess if someone was really crazy, they could do all the initialization
    // themselves.
    /// <summary>
    ///     Invoked before <see cref="UIModItem.OnInitialize"/> is called.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> to cancel regular initialization behavior,
    ///     <see langword="true"/> to enable regular execution.
    /// </returns>
    public virtual bool PreInitialize(UIModItem element)
    {
        return true;
    }

    /// <summary>
    ///     Invoked after <see cref="UIModItem.OnInitialize"/> is called
    ///     regardless of what <see cref="PreInitialize"/> returns.
    /// </summary>
    public virtual void PostInitialize(UIModItem element) { }

    /// <summary>
    ///     If <see cref="PreInitialize"/> returns <see langword="true"/>, this
    ///     method is invoked to modify the mod icon during initialization.
    /// </summary>
    public virtual UIImage ModifyModIcon(UIModItem element, UIImage modIcon)
    {
        return modIcon;
    }

    /// <summary>
    ///     If <see cref="PreInitialize"/> returns <see langword="true"/>, this
    ///     method is invoked to modify the mod name during initialization.
    /// </summary>
    public virtual UIText ModifyModName(UIModItem element, UIText modName)
    {
        return modName;
    }
    /// <summary>
    ///     Invoked before hover colors are set.
    /// </summary>

    public virtual bool PreSetHoverColors(UIModItem element, bool hovered)
    {
        return true;
    }

    /// <summary>
    ///     Invoked after hover colors are set.
    /// </summary>
    public virtual void PostSetHoverColors(UIModItem element, bool hovered) { }

    /// <summary>
    ///     Invoked before the element is drawn.
    /// </summary>
    public virtual bool PreDraw(UIModItem element, SpriteBatch sb)
    {
        return true;
    }

    /// <summary>
    ///     Invoked after the element is drawn.
    /// </summary>
    public virtual void PostDraw(UIModItem element, SpriteBatch sb) { }

    /// <summary>
    ///     Invoked specifically before the panel is drawn, assuming
    ///     <see cref="PreDraw"/> returned <see langword="true"/>.
    /// </summary>
    public virtual bool PreDrawPanel(UIModItem element, SpriteBatch sb)
    {
        return true;
    }

    /// <summary>
    ///     Invoked specifically after the panel is drawn, assuming
    ///     <see cref="PreDraw"/> returned <see langword="true"/>.
    /// </summary>
    public virtual void PostDrawPanel(UIModItem element, SpriteBatch sb) { }

    /// <summary>
    ///     Modifies the "Enabled"/"Disabled" button text.
    /// </summary>
    public virtual Color ModifyEnabledTextColor(bool enabled, Color color)
    {
        return color;
    }
}