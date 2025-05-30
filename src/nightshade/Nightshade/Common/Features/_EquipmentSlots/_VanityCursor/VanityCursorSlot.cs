using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

internal sealed class VanityCursorSlot : EquipSlot
{
    private sealed class VanityCursorSlotContext : CustomItemSlot
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ItemSlot.canShareAt[Type] = true;
        }
    }

    public override ref Item GetItem(bool dye)
    {
        var vanityCursorPlayer = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
        return ref vanityCursorPlayer.Vanity[dye ? 2 : 0]!;
    }

    public override int GetContext()
    {
        return ModContent.GetInstance<VanityCursorSlotContext>().Type;
    }
}