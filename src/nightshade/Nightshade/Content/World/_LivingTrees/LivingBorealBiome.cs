using Microsoft.Xna.Framework;
using Nightshade.Common.Utilities;
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
		LivingBorealBiome tree = new LivingBorealBiome();
		tree.Place(Main.MouseWorld.ToTileCoordinates(), new StructureMap());
	}
}

public sealed class LivingBorealBiome : MicroBiome
{
	private static ushort WoodType => (ushort)TileID.BorealWood;

	private static ushort LeafType => (ushort)TileID.PineTree;

	private static ushort PlatformType => (ushort)TileID.Platforms;

	private static ushort WallType => (ushort)WallID.BorealWood;

	private static ushort PotType => (ushort)TileID.Pots;

	public override bool Place(Point origin, StructureMap structures)
	{
		int treeHeight = WorldGen.genRand.Next(55, 65);
		int treeWidth = WorldGen.genRand.Next(20, 27);

		Point bottom = new Point(origin.X, origin.Y + 85);

		if (!WorldUtils.Find(bottom, Searches.Chain(new Searches.Down(120), new NightshadeGenUtil.Conditions.IsNotTile()), out bottom))
			bottom = new Point(origin.X, origin.Y + WorldGen.genRand.Next(80, 120));

		BorealRootNode[] nodes = CreateLootNodes(bottom, 5, 10, 4, 7);

		Rectangle area = new Rectangle(origin.X - treeWidth, origin.Y - treeHeight, treeWidth * 2, bottom.Y - origin.Y + 7);

		PlacePassage(origin, WorldGen.genRand.Next(nodes).Center);
		PlaceTree(origin, treeWidth, treeHeight);
		PlaceNodes(nodes);

		structures?.AddProtectedStructure(area);

		return true;
	}

	public record struct BorealRootNode(Point Center, int Radius);

	public static BorealRootNode[] CreateLootNodes(Point origin, int count, int maxDist, int minRadius, int maxRadius)
	{
		BorealRootNode[] nodes = new BorealRootNode[count];
		for (int i = 0; i < count; i++)
		{
			Point randomOffset = new Point(WorldGen.genRand.Next(-maxDist, maxDist), WorldGen.genRand.Next(-maxDist, maxDist));
			int size = WorldGen.genRand.Next(minRadius, maxRadius) + 1 + count / 10;
			nodes[i] = new BorealRootNode(origin + randomOffset, (int)size);
		}

		return nodes;
	}

