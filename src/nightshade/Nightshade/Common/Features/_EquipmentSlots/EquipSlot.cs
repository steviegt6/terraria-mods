using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal abstract class EquipSlot : ModType
{
    public virtual bool CanBeToggled => false;

    public virtual bool IsEffectHidden => false;

    protected sealed override void Register()
    {
        EquipSlotLoader.SLOTS.Add(this);
    }

    protected sealed override void InitTemplateInstance()
    {
        base.InitTemplateInstance();
    }

    public abstract ref Item GetItem(bool dye);

    public abstract int GetContext();

    public virtual void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered) { }

    public virtual void DrawToggle(string? hoverText, Texture2D toggleButton, Rectangle toggleRect) { }
}