using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
	private static ushort WoodType => (ushort)TileID.LivingWood;

	private static ushort LeafType => (ushort)TileID.LivingUltrabrightFire;

	private static ushort PlatformType => (ushort)TileID.Platforms;

	private static ushort WallType => (ushort)WallID.LivingWoodUnsafe;

	private static ushort PotType => (ushort)TileID.Pots;

	public override bool Place(Point origin, StructureMap structures)
	{
		var nodes = CreateNodes(origin, 8, 10, 0.6f);
		PlaceNodes(origin, nodes);

		return true;
	}

	public record struct BorealPassageNode(Point Center, float Size);

	public static BorealPassageNode[] CreateNodes(Point origin, int count, int maxDist, float sizeMod)
	{
		BorealPassageNode[] nodes = new BorealPassageNode[count];
		for (int i = 0; i < count; i++)
		{
			Point randomOffset = new Point(WorldGen.genRand.Next(-maxDist, maxDist), WorldGen.genRand.Next(-maxDist, maxDist));
			float size = WorldGen.genRand.NextFloat(0.6f, 1.3f) * sizeMod;
			nodes[i] = new BorealPassageNode(origin + randomOffset, size);
		}

		return nodes;
	}

	private static double GetNodeStrength(Point testPoint, BorealPassageNode[] nodes)
	{
		double strength = 0.0;
		float totalStrength = 0f;
		foreach (var node in nodes)
		{
			Point diff = testPoint - node.Center;
			strength += node.Size / (Math.Pow(diff.X * diff.X + diff.Y * diff.Y, 0.75) + 0.25f);
			totalStrength += node.Size;
		}

		return strength;
	}

	public static void PlaceNodes(Point origin, BorealPassageNode[] nodes)
	{
		int left = origin.X;
		int right = origin.X;
		int top = origin.Y;
		int bottom = origin.Y;

		const int maxRadius = 29;
		foreach (var node in nodes)
		{
			left = Math.Min(left, node.Center.X - maxRadius * 2);
			right = Math.Max(right, node.Center.X + maxRadius * 2);
			top = Math.Min(top, node.Center.Y - maxRadius * 2);
			bottom = Math.Max(bottom, node.Center.Y + maxRadius * 2);
		}

		for (int j = top - 1; j <= bottom; j++)
		{
			for (int i = left - 1; i <= right; i++)
			{
				if (!WorldGen.InWorld(i, j))
					continue;

				double strength = GetNodeStrength(new Point(i, j), nodes);
				if (strength * maxRadius > 6)
				{
					if (strength * maxRadius < 8.5)
						Main.tile[i, j].ResetToType(WoodType);
					else
					{
						Main.tile[i, j].ClearEverything();
						Main.tile[i, j].WallType = WallType;
					}

					WorldGen.SquareTileFrame(i, j);
					WorldGen.SquareWallFrame(i, j);
				}
			}
		}
	}

	// Tree
	// Hole in Tree
	// Vertical Shaft below
	// Nodes with loot
}
