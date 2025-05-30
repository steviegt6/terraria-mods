using System;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Content.Features.Cursor;

public sealed class CursorSlot : ModAccessorySlot
{
    public override bool DrawVanitySlot => false;

    public override string FunctionalTexture => Assets.Images.UI.Cursor_Slot.KEY;

    public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
    {
        return context switch
        {
            AccessorySlotType.DyeSlot => checkItem.dye > 0,
            AccessorySlotType.FunctionalSlot => checkItem.ModItem is ICursorStyle,
            AccessorySlotType.VanitySlot => false,
            _ => throw new ArgumentOutOfRangeException(nameof(context)),
        };
    }
}