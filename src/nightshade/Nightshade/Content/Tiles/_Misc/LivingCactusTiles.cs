using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Content.Items;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Tiles;

public sealed class LivingCactus : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingCactus.KEY;

    public override void SetStaticDefaults()
    {
        TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
        TileID.Sets.CanBeClearedDuringOreRunner[Type] = false;

        Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;

		TileID.Sets.ChecksForMerge[Type] = true;
        Main.tileMerge[Type][ModContent.TileType<LivingCactusWood>()] = true;
        Main.tileMerge[Type][TileID.Sand] = true;
        Main.tileMerge[TileID.Sand][Type] = true;
        Main.tileMerge[Type][TileID.Sandstone] = true;
        Main.tileMerge[TileID.Sandstone][Type] = true;
        Main.tileMerge[Type][TileID.HardenedSand] = true;
        Main.tileMerge[TileID.HardenedSand][Type] = true;
        Main.tileMerge[Type][TileID.Dirt] = true;
        Main.tileMerge[TileID.Dirt][Type] = true;
        Main.tileMerge[Type][TileID.Grass] = true;
        Main.tileMerge[TileID.Grass][Type] = true;

        DustType = DustID.OasisCactus;
        HitSound = SoundID.Dig;

		RegisterItemDrop(ItemID.Cactus);

		AddMapEntry(new Color(121, 158, 29));
    }

	public override IEnumerable<Item> GetItemDrops(int i, int j) => [new Item(ItemID.Cactus)];

	public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
    {
        base.ModifyFrameMerge(i, j, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

        WorldGen.TileMergeAttempt(-2, ModContent.TileType<LivingCactusWood>(), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
    }
}

public sealed class LivingCactusWood : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingCactusWood.KEY;

    public override void SetStaticDefaults()
    {
        TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
        TileID.Sets.CanBeClearedDuringOreRunner[Type] = false;

        Main.tileSolid[Type] = true;
		Main.tileBrick[Type] = true;
		Main.tileBlockLight[Type] = true;

		TileID.Sets.ChecksForMerge[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<LivingCactus>()] = true;
        //Main.tileMerge[Type][TileID.Sand] = true;
        //Main.tileMerge[TileID.Sand][Type] = true;
        //Main.tileMerge[Type][TileID.Sandstone] = true;
        //Main.tileMerge[TileID.Sandstone][Type] = true;
        //Main.tileMerge[Type][TileID.HardenedSand] = true;
        //Main.tileMerge[TileID.HardenedSand][Type] = true;
        //Main.tileMerge[Type][TileID.Dirt] = true;
        //Main.tileMerge[TileID.Dirt][Type] = true;
        //Main.tileMerge[Type][TileID.Grass] = true;
        //Main.tileMerge[TileID.Grass][Type] = true;

        DustType = DustID.PalmWood;
        HitSound = SoundID.Dig;

		RegisterItemDrop(ModContent.ItemType<CactusWoodBlock>());

		AddMapEntry(new Color(207, 167, 82));
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j) => [new Item(ModContent.ItemType<CactusWoodBlock>())];
}