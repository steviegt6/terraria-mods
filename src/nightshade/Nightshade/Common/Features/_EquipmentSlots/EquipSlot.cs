using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Nightshade.Common.Features;

internal abstract class EquipSlot
{
    public virtual bool CanBeToggled => false;

    public virtual bool IsEffectHidden => false;

    public abstract ref Item GetItem(bool dye);

    public abstract CustomItemSlotContext GetContext();

    public virtual void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered) { }

    public virtual void DrawToggle(string? hoverText, Texture2D toggleButton, Rectangle toggleRect) { }
}