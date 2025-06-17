using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace Nightshade.Common.Features;

internal sealed class PotLootImpl : ModSystem
{
    public override void Load()
    {
        base.Load();

        // Vanilla logic is completely rewritten here in favor of directly
        // implementing everything Nightshade needs.
        On_WorldGen.CheckPot += CheckPot_EncodeVanillaAndModdedStyles;
        On_WorldGen.SpawnThingsFromPot += SpawnThingsFromPot_HandleVanillaAndModdedStyles;
    }

    private static void CheckPot_EncodeVanillaAndModdedStyles(On_WorldGen.orig_CheckPot orig, int i, int j, int type)
    {
        if (WorldGen.destroyObject)
        {
            return;
        }

        var startX = 0;
        var startY = j;
        for (startX += Main.tile[i, j].frameX / 18; startX > 1; startX -= 2) { }

        startX *= -1;
        startX += i;
        
        var frameY = Main.tile[i, j].frameY / 18;
        var styleIndex = 0;
        while (frameY > 1)
        {
            frameY -= 2;
            styleIndex++;
        }

        startY -= frameY;
        
        var dontHandleBreak = false;
        for (var x = startX; x < startX + 2; x++)
        {
            for (var y = startY; y < startY + 2; y++)
            {
                // if (Main.tile[x, y] == null)
                // {
                //     Main.tile[x, y] = new Tile();
                // }

                int frameXOffset;
                for (frameXOffset = Main.tile[x, y].frameX / 18; frameXOffset > 1; frameXOffset -= 2) { }

                if (!Main.tile[x, y].active() || Main.tile[x, y].type != type || frameXOffset != x - startX || Main.tile[x, y].frameY != (y - startY) * 18 + styleIndex * 36)
                {
                    dontHandleBreak = true;
                }
            }

            // if (Main.tile[x, startY + 2] == null)
            // {
            //     Main.tile[x, startY + 2] = new Tile();
            // }

            if (!WorldGen.SolidTile2(x, startY + 2))
            {
                dontHandleBreak = true;
            }
        }

        if (!dontHandleBreak)
        {
            return;
        }

        WorldGen.destroyObject = true;

        var drop = TileLoader.Drop(i, j, type);
        for (var m = startX; m < startX + 2; m++)
        {
            for (var n = startY; n < startY + 2; n++)
            {
                if (Main.tile[m, n].type == type && Main.tile[m, n].active())
                {
                    WorldGen.KillTile(m, n);
                }
            }
        }
        
        using (new Item.DisableNewItemMethod(!drop))
        {
            switch (styleIndex)
            {
                case 0:
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 51);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 52);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 53);
                    break;

                case 1:
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 166);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 167);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 168);
                    break;

                case 2:
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 169);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 170);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 171);
                    break;

                case 3:
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 172);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 173);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 174);
                    break;

                case 4:
                case 5:
                case 6:
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 197);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 198);
                    break;

                default:
                    if (styleIndex is >= 7 and <= 9)
                    {
                        Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 199);
                        Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 200);
                    }
                    else if (styleIndex is >= 10 and <= 12)
                    {
                        Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 201);
                        Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 202);
                    }
                    else if (styleIndex is >= 13 and <= 15)
                    {
                        Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 203);
                        Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 204);
                    }
                    else
                    {
                        if (styleIndex is >= 16 and <= 18 or >= 19 and <= 21 or >= 22 and <= 24)
                        {
                            break;
                        }

                        if (styleIndex is >= 25 and <= 27)
                        {
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(217, 220));
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(217, 220));
                        }
                        else if (styleIndex is >= 28 and <= 30)
                        {
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(315, 317));
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(315, 317));
                        }
                        else if (styleIndex is >= 31 and <= 33)
                        {
                            int num6 = WorldGen.genRand.Next(2, 5);
                            for (var num7 = 0; num7 < num6; num7++)
                            {
                                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 698 + WorldGen.genRand.Next(6));
                            }
                        }
                        else if (styleIndex is >= 34 and <= 36)
                        {
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 1122);
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 1123);
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 1124);
                        }
                    }
                    break;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && type != 653)
            {
                WorldGen.SpawnThingsFromPot(i, j, startX, startY, styleIndex);
            }
        }

        WorldGen.destroyObject = false;
    }

    private static void SpawnThingsFromPot_HandleVanillaAndModdedStyles(On_WorldGen.orig_SpawnThingsFromPot orig, int i, int j, int x2, int y2, int style) { }
}