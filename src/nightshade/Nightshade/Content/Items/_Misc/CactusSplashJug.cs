using Microsoft.Xna.Framework;

using Nightshade.Content.Projectiles;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

public class CactusSplashJug : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.CactusSplashJug.KEY;

	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 24;
		Item.useAnimation = Item.useTime = 24;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTurn = true;
		Item.maxStack = Item.CommonMaxStack;
		Item.consumable = true;
		Item.UseSound = SoundID.Item1 with { Pitch = 0.9f, PitchVariance = 0.1f };
		Item.noUseGraphic = true;

		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(silver: 20);

		Item.shoot = ModContent.ProjectileType<CactusSplashJugThrown>();
		Item.shootSpeed = 12f;
	}

	public override bool AltFunctionUse(Player player) => true;

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Projectile.NewProjectileDirect(source, position, velocity + player.velocity * 0.33f, type, damage, knockback, player.whoAmI, player.altFunctionUse == 2 ? 1 : 0);
		return false;
	}

	public override void AddRecipes()
	{
		CreateRecipe(3)
			.AddIngredient(ItemID.HealingPotion, 3)
			.AddIngredient(ItemID.Cactus, 10)
			.AddIngredient(ItemID.Waterleaf, 3)
			.AddIngredient(ItemID.FossilOre)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
