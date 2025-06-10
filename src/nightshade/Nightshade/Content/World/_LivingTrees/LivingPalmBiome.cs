using Microsoft.Xna.Framework;
using Nightshade.Common.Utilities;
using Nightshade.Content.Tiles;
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

	public float StartCurl { get; set; }

	public float CurlStrength { get; set; }

	public override bool Place(Point origin, StructureMap structures)
	{
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(300),
			new Conditions.IsSolid()), out origin))
			return false;

		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Rectangle(400, 20), 
			new Conditions.IsTile(TileID.Sand), new NightshadeGenUtil.Conditions.IsSolidSurface()), out origin))
			return false;

		PlaceStem(origin, out Point headPosition, out float curl);
		PlaceHead(curl * 0.2f, headPosition);

		int left = Math.Min(origin.X, headPosition.X) - 35;
		int top = headPosition.Y - 20;
		int height = Math.Abs(headPosition.Y - origin.Y) + 35;
		structures.AddProtectedStructure(new Rectangle(left, top, 70, height));

		return true;
	}

	private void PlaceStem(Point origin, out Point headPosition, out float finalCurl)
	{
		headPosition = origin;

		Vector2 curPos = origin.ToVector2();

		float length = 30;
		float halfSize = 4.5f;

		float curl = 0f;

		for (float k = 0; k <= length; k++)
		{
			int size = (int)(halfSize - MathF.Cbrt(k / length) * 2);

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

			curl += 0.04f / (2f - k / length) * CurlStrength;
		}

		for (int i = 0; i < 3; i++)
		{
			WorldUtils.Gen(origin, new ShapeRoot(MathHelper.Pi / 5f * (i - 1) + MathHelper.PiOver2, 17, 3, 1), new Actions.SetTile(WoodType));
		}

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
			direction.Y -= 0.1f;
			GenerateLeaf(origin, direction.RotatedBy(rotation), 3, 20f, Math.Sign(rotation) * 0.02f);
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
					}
				}
			}

			curPos = nextPos;
			direction += new Vector2(0, curl).RotatedBy(angle);
			direction += new Vector2(0, WorldGen.genRand.NextFloat(0.03f, 0.05f));
		}
	}
}
