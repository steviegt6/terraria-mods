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
public abstract class ModPanelStyleExt : ModPanelStyle
{
    public override bool PreInitialize(UIPanel element)
    {
        return PreInitialize((UIModItem)element);
    }

    public override void PostInitialize(UIPanel element)
    {
        PostInitialize((UIModItem)element);
    }

    public override UIImage? ModifyModIcon(UIPanel element, UIImage modIcon, ref int modIconAdjust)
    {
        return ModifyModIcon((UIModItem)element, modIcon, ref modIconAdjust);
    }

    public override UIText ModifyModName(UIPanel element, UIText modName)
    {
        return ModifyModName((UIModItem)element, modName);
    }

    public override bool PreSetHoverColors(UIPanel element, bool hovered)
    {
        return PreSetHoverColors((UIModItem)element, hovered);
    }

    public override void PostSetHoverColors(UIPanel element, bool hovered)
    {
        PostSetHoverColors((UIModItem)element, hovered);
    }

    public override bool PreDraw(UIPanel element, SpriteBatch sb)
    {
        return PreDraw((UIModItem)element, sb);
    }

    public override void PostDraw(UIPanel element, SpriteBatch sb)
    {
        PostDraw((UIModItem)element, sb);
    }

    public override bool PreDrawPanel(UIPanel element, SpriteBatch sb)
    {
        return PreDrawPanel((UIModItem)element, sb);
    }

    public override void PostDrawPanel(UIPanel element, SpriteBatch sb)
    {
        PostDrawPanel((UIModItem)element, sb);
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual bool PreInitialize(UIModItem element)
    {
        return true;
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual void PostInitialize(UIModItem element) { }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual UIImage? ModifyModIcon(UIModItem element, UIImage modIcon, ref int modIconAdjust)
    {
        return modIcon;
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual UIText ModifyModName(UIModItem element, UIText modName)
    {
        return modName;
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual bool PreSetHoverColors(UIModItem element, bool hovered)
    {
        return true;
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual void PostSetHoverColors(UIModItem element, bool hovered) { }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual bool PreDraw(UIModItem element, SpriteBatch sb)
    {
        return true;
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual void PostDraw(UIModItem element, SpriteBatch sb) { }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual bool PreDrawPanel(UIModItem element, SpriteBatch sb)
    {
        return true;
    }

    /// <inheritdoc cref="ModPanelStyle.PreInitialize"/>
    public virtual void PostDrawPanel(UIModItem element, SpriteBatch sb) { }
}