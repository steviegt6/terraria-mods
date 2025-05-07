using Microsoft.Xna.Framework;

using Terraria;

namespace Nightshade.Common.Utilities;

internal static class NightshadeGenUtil
{
	public static int GetNearestSolidHeight(int x, int y, int maxDistance)
	{
		int i = 0;
		while (i < maxDistance)
		{
			if (WorldGen.InWorld(x, y + i))
			{
				if (WorldGen.SolidOrSlopedTile(x, y + i))
					return y + i;
			}
			if (WorldGen.InWorld(x, y - i))
			{
				if (WorldGen.SolidOrSlopedTile(x, y - i))
					return y - i;
			}

			i++;
		}

		return y;
	}

	public static int GetNearestSurface(int x, int y, int maxDistance)
	{
		if (maxDistance < 2)
			return y;

		int i = 0;
		while (i < maxDistance)
		{
			if (WorldGen.InWorld(x, y + i) && WorldGen.InWorld(x, y + i - 1))
			{
				if (WorldGen.SolidOrSlopedTile(x, y + i) && !WorldGen.SolidOrSlopedTile(x, y + i - 1))
					return y + i;
			}
			if (WorldGen.InWorld(x, y - i) && WorldGen.InWorld(x, y - i - 1))
			{
				if (WorldGen.SolidOrSlopedTile(x, y - i) && !WorldGen.SolidOrSlopedTile(x, y - i - 1))
					return y - i;
			}

			i++;
		}

		return y;
	}

	public static float GetAverageSurfaceSlope(int x, int y, int halfWidth, int jumpHeight = 15)
	{
		if (halfWidth < 2)
			return 0f;

		int dx = 0;
		float dy = 0f;
		int lastHeight = GetNearestSurface(x - halfWidth, y, jumpHeight * 4);
		for (int i = x - halfWidth; i < x + halfWidth; i++)
		{
			int j = GetNearestSurface(i, lastHeight, jumpHeight);
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
}
