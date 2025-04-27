using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Content.Tiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

internal sealed class LivingCactusBiome : MicroBiome
{
    private static ushort CactusType => (ushort)ModContent.TileType<LivingCactus>();

    private static ushort WoodType => (ushort)ModContent.TileType<LivingCactusWood>();

    private static ushort PlatformType => TileID.Platforms;

    private static ushort WoodWallType => WallID.LivingWood;

    private static ushort PotType => (ushort)ModContent.TileType<LivingCactusPot>();

    public bool Round { get; set; }

    public override bool Place(Point origin, StructureMap? structures)
    {
        int height = WorldGen.genRand.Next(40, 55);
        int width = height / 2;

        if (Round)
        {
            height = WorldGen.genRand.Next(25, 28);
            width = (int)(height * WorldGen.genRand.NextFloat(0.8f, 1f));
		}

		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid().AreaAnd(6, 2), new Conditions.IsTile(TileID.Sand, TileID.HardenedSand, TileID.Sandstone)), out origin))
        {
            return false;
        }

		Rectangle cactusBounds = new Rectangle(origin.X - width / 2 - 5, origin.Y - height / 2 - 5, width + 10, height + 10);

        if (!structures?.CanPlace(cactusBounds) ?? false)
        {
            return false;
        }

        if (Round)
            GenerateRoundLivingCactus(origin.X, origin.Y, width, height);
        else
			GenerateTallLivingCactus(origin.X, origin.Y, height);

		structures?.AddProtectedStructure(cactusBounds, 10);

        return true;
    }

    private static bool CanPlaceCactusOutline(int x, int y, int thickness, bool checkCorners)
    {
		bool onOutside = Main.tile[x, y].WallType != WoodWallType && Main.tile[x, y].TileType != WoodType;
        bool onEdge = false;
        for (int i = x - thickness; i <= x + thickness; i++)
        {
            for (int j = y - thickness; j <= y + thickness; j++)
            {
                if (i == x && y == j)
                {
                    continue;
                }

                if (!WorldGen.InWorld(i, j))
                {
                    continue;
                }

                if (!checkCorners)
                {
                    if ((i == x - thickness && j == y - thickness) ||
                        (i == x + thickness && j == y - thickness) ||
                        (i == x - thickness && j == y + thickness) ||
                        (i == x + thickness && j == y + thickness))
                    {
                        continue;
                    }
                }

                if (Main.tile[i, j].TileType == WoodType)
                {
                    onEdge = true;
                }
            }
        }
        return onOutside && onEdge;
    }

    private static void GenerateRoundLivingCactus(int x, int y, int width, int height)
	{
		int curvature = WorldGen.genRand.Next(8, 15); // Higher is less curved
		int curveDir = WorldGen.genRand.NextBool().ToDirectionInt();
		int offsetX = 0;
		int[] stalkXOffsets = new int[height];
		int halfWidth = width / 2;

		for (int j = 0; j < height; j++)
		{
			if (j % curvature == 0 && j > height / 4 && j < height / 2)
			{
				offsetX += WorldGen.genRand.Next(0, 2) * curveDir;
			}

			stalkXOffsets[j] = offsetX;
		}

		for (int j = 0; j < height; j++)
		{
            float lowerCurveProgress = Utils.GetLerpValue(height * 0.7f, 0, j, true);

			for (int i = -halfWidth; i < halfWidth; i++)
			{
                int offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * curveDir * (curvature / 3 + 1), 0, stalkXOffsets.Length - 1)];
				int centerX = x;
                int centerY = y - height / 2 + (int)MathHelper.Lerp(0, height / 8, Utils.GetLerpValue(height / 2, 0, j, true));

				if (!WorldGen.InWorld(x + i + offX, y - j))
				{
					continue;
				}

				bool place = i <= -halfWidth || i >= halfWidth - 1;

                int normI = i + centerX - x;
                int normJ = j + centerY - y;
				double distance = MathF.Sqrt(normI * normI + normJ * normJ);
				bool withinBound = distance < halfWidth;
				if (withinBound)
				{
					place = distance > halfWidth - 1.4;
				}
				else
				{
					continue;
				}

				if (place)
				{
					Main.tile[x + i + offX, y - j].ResetToType(WoodType);
				}
				else
				{
					Main.tile[x + i + offX, y - j].ClearTile();
				}

				Main.tile[x + i + offX, y - j].WallType = WoodWallType;
			}
		}

		const int outer_thick = 2;
		for (int j = 0; j < height + outer_thick; j++)
		{
			for (int i = -halfWidth - outer_thick; i < halfWidth + outer_thick; i++)
			{
				int offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * curveDir, 0, stalkXOffsets.Length - 1)];

				if (!WorldGen.InWorld(x + i + offX, y - j))
				{
					continue;
				}

				if (CanPlaceCactusOutline(x + i + offX, y - j, outer_thick, false))
				{
					Main.tile[x + i + offX, y - j].ResetToType(CactusType);
				}
			}
		}

		for (int i = -2; i < 3; i++)
		{
			GenerateRoot(x + i, y - 2, 2.6, 11.0, new Vector2(i, 3.5f / (Math.Abs(i) + 1)));
		}

		WorldUtils.Gen(new Point(x, y - 5), new Shapes.HalfCircle(4), new Actions.SetLiquid());

		PlacePotsEverywhere(x, y - height / 5, 32);
	}

	private static void GenerateTallLivingCactus(int x, int y, int height)
    {
        const int stalk_half_width = 4;
        int mainStalkCurvature = WorldGen.genRand.Next(5, 8); // Higher is less curved
        int stalkCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();

        int offsetX = 0;
		int[] stalkXOffsets = new int[height];

        int timeUntilArm = WorldGen.genRand.Next(5, 20);
        int lastArmDirection = WorldGen.genRand.NextBool().ToDirectionInt();
		List<Point> arms = new List<Point>();

        for (int j = 0; j < height; j++)
        {
            if (j % mainStalkCurvature == 0 && j > 0 && j < height * 3 / 4)
            {
                offsetX += WorldGen.genRand.Next(0, 2) * stalkCurveDir;
            }

            stalkXOffsets[j] = offsetX;
        }

        for (int j = 0; j < height; j++)
        {
            timeUntilArm--;
            if (timeUntilArm == 0 && arms.Count < 2)
            {
                int armX = lastArmDirection > 0 ? x : x - 1;
                arms.Add(new Point(armX + stalkXOffsets[j] + WorldGen.genRand.Next(9, 14) * lastArmDirection, y - j));
                lastArmDirection *= -1;

                if (WorldGen.genRand.NextBool(3))
                {
                    timeUntilArm = WorldGen.genRand.Next(3, height / 3);
                }
            }

            for (int i = -stalk_half_width; i < stalk_half_width; i++)
            {
                int offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * stalkCurveDir * (mainStalkCurvature / 3 + 1), 0, stalkXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

                bool placeWide = offX != stalkXOffsets[j];

                bool place = i is <= -stalk_half_width or >= stalk_half_width - 1;

                if (j > height - stalk_half_width)
                {
                    int normJ = j - height + stalk_half_width;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    bool withinCap = distance < stalk_half_width - 0.5;
                    if (withinCap)
                    {
                        place = distance > stalk_half_width - 1.75;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (place)
                {
                    Main.tile[x + i + offX, y - j].ResetToType(WoodType);
                }
                else
                {
                    Main.tile[x + i + offX, y - j].ClearTile();
                }

                Main.tile[x + i + offX, y - j].WallType = WoodWallType;

                if (placeWide)
                {
                    if (Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].TileType != WoodType)
                    {
                        Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].ClearTile();
                    }

                    Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].WallType = WoodWallType;
                }
            }
        }

        PlaceLootChest(x + offsetX, y - height + stalk_half_width * 2);

        const int stalk_outer_thick = 2;
        for (int j = 0; j < height + stalk_outer_thick; j++)
        {
            for (int i = -stalk_half_width - stalk_outer_thick; i < stalk_half_width + stalk_outer_thick; i++)
            {
                int offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * stalkCurveDir, 0, stalkXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

                if (CanPlaceCactusOutline(x + i + offX, y - j, stalk_outer_thick, false))
                {
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
                }
            }
        }

        foreach (Point point in arms)
        {
            int armDirection = Math.Sign(point.X - x);
            int passageLength = Math.Abs(point.X - x);
            GenerateArm(point.X, point.Y, armDirection, passageLength, WorldGen.genRand.Next(15, 20));
        }

        for (int i = -2; i < 3; i++)
        {
            GenerateRoot(x + i, y, 2.3, 13.0, new Vector2(i, 2.5f / (Math.Abs(i) + 1)));
        }

        WorldUtils.Gen(new Point(x, y - 1), new Shapes.HalfCircle(4), new Actions.SetLiquid());

        PlacePotsEverywhere(x, y - height / 5, 32);
    }

    private static void GenerateArm(int x, int y, int direction, int passageLength, int armHeight)
    {
        int armCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();
        int armCurvature = WorldGen.genRand.Next(4, 8);
        int offsetX = 0;
		int[] armXOffsets = new int[armHeight];

        const int arm_half_width = 3;
        const int passage_height = 5;

        for (int j = 0; j < armHeight; j++)
        {
            if (j % armCurvature == 0 && j > passage_height + 2 && j < armHeight * 3 / 4)
            {
                offsetX += WorldGen.genRand.Next(0, 2) * armCurveDir;
            }

            armXOffsets[j] = offsetX;
        }

        for (int j = 0; j < armHeight; j++)
        {
            if (j < passage_height)
            {
                for (int i = 1; i < passageLength - 1; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                    {
                        continue;
                    }

					bool place = Math.Abs(j - passage_height / 2) > 1;
                    if (place && Main.tile[x - i * direction, y - j].WallType != WoodWallType)
                    {
                        Main.tile[x - i * direction, y - j].ResetToType(WoodType);
                    }
                    else
                    {
                        if (!place)
                        {
                            Main.tile[x - i * direction, y - j].ClearTile();
                        }

                        Main.tile[x - i * direction, y - j].WallType = WoodWallType;
                    }
                }
            }

            for (int i = -arm_half_width; i < arm_half_width; i++)
            {
                int offX = armXOffsets[Math.Clamp(j + 2 * Math.Sign(i) * armCurveDir, 0, armXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

				bool placeWide = offX != armXOffsets[j];

				bool place = i is <= -arm_half_width or >= arm_half_width - 1;

                if (j < arm_half_width)
                {
                    int normJ = j - arm_half_width;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
					bool withinCap = distance < arm_half_width - 0.5;
                    if (withinCap)
                    {
                        place = distance > arm_half_width - 1.5;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (j > armHeight - arm_half_width)
                {
                    int normJ = j - armHeight + arm_half_width;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    bool withinCap = distance < arm_half_width - 0.5;
                    if (withinCap)
                    {
                        place = distance > arm_half_width - 1.5;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (place && Main.tile[x + i + offX, y - j].WallType != WoodWallType)
                {
                    Main.tile[x + i + offX, y - j].ResetToType(WoodType);
                }
                else
                {
                    Main.tile[x + i + offX, y - j].ClearTile();
                }

                Main.tile[x + i + offX, y - j].WallType = WoodWallType;

                if (placeWide)
                {
                    if (Main.tile[x + i + armXOffsets[j], y - j].TileType != WoodType)
                    {
                        Main.tile[x + i + armXOffsets[j], y - j].ClearTile();
                    }

                    Main.tile[x + i + armXOffsets[j], y - j].WallType = WoodWallType;
                }
            }
        }

        if (!WorldGen.genRand.NextBool(10))
        {
            TryPlacePlatform(x + offsetX, y - armHeight + arm_half_width * 2, -direction, arm_half_width);
            TryPlacePlatform(x + offsetX, y - armHeight + arm_half_width * 2, direction, arm_half_width);
            WorldGen.PlaceTile(x + offsetX - 1, y - armHeight + arm_half_width * 2 - 1, PotType, true);
        }

        const int arm_outer_thick = 2;
        for (int j = -arm_outer_thick; j < armHeight + arm_outer_thick; j++)
        {
            if (j < passage_height + arm_outer_thick)
            {
                for (int i = -1; i < passageLength; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                    {
                        continue;
                    }

                    if (CanPlaceCactusOutline(x - i * direction, y - j, arm_outer_thick, false))
                    {
                        Main.tile[x - i * direction, y - j].ResetToType(CactusType);
                    }
                }
            }

            for (int i = -arm_half_width - arm_outer_thick; i < arm_half_width + arm_outer_thick; i++)
            {
                int offX = armXOffsets[Math.Clamp(j + 2 * Math.Sign(i) * armCurveDir, 0, armXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

                if (CanPlaceCactusOutline(x + i + offX, y - j, arm_outer_thick, false))
                {
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
                }
            }
        }

        // 8 = 2 * stalkHalfWidth, should be fine for varied size
        TryPlacePlatform(x - passageLength / 2 * direction, y, -direction);
    }

    private static void GenerateRoot(int x, int y, double size, double distance, Vector2 direction)
    {
        double xi = x - size / 2;
        double yi = y - size / 2;
        for (int k = 0; k < (int)distance; k++)
        {
            int thickness = (int)Math.Ceiling(Utils.Lerp(size, 0, Math.Sqrt(k / distance)));
            int thickX = thickness + (int)Math.Abs(direction.X);
            int thickY = thickness + (int)Math.Abs(direction.Y);
            for (int i = 0; i < thickX; i++)
            {
                for (int j = 0; j < thickY; j++)
                {
                    Main.tile[(int)xi + i, (int)yi + j].ResetToType(WoodType);

                    if (WorldGen.genRand.NextBool(10) && size > 1.0 && k > distance / 3.0)
                    {
                        GenerateRoot((int)xi + i, (int)yi + j, size / 3.0, distance / 3, new Vector2(direction.X * 0.5f, direction.Y));
                    }
                }
            }

            direction = direction.RotatedBy(WorldGen.genRand.NextFloat(-0.15f, 0.15f));

            xi += direction.X;
            yi += direction.Y;

            direction.Y *= 0.8f + 0.3f * (float)Math.Sqrt(k / distance);
        }
    }

    private static void TryPlacePlatform(int x, int y, int direction, int max = 8)
    {
        bool valid = true;
        int solidCount = 0;
        int validCount = 0;
        int r = 0;
        while (true)
        {
            if (!Main.tile[x + r * direction, y].HasTile && Main.tile[x + r * direction, y].WallType == WoodWallType)
            {
                validCount++;
            }
            else if (Main.tile[x + r * direction, y].HasTile && validCount < 1)
            {
                solidCount++;
            }
            else
            {
                break;
            }

            r++;
            if (validCount > max)
            {
                valid = false;
                break;
            }
        }

        if (!valid)
        {
            return;
        }

        for (int i = 0; i < validCount; i++)
        {
            if (!WorldGen.SolidTile(x + (i + solidCount) * direction, y))
            {
                Main.tile[x + (i + solidCount) * direction, y].ResetToType(PlatformType);
            }
        }
    }

    private static void PlaceLootChest(int x, int y, int width = 4)
    {
        TryPlacePlatform(x, y, 1, width / 2 + 1);
        TryPlacePlatform(x, y, -1, width / 2 + 1);
        WorldGen.PlaceChest(x - 1 + WorldGen.genRand.Next(-width / 2 + 1, width / 2), y - 1, style: 10);
    }

    private static void PlacePotsEverywhere(int x, int y, int radius)
    {
        for (int i = x - radius; i < x + radius; i++)
        {
            for (int j = y - radius; j < y + radius; j++)
            {
                if (WorldGen.genRand.NextBool(5))
                {
                    WorldGen.PlacePot(i, j, PotType);
                }
            }
        }
    }
}