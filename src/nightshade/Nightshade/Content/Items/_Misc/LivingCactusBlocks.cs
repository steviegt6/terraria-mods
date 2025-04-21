using Nightshade.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

internal sealed class LivingCactusBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactus>());

        Item.value = 100;
        Item.rare = ItemRarityID.Green;
    }
}

internal sealed class LivingCactusWoodBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactusWood>());

        Item.value = 100;
        Item.rare = ItemRarityID.Green;
    }
}