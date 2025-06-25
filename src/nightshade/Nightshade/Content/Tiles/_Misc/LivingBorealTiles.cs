using Microsoft.Xna.Framework;

using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Tiles;

public sealed class LivingBorealWood : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingBorealWood.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

		Main.tileSolid[Type] = true;
		Main.tileBrick[Type] = true;
		Main.tileBlockLight[Type] = true;

		TileID.Sets.ChecksForMerge[Type] = true;
		Main.tileBlockLight[Type] = true;

		RegisterItemDrop(ItemID.BorealWood);

		DustType = DustID.BorealWood;

        AddMapEntry(new Color(107, 86, 71));
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j) => [new Item(ItemID.BorealWood)];

	public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
    {
        base.ModifyFrameMerge(i, j, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

        WorldGen.TileMergeAttempt(-2, ModContent.TileType<LivingBorealLeaf>(), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
    }
}

public sealed class LivingBorealLeaf : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingBorealLeaf.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;

		TileID.Sets.ChecksForMerge[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<LivingBorealWood>()] = true;
		Main.tileMerge[Type][TileID.SnowBlock] = true;
		Main.tileMerge[TileID.SnowBlock][Type] = true;

		DustType = DustID.Grass;
        HitSound = SoundID.Grass;

		AddMapEntry(new Color(24, 137, 94));
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j) => [];

	public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
	{
		base.ModifyFrameMerge(i, j, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

		//WorldGen.TileMergeAttempt(-2, TileID.SnowBlock, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
	}
}