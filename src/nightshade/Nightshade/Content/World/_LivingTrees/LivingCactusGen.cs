using Microsoft.Xna.Framework;
using Nightshade.Content.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.World;

public static class LivingCactusGen
{
    private static ushort CactusType => (ushort)ModContent.TileType<LivingCactus>();
    private static ushort WoodType => (ushort)ModContent.TileType<LivingCactusWood>();
    private static ushort PlatformType => (ushort)TileID.Platforms;
    private static ushort WoodWallType => (ushort)WallID.LivingWood;
    private static ushort PotType => (ushort)ModContent.TileType<LivingCactusPot>();

    private static bool CanPlaceCactusOutline(int x, int y, int thickness, bool checkCorners)
    {
        bool onOutside = Main.tile[x, y].WallType != WoodWallType && Main.tile[x, y].TileType != WoodType;
        bool onEdge = false;
        for (int i = x - thickness; i <= x + thickness; i++)
        {
            for (int j = y - thickness; j <= y + thickness; j++)
            {
                if (i == x && y == j)
                    continue;

                if (!WorldGen.InWorld(i, j))
                    continue;

                if (!checkCorners)
                {
                    if ((i == x - thickness && j == y - thickness) ||
                        (i == x + thickness && j == y - thickness) ||
                        (i == x - thickness && j == y + thickness) ||
                        (i == x + thickness && j == y + thickness))
                        continue;
                }

                if (Main.tile[i, j].TileType == WoodType)
                    onEdge = true;
            }
        }

        return onOutside && onEdge;
    }

    public static void GenerateLivingCactus(int x, int y)
    {
        const int stalkHalfWidth = 3;
        int stalkHeight = WorldGen.genRand.Next(40, 55);
        int mainStalkCurvature = WorldGen.genRand.Next(5, 8); // Higher is less curved
        int mainStalkCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();

        int offsetX = 0;
        int[] stalkXOffsets = new int[stalkHeight];

        int timeUntilArm = WorldGen.genRand.Next(5, 20);
        int lastArmDirection = WorldGen.genRand.NextBool().ToDirectionInt();
        List<Point> arms = new List<Point>();

        for (int j = 0; j < stalkHeight; j++)
        {
            if (j % mainStalkCurvature == 0 && j > 0 && j < stalkHeight * 3 / 4)
                offsetX += WorldGen.genRand.Next(0, 2) * mainStalkCurveDir;

            stalkXOffsets[j] = offsetX;

            timeUntilArm--;
            if (timeUntilArm == 0 && arms.Count < 2)
            {
                int armX = lastArmDirection > 0 ? x : x - 1;
                arms.Add(new Point(armX + offsetX + WorldGen.genRand.Next(8, 11) * lastArmDirection, y - j));
                lastArmDirection *= -1;

                if (WorldGen.genRand.NextBool(3))
                    timeUntilArm = WorldGen.genRand.Next(3, stalkHeight / 3);
            }

            for (int i = -stalkHalfWidth; i < stalkHalfWidth; i++)
            {
                if (!WorldGen.InWorld(x + i + offsetX, y - j))
                    continue;

                bool place = true;
                if (i > -stalkHalfWidth && i < stalkHalfWidth - 1)
                    place = false;

                if (j > stalkHeight - stalkHalfWidth)
                {
                    int normJ = j - stalkHeight + stalkHalfWidth;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    bool withinCap = distance < stalkHalfWidth - 0.75;
                    if (withinCap)
                        place = distance > stalkHalfWidth - 1.5;
                    else
                        continue;
                }

                if (place)
                    Main.tile[x + i + offsetX, y - j].ResetToType(WoodType);
                else
                {
                    Main.tile[x + i + offsetX, y - j].ClearTile();
                    Main.tile[x + i + offsetX, y - j].WallType = WoodWallType;
                }
            }
        }

        PlaceLootChest(x + offsetX, y - stalkHeight + stalkHalfWidth * 2, 4);

        const int stalkOuterThick = 1;
        for (int j = 0; j < stalkHeight + stalkOuterThick; j++)
        {
            int offX = stalkXOffsets[Math.Clamp(j, 0, stalkHeight - 1)];
            for (int i = -stalkHalfWidth - stalkOuterThick; i < stalkHalfWidth + stalkOuterThick; i++)
            {
                if (!WorldGen.InWorld(x + i + offX, y - j))
                    continue;

                if (CanPlaceCactusOutline(x + i + offX, y - j, stalkOuterThick, true))
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
            }
        }

        foreach (Point point in arms)
        {
            int passageLength = Math.Abs(point.X - x) + Math.Abs(stalkXOffsets[Math.Clamp(point.Y - y, 0, stalkHeight - 1)]);
            GenerateArm(point.X, point.Y, Math.Sign(point.X - x), passageLength, WorldGen.genRand.Next(15, 20));
        }

        PlacePotsEverywhere(x, y - stalkHeight / 2, 30);
        PlacePotsEverywhere(x, y, 40);
    }

