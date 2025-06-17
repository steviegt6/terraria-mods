using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Nightshade.Common.Features;

internal sealed class PotLootImpl : ModSystem
{
    private static readonly CustomPot vanilla_pot = new VanillaPot(echo: false);
    private static readonly CustomPot vanilla_pot_echo = new VanillaPot(echo: true);

    public override void Load()
    {
        base.Load();

        // Vanilla logic is completely rewritten here in favor of directly
        // implementing everything Nightshade needs.
        On_WorldGen.CheckPot += CheckPot_EncodeVanillaAndModdedStyles;
        On_WorldGen.SpawnThingsFromPot += SpawnThingsFromPot_HandleVanillaAndModdedStyles;
    }

    public static bool TryGetPot(
        int type,
        [NotNullWhen(returnValue: true)] out CustomPot? pot
    )
    {
        switch (type)
        {
            case TileID.Pots:
                pot = vanilla_pot;
                return true;
            
            case TileID.PotsEcho:
                pot = vanilla_pot_echo;
                return true;
        }

        if (TileLoader.GetTile(type) is IPot potTile)
        {
            pot = potTile.Pot;
            return true;
        }

        pot = null;
        return false;

        static bool IsVanillaPot(int type)
        {
            return type is TileID.Pots or TileID.PotsEcho;
        }
    }

    private static bool IsWithinVanillaBounds(int type)
    {
        return type is >= VanillaPot.POT_0_FOREST and <= VanillaPot.POT_36_UNDERGROUND_DESERT;
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

        if (!TryGetPot(type, out var pot))
        {
            return;
        }

        pot.PlayBreakSound(i, j, styleIndex);

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
            pot.SpawnGore(i, j, styleIndex);

            if (Main.netMode != NetmodeID.MultiplayerClient && type != 653)
            {
                WorldGen.SpawnThingsFromPot(i, j, startX, startY, styleIndex);
            }
        }

        WorldGen.destroyObject = false;
    }

    private static void SpawnThingsFromPot_HandleVanillaAndModdedStyles(On_WorldGen.orig_SpawnThingsFromPot orig, int i, int j, int x2, int y2, int style) { }
}