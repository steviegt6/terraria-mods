using System.Diagnostics.CodeAnalysis;

using Daybreak.Common.CIL;
using Daybreak.Common.Features.Hooks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace Daybreak.Common.Features.Tiles;

/// <summary>
///     Behaves like a Terraria lily pad.
/// </summary>
public interface ILilyPad
{
    /// <summary>
    ///     Whether to use vanilla's <see cref="Main.DrawTileInWater"/> logic.
    /// </summary>
    /// <remarks>
    ///     If enabled, this also disabled regular solid tile drawing.
    /// </remarks>
    bool VanillaDrawTileInWater { get; }
}

/// <summary>
///     Feature-complete lily pad implementation.
/// </summary>
public abstract class LilyPadTile : ModTile, ILilyPad
{
    bool ILilyPad.VanillaDrawTileInWater => true;

    /// <inheritdoc />
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        TileID.Sets.TileCutIgnore.Regrowth[Type] = true;

        Main.tileCut[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoFail[Type] = true;

        On_TileDrawing.GetTileDrawData += GetTileDrawData_UseLilyPadDrawData;
    }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        WorldGen.CheckLilyPad(i, j);
        return false;
    }

    private void GetTileDrawData_UseLilyPadDrawData(
        On_TileDrawing.orig_GetTileDrawData orig,
        TileDrawing self,
        int x,
        int y,
        Tile tileCache,
        ushort typeCache,
        ref short tileFrameX,
        ref short tileFrameY,
        out int tileWidth,
        out int tileHeight,
        out int tileTop,
        out int halfBrickHeight,
        out int addFrX,
        out int addFrY,
        out SpriteEffects tileSpriteEffect,
        out Texture2D glowTexture,
        out Rectangle glowSourceRect,
        out Color glowColor
    )
    {
        if (typeCache == Type)
        {
            typeCache = TileID.LilyPad;
        }

        orig(
            self,
            x,
            y,
            tileCache,
            typeCache,
            ref tileFrameX,
            ref tileFrameY,
            out tileWidth,
            out tileHeight,
            out tileTop,
            out halfBrickHeight,
            out addFrX,
            out addFrY,
            out tileSpriteEffect,
            out glowTexture,
            out glowSourceRect,
            out glowColor
        );
    }
}

internal partial static class WaterFoliageHandler
{
    [OnLoad]
    public static void Load_LilyPads()
    {
        On_WorldGen.PlaceLilyPad += PlaceLilyPad;
        On_WorldGen.CheckLilyPad += CheckLilyPad;

        IL_TileDrawing.DrawSingleTile += DrawSingleTile_SkipDrawingForLilyPads;
        IL_Main.DrawTileInWater += DrawTileInWater_UseLilyPadDrawLogicForAllLilyPads;
        
        IL_Liquid.DelWater += DelWater_CheckLilyPads;
    }

    private static bool PlaceLilyPad(On_WorldGen.orig_PlaceLilyPad orig, int x, int j)
    {
        int num = j;
        if (x < 50 || x > Main.maxTilesX - 50 || num < 50 || num > Main.maxTilesY - 50)
            return false;

        if (Main.tile[x, num].active() || Main.tile[x, num].liquid == 0 || Main.tile[x, num].liquidType() != 0)
            return false;

        while (Main.tile[x, num].liquid > 0 && num > 50)
        {
            num--;
        }

        num++;
        if (Main.tile[x, num].active() || Main.tile[x, num - 1].active() || Main.tile[x, num].liquid == 0 || Main.tile[x, num].liquidType() != 0)
            return false;

        if (Main.tile[x, num].wall != 0 && Main.tile[x, num].wall != 15 && Main.tile[x, num].wall != 70 && (Main.tile[x, num].wall < 63 || Main.tile[x, num].wall > 68))
            return false;

        int num2 = 5;
        int num3 = 0;
        for (int i = x - num2; i <= x + num2; i++)
        {
            for (int k = num - num2; k <= num + num2; k++)
            {
                if (Main.tile[i, k].active() && Main.tile[i, k].type == 518)
                    num3++;
            }
        }

        if (num3 > 3)
            return false;

        int l;
        for (l = num; (!Main.tile[x, l].active() || !Main.tileSolid[Main.tile[x, l].type] || Main.tileSolidTop[Main.tile[x, l].type]) && l < Main.maxTilesY - 50; l++)
        {
            if (Main.tile[x, l].active() && Main.tile[x, l].type == 519)
                return false;
        }

        int num4 = 12;
        if (l - num > num4)
            return false;

        if (l - num < 3)
            return false;

        int type = Main.tile[x, l].type;
        int num5 = -1;
        if (type == 2 || type == 477)
            num5 = 0;

        if (type == 109 || type == 109 || type == 116)
            num5 = 18;

        if (type == 60)
            num5 = 36;

        if (num5 < 0)
            return false;

        Main.tile[x, num].active(active: true);
        Main.tile[x, num].type = 518;
        if (genRand.Next(2) == 0)
        {
            Main.tile[x, num].frameX = (short)(18 * genRand.Next(3));
        }
        else if (genRand.Next(15) == 0)
        {
            Main.tile[x, num].frameX = (short)(18 * genRand.Next(18));
        }
        else
        {
            int num6 = Main.maxTilesX / 5;
            if (x < num6)
                Main.tile[x, num].frameX = (short)(18 * genRand.Next(6, 9));
            else if (x < num6 * 2)
                Main.tile[x, num].frameX = (short)(18 * genRand.Next(9, 12));
            else if (x < num6 * 3)
                Main.tile[x, num].frameX = (short)(18 * genRand.Next(3, 6));
            else if (x < num6 * 4)
                Main.tile[x, num].frameX = (short)(18 * genRand.Next(15, 18));
            else
                Main.tile[x, num].frameX = (short)(18 * genRand.Next(12, 15));
        }

        Main.tile[x, num].frameY = (short)num5;
        Main.tile[x, num].halfBrick(halfBrick: false);
        Main.tile[x, num].slope(0);
        SquareTileFrame(x, num);
        return true;
    }