    private static void GenerateArm(int x, int y, int direction, int passageLength, int armHeight)
    {
        int armCurvatureDirection = WorldGen.genRand.NextBool().ToDirectionInt();
        int armCurvature = WorldGen.genRand.Next(8, 12);
        int offsetX = 0;
        int[] armXOffsets = new int[armHeight];

        const int armHalfWidth = 2;
        const int passageHeight = 5;
        for (int j = 0; j < armHeight; j++)
        {
            if (j % armCurvature == 0 && j > passageHeight + 2 && j < armHeight * 3 / 4)
                offsetX += WorldGen.genRand.Next(-1, 2) * armCurvatureDirection;

            armXOffsets[j] = offsetX;

            if (j < passageHeight)
            {
                for (int i = 1; i < passageLength; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                        continue;

                    bool place = Math.Abs(j - passageHeight / 2) > 1;
                    if (place && Main.tile[x - i * direction, y - j].WallType != WoodWallType)
                        Main.tile[x - i * direction, y - j].ResetToType(WoodType);
                    else
                    {
                        if (!place)
                            Main.tile[x - i * direction, y - j].ClearTile();

                        Main.tile[x - i * direction, y - j].WallType = WoodWallType;
                    }
                }
            }

            for (int i = -armHalfWidth; i < armHalfWidth; i++)
            {
                if (!WorldGen.InWorld(x + i + offsetX, y - j))
                    continue;

                bool place = true;
                if (i > -armHalfWidth && i < armHalfWidth - 1)
                    place = false;

                if (j < armHalfWidth)
                {
                    int normJ = j - armHalfWidth;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    bool withinCap = distance < armHalfWidth - 0.5;
                    if (withinCap)
                        place = distance > armHalfWidth - 1.5;
                    else
                        continue;
                }

                if (j > armHeight - armHalfWidth)
                {
                    int normJ = j - armHeight + armHalfWidth;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    bool withinCap = distance < armHalfWidth - 0.5;
                    if (withinCap)
                        place = distance > armHalfWidth - 1.5;
                    else
                        continue;
                }

                if (place && Main.tile[x + i + offsetX, y - j].WallType != WoodWallType)
                    Main.tile[x + i + offsetX, y - j].ResetToType(WoodType);
                else
                {
                    Main.tile[x + i + offsetX, y - j].WallType = WoodWallType;
                    Main.tile[x + i + offsetX, y - j].ClearTile();
                }
            }
        }

        TryPlacePlatform(x - passageLength / 2 * direction, y, -direction, 4);
        if (!WorldGen.genRand.NextBool(10))
        {
            TryPlacePlatform(x + offsetX + direction, y - armHeight + armHalfWidth * 2, -direction, 4);
            WorldGen.PlaceTile(x + offsetX - 1, y - armHeight + armHalfWidth * 2 - 1, PotType, true);
        }

        const int armOuterThick = 1;
        for (int j = -armOuterThick; j < armHeight + armOuterThick; j++)
        {
            if (j < passageHeight + armOuterThick)
            {
                for (int i = 0; i < passageLength; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                        continue;

                    if (CanPlaceCactusOutline(x - i * direction, y - j, armOuterThick, true))
                        Main.tile[x - i * direction, y - j].ResetToType(CactusType);
                }
            }

            int offX = armXOffsets[Math.Clamp(j, 0, armHeight - 1)];
            for (int i = -armHalfWidth - armOuterThick; i < armHalfWidth + armOuterThick; i++)
            {
                if (!WorldGen.InWorld(x + i + offX, y - j))
                    continue;

                if (CanPlaceCactusOutline(x + i + offX, y - j, armOuterThick, true))
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
            }
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
                validCount++;
            else if (Main.tile[x + r * direction, y].HasTile && validCount < 1)
                solidCount++;
            else
                break;

            r++;
            if (validCount > max)
            {
                valid = false;
                break;
            }
        }

        if (!valid)
            return;

        for (int i = 0; i < validCount; i++)
        {
            if (!WorldGen.SolidTile(x + (i + solidCount) * direction, y))
                Main.tile[x + (i + solidCount) * direction, y].ResetToType(PlatformType);
        }
    }

    private static void PlaceLootChest(int x, int y, int width = 4)
    {
        TryPlacePlatform(x - width / 2, y, 1, 5);
        WorldGen.PlaceChest(x - 1, y - 1, style: 10);
    }

    private static void PlacePotsEverywhere(int x, int y, int radius)
    {
        for (int i = x - radius; i < x + radius; i++)
        {
            for (int j = y - radius; j < y + radius; j++)
            {
                if (WorldGen.genRand.NextBool(7))
                    WorldGen.PlacePot(i, j, PotType);
            }
        }
    }

    private static void GenerateRoot(int x, int y, double size)
    {

    }
}