	public static void PlaceTree(Point origin, int width, int height)
	{
		int leafStart = height / 3;
		int leftSize = width;
		int rightSize = width;
		const int heightOffGround = 17;
		const int trunkThickness = 5;
		List<Point> branches = new List<Point>();
		int branchLeft = heightOffGround - 3;
		int branchRight = heightOffGround - 3;

		for (int j = 0; j > -height; j--)
		{
			double threshold = (1.0 - ((double)Math.Abs(j) / height)) * width / 3;

			for (int i = -leftSize - 2; i <= rightSize + 2; i++)
			{
				int x = origin.X + i;
				int y = origin.Y + j;

				if (!WorldGen.InWorld(x, y))
					continue;

				if (i > -leftSize && i < rightSize)
				{
					int yForLeaves = (int)(y + Math.Sqrt(Math.Abs(i)) - heightOffGround);
					int leafStretch = 1 + (i > 0 ? Math.Abs(rightSize) : Math.Abs(leftSize)) / 3;
					for (int l = 0; l < leafStretch; l++)
					{
						if (!WorldGen.InWorld(x + l * Math.Sign(i), yForLeaves))
							continue;

						Main.tile[x + l * Math.Sign(i), yForLeaves].ResetToType(LeafType);
						WorldGen.SquareTileFrame(x + l * Math.Sign(i), yForLeaves);
					}
				}

				if (Math.Abs(i) < trunkThickness * (1.0 - ((double)Math.Abs(j) / (height - 4))))
				{
					int holeSize = (int)Math.Floor((trunkThickness - 1.1f) * MathF.Cbrt(Utils.GetLerpValue(-heightOffGround / 2, heightOffGround / 4, j, true)));
					if (Math.Abs(i) > holeSize || j < -heightOffGround / 2)
						Main.tile[x, y].ResetToType(WoodType);
					else
					{
						Main.tile[x, y].ClearEverything();
						Main.tile[x, y].WallType = WallType;
					}

					WorldGen.SquareTileFrame(x, y);
					WorldGen.SquareWallFrame(x, y);
				}
			}

			leftSize = (int)Math.Floor(leftSize / WorldGen.genRand.NextFloat(1.05f, 1.2f));
			rightSize = (int)Math.Floor(rightSize / WorldGen.genRand.NextFloat(1.05f, 1.2f));

			if (Math.Abs(j) < height - 4)
			{
				if (leftSize <= threshold)
				{
					leftSize += (int)(WorldGen.genRand.NextFloat(0.5f, 0.9f) * width / (Math.Abs(j) / (float)(height / 5) + 1f));
					if (Math.Abs(j + 10) < 9)
						leftSize += 6;
				}
				else if (leftSize <= threshold * 1.5 && branchLeft <= 0)
				{
					branches.Add(new Point(-leftSize, Math.Abs(j)));
					branchLeft = 5;
				}

				if (rightSize <= threshold)
				{
					rightSize += (int)(WorldGen.genRand.NextFloat(0.5f, 0.9f) * width / (Math.Abs(j) / (float)(height / 5) + 1f));
					if (Math.Abs(j + 10) < 9)
						rightSize += 6;
				}
				else if (rightSize <= threshold * 1.5 && branchRight <= 0)
				{
					branches.Add(new Point(rightSize, Math.Abs(j)));
					branchRight = 5;
				}

				branchLeft--;
				branchRight--;
			}
		}

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

		for (int k = 0; k < steps; k++)
		{
			int channelRadius = (int)(6 - 2 * Math.Cbrt((double)k / steps));

			rotater = MathHelper.Lerp(rotater, WorldGen.genRand.NextFloat(-0.7f, 0.7f), 0.1f);
			for (int j = -channelRadius; j <= channelRadius; j++)
			{
				for (int i = -channelRadius; i <= channelRadius; i++)
				{
					int x = (int)currentPoint.X + i;
					int y = (int)currentPoint.Y + j;

					double distance = Math.Sqrt(i * i + j * j);

					if (distance < channelRadius - 0.5)
					{
						if (distance > (channelRadius - 2.4) && Main.tile[x, y].WallType != WallType)
							Main.tile[x, y].ResetToType(WoodType);
						else
						{
							Main.tile[x, y].ClearEverything();
							Main.tile[x, y].WallType = WallType;
						}

						WorldGen.SquareTileFrame(x, y);
						WorldGen.SquareWallFrame(x, y);
					}
				}
			}

			if (--branch <= 0)
			{
				branch = WorldGen.genRand.Next(18, 24);
				int branchDir = WorldGen.genRand.NextBool().ToDirectionInt();
				WorldUtils.Gen(new Point((int)currentPoint.X + channelRadius * branchDir, (int)currentPoint.Y), new ShapeBranch(0.0, branchDir * 8), new Actions.SetTile(WoodType));
			}

			Vector2 direction = currentPoint.DirectionTo(endPoint.ToVector2()).SafeNormalize(Vector2.Zero);
			currentPoint += direction.RotatedBy(rotater) * WorldGen.genRand.NextFloat(0.95f, 1.05f);
		}
	}

	public static void PlaceNodes(BorealRootNode[] nodes)
	{
		for (int k = 0; k < nodes.Length; k++)
		{
			BorealRootNode node = nodes[k];

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
						if (distance > node.Radius * 1.1 - 2.1 && Main.tile[x, y].WallType != WallType)
							Main.tile[x, y].ResetToType(WoodType);
						else
						{
							Main.tile[x, y].ClearEverything();
							Main.tile[x, y].WallType = WallType;
						}
					}

					WorldGen.SquareTileFrame(x, y);
					WorldGen.SquareWallFrame(x, y);
				}
			}
		}
	}
}
