using System.Collections.Generic;

using Daybreak.Common.Features.Hooks;
using Daybreak.Common.Features.Tiles;

using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content;

public sealed class LavaLilyPads : LilyPadTile, IPlaceTileHook
{
    private sealed class LavaLilyPadGlobalTile : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            base.RandomUpdate(i, j, type);

            if (Main.tile[i, j].LiquidAmount <= 32 || Main.tile[i, j].LiquidType != LiquidID.Lava || !WorldGen.genRand.NextBool(600))
            {
                return;
            }

            WorldGen.PlaceTile(i, j, ModContent.TileType<LavaLilyPads>());

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendTileSquare(-1, i, j);
            }
        }
    }

    public override string Texture => Assets.Images.Tiles.Misc.LavaLilyPads.KEY;

    public override void Load()
    {
        base.Load();

        ModSystemHooks.ModifyWorldGenTasks.Event += GenPasses;
    }

    private static void GenPasses(ModSystem self, List<GenPass> tasks, ref double totalWeight)
    {
        var waterPlantsPass = tasks.FindIndex(x => x.Name == "Water Plants");
        if (waterPlantsPass == -1)
        {
            return;
        }

        tasks.Insert(
            waterPlantsPass + 1,
            new PassLegacy(
                "Lava Plants",
                (progress, _) =>
                {
                    progress.Message = Mods.Nightshade.WorldGen.Passes.LavaLilies.GetTextValue();

                    for (var y = 1; y < Main.maxTilesY; y++)
                    {
                        progress.Set(y / (double)Main.maxTilesY);

                        for (var x = 20; x < Main.maxTilesX - 20; x++)
                        {
                            if (WorldGen.genRand.NextBool(5) && Main.tile[x, y].liquid > 0)
                            {
                                if (!Main.tile[x, y].active())
                                {
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        WorldGen.PlaceTile(x, y, ModContent.TileType<LavaLilyPads>());
                                    }
                                    else
                                    {
                                        var point = ModContent.GetInstance<LavaCattails>().PlaceTile(x, y);
                                        if (WorldGen.InWorld(point.X, point.Y))
                                        {
                                            var num31 = WorldGen.genRand.Next(14);
                                            for (var num32 = 0; num32 < num31; num32++)
                                            {
                                                ModContent.GetInstance<LavaCattails>().GrowCattail(point.X, point.Y);
                                            }

                                            WorldGen.SquareTileFrame(point.X, point.Y);
                                        }
                                    }
                                }

                                /*if ((!Main.tile[x, y].active() || Main.tile[x, y].type == 61 || Main.tile[x, y].type == 74) && PlaceBamboo(x, y))
                                {
                                    int num33 = genRand.Next(10, 20);
                                    for (int num34 = 0; num34 < num33 && PlaceBamboo(x, y - num34); num34++) { }
                                }*/
                            }
                        }

                        /*int num35 = Main.UnderworldLayer;
                        while ((double)num35 > Main.worldSurface)
                        {
                            if (Main.tile[x, num35].type == 53 && genRand.Next(3) != 0)
                                GrowCheckSeaweed(x, num35);
                            else if (Main.tile[x, num35].type == 549)
                                GrowCheckSeaweed(x, num35);

                            num35--;
                        }*/
                    }
                }
            )
        );
    }

    public override void RandomUpdate(int i, int j)
    {
        base.RandomUpdate(i, j);

        if (Main.tile[i, j].liquid == 0 || (Main.tile[i, j].liquid / 16 >= 9 && WorldGen.SolidTile(i, j - 1)) || (Main.tile[i, j - 1].liquid > 0 && Main.tile[i, j - 1].active()))
        {
            WorldGen.KillTile(i, j);

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
            }
        }
        else
        {
            CheckLilyPad(i, j);
        }
    }

    public override void CheckLilyPad(int x, int y)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            return;
        }

        if (Main.tile[x, y].liquidType() != LiquidID.Lava)
        {
            WorldGen.KillTile(x, y);

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
            }

            return;
        }

        var num = y;
        while ((!Main.tile[x, num].active() || !Main.tileSolid[Main.tile[x, num].type] || Main.tileSolidTop[Main.tile[x, num].type]) && num < Main.maxTilesY - 50)
        {
            num++;
        }

        // Vanilla logic for determining the variant.  We only have one.
        var num2 = 0; // WorldGen.SolidTile(i, num) ? 0 : -1;
        /*var type = (int)Main.tile[i, num].type;
        var num2 = -1;
        if (type is 2 or 477)
        {
            num2 = 0;
        }

        if (type is 109 or 116)
        {
            num2 = 18;
        }

        if (type == 60)
        {
            num2 = 36;
        }*/

        if (num2 >= 0)
        {
            if (num2 != Main.tile[x, y].frameY)
            {
                Main.tile[x, y].frameY = (short)num2;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, x, y);
                }
            }

            if (Main.tile[x, y - 1].liquid > 0 && !Main.tile[x, y - 1].active())
            {
                Main.tile[x, y - 1].active(active: true);
                Main.tile[x, y - 1].type = (ushort)ModContent.TileType<LavaLilyPads>();
                Main.tile[x, y - 1].frameX = Main.tile[x, y].frameX;
                Main.tile[x, y - 1].frameY = Main.tile[x, y].frameY;
                Main.tile[x, y - 1].halfBrick(halfBrick: false);
                Main.tile[x, y - 1].slope(0);
                Main.tile[x, y].active(active: false);
                Main.tile[x, y].type = 0;
                WorldGen.SquareTileFrame(x, y - 1, resetFrame: false);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, x, y - 1, 1, 2);
                }
            }
            else
            {
                if (Main.tile[x, y].liquid != 0)
                {
                    return;
                }

                var tileSafely = Framing.GetTileSafely(x, y + 1);
                if (!tileSafely.active())
                {
                    Main.tile[x, y + 1].active(active: true);
                    Main.tile[x, y + 1].type = (ushort)ModContent.TileType<LavaLilyPads>();
                    Main.tile[x, y + 1].frameX = Main.tile[x, y].frameX;
                    Main.tile[x, y + 1].frameY = Main.tile[x, y].frameY;
                    Main.tile[x, y + 1].halfBrick(halfBrick: false);
                    Main.tile[x, y + 1].slope(0);
                    Main.tile[x, y].active(active: false);
                    Main.tile[x, y].type = 0;
                    WorldGen.SquareTileFrame(x, y + 1, resetFrame: false);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, x, y, 1, 2);
                    }
                }
                else if (tileSafely.active() && !TileID.Sets.Platforms[tileSafely.type] && (!Main.tileSolid[tileSafely.type] || Main.tileSolidTop[tileSafely.type]))
                {
                    WorldGen.KillTile(x, y);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
                    }
                }
            }
        }
        else
        {
            WorldGen.KillTile(x, y);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
            }
        }
    }

    public void PlaceTile(int x, int j, bool mute, bool forced, int plr, int style)
    {
        var num = j;
        if (x < 50 || x > Main.maxTilesX - 50 || num < 50 || num > Main.maxTilesY - 50)
        {
            return;
        }

        if (Main.tile[x, num].active() || Main.tile[x, num].liquid == 0 || Main.tile[x, num].liquidType() != LiquidID.Lava)
        {
            return;
        }

        while (Main.tile[x, num].liquid > 0 && num > 50)
        {
            num--;
        }

        num++;
        if (Main.tile[x, num].active() || Main.tile[x, num - 1].active() || Main.tile[x, num].liquid == 0 || Main.tile[x, num].liquidType() != LiquidID.Lava)
        {
            return;
        }

        /*if (Main.tile[x, num].wall != 0 && Main.tile[x, num].wall != 15 && Main.tile[x, num].wall != 70 && (Main.tile[x, num].wall < 63 || Main.tile[x, num].wall > 68))
        {
            return;
        }*/

        var num2 = 5;
        var num3 = 0;
        for (var i = x - num2; i <= x + num2; i++)
        {
            for (var k = num - num2; k <= num + num2; k++)
            {
                if (Main.tile[i, k].active() && Main.tile[i, k].type == ModContent.TileType<LavaLilyPads>())
                {
                    num3++;
                }
            }
        }

        if (num3 > 3)
        {
            return;
        }

        int l;
        for (l = num; (!Main.tile[x, l].active() || !Main.tileSolid[Main.tile[x, l].type] || Main.tileSolidTop[Main.tile[x, l].type]) && l < Main.maxTilesY - 50; l++)
        {
            if (Main.tile[x, l].active() && Main.tile[x, l].type == ModContent.TileType<LavaCattails>())
            {
                return;
            }
        }

        var num4 = 12;
        if (l - num > num4)
        {
            return;
        }

        if (l - num < 3)
        {
            return;
        }

        var num5 = 0; // WorldGen.SolidTile(x, l) ? 0 : -1;
        /*int type = Main.tile[x, l].type;
        var num5 = -1;
        if (type == 2 || type == 477)
        {
            num5 = 0;
        }

        if (type == 109 || type == 109 || type == 116)
        {
            num5 = 18;
        }

        if (type == 60)
        {
            num5 = 36;
        }

        if (num5 < 0)
        {
            return false;
        }*/

        Main.tile[x, num].active(active: true);
        Main.tile[x, num].type = (ushort)ModContent.TileType<LavaLilyPads>();

        var obsidian = j > Main.UnderworldLayer || WorldGen.genRand.NextBool(5);
        var flower = WorldGen.genRand.NextBool(3);

        Main.tile[x, num].frameX = (short)(WorldGen.genRand.Next(3) + (!obsidian).ToInt() * 3 + flower.ToInt() * 3 * 18);
        Main.tile[x, num].frameY = (short)num5;
        Main.tile[x, num].halfBrick(halfBrick: false);
        Main.tile[x, num].slope(0);
        WorldGen.SquareTileFrame(x, num);
    }
}