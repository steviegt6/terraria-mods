using Nightshade.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

public sealed class CactusWoodBlock : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.CactusWoodBlock.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.DefaultToPlaceableTile(ModContent.TileType<CactusWood>());
	}
}