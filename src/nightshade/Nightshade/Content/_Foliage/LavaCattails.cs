using Daybreak.Common.Features.Tiles;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content;

public sealed class LavaCattails : CattailTile, IPlaceTileHook
{
    private sealed class LavaCattailGlobalTile : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            base.RandomUpdate(i, j, type);

            if (Main.tile[i, j].LiquidAmount <= 32 || Main.tile[i, j].LiquidType != LiquidID.Lava || !WorldGen.genRand.NextBool(2))
            {
                return;
            }

            WorldGen.PlaceTile(i, j, ModContent.TileType<LavaCattails>());

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendTileSquare(-1, i, j);
            }
        }
    }

    public override string Texture => Assets.Images.Tiles.Misc.LavaCattails.KEY;

    public override void CheckCattail(int x, int j)
    {
        var num = j;
        var flag = false;
        var num2 = num;
        while ((!Main.tile[x, num2].active() || !Main.tileSolid[Main.tile[x, num2].type] || Main.tileSolidTop[Main.tile[x, num2].type]) && num2 < Main.maxTilesY - 50)
        {
            if (Main.tile[x, num2].active() && Main.tile[x, num2].type != Type)
            {
                flag = true;
            }

            if (!Main.tile[x, num2].active())
            {
                break;
            }

            num2++;
            /*if (Main.tile[x, num2] == null)
            {
                return;
            }*/
        }

        num = num2 - 1;
        /*if (Main.tile[x, num] == null)
        {
            return;
        }*/

        while ( /*Main.tile[x, num] != null &&*/ Main.tile[x, num].liquid > 0 && num > 50)
        {
            if ((Main.tile[x, num].active() && Main.tile[x, num].type != Type) || Main.tile[x, num].liquidType() != LiquidID.Lava)
            {
                flag = true;
            }

            num--;
            /*if (Main.tile[x, num] == null)
            {
                return;
            }*/
        }

        num++;
        /*if (Main.tile[x, num] == null)
        {
            return;
        }*/

        var num3 = num;
        var num4 = WorldGen.catTailDistance;
        if (num2 - num3 > num4)
        {
            flag = true;
        }

        var num5 = 0;
        /*int type = Main.tile[x, num2].type;
        var num5 = -1;
        switch (type)
        {
            case 2:
            case 477:
                num5 = 0;
                break;

            case 53:
                num5 = 18;
                break;

            case 199:
            case 234:
            case 662:
                num5 = 54;
                break;

            case 23:
            case 112:
            case 661:
                num5 = 72;
                break;

            case 70:
                num5 = 90;
                break;
        }*/

        if (!Main.tile[x, num2].nactive())
        {
            flag = true;
        }

        if (num5 < 0)
        {
            flag = true;
        }

        num = num2 - 1;
        if ( /*Main.tile[x, num] != null &&*/ !Main.tile[x, num].active())
        {
            for (var num6 = num; num6 >= num3; num6--)
            {
                /*if (Main.tile[x, num6] == null)
                {
                    return;
                }*/

                if (Main.tile[x, num6].active() && Main.tile[x, num6].type == Type)
                {
                    num = num6;
                    break;
                }
            }
        }

        while ( /*Main.tile[x, num] != null &&*/ Main.tile[x, num].active() && Main.tile[x, num].type == Type)
        {
            num--;
        }

        num++;
        if ( /*Main.tile[x, num2 - 1] != null &&*/ Main.tile[x, num2 - 1].liquid < 127 && WorldGen.genRand.NextBool(4))
        {
            flag = true;
        }

        if ( /*Main.tile[x, num] != null &&*/ Main.tile[x, num].frameX >= 180 && Main.tile[x, num].liquid > 127 && WorldGen.genRand.NextBool(4))
        {
            flag = true;
        }

        if ( /*Main.tile[x, num] != null && Main.tile[x, num2 - 1] != null &&*/ Main.tile[x, num].frameX > 18)
        {
            if (Main.tile[x, num2 - 1].frameX < 36 || Main.tile[x, num2 - 1].frameX > 72)
            {
                flag = true;
            }
            else if (Main.tile[x, num].frameX < 90)
            {
                flag = true;
            }
            else if (Main.tile[x, num].frameX >= 108 && Main.tile[x, num].frameX <= 162)
            {
                Main.tile[x, num].frameX = 90;
            }
        }

        if (num2 > num + 4 /*&& Main.tile[x, num + 4] != null && Main.tile[x, num + 3] != null*/ && Main.tile[x, num + 4].liquid == 0 && Main.tile[x, num + 3].type == Type)
        {
            flag = true;
        }

        if (flag)
        {
            var num7 = num3;
            if (num < num3)
            {
                num7 = num;
            }

            num7 -= 4;
            for (var i = num7; i <= num2; i++)
            {
                if ( /*Main.tile[x, i] != null &&*/ Main.tile[x, i].active() && Main.tile[x, i].type == Type)
                {
                    WorldGen.KillTile(x, i);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, i);
                    }

                    WorldGen.SquareTileFrame(x, i);
                }
            }
        }
        else
        {
            if (num5 == Main.tile[x, num].frameY)
            {
                return;
            }

            for (var k = num; k < num2; k++)
            {
                if ( /*Main.tile[x, k] != null &&*/ Main.tile[x, k].active() && Main.tile[x, k].type == Type)
                {
                    Main.tile[x, k].frameY = (short)num5;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, x, num);
                    }
                }
            }
        }
    }

    public void PlaceTile(int i, int j, bool mute, bool forced, int plr, int style)
    {
        PlaceTile(i, j);
    }

    public Point PlaceTile(int x, int j)
    {
        var num = j;
        var result = new Point(-1, -1);
        if (x < 50 || x > Main.maxTilesX - 50 || num < 50 || num > Main.maxTilesY - 50)
        {
            return result;
        }

        if ((Main.tile[x, num].active() /*&& Main.tile[x, num].type != 71*/) || Main.tile[x, num].liquid == 0 || Main.tile[x, num].liquidType() != LiquidID.Lava)
        {
            return result;
        }

        while (Main.tile[x, num].liquid > 0 && num > 50)
        {
            num--;
        }

        num++;
        if (Main.tile[x, num].active() || Main.tile[x, num - 1].active() || Main.tile[x, num].liquid == 0 || Main.tile[x, num].liquidType() != LiquidID.Lava)
        {
            return result;
        }

        /*if (Main.tile[x, num].wall != 0 && Main.tile[x, num].wall != 80 && Main.tile[x, num].wall != 81 && Main.tile[x, num].wall != 69 && (Main.tile[x, num].wall < 63 || Main.tile[x, num].wall > 68))
        {
            return result;
        }*/

        var num2 = 7;
        var num3 = 0;
        for (var i = x - num2; i <= x + num2; i++)
        {
            for (var k = num - num2; k <= num + num2; k++)
            {
                if (Main.tile[i, k].active() && Main.tile[i, k].type == Type)
                {
                    num3++;
                    break;
                }
            }
        }

        if (num3 > 3)
        {
            return result;
        }

        int l;
        for (l = num; (!Main.tile[x, l].active() || !Main.tileSolid[Main.tile[x, l].type] || Main.tileSolidTop[Main.tile[x, l].type]) && l < Main.maxTilesY - 50; l++)
        {
            if (Main.tile[x, l].active() /*&& Main.tile[x, l].type != 71*/)
            {
                return result;
            }
        }

        var num4 = WorldGen.catTailDistance - 1;
        if (l - num > num4)
        {
            return result;
        }

        if (l - num < 2)
        {
            return result;
        }

        // int type = Main.tile[x, l].type;
        if (!Main.tile[x, l].nactive())
        {
            return result;
        }

        var num5 = 0;
        // var num5 = -1;
        /*switch (type)
        {
            case 2:
            case 477:
                num5 = 0;
                break;

            case 53:
                if (x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance)
                {
                    return result;
                }
                num5 = 18;
                break;

            case 199:
            case 234:
            case 662:
                num5 = 54;
                break;

            case 23:
            case 112:
            case 661:
                num5 = 72;
                break;

            case 70:
                num5 = 90;
                break;
        }*/

        if (num5 < 0)
        {
            return result;
        }

        if (Main.tile[x, l].topSlope() && WorldGen.gen && !WorldGen.genRand.NextBool(3))
        {
            Main.tile[x, l].slope(0);
        }
        else if (Main.tile[x, l].topSlope() || Main.tile[x, l].halfBrick())
        {
            return result;
        }

        num = l - 1;
        Main.tile[x, num].active(active: true);
        Main.tile[x, num].type = Type;
        Main.tile[x, num].frameX = 0;
        Main.tile[x, num].frameY = (short)num5;
        Main.tile[x, num].halfBrick(halfBrick: false);
        Main.tile[x, num].slope(0);
        Main.tile[x, num].CopyPaintAndCoating(Main.tile[x, num + 1]);
        WorldGen.SquareTileFrame(x, num);
        return new Point(x, num);
    }
}