using Nightshade.Content.Tiles;

using Terraria.ModLoader;

namespace Nightshade.Content.Items;

internal sealed class CoconutChest : ModItem
{
    public override string Texture => Assets.Images.Items.Furniture.CoconutChest.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<CoconutChestTile>());

        (Item.width, Item.height) = (32, 30);
        
        // TODO: value
    }
}