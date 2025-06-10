using Microsoft.Xna.Framework;

using Terraria;
using Terraria.WorldBuilding;

namespace Nightshade.Common.Utilities;

public static class NightshadeGenUtil
{
	public static class Conditions
	{
		public sealed class IsSolidSurface(bool allowWet = false) : GenCondition
		{
			protected override bool CheckValidity(int x, int y)
			{
				if (!WorldGen.InWorld(x, y, 5))
					return false;

				if (Main.tile[x, y].HasTile)
				{
					bool hasAir = Main.tileSolid[Main.tile[x, y].TileType] && !Main.tile[x, y - 1].HasTile && !Main.tile[x, y - 2].HasTile;
					if (allowWet)
						return hasAir;

					return hasAir && Main.tile[x, y - 1].LiquidAmount < 1 && Main.tile[x, y - 2].LiquidAmount < 1;
				}

				return false;
			}
		}
	}

	public static int GetNearestSolidHeight(int x, int y, int maxDistance)
	{
		var i = 0;
		while (i < maxDistance)
		{
			if (WorldGen.InWorld(x, y + i))
			{
				if (WorldGen.SolidOrSlopedTile(x, y + i))
				{
					return y + i;
				}
			}
			if (WorldGen.InWorld(x, y - i))
			{
				if (WorldGen.SolidOrSlopedTile(x, y - i))
				{
					return y - i;
				}
			}

			i++;
		}

		return y;
	}

	public static int GetNearestSurface(int x, int y, int maxDistance)
	{
		if (maxDistance < 2)
		{
			return y;
		}

		var i = 0;
		while (i < maxDistance)
		{
			if (WorldGen.InWorld(x, y + i) && WorldGen.InWorld(x, y + i - 1))
			{
				if (WorldGen.SolidOrSlopedTile(x, y + i) && !WorldGen.SolidOrSlopedTile(x, y + i - 1))
				{
					return y + i;
				}
			}
			if (WorldGen.InWorld(x, y - i) && WorldGen.InWorld(x, y - i - 1))
			{
				if (WorldGen.SolidOrSlopedTile(x, y - i) && !WorldGen.SolidOrSlopedTile(x, y - i - 1))
				{
					return y - i;
				}
			}

			i++;
		}

		return y;
	}

	public static float GetAverageSurfaceSlope(int x, int y, int halfWidth, int jumpHeight = 15)
	{
		if (halfWidth < 2)
		{
			return 0f;
		}

		var dx = 0;
		var dy = 0f;
		var lastHeight = GetNearestSurface(x - halfWidth, y, jumpHeight * 4);
		for (var i = x - halfWidth; i < x + halfWidth; i++)
		{
			var j = GetNearestSurface(i, lastHeight, jumpHeight);
			Dust.QuickDust(i, j, Color.Red);

			if (dx > 0)
			{
				dy += j - lastHeight;
			}

			lastHeight = j;
			dx++;
		}

		return dy / (halfWidth * 2);
	}

	public static void AddLootToChest(ref Chest chest, params Item[] items)
	{
		for (var i = 0; i < chest.item.Length; i++)
		{
			if (i < items.Length)
			{
				chest.item[i] = items[i];
			}
			else
			{
				chest.item[i].TurnToAir(true);
			}
		}
	}
}
