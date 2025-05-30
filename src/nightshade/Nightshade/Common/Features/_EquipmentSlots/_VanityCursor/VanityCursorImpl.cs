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
    }

    private static bool CountVanityCursorsAsMiscEquipment(On_ItemSlot.orig_IsMiscEquipment orig, Item item)
    {
        if (!item.IsAir && VanityCursorSets.IsVanityCursor[item.type])
        {
            return true;
        }

        return orig(item);
    }

    private static void SelectEquipPageForVanityCursors(On_ItemSlot.orig_SelectEquipPage orig, Item item)
    {
        orig(item);

        if (!item.IsAir && VanityCursorSets.IsVanityCursor[item.type])
        {
            Main.EquipPage = (int)EquipmentPageId.Misc;
        }
    }
}