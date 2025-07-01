using Daybreak.Common.Features.Tiles;

using Terraria;
using Terraria.ID;

namespace Nightshade.Content;

public sealed class LavaLilyPads : LilyPadTile
{
    public override string Texture { get; }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        const int liquid_type = LiquidID.Lava;
        
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            return false;
        }

        if (Main.tile[i, j].liquidType() != liquid_type)
        {
            WorldGen.KillTile(i, j);
            
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
            }

            return false;
        }

        var num = j;
        while ((!Main.tile[i, num].active() || !Main.tileSolid[Main.tile[i, num].type] || Main.tileSolidTop[Main.tile[i, num].type]) && num < Main.maxTilesY - 50)
        {
            num++;
        }

        // Vanilla logic for determining the variant.  We only have one.
        var num2 = 0;
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
            if (num2 != Main.tile[i, j].frameY)
            {
                Main.tile[i, j].frameY = (short)num2;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, i, j);
                }
            }

            if (Main.tile[i, j - 1].liquid > 0 && !Main.tile[i, j - 1].active())
            {
                Main.tile[i, j - 1].active(active: true);
                Main.tile[i, j - 1].type = Type;
                Main.tile[i, j - 1].frameX = Main.tile[i, j].frameX;
                Main.tile[i, j - 1].frameY = Main.tile[i, j].frameY;
                Main.tile[i, j - 1].halfBrick(halfBrick: false);
                Main.tile[i, j - 1].slope(0);
                Main.tile[i, j].active(active: false);
                Main.tile[i, j].type = 0;
                WorldGen.SquareTileFrame(i, j - 1, resetFrame: false);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, i, j - 1, 1, 2);
                }
            }
            else
            {
                if (Main.tile[i, j].liquid != 0)
                {
                    return false;
                }

                var tileSafely = Framing.GetTileSafely(i, j + 1);
                if (!tileSafely.active())
                {
                    Main.tile[i, j + 1].active(active: true);
                    Main.tile[i, j + 1].type = Type;
                    Main.tile[i, j + 1].frameX = Main.tile[i, j].frameX;
                    Main.tile[i, j + 1].frameY = Main.tile[i, j].frameY;
                    Main.tile[i, j + 1].halfBrick(halfBrick: false);
                    Main.tile[i, j + 1].slope(0);
                    Main.tile[i, j].active(active: false);
                    Main.tile[i, j].type = 0;
                    WorldGen.SquareTileFrame(i, j + 1, resetFrame: false);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 1, 2);
                    }
                }
                else if (tileSafely.active() && !TileID.Sets.Platforms[tileSafely.type] && (!Main.tileSolid[tileSafely.type] || Main.tileSolidTop[tileSafely.type]))
                {
                    WorldGen.KillTile(i, j);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                    }
                }
            }
        }
        else
        {
            WorldGen.KillTile(i, j);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
            }
        }

        return false;
    }
}