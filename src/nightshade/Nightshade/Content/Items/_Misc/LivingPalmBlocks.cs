using Nightshade.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

internal sealed class LivingPalmWoodBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingPalmWood>());

        Item.rare = ItemRarityID.Green;
    }
}

internal sealed class LivingPalmLeafBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmLeafBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingPalmLeaf>());

        Item.rare = ItemRarityID.Green;
    }
}