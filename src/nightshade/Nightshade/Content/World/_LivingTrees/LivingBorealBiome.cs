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
		var nodes = CreateNodes(origin, 8, 10, 8, 4);
		PlaceNodes(nodes);

		return true;
	}

	public record struct BorealPassageNode(Point Center, int Radius);

	public static BorealPassageNode[] CreateNodes(Point origin, int count, int maxDist, int minRadius, int maxRadius)
	{
		BorealPassageNode[] nodes = new BorealPassageNode[count];
		for (int i = 0; i < count; i++)
		{
			Point randomOffset = new Point(WorldGen.genRand.Next(-maxDist, maxDist), WorldGen.genRand.Next(-maxDist, maxDist));
			int size = WorldGen.genRand.Next(minRadius, maxRadius);
			nodes[i] = new BorealPassageNode(origin + randomOffset, (int)size);
		}

		return nodes;
	}

	public static void PlaceNodes(BorealPassageNode[] nodes)
	{
		for (int k = 0; k < nodes.Length; k++)
		{
			BorealPassageNode node = nodes[k];
			for (int j = -node.Radius; j < node.Radius; j++)
			{
				for (int i = -node.Radius; i < node.Radius; i++)
				{
					int x = node.Center.X + i;
					int y = node.Center.Y + j;
					if (!WorldGen.InWorld(x, y))
						continue;

					double distance = Math.Sqrt(i * i + j * j);

					if (distance < node.Radius)
					{
						Main.tile[x, y].ResetToType(WoodType);
					}
				}
			}
		}
	}

	// Tree
	// Hole in Tree
	// Vertical Shaft below
	// Nodes with loot
}
