using Nightshade.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

internal sealed class LivingCactusWoodWand : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusWoodWand.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (28, 30);

        Item.value = 100;
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