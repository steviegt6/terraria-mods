using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Nightshade.Content.Tiles;

public sealed class CactusWoodPlatform : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.CactusWoodFurniture.CactusWoodPlatform.KEY;

    public override void SetStaticDefaults()
    {
		Main.tileLighted[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileSolid[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileTable[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.Platforms[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		TileObjectData.newTile.CoordinateHeights = [16];
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.StyleMultiplier = 27;
		TileObjectData.newTile.StyleWrapLimit = 27;
		TileObjectData.newTile.UsesCustomCanPlace = false;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.addTile(Type);

		AdjTiles = [TileID.Platforms];
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

		DustType = DustID.PalmWood;
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(207, 167, 82));
	}

	public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;

	public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}