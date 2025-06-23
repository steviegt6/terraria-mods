using Nightshade.Content.Tiles._Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items._Misc;

public sealed class HangingCoconutItem : ModItem
{
	public override string Texture => Assets.Images.Tiles.Misc.FallingCoconutProjectile.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.DefaultToPlaceableTile(ModContent.TileType<HangingCoconut>());
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(silver: 20);
	}

	public override void AddRecipes()
	{
		base.AddRecipes();

		CreateRecipe()
			.AddIngredient(ItemID.Coconut)
			.AddCondition(Condition.InGraveyard)
			.Register();
	}
}
