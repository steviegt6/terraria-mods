using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
internal abstract class EquipSlot : ModType
{
    protected sealed override void Register()
    {
        EquipSlotLoader.SLOTS.Add(this);
    }

    protected sealed override void InitTemplateInstance()
    {
        base.InitTemplateInstance();
    }

    public abstract ref Item GetItem(EquipSlotKind kind);

    public abstract int GetContext(EquipSlotKind kind);

    public virtual bool CanBeToggled(EquipSlotKind kind)
    {
        return false;
    }

    public virtual void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered, EquipSlotKind kind) { }

    public virtual void DrawToggle(string? hoverText, Texture2D toggleButton, Rectangle toggleRect, EquipSlotKind kind) { }
}