using Terraria.ModLoader;
using Terraria.ID;

using Nightshade.Content.Tiles;

namespace Nightshade.Content.Items;

public class LivingPalmWoodBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<LivingPalmWood>());

        Item.value   = 100;
        Item.rare    = ItemRarityID.Green;
    }
}
public class LivingPalmLeafBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmLeafBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<LivingPalmLeaf>());

        Item.value   = 100;
        Item.rare    = ItemRarityID.Green;
    }
}