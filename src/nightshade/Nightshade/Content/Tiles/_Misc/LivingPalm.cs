using Nightshade.Content.Items;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Tiles;

internal sealed class LivingPalmWood : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingPalmWood.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;

        RegisterItemDrop(ModContent.ItemType<LivingPalmWoodBlock>());

        TileID.Sets.ChecksForMerge[Type] = true;
        Main.tileMerge[Type][ModContent.TileType<LivingPalmLeaf>()] = true;
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

        DustType = DustID.PalmWood;
        HitSound = SoundID.Grass;
    }

    public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
    {
        base.ModifyFrameMerge(i, j, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

        WorldGen.TileMergeAttempt(-2, ModContent.TileType<LivingPalmLeaf>(), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
    }
}

internal sealed class LivingPalmLeaf : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingPalmLeaf.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.tileSolid[Type] = true;

        RegisterItemDrop(ModContent.ItemType<LivingPalmWoodBlock>());

        TileID.Sets.ChecksForMerge[Type] = true;
        Main.tileMerge[Type][ModContent.TileType<LivingPalmWood>()] = true;
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
        Main.tileBlockLight[Type] = true;

        DustType = DustID.PalmWood;
        HitSound = SoundID.Grass;
    }
}