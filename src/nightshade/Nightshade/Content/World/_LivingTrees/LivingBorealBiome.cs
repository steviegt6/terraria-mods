using Microsoft.Xna.Framework;
using Nightshade.Common.Utilities;
using Nightshade.Content.Tiles;
using Nightshade.Content.Tiles.Furniture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World._LivingTrees;

public sealed class GenTreeCommand : ModCommand
{
	public override string Command => "gentree";

	public override CommandType Type => CommandType.Chat;

	public override void Action(CommandCaller caller, string input, string[] args)
	{
		int treeHeight = WorldGen.genRand.Next(55, 65);
		int treeWidth = WorldGen.genRand.Next(20, 23);

		LivingBorealBiome.PlaceTree(Main.MouseWorld.ToTileCoordinates(), treeWidth, treeHeight);
	}
}

public sealed class LivingBorealBiome : MicroBiome
{
	private static ushort WoodType => (ushort)ModContent.TileType<LivingBorealWood>();

	private static ushort LeafType => (ushort)ModContent.TileType<LivingBorealLeaf>();

	private static ushort VineType => (ushort)ModContent.TileType<LivingBorealVine>();

	private static ushort PlatformType => (ushort)TileID.Platforms;

	private static ushort WallType => (ushort)WallID.LivingWood;

