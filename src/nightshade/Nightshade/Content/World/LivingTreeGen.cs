using Microsoft.Xna.Framework;
using Nightshade.Content.Items;
using Nightshade.Content.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

public class LivingTreeGen : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
    }

    public override void PostAddRecipes()
    {
        if (ModLoader.TryGetMod("BlockarozToolkit", out Mod toolkit))
            toolkit.Call("NewDebug", Mod, "Enable tree generation", ItemID.Acorn, () =>
            {
                Point point = Main.MouseWorld.ToTileCoordinates();

                Dust d = Dust.NewDustPerfect(point.ToWorldCoordinates(), DustID.t_Cactus, Main.rand.NextVector2Circular(1, 1), Scale: 0.5f);
                d.fadeIn = 1.5f;
                d.noGravity = true;

                if (Main.mouseRight && Main.mouseRightRelease && !Main.mapFullscreen)
                {
                    GenerateLivingCactus(point.X, point.Y);

                    for (int i = 0; i < 15; i++)
                    {
                        Dust e = Dust.NewDustPerfect(point.ToWorldCoordinates(), DustID.t_Cactus, Main.rand.NextVector2Circular(4, 4), Scale: 2f);
                        e.fadeIn = 2f;
                        e.noGravity = true;
                    }
                }
            }, true);
    }

    public static void PlacePlatformsAlongWidth(int x, int y)
    {

    }

    public static void GenerateLivingCactus(int x, int y)
    {
        ushort cactusType = (ushort)ModContent.TileType<LivingCactus>();
        ushort woodType = (ushort)ModContent.TileType<LivingCactusWood>();
        ushort platformType = (ushort)TileID.Platforms;
        ushort woodWallType = (ushort)WallID.LivingWood;

        const int CactusStalkThick = 2;
        const int CactusArmThick = 1;


        const int mainHalfWidth = 5;
        int mainHeight = WorldGen.genRand.Next(50, 65);
        int mainStalkCurvature = WorldGen.genRand.Next(9, 15); // Higher is less curved
        int mainStalkCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();

        List<Point> armPoints = new List<Point>();
        List<int> armHeights = new List<int>();
        List<int[]> armXOffsets = new List<int[]>();

        int offsetX = 0;
        int[] xOffsets = new int[mainHeight];
        int timeUntilArm = WorldGen.genRand.Next(8, mainHeight / 3);
        int lastArmDirection = WorldGen.genRand.NextBool().ToDirectionInt();
        for (int j = 0; j < mainHeight; j++)
        {
            if (j % mainStalkCurvature == 0 && j > 0 && j < mainHeight * 3 / 4)
                offsetX += WorldGen.genRand.Next(0, 2) * mainStalkCurveDir;

            xOffsets[j] = offsetX;
            timeUntilArm--;
            if (timeUntilArm == 0 && armPoints.Count < 2)
            {
                armHeights.Add(WorldGen.genRand.Next(20, 35));
                armPoints.Add(new Point(x + offsetX - WorldGen.genRand.Next(mainHalfWidth * 2 + 1, mainHalfWidth * 2 + 5) * lastArmDirection, y - j));
                timeUntilArm = WorldGen.genRand.NextBool(3) ? WorldGen.genRand.Next(1, 20) : -1;
                lastArmDirection *= -1;
            }

            for (int i = -mainHalfWidth; i < mainHalfWidth; i++)
            {
                if (!WorldGen.InWorld(x + i + offsetX, y - j))
                    continue;

                if (j > mainHeight - mainHalfWidth)
                {
                    int normI = i > 0 ? i + 1 : i;
                    int normJ = j - mainHeight + mainHalfWidth;
                    bool withinCap = MathF.Sqrt(normI * normI + normJ * normJ) < mainHalfWidth;
                    if (!withinCap)
                        continue;
                }

                Main.tile[x + i + offsetX, y - j].ResetToType(cactusType);
            }
        }

        const int armHalfWidth = 3;
        const int armConnectThickness = 7;
        for (int k = 0; k < armPoints.Count; k++)
        {
            Point p = armPoints[k];
            int armHeight = armHeights[k];
            int armOffX = 0;
            armXOffsets.Add(new int[armHeight]);
            int armCurvature = WorldGen.genRand.Next(12, 25); // Higher is less curved
            int armCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();

            int dir = Math.Sign(x - p.X);
            for (int j = 0; j < armConnectThickness; j++)
            {
                for (int i = 0; i < Math.Abs(x - p.X); i++)
                {
                    int fixedX = dir > 0 ? p.X - 1 : p.X;

                    if (!WorldGen.InWorld(fixedX + i * dir, p.Y - j))
                        continue;

                    Main.tile[fixedX + i * dir, p.Y - j].ResetToType(cactusType);
                }
            }

            for (int j = 0; j < armHeight; j++)
            {
                if (j % armCurvature == 0 && j > 0 && j < armHeight * 3 / 4)
                    armOffX += WorldGen.genRand.Next(0, 2) * armCurveDir;

                armXOffsets[k][j] = armOffX;
                for (int i = -armHalfWidth; i < armHalfWidth; i++)
                {
                    int fixedX = dir > 0 ? p.X - 1 : p.X;

                    if (!WorldGen.InWorld(fixedX + i + armOffX, p.Y - j))
                        continue;

                    int normI = i > 0 ? i + 1 : i;
                    if (j < armHalfWidth)
                    {
                        int normJ = j - armHalfWidth;
                        bool withinCap = Math.Sqrt(normI * normI + normJ * normJ) < armHalfWidth;
                        if (!withinCap)
                            continue;
                    }
                    if (j > armHeight - armHalfWidth)
                    {
                        int normJ = j - armHeight + armHalfWidth;
                        bool withinCap = Math.Sqrt(normI * normI + normJ * normJ) < armHalfWidth;
                        if (!withinCap)
                            continue;
                    }

                    Main.tile[fixedX + i + armOffX, p.Y - j].ResetToType(cactusType);
                }
            }
        }

        // Hollowing
        for (int j = 0; j < mainHeight - CactusStalkThick; j++)
        {
            for (int i = -mainHalfWidth + CactusStalkThick; i < mainHalfWidth - CactusStalkThick; i++)
            {
                if (!WorldGen.InWorld(x + i + xOffsets[j], y - j))
                    continue;

                int normI = i > 0 ? i + 1 : i;

                bool place = false;
                if (Math.Abs(normI) > mainHalfWidth - 3)
                    place = true;

                if (j > mainHeight - mainHalfWidth)
                {
                    int normJ = j - mainHeight + mainHalfWidth;
                    bool withinCap = Math.Sqrt(normI * normI + normJ * normJ) < mainHalfWidth - CactusStalkThick;
                    if (!withinCap)
                        continue;
                    else
                        place = true;
                }

                if (place)
                    Main.tile[x + i + xOffsets[j], y - j].ResetToType(woodType);
                else
                    Main.tile[x + i + xOffsets[j], y - j].ClearTile();

                Main.tile[x + i + xOffsets[j], y - j].WallType = woodWallType;
            }
        }

        for (int k = 0; k < armPoints.Count; k++)
        {
            Point p = armPoints[k];
            int armHeight = armHeights[k];

            int dir = Math.Sign(x - p.X);
            for (int j = CactusArmThick; j < armConnectThickness - CactusArmThick; j++)
            {
                bool place = true;
                if (Math.Abs(j - armConnectThickness / 2) < 2)
                    place = false;

                for (int i = 1; i < Math.Abs(x - p.X) + 1; i++)
                {
                    int fixedX = dir > 0 ? p.X - 1 : p.X;

                    if (!WorldGen.InWorld(fixedX + i * dir + xOffsets[j], p.Y - j))
                        continue;

                    if (place && Main.tile[fixedX + i * dir + xOffsets[j], p.Y - j].HasTile)
                        Main.tile[fixedX + i * dir + xOffsets[j], p.Y - j].ResetToType(woodType);
                    else
                        Main.tile[fixedX + i * dir + xOffsets[j], p.Y - j].ClearTile();

                    Main.tile[fixedX + i * dir + xOffsets[j], p.Y - j].WallType = woodWallType;
                }
            }

            for (int j = CactusArmThick; j < armHeight - CactusArmThick; j++)
            {
                for (int i = -armHalfWidth + CactusArmThick; i < armHalfWidth - CactusArmThick; i++)
                {
                    if (!WorldGen.InWorld(p.X + i + armXOffsets[k][j], p.Y - j))
                        continue;

                    int fixedX = dir > 0 ? p.X - 1 : p.X;
                    int normI = i > 0 ? i + 1 : i;

                    bool place = false;
                    if (Math.Abs(normI) > armHalfWidth - 2)
                        place = true;

                    if (j <= armHalfWidth)
                    {
                        int normJ = j - armHalfWidth;
                        double distance = MathF.Sqrt(normI * normI + normJ * normJ);
                        bool withinCap = distance < armHalfWidth - CactusArmThick;
                        if (!withinCap)
                            continue;
                        else
                            place = dir < 0 || distance > armHalfWidth - CactusArmThick - 2;
                    }
                    if (j > armHeight - armHalfWidth)
                    {
                        int normJ = j - armHeight + armHalfWidth;
                        double distance = MathF.Sqrt(normI * normI + normJ * normJ);
                        bool withinCap = distance < armHalfWidth - CactusArmThick;
                        if (!withinCap)
                            continue;
                        else
                            place = true; // distance > armHalfWidth - CactusArmThick - 2;
                    }

                    if (place && Main.tile[fixedX + i + armXOffsets[k][j], p.Y - j].HasTile)
                        Main.tile[fixedX + i + armXOffsets[k][j], p.Y - j].ResetToType(woodType);
                    else
                    {
                        Main.tile[fixedX + i + armXOffsets[k][j], p.Y - j].ClearTile();
                        if (j == armHeight - CactusArmThick * 2)
                        {

                        }
                    }

                    Main.tile[fixedX + i + armXOffsets[k][j], p.Y - j].WallType = woodWallType;
                }
            }
        }
    }
}
