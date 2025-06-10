using Microsoft.Xna.Framework;
using Nightshade.Content.Tiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

public sealed class GenTreeCommand : ModCommand
{
	public override string Command => "gentree";

	public override CommandType Type => CommandType.Chat;

	public override void Action(CommandCaller caller, string input, string[] args)
	{
		LivingPalmBiome livingPalmTreeBiome = new LivingPalmBiome();
		Point origin = Main.MouseWorld.ToTileCoordinates();
		livingPalmTreeBiome.Place(origin, GenVars.structures);
		Rectangle structureRect = new Rectangle(origin.X - 22, origin.Y - 38, 44, 100);
		Dust.QuickBox(structureRect.TopLeft() * 16, structureRect.BottomRight() * 16, 100, Color.Red, null);
		for (int i = 0; i < 20; i++)
		{
			Dust d = Dust.NewDustPerfect(Main.MouseWorld, DustID.WaterCandle, new Vector2(3, 0).RotatedBy(i / 20f * MathHelper.TwoPi), Scale: 2f);
			d.noGravity = true;
		}
	}
}
public sealed class LivingPalmBiome : MicroBiome
{
	private static ushort WoodType => (ushort)ModContent.TileType<LivingPalmWood>();

	private static ushort LeafType => (ushort)ModContent.TileType<LivingPalmLeaf>();

	private static ushort WallType => (ushort)WallID.LivingWoodUnsafe;

	public override bool Place(Point origin, StructureMap structures)
	{
		PlaceStem(origin, out Point headPosition, out float curl);
		PlaceHead(curl * 0.2f, headPosition);

		return true;
	}

	private static void PlaceStem(Point origin, out Point headPosition, out float finalCurl)
	{
		headPosition = origin;

		Vector2 curPos = origin.ToVector2();

		float length = 30;
		float halfSize = 4f;

		float startCurl = WorldGen.genRand.NextFloat(-0.3f, 0.3f);
		float curlStrength = WorldGen.genRand.NextFloat(0.5f, 1f) * WorldGen.genRand.NextBool().ToDirectionInt();

		float curl = 0f;

		for (float k = 0; k <= length; k++)
		{
			int size = (int)(halfSize - MathF.Sqrt(k / length));

			Vector2 nextPos = curPos + new Vector2(0, -1).RotatedBy(curl - startCurl);
			float angle = curPos.AngleTo(nextPos);

			for (int i = -size; i <= size; i++)
			{
				for (int j = -size; j <= size; j++)
				{
					int x = (int)(curPos.X + i);
					int y = (int)(curPos.Y + j);

					if (!WorldGen.InWorld(x, y))
						continue;

					float distance = MathF.Sqrt(i * i + j * j);
					if (distance <= size)
					{
						if (distance >= size - 1.8)
						{
							if (Main.tile[x, y].WallType != WallType)
								Main.tile[x, y].ResetToType(WoodType);
						}
						else
							Main.tile[x, y].ClearEverything();

						if (distance <= size - 1)
							Main.tile[x, y].WallType = WallType;

						WorldGen.SquareTileFrame(x, y);
						WorldGen.SquareWallFrame(x, y);
					}
				}
			}

			curPos = nextPos;

			curl += 0.04f / (2f - k / length) * curlStrength;
		}

		headPosition = curPos.ToPoint();
		finalCurl = startCurl + curl;
	}

	private static void PlaceHead(float rotation, Point origin)
	{
		const int chamberSize = 5;
		for (int i = -chamberSize; i <= chamberSize; i++)
		{
			for (int j = -chamberSize; j < chamberSize; j++)
			{
				int x = (int)(origin.X + i);
				int y = (int)(origin.Y + j);
				if (!WorldGen.InWorld(x, y))
					continue;

				float distance = MathF.Sqrt(i * i + j * j);
				if (distance <= chamberSize)
				{
					if (distance > chamberSize - 1.5)
					{
						if (Main.tile[x, y].TileType != WoodType)
							Main.tile[x, y].ResetToType(WoodType);
					}
					else
						Main.tile[x, y].ClearEverything();

					WorldGen.SquareTileFrame(x, y);
				}
			}
		}

		PlaceLootChest(origin.X, origin.Y + chamberSize - 1);

		origin.Y -= chamberSize;

		//int leafCount = WorldGen.genRand.Next(5, 8);
		//for (int i = 0; i < leafCount; i++)
		//{
		//	Vector2 direction = new Vector2(0, -1f).RotatedBy((float)i / leafCount * MathHelper.TwoPi + WorldGen.genRand.NextFloat(-0.15f, 0.15f));
		//	direction.Y *= WorldGen.genRand.NextFloat(0.3f, 0.6f);
		//	GenerateLeaf(origin, direction.RotatedBy(rotation), 3, 20f, Math.Sign(rotation) * 0.02f);
		//}
	}

	private static void PlaceLootChest(int x, int y)
	{
		int index = WorldGen.PlaceChest(x - 1, y - 1, type: (ushort)ModContent.TileType<CoconutChestTile>());
		if (index < 0)
		{
			return;
		}
	}

	private static void GenerateLeaf(Point origin, Vector2 direction, float halfSize, float length, float curl)
	{
		Vector2 curPos = origin.ToVector2();

		for (float k = 0; k <= length; k++)
		{
			int size = (int)(MathF.Sin(k / length * MathHelper.Pi) * halfSize * (1f - k / length * 0.5f)) + 1;

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
					if (distance <= size || size < 3)
					{
						if (Main.tile[x, y].TileType != WoodType && Main.tile[x, y].WallType != WallType)
							Main.tile[x, y].ResetToType(LeafType);

						WorldGen.SquareTileFrame(x, y);
					}
				}
			}

			curPos = nextPos;
			direction += new Vector2(0, curl).RotatedBy(angle);
			direction += new Vector2(0, WorldGen.genRand.NextFloat(0.02f, 0.04f));
		}
	}
}
