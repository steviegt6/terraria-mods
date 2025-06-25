using Microsoft.Xna.Framework;
using Nightshade.Common.Utilities;
using Nightshade.Content.Tiles;
using Nightshade.Content.Tiles._Misc;
using Nightshade.Content.Tiles.Furniture;
using System;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

public sealed class LivingPalmBiome : MicroBiome
{
	private static ushort WoodType => (ushort)ModContent.TileType<LivingPalmWood>();

	private static ushort LeafType => (ushort)ModContent.TileType<LivingPalmLeaf>();

	private static ushort WallType => (ushort)WallID.LivingWoodUnsafe;

	private static ushort CoconutType => (ushort)ModContent.TileType<HangingCoconut>();

	public float StartCurl { get; set; }

	public float CurlStrength { get; set; }

	public override bool Place(Point origin, StructureMap structures)
	{
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Rectangle(200, 200),
			new Conditions.IsTile(TileID.Sand), new NightshadeGenUtil.Conditions.IsSolidSurface()), out origin))
			return false;

		Rectangle kindaArea = new Rectangle(origin.X - 40, origin.Y - 100, 80, 100);
		if (!structures?.CanPlace(kindaArea) ?? false)
			return false;

		PlaceStem(origin, out Point headPosition, out float curl);
		PlaceHead(curl * 0.2f, headPosition);

		int left = Math.Min(origin.X, headPosition.X) - 30;
		int top = headPosition.Y - 20;
		int height = Math.Abs(headPosition.Y - origin.Y) + 35;
		Rectangle area = new Rectangle(left, top, 60, height);
		ClearTrees(area);

		structures?.AddProtectedStructure(area, 4);

		return true;
	}

	private static void ClearTrees(Rectangle area)
	{
		for (int j = area.Top; j < area.Bottom; j++)
		{
			for (int i = area.Left; i < area.Right; i++)
			{
				if (!WorldGen.InWorld(i, j))
					continue;

				if (Main.tile[i, j].TileType == TileID.PalmTree)
					Main.tile[i, j].ClearTile();
			}
		}
	}

	private void PlaceStem(Point origin, out Point headPosition, out float finalCurl)
	{
		Vector2 curPos = origin.ToVector2();

		float length = WorldGen.genRand.Next(24, 30);
		const float halfSize = 3.6f;

		float curl = 0f;

		for (float k = 0; k <= length; k++)
		{
			int size = (int)(halfSize - MathF.Sqrt(k / length));

			if (Main.zenithWorld)
				size = (int)((Math.Sin(6 * k / length - 3.7) * 0.32 + 1) * (halfSize + 1));

			Vector2 nextPos = curPos + new Vector2(0, -1).RotatedBy(curl - StartCurl);

			for (int i = -size; i <= size; i++)
			{
				for (int j = -size; j <= size; j++)
				{
					int x = (int)(curPos.X + i);
					int y = (int)(curPos.Y + j);

					if (!WorldGen.InWorld(x, y))
						continue;

					float distance = MathF.Sqrt(i * i * 1.1f + j * j);
					if (distance < size)
					{
						if (Main.tile[x, y].WallType != WallType)
							Main.tile[x, y].ResetToType(WoodType);
					}
				}
			}

			curPos = nextPos;

			if (Main.zenithWorld)
				curl += (WorldGen.genRand.NextFloat(0.01f) + 0.12f) * CurlStrength / (2f - k / length);
			else
				curl += (WorldGen.genRand.NextFloat(0.01f) + 0.01f) * CurlStrength / (2f - k / length);
		}

		for (int i = 0; i < 3; i++)
			WorldUtils.Gen(origin, new ShapeRoot(MathHelper.Pi / 5f * (i - 1) + MathHelper.PiOver2, 17, 3, 1.4), new Actions.SetTile(WoodType));

		headPosition = curPos.ToPoint();
		finalCurl = StartCurl + curl;
	}

	private void PlaceHead(float rotation, Point origin)
	{
		const int chamberSize = 5;
		for (int i = -chamberSize; i <= chamberSize; i++)
		{
			for (int j = -chamberSize; j <= chamberSize; j++)
			{
				int modI = i >= 0 ? i + 1 : i;
				int modJ = j >= 0 ? j + 1 : j;
				int x = (int)(origin.X + i);
				int y = (int)(origin.Y + j);
				if (!WorldGen.InWorld(x, y))
					continue;

				float distance = MathF.Sqrt(modI * modI + modJ * modJ);
				if (distance < chamberSize)
				{
					if (distance >= chamberSize - 2.1f)
					{
						if (Main.tile[x, y].TileType != WoodType)
							Main.tile[x, y].ResetToType(WoodType);
					}
					else
					{
						Main.tile[x, y].ClearTile();
						Main.tile[x, y].WallType = WallType;
					}
				}
			}
		}

		PlaceLootChest(origin.X, origin.Y + chamberSize - 3);

		origin.Y -= chamberSize - 1;

		int leafCount = WorldGen.genRand.Next(5, 8);
		for (int i = 0; i < leafCount; i++)
		{
			Vector2 direction = new Vector2(0, -1f).RotatedBy((float)i / leafCount * MathHelper.TwoPi + WorldGen.genRand.NextFloat(-0.35f, 0.35f));
			direction.Y *= WorldGen.genRand.NextFloat(0.5f, 0.8f);
			direction.Y -= 0.2f;
			GenerateLeaf(origin, direction.RotatedBy(rotation), 5, WorldGen.genRand.NextFloat(18f, 22f), Math.Sign(rotation) * 0.02f);
		}

		PlaceCoconuts(origin.X, origin.Y + 4, 10);
	}

	private static void PlaceCoconuts(int x, int y, int radius)
	{
		int cocoCount = WorldGen.genRand.Next(4);
		for (int i = x - radius; i < x + radius; i++)
		{
			for (int j = y - radius; j < y + radius; j++)
			{
				int modX = i - x;
				int modY = j - y;
				double dist = Math.Sqrt(modX * modX + modY * modY);
				if (!WorldGen.InWorld(i, j) || !WorldGen.InWorld(i, j - 1) || dist > radius)
					continue;

				if (Main.tile[i, j].WallType == WallID.None && Main.tile[i, j - 1].TileType == LeafType && cocoCount <= 0)
				{
					cocoCount = WorldGen.genRand.Next(2, 5);
					WorldGen.PlaceTile(i, j, CoconutType, true);
				}
				else
					cocoCount--;
			}
		}
	}

	private static void PlaceLootChest(int x, int y)
	{
		Main.tileShine[(ushort)ModContent.TileType<CoconutChestTile>()] = 1200;
		int index = WorldGen.PlaceChest(x - 1, y - 1, type: (ushort)ModContent.TileType<CoconutChestTile>());
		if (index < 0)
			return;
	}

	private static void GenerateLeaf(Point origin, Vector2 direction, float halfSize, float length, float curl)
	{
		Vector2 curPos = origin.ToVector2();

		for (float k = 0; k <= length; k++)
		{
			int size = (int)(MathF.Sin(k / length * MathHelper.Pi) * halfSize * (1f - k / length * 0.5f) + k / length + 0.2);

			Vector2 nextPos = curPos + direction;
			float angle = curPos.AngleTo(nextPos);

			for (int i = -size; i <= size; i++)
			{
				for (int j = -size; j < size; j++)
				{
					int x = (int)(curPos.X + i);
					int y = (int)(curPos.Y + j);
					if (!WorldGen.InWorld(x, y))
						continue;

					float distance = MathF.Sqrt(i * i + j * j);
					if (distance <= size || size < 2)
					{
						if (Main.tile[x, y].TileType != WoodType && Main.tile[x, y].WallType != WallType)
							Main.tile[x, y].ResetToType(LeafType);
					}
				}
			}

			curPos = nextPos;
			direction += new Vector2(0, curl).RotatedBy(angle);
			direction += new Vector2(0, WorldGen.genRand.NextFloat(0.03f, 0.05f));
		}
	}
}
