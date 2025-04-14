using Terraria.ModLoader;
using Terraria.ID;

using Nightshade.Content.Tiles;

namespace Nightshade.Content.Items;

public class LivingCactusBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactus>());

        Item.value   = 100;
        Item.rare    = ItemRarityID.Green;
    }
}
public class LivingCactusWoodBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactusWood>());

        Item.value   = 100;
        Item.rare    = ItemRarityID.Green;
    }
}