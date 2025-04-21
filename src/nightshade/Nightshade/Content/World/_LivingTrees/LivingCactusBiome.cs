using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Content.Tiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

public class LivingCactusBiome : MicroBiome
{
    private static ushort CactusType => (ushort)ModContent.TileType<LivingCactus>();
    private static ushort WoodType => (ushort)ModContent.TileType<LivingCactusWood>();
    private static ushort PlatformType => (ushort)TileID.Platforms;
    private static ushort WoodWallType => (ushort)WallID.LivingWood;
    private static ushort PotType => (ushort)ModContent.TileType<LivingCactusPot>();

    public override bool Place(Point origin, StructureMap structures)
    {
        int height = WorldGen.genRand.Next(40, 55);
        int width = height / 2;

        if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid().AreaAnd(6, 2), new Conditions.IsTile(TileID.Sand, TileID.HardenedSand, TileID.Sandstone)), out origin))
            return false;

        Rectangle cactusBounds = new Rectangle(origin.X - width / 2 - 5, origin.Y - height / 2 - 5, width + 10, height + 10);

        if (!structures?.CanPlace(cactusBounds) ?? false)
            return false;

        GenerateLivingCactus(origin.X, origin.Y, height);
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

    public static void GenerateLivingCactus(int x, int y, int height)
    {
        const int stalkHalfWidth = 4;
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
                offsetX += WorldGen.genRand.Next(0, 2) * stalkCurveDir;

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
                    timeUntilArm = WorldGen.genRand.Next(3, height / 3);
            }

            for (int i = -stalkHalfWidth; i < stalkHalfWidth; i++)
            {
                int offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * stalkCurveDir * (mainStalkCurvature / 3 + 1), 0, stalkXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                    continue;

                bool placeWide = offX != stalkXOffsets[j];

                bool place = true;
                if (i > -stalkHalfWidth && i < stalkHalfWidth - 1)
                    place = false;

                if (j > height - stalkHalfWidth)
                {
                    int normJ = j - height + stalkHalfWidth;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    bool withinCap = distance < stalkHalfWidth - 0.5;
                    if (withinCap)
                        place = distance > stalkHalfWidth - 1.75;
                    else
                        continue;
                }

                if (place)
                    Main.tile[x + i + offX, y - j].ResetToType(WoodType);
                else
                    Main.tile[x + i + offX, y - j].ClearTile();

                Main.tile[x + i + offX, y - j].WallType = WoodWallType;

                if (placeWide)
                {
                    if (Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].TileType != WoodType)
                        Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].ClearTile();

                    Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].WallType = WoodWallType;
                }
            }
        }

        PlaceLootChest(x + offsetX, y - height + stalkHalfWidth * 2, stalkHalfWidth);

        const int stalkOuterThick = 2;
        for (int j = 0; j < height + stalkOuterThick; j++)
        {
            for (int i = -stalkHalfWidth - stalkOuterThick; i < stalkHalfWidth + stalkOuterThick; i++)
            {
                int offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * stalkCurveDir, 0, stalkXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                    continue;

                if (CanPlaceCactusOutline(x + i + offX, y - j, stalkOuterThick, false))
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
            }
        }

        foreach (Point point in arms)
        {
            int armDirection = Math.Sign(point.X - x);
            int offsetLength = Math.Abs(stalkXOffsets[0] - stalkXOffsets[^1]);
            int passageLength = Math.Abs(point.X - x);
            GenerateArm(point.X, point.Y, armDirection, passageLength, WorldGen.genRand.Next(15, 20));
        }

        for (int i = -2; i < 3; i++)
            GenerateRoot(x + i, y, 2.3, 13.0, new Vector2(i, 2.5f / (Math.Abs(i) + 1)));

        WorldUtils.Gen(new Point(x, y - 1), new Shapes.HalfCircle(4), new Actions.SetLiquid(LiquidID.Water));

        PlacePotsEverywhere(x, y - height / 5, 32);
    }

    private static void GenerateArm(int x, int y, int direction, int passageLength, int armHeight)
    {
        int armCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();
        int armCurvature = WorldGen.genRand.Next(4, 8);
        int offsetX = 0;
        int[] armXOffsets = new int[armHeight];

        const int armHalfWidth = 3;
        const int passageHeight = 5;

        for (int j = 0; j < armHeight; j++)
        {
            if (j % armCurvature == 0 && j > passageHeight + 2 && j < armHeight * 3 / 4)
                offsetX += WorldGen.genRand.Next(0, 2) * armCurveDir;

            armXOffsets[j] = offsetX;
        }

        for (int j = 0; j < armHeight; j++)
        {
            if (j < passageHeight)
            {
                for (int i = 1; i < passageLength - 1; i++)
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
                int offX = armXOffsets[Math.Clamp(j + 2 * Math.Sign(i) * armCurveDir, 0, armXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                    continue;

                bool placeWide = offX != armXOffsets[j];

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

                if (place && Main.tile[x + i + offX, y - j].WallType != WoodWallType)
                    Main.tile[x + i + offX, y - j].ResetToType(WoodType);
                else
                    Main.tile[x + i + offX, y - j].ClearTile();

                Main.tile[x + i + offX, y - j].WallType = WoodWallType;

                if (placeWide)
                {
                    if (Main.tile[x + i + armXOffsets[j], y - j].TileType != WoodType)
                        Main.tile[x + i + armXOffsets[j], y - j].ClearTile();

                    Main.tile[x + i + armXOffsets[j], y - j].WallType = WoodWallType;
                }
            }
        }

        if (!WorldGen.genRand.NextBool(10))
        {
            TryPlacePlatform(x + offsetX, y - armHeight + armHalfWidth * 2, -direction, armHalfWidth);
            TryPlacePlatform(x + offsetX, y - armHeight + armHalfWidth * 2, direction, armHalfWidth);
            WorldGen.PlaceTile(x + offsetX - 1, y - armHeight + armHalfWidth * 2 - 1, PotType, true);
        }

        const int armOuterThick = 2;
        for (int j = -armOuterThick; j < armHeight + armOuterThick; j++)
        {
            if (j < passageHeight + armOuterThick)
            {
                for (int i = -1; i < passageLength; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                        continue;

                    if (CanPlaceCactusOutline(x - i * direction, y - j, armOuterThick, false))
                        Main.tile[x - i * direction, y - j].ResetToType(CactusType);
                }
            }

            for (int i = -armHalfWidth - armOuterThick; i < armHalfWidth + armOuterThick; i++)
            {
                int offX = armXOffsets[Math.Clamp(j + 2 * Math.Sign(i) * armCurveDir, 0, armXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                    continue;

                if (CanPlaceCactusOutline(x + i + offX, y - j, armOuterThick, false))
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
            }
        }

        // 8 = 2 * stalkHalfWidth, should be fine for varied size
        TryPlacePlatform(x - passageLength / 2 * direction, y, -direction, 8);
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
                        GenerateRoot((int)xi + i, (int)yi + j, size / 3.0, distance / 3, new Vector2(direction.X * 0.5f, direction.Y));
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
                    WorldGen.PlacePot(i, j, PotType);
            }
        }
    }
}
