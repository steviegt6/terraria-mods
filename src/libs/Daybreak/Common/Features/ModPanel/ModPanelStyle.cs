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
///     <br />
///     If you are using an assembly publicizer, you may instead extend
///     <see cref="ModPanelStyleExt"/>, which lets you directly interface with
///     the <see cref="UIModItem"/> instead of the generic <see cref="UIPanel"/>
///     instance.
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
    public virtual bool PreInitialize(UIPanel element)
    {
        return true;
    }

    /// <summary>
    ///     Invoked after <see cref="UIModItem.OnInitialize"/> is called
    ///     regardless of what <see cref="PreInitialize"/> returns.
    /// </summary>
    public virtual void PostInitialize(UIPanel element) { }

    /// <summary>
    ///     If <see cref="PreInitialize"/> returns <see langword="true"/>, this
    ///     method is invoked to modify the mod icon during initialization.
    ///     <br />
    ///     To remove the icon and shift relevant elements to the left, return
    ///     <see langword="null"/>
    /// </summary>
    public virtual UIImage? ModifyModIcon(UIPanel element, UIImage modIcon, ref int modIconAdjust)
    {
        return modIcon;
    }

    /// <summary>
    ///     If <see cref="PreInitialize"/> returns <see langword="true"/>, this
    ///     method is invoked to modify the mod name during initialization.
    /// </summary>
    public virtual UIText ModifyModName(UIPanel element, UIText modName)
    {
        return modName;
    }

    /// <summary>
    ///     Invoked before hover colors are set.
    /// </summary>
    public virtual bool PreSetHoverColors(UIPanel element, bool hovered)
    {
        return true;
    }

    /// <summary>
    ///     Invoked after hover colors are set.
    /// </summary>
    public virtual void PostSetHoverColors(UIPanel element, bool hovered) { }

    /// <summary>
    ///     Invoked before the element is drawn.
    /// </summary>
    public virtual bool PreDraw(UIPanel element, SpriteBatch sb)
    {
        return true;
    }

    /// <summary>
    ///     Invoked after the element is drawn.
    /// </summary>
    public virtual void PostDraw(UIPanel element, SpriteBatch sb) { }

    /// <summary>
    ///     Invoked specifically before the panel is drawn, assuming
    ///     <see cref="PreDraw"/> returned <see langword="true"/>.
    /// </summary>
    public virtual bool PreDrawPanel(UIPanel element, SpriteBatch sb)
    {
        return true;
    }

    /// <summary>
    ///     Invoked specifically after the panel is drawn, assuming
    ///     <see cref="PreDraw"/> returned <see langword="true"/>.
    /// </summary>
    public virtual void PostDrawPanel(UIPanel element, SpriteBatch sb) { }

    /// <summary>
    ///     Modifies the "Enabled"/"Disabled" button text.
    /// </summary>
    public virtual Color ModifyEnabledTextColor(bool enabled, Color color)
    {
        return color;
    }
}