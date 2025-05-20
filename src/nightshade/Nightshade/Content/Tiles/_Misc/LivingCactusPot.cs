using Microsoft.Xna.Framework;

using Nightshade.Content.Gores;
using Nightshade.Content.Items;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Nightshade.Content.Tiles;

internal sealed class LivingCactusPot : AbstractPot
{
	public override string Texture => Assets.Images.Tiles.Misc.LivingCactusPot.KEY;

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();

		AddMapEntry(new Color(47, 79, 79), Language.GetText("MapObject.Pot")); // dark slate gray
		DustType = 29;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		base.KillMultiTile(i, j, frameX, frameY);

		if (Main.netMode == NetmodeID.Server)
		{
			return;
		}

		var goreAmt = Main.rand.Next(1, 2 + 1);
		for (var k = 0; k < goreAmt; k++)
		{
			for (var l = 1; l < 3; l++)
			{
				Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.NextVector2CircularEdge(3f, 3f), Mod.Find<ModGore>($"LivingCactusPotGore{l}").Type);
			}
		}
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		if (WorldGen.genRand.NextBool(45))
			return [new Item(ItemID.RegenerationPotion)];

		if (WorldGen.genRand.NextBool(45))
			return [new Item(ItemID.ThornsPotion)];

		switch (Main.rand.Next(6))
		{
			case 0: return [new Item(ItemID.WoodenArrow, WorldGen.genRand.Next(8, 20))];

			case 1: return [new Item(ItemID.Grenade, WorldGen.genRand.Next(8, 20))];

			case 2: return [new Item(ModContent.ItemType<CactusSplashJug>(), WorldGen.genRand.Next(5, 10))];

			default:
				int moneyAmount = WorldGen.genRand.Next(100, 301);

				// In vanilla, money dropped from pots has a lot of funny logic thats all embedded in a single function.
				// Tmod has no support for pot drop logic either.
				// So we will not do funny pot drop logic.

				return [
					new Item(ItemID.CopperCoin, moneyAmount % 100),
					new Item(ItemID.SilverCoin, (int)MathF.Floor(moneyAmount / 100))
					];
		}

	}
}

internal sealed class LivingCactusPotPlacer : ModItem
{
	public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodBlock.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactusPot>());

		Item.value = 100;
		Item.rare = ItemRarityID.Green;
	}
}

internal sealed class PotGoreLoader : ILoadable
{
	public void Load(Mod mod)
	{
		mod.AddContent(new GenericGore("LivingCactusPotGore1", Assets.Images.Gores.Misc.CactusPotGore1.KEY));
		mod.AddContent(new GenericGore("LivingCactusPotGore2", Assets.Images.Gores.Misc.CactusPotGore2.KEY));
	}

	public void Unload() { }
}