    private static void CheckLilyPad(On_WorldGen.orig_CheckLilyPad orig, int x, int y)
    {
        if (Main.netMode == 1)
            return;

        if (Main.tile[x, y].liquidType() != 0)
        {
            KillTile(x, y);
            if (Main.netMode == 2)
                NetMessage.SendData(17, -1, -1, null, 0, x, y);

            return;
        }

        int num = y;
        while ((!Main.tile[x, num].active() || !Main.tileSolid[Main.tile[x, num].type] || Main.tileSolidTop[Main.tile[x, num].type]) && num < Main.maxTilesY - 50)
        {
            num++;
            if (Main.tile[x, num] == null)
                return;
        }

        int type = Main.tile[x, num].type;
        int num2 = -1;
        if (type == 2 || type == 477)
            num2 = 0;

        if (type == 109 || type == 109 || type == 116)
            num2 = 18;

        if (type == 60)
            num2 = 36;

        if (num2 >= 0)
        {
            if (num2 != Main.tile[x, y].frameY)
            {
                Main.tile[x, y].frameY = (short)num2;
                if (Main.netMode == 2)
                    NetMessage.SendTileSquare(-1, x, y);
            }

            if (Main.tile[x, y - 1].liquid > 0 && !Main.tile[x, y - 1].active())
            {
                Main.tile[x, y - 1].active(active: true);
                Main.tile[x, y - 1].type = 518;
                Main.tile[x, y - 1].frameX = Main.tile[x, y].frameX;
                Main.tile[x, y - 1].frameY = Main.tile[x, y].frameY;
                Main.tile[x, y - 1].halfBrick(halfBrick: false);
                Main.tile[x, y - 1].slope(0);
                Main.tile[x, y].active(active: false);
                Main.tile[x, y].type = 0;
                SquareTileFrame(x, y - 1, resetFrame: false);
                if (Main.netMode == 2)
                    NetMessage.SendTileSquare(-1, x, y - 1, 1, 2);
            }
            else
            {
                if (Main.tile[x, y].liquid != 0)
                    return;

                Tile tileSafely = Framing.GetTileSafely(x, y + 1);
                if (!tileSafely.active())
                {
                    Main.tile[x, y + 1].active(active: true);
                    Main.tile[x, y + 1].type = 518;
                    Main.tile[x, y + 1].frameX = Main.tile[x, y].frameX;
                    Main.tile[x, y + 1].frameY = Main.tile[x, y].frameY;
                    Main.tile[x, y + 1].halfBrick(halfBrick: false);
                    Main.tile[x, y + 1].slope(0);
                    Main.tile[x, y].active(active: false);
                    Main.tile[x, y].type = 0;
                    SquareTileFrame(x, y + 1, resetFrame: false);
                    if (Main.netMode == 2)
                        NetMessage.SendTileSquare(-1, x, y, 1, 2);
                }
                else if (tileSafely.active() && !TileID.Sets.Platforms[tileSafely.type] && (!Main.tileSolid[tileSafely.type] || Main.tileSolidTop[tileSafely.type]))
                {
                    KillTile(x, y);
                    if (Main.netMode == 2)
                        NetMessage.SendData(17, -1, -1, null, 0, x, y);
                }
            }
        }
        else
        {
            KillTile(x, y);
            if (Main.netMode == 2)
                NetMessage.SendData(17, -1, -1, null, 0, x, y);
        }
    }

    private static void DrawSingleTile_SkipDrawingForLilyPads(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdcI4(TileID.LilyPad));
        c.Substitute(TileID.LilyPad, type => TileLoader.GetTile(type) is ILilyPad { VanillaDrawTileInWater: true });
    }
    
    private static void DrawTileInWater_UseLilyPadDrawLogicForAllLilyPads(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdcI4(TileID.LilyPad));
        c.Substitute(TileID.LilyPad, type => TileLoader.GetTile(type) is ILilyPad { VanillaDrawTileInWater: true });
    }
    
    private static void DelWater_CheckLilyPads(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdcI4(TileID.LilyPad));
        c.Substitute(TileID.LilyPad, type => TileLoader.GetTile(type) is ILilyPad);
    }
}