using Nightshade.Content.Tiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

public sealed class LivingPalmWoodWand : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodWand.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		(Item.width, Item.height) = (28, 30);

		Item.value = Item.buyPrice(silver: 1);
		Item.rare = ItemRarityID.Green;

		Item.tileWand = ItemID.PalmWood;
		Item.createTile = ModContent.TileType<LivingPalmWood>();

		Item.autoReuse = true;
		Item.useAnimation = 20;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
	}
}

public sealed class LivingPalmLeafWand : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmLeafWand.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		(Item.width, Item.height) = (28, 30);

		Item.value = Item.buyPrice(silver: 1);
		Item.rare = ItemRarityID.Green;

		Item.tileWand = ItemID.PalmWood;
		Item.createTile = ModContent.TileType<LivingPalmLeaf>();

		Item.autoReuse = true;
		Item.useAnimation = 20;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
	}
}