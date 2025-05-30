using Daybreak.Common.Features.Hooks;

using Nightshade.Common.Features;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items._Cursors;

internal abstract class HairDyeCursor(int hairDye) : ModItem
{
    private readonly int hairDye = hairDye;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        VanityCursorSets.IsVanityCursor[Type] = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.CloneDefaults(hairDye);

        Item.value *= 5;
        Item.consumable = false;

        Item.vanity = true;
        Item.hasVanityEffects = true;

        Item.maxStack = 1;
    }

    [OnLoad]
    private static void UpdateVisibleAccessories()
    {
        On_Player.UpdateVisibleAccessory += (orig, self, slot, item, modded) =>
        {
            orig(self, slot, item, modded);

            if (item.ModItem is HairDyeCursor hairDyeCursor)
            {
                self.GetModPlayer<VanityCursorPlayer>().HairDye = hairDyeCursor.hairDye;
            }
        };
    }
}

internal sealed class LifeCursor() : HairDyeCursor(ItemID.LifeHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.LifeCursor.KEY;
}

internal sealed class ManaCursor() : HairDyeCursor(ItemID.ManaHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.ManaCursor.KEY;
}

internal sealed class DepthCursor() : HairDyeCursor(ItemID.DepthHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.DepthCursor.KEY;
}

internal sealed class MoneyCursor() : HairDyeCursor(ItemID.MoneyHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.MoneyCursor.KEY;
}

internal sealed class TimeCursor() : HairDyeCursor(ItemID.TimeHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.TimeCursor.KEY;
}

internal sealed class BiomeCursor() : HairDyeCursor(ItemID.BiomeHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.BiomeCursor.KEY;
}

internal sealed class PartyCursor() : HairDyeCursor(ItemID.PartyHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.PartyCursor.KEY;
}

internal sealed class SpeedCursor() : HairDyeCursor(ItemID.SpeedHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.SpeedCursor.KEY;
}