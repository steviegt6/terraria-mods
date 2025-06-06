using MonoMod.Cil;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

internal sealed class VanityCursorImpl : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_ItemSlot.IsMiscEquipment += CountVanityCursorsAsMiscEquipment;
        On_ItemSlot.SelectEquipPage += SelectEquipPageForVanityCursors;

        IL_Main.MouseText_DrawItemTooltip_GetLinesInfo += CountOurSetsAsEquipable;
    }

    private static bool CountVanityCursorsAsMiscEquipment(On_ItemSlot.orig_IsMiscEquipment orig, Item item)
    {
        if (!item.IsAir && (VanityCursorSets.IsVanityCursor[item.type] || VanityCursorSets.IsCursorTrail[item.type]))
        {
            return true;
        }

        return orig(item);
    }

    private static void SelectEquipPageForVanityCursors(On_ItemSlot.orig_SelectEquipPage orig, Item item)
    {
        orig(item);

        if (!item.IsAir && (VanityCursorSets.IsVanityCursor[item.type] || VanityCursorSets.IsCursorTrail[item.type]))
        {
            Main.EquipPage = (int)EquipmentPageId.Misc;
        }
    }

    private static void CountOurSetsAsEquipable(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchLdfld<Item>(nameof(Item.accessory)));

        c.EmitLdarg0(); // item
        c.EmitDelegate(
            static (bool accessory, Item item) => accessory || VanityCursorSets.IsVanityCursor[item.type] || VanityCursorSets.IsCursorTrail[item.type]
        );
    }
}