	public override bool Place(Point origin, StructureMap structures)
	{
		int treeHeight = WorldGen.genRand.Next(55, 65);
		int treeWidth = WorldGen.genRand.Next(20, 23);

		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(250), new Conditions.IsSolid()), out origin))
			return false;

		var realOrigin = origin;
		var areaCondition = new NightshadeGenUtil.Conditions.IsNotTile().AreaAnd(treeWidth / 2, treeHeight / 2);
		Point areaCheckPoint = origin - new Point(treeWidth / 4, treeHeight / 2);

		if (!WorldUtils.Find(areaCheckPoint, Searches.Chain(new Searches.Rectangle(treeWidth * 2, treeHeight / 2), areaCondition), out areaCheckPoint))
			return false;

		origin = new Point(areaCheckPoint.X + treeWidth / 4, areaCheckPoint.Y + treeHeight / 4);

		Point bottom = new Point(origin.X, origin.Y + 85);

		if (!WorldUtils.Find(bottom, Searches.Chain(new Searches.Down(120), new NightshadeGenUtil.Conditions.IsNotTile()), out bottom))
			bottom = new Point(origin.X, origin.Y + WorldGen.genRand.Next(80, 120));

		BorealLootNode[] nodes = CreateLootNodes(bottom, 7, 10, 4, 7);

		Rectangle area = new Rectangle(origin.X - treeWidth, origin.Y - treeHeight, treeWidth * 2, bottom.Y - origin.Y + 7);

		if (!structures?.CanPlace(area) ?? false)
			return false;

		PlaceTree(realOrigin, treeWidth, treeHeight);
		PlacePassage(realOrigin, nodes.MinBy(n => n.Center.Y).Center);
		PlaceLootNodes(nodes);

		structures?.AddProtectedStructure(area);

		return true;
	}

	public record struct BorealLootNode(Point Center, int Radius);

	public static BorealLootNode[] CreateLootNodes(Point origin, int count, int maxDist, int minRadius, int maxRadius)
	{
		BorealLootNode[] nodes = new BorealLootNode[count];
		for (int i = 0; i < count; i++)
		{
			Point randomOffset = new Point(WorldGen.genRand.Next(-maxDist, maxDist), WorldGen.genRand.Next(-maxDist, maxDist));
			int size = WorldGen.genRand.Next(minRadius, maxRadius) + 1 + count / 10;
			nodes[i] = new BorealLootNode(origin + randomOffset, (int)size);
		}

		return nodes;
	}

	public static void PlaceTree(Point origin, int width, int height)
	{
		int leafStart = height / 3;
		int leftSize = width;
		int rightSize = width;
		const int heightOffGround = 17;
		const int trunkThickness = 7;
		List<Point> branches = new List<Point>();
		int branchLeft = heightOffGround - 3;
		int branchRight = heightOffGround - 3;

		// Roots
		const int rootCount = 5;
		for (int k = 0; k < rootCount; k++)
		{
			Point rootPosition = new Point((int)(origin.X + ((double)k / (rootCount - 1) - 0.5) * trunkThickness), origin.Y);
			double rootAngle = MathHelper.PiOver2 + (0.5f - (double)k / (rootCount - 1));
			WorldUtils.Gen(rootPosition, new ShapeRoot(rootAngle, 30), new Actions.SetTile(WoodType));
		}

		// Tree
		for (int j = 0; j > -height; j--)
		{
			double threshold = (1.0 - ((double)Math.Abs(j) / height)) * width / 3;

			for (int i = -leftSize - 2; i <= rightSize + 2; i++)
			{
				int x = origin.X + i;
				int y = origin.Y + j;

				if (!WorldGen.InWorld(x, y))
					continue;
				
				// Leaves
				if (i > -leftSize && i < rightSize)
				{
					int yForLeaves = (int)(y + Math.Sqrt(Math.Abs(i)) - heightOffGround);
					int leafStretch = 1 + (i > 0 ? Math.Abs(rightSize) : Math.Abs(leftSize)) / 3;

					// Snow on leaves
					for (int l = 0; l < leafStretch; l++)
					{
						int snowCount = 1 + (int)(Math.Min(4, leafStretch - l) * Math.Abs((double)j) / height * 3);
						for (int s = 0; s <= snowCount; s++)
						{
							if (!WorldGen.InWorld(x + l * Math.Sign(i), yForLeaves - s))
								continue;

							if (Main.tile[x + l * Math.Sign(i), yForLeaves - s].HasTile)
								continue;

							Main.tile[x + l * Math.Sign(i), yForLeaves - s].ResetToType(TileID.SnowBlock);
							WorldGen.SquareTileFrame(x + l * Math.Sign(i), yForLeaves);
						}

						if (!WorldGen.InWorld(x + l * Math.Sign(i), yForLeaves))
							continue;

						bool snowChance = j > -height * 2 / 3 
							? WorldGen.genRand.NextBool(4) 
							: !WorldGen.genRand.NextBool(3);

						bool snowed = j < -height / 3 && snowChance;
						Main.tile[x + l * Math.Sign(i), yForLeaves].ResetToType(snowed ? TileID.SnowBlock : LeafType);
						WorldGen.SquareTileFrame(x + l * Math.Sign(i), yForLeaves);
					}
				}

				// Trunk
				if (Math.Abs(i) < trunkThickness * (1.0 - Math.Sqrt((double)Math.Abs(j) / (height - 4))))
				{
					int holeSize = (int)Math.Floor((trunkThickness - 3f) * MathF.Cbrt(Utils.GetLerpValue(-heightOffGround / 2, heightOffGround / 5, j, true)));
					if (Math.Abs(i) > holeSize || j < -heightOffGround / 2)
						Main.tile[x, y].ResetToType(WoodType);
					else
					{
						Main.tile[x, y].ClearEverything();
						Main.tile[x, y].WallType = WallType;
					}
					WorldGen.SquareTileFrame(x, y);
				}
			}

			leftSize = (int)Math.Floor(leftSize / WorldGen.genRand.NextFloat(1.05f, 1.2f));
			rightSize = (int)Math.Floor(rightSize / WorldGen.genRand.NextFloat(1.05f, 1.2f));

			if (Math.Abs(j) < height - 4)
			{
				if (leftSize <= threshold)
					leftSize += (int)(WorldGen.genRand.NextFloat(0.5f, 0.9f) * width / (Math.Abs(j) / (float)(height / 3) + 1f));

				else if (leftSize <= threshold * 1.5 && branchLeft <= 0)
				{
					branches.Add(new Point(-leftSize, Math.Abs(j)));
					branchLeft = 5;
				}

				if (rightSize <= threshold)
					rightSize += (int)(WorldGen.genRand.NextFloat(0.5f, 0.9f) * width / (Math.Abs(j) / (float)(height / 3) + 1f));

				else if (rightSize <= threshold * 1.5 && branchRight <= 0)
				{
					branches.Add(new Point(rightSize, Math.Abs(j)));
					branchRight = 5;
				}

				branchLeft--;
				branchRight--;
			}
		}

		// Branches in tree
		foreach (Point branch in branches)
		{
			float angle = MathHelper.PiOver2 + (MathHelper.PiOver2 + 0.2f) * Math.Sign(branch.X);
			WorldUtils.Gen(new Point(origin.X, origin.Y - branch.Y + 2), new ShapeBranch(angle, Math.Abs(branch.X) * 1.66), new Actions.SetTile(WoodType));
		}
	}

	public static void PlacePassage(Point startPoint, Point endPoint)
	{
		Vector2 currentPoint = startPoint.ToVector2();
		Vector2 difference = (endPoint - startPoint).ToVector2();
		int steps = (int)difference.Length();
		float rotater = WorldGen.genRand.NextFloat(-0.7f, 0.7f);
		int branch = WorldGen.genRand.Next(15, 20);
		int platform = 0;
		HashSet<Point> inside = new HashSet<Point>();

		for (int k = 0; k < steps; k++)
		{
			int channelRadius = (int)(5 - 1 * Math.Cbrt((double)k / steps));

			rotater = MathHelper.Lerp(rotater, WorldGen.genRand.NextFloat(-0.7f, 0.7f), 0.1f);
			for (int j = -channelRadius; j <= channelRadius; j++)
			{
				for (int i = -channelRadius; i <= channelRadius; i++)
				{
					int x = (int)currentPoint.X + i;
					int y = (int)currentPoint.Y + j;

					double distance = Math.Sqrt(i * i + j * j);

					if (k < 3 && Main.tile[x, y].WallType == WallType)
						inside.Add(new Point(x, y));

					if (distance < channelRadius - 0.5)
					{
						double channelSize = channelRadius - 2.5 + (double)k / steps * 0.3;
						if (distance > channelSize && !inside.Contains(new Point(x, y)))
							Main.tile[x, y].ResetToType(WoodType);
						else
						{
							Main.tile[x, y].ClearEverything();
							inside.Add(new Point(x, y));
						}

						if (distance < channelSize + 0.9 || inside.Contains(new Point(x, y)))
							Main.tile[x, y].WallType = WallType;
					}
				}
			}

			if (--platform <= 0)
			{
				platform = WorldGen.genRand.Next(20, 50);
				NightshadeGenUtil.PlacePlatform((int)currentPoint.X, (int)currentPoint.Y, PlatformType);
			}

			if (--branch <= 0 && k < steps)
			{
				branch = WorldGen.genRand.Next(18, 24);
				int branchDir = WorldGen.genRand.NextBool().ToDirectionInt();
				WorldUtils.Gen(new Point((int)currentPoint.X + channelRadius * branchDir, (int)currentPoint.Y), new ShapeBranch(0.0, branchDir * 8), new Actions.SetTile(WoodType));
			}

			Vector2 direction = currentPoint.DirectionTo(endPoint.ToVector2()).SafeNormalize(Vector2.Zero);
			currentPoint += direction.RotatedBy(rotater) * WorldGen.genRand.NextFloat(0.95f, 1.05f);
		}
	}

	public static void PlaceLootNodes(BorealLootNode[] nodes)
	{
		HashSet<Point> insides = new HashSet<Point>();

		for (int k = 0; k < nodes.Length; k++)
		{
			BorealLootNode node = nodes[k];

			float distX = WorldGen.genRand.NextFloat(1f, 1.2f);
			float distY = WorldGen.genRand.NextFloat(1f, 1.2f);
			float distR = WorldGen.genRand.NextFloat(-4f, 4f);
			for (int j = -node.Radius; j <= node.Radius; j++)
			{
				for (int i = -node.Radius; i <= node.Radius; i++)
				{
					int modI = i > 0 ? i : i - 1;
					int modJ = j > 0 ? j : j - 1;
					int x = node.Center.X + i;
					int y = node.Center.Y + j;

					if (!WorldGen.InWorld(x, y))
						continue;

					Vector2 distorted = new Vector2(modI * distX, modJ * distY).RotatedBy(distR);
					double distance = distorted.Length();

					if (distance < node.Radius * 1.1)
					{
						if (distance > node.Radius * 1.1 - 2.1 && !insides.Contains(new Point(x, y)))
							Main.tile[x, y].ResetToType(TileID.SnowBlock);
						else
							insides.Add(new Point(x, y));

						if (distance < node.Radius * 1.1 - 1.5)
						{
							if (insides.Contains(new Point(x, y)))
								Main.tile[x, y].ClearEverything();

							Main.tile[x, y].WallType = WallID.SnowWallUnsafe;
						}
					}
				}
			}
		}

		// Loot
		int placeTime = WorldGen.genRand.Next(3);

		int chestCount = 1 + WorldGen.genRand.NextBool(10).ToInt();
		int heartCount = 1 + WorldGen.genRand.Next(3);
		foreach (Point p in insides)
		{
			placeTime--;

			if (placeTime <= 0)
			{
				placeTime = WorldGen.genRand.Next(1, 6);

				if (chestCount > 0)
				{
					if (TryPlaceLootChest(p.X, p.Y))
						chestCount--;
				}

				if (heartCount > 0)
				{
					if (WorldGen.PlaceObject(p.X, p.Y, TileID.Heart, true))
						heartCount--;
				}
			}
		}
	}

	private static bool TryPlaceLootChest(int x, int y)
	{
		int chest = WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<CoconutChestTile>());
		if (chest < 0)
			return false;

		return true;
	}
}
