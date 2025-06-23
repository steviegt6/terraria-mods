using Nightshade.Content.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

public sealed class CactusWoodPlatformBlock : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.CactusWoodFurniture.CactusWoodPlatformBlock.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.DefaultToPlaceableTile(ModContent.TileType<CactusWoodPlatform>());

		Item.rare = ItemRarityID.Green;
	}
}