using Nightshade.Content.Tiles;
using Nightshade.Content.Walls;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

public sealed class LivingCactusWand : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.LivingCactusWand.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		(Item.width, Item.height) = (28, 30);

		Item.value = Item.buyPrice(silver: 1);
		Item.rare = ItemRarityID.Green;

		Item.tileWand = ItemID.Cactus;
		Item.createTile = ModContent.TileType<LivingCactus>();

		Item.autoReuse = true;
		Item.useAnimation = 20;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Cactus, 15)
			.AddIngredient<CactusWoodBlock>(15)
			.AddTile(TileID.LivingLoom)
			.Register();
	}
}

public sealed class LivingCactusWoodWand : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.LivingCactusWoodWand.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		(Item.width, Item.height) = (28, 30);

		Item.value = Item.buyPrice(silver: 1);
		Item.rare = ItemRarityID.Green;

		Item.tileWand = ModContent.ItemType<CactusWoodBlock>();
		Item.createTile = ModContent.TileType<LivingCactusWood>();

		Item.autoReuse = true;
		Item.useAnimation = 20;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient<CactusWoodBlock>(30)
			.AddTile(TileID.LivingLoom)
			.Register();
	}
}

public sealed class LivingCactusWoodWallBlock : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.LivingCactusWoodWallBlock.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.DefaultToPlaceableWall(ModContent.WallType<LivingCactusWoodWall>());
	}
}