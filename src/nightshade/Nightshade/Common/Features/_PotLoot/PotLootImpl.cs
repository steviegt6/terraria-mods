using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal sealed class PotLootImpl : ModSystem
{
    public static readonly PotBehavior POT_BEHAVIOR_VANILLA = new VanillaPotBehavior(echo: false);
    public static readonly PotBehavior POT_BEHAVIOR_VANILLA_ECHO = new VanillaPotBehavior(echo: true);

    public override void Load()
    {
        base.Load();

        // Vanilla logic is completely rewritten here in favor of directly
        // implementing everything Nightshade needs.
        On_WorldGen.CheckPot += CheckPot_EncodeVanillaAndModdedStyles;
    }

    public static bool TryGetPot(
        int type,
        [NotNullWhen(returnValue: true)] out PotBehavior? pot
    )
    {
        switch (type)
        {
            case TileID.Pots:
                pot = POT_BEHAVIOR_VANILLA;
                return true;

            case TileID.PotsEcho:
                pot = POT_BEHAVIOR_VANILLA_ECHO;
                return true;
        }

        if (TileLoader.GetTile(type) is IPot potTile)
        {
            pot = potTile.Behavior;
            return true;
        }

        pot = null;
        return false;
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
        var style = 0;
        while (frameY > 1)
        {
            frameY -= 2;
            style++;
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

                if (!Main.tile[x, y].active() || Main.tile[x, y].type != type || frameXOffset != x - startX || Main.tile[x, y].frameY != (y - startY) * 18 + style * 36)
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

        pot.PlayBreakSound(i, j, style);

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
            pot.SpawnGore(i, j, style);

            if (Main.netMode != NetmodeID.MultiplayerClient && pot.ShouldTryForLoot(i, j, style))
            {
                SpawnThingsFromPot_HandleVanillaAndModdedStyles(i, j, startX, startY, pot, style);
            }
        }

        WorldGen.destroyObject = false;
    }

    private static void SpawnThingsFromPot_HandleVanillaAndModdedStyles(int i, int j, int x2, int y2, PotBehavior potBehavior, int style)
    {
        if (WorldGen.gen)
        {
            return;
        }

        var aboveRockLayer = j < Main.rockLayer;
        var aboveUnderworldLayer = j < Main.UnderworldLayer;

        if (Main.remixWorld)
        {
            aboveRockLayer = j > Main.rockLayer && j < Main.UnderworldLayer;
            aboveUnderworldLayer = j > Main.worldSurface && j < Main.rockLayer;
        }

        var coinMultiplier = 1f;
        // var isUndergroundDesertPot = style is >= 34 and <= 36;
        potBehavior.ModifyCoinMultiplier(i, j, style, ref coinMultiplier);

        coinMultiplier = (coinMultiplier * 2f + 1f) / 3f;
        var coinPortalChance = (int)(500f / ((coinMultiplier + 1f) / 2f));

        if (Player.GetClosestRollLuck(i, j, coinPortalChance) == 0f)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(WorldGen.GetProjectileSource_TileBreak(i, j), i * 16 + 16, j * 16 + 16, 0f, -12f, 518, 0, 0f, Main.myPlayer);
            }

            return;
        }

        if (WorldGen.genRand.NextBool(35) && Main.wallDungeon[Main.tile[i, j].wall] && j > Main.worldSurface)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 327);
            return;
        }

        if (Main.getGoodWorld && WorldGen.genRand.NextBool(6))
        {
            Projectile.NewProjectile(WorldGen.GetProjectileSource_TileBreak(i, j), i * 16 + 16, j * 16 + 8, Main.rand.Next(-100, 101) * 0.002f, 0f, 28, 0, 0f, Main.myPlayer, 16f, 16f);
            return;
        }

        if (Main.remixWorld && Main.netMode != NetmodeID.MultiplayerClient && WorldGen.genRand.NextBool(5))
        {
            var player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 75);
            }
            else if (player.ZoneJungle)
            {
                var num2 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -10);
                if (num2 > -1)
                {
                    Main.npc[num2].ai[1] = 75f;
                    Main.npc[num2].netUpdate = true;
                }
            }
            else if (j > Main.rockLayer && j < Main.maxTilesY - 350)
            {
                var num3 = Main.rand.NextBool(9) ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -7) : Main.rand.Next(7) == 0 ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -8) : Main.rand.Next(6) == 0 ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -9) : Main.rand.Next(3) != 0 ? NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, 1) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -3);
                if (num3 > -1)
                {
                    Main.npc[num3].ai[1] = 75f;
                    Main.npc[num3].netUpdate = true;
                }
            }
            else if (j > Main.worldSurface && j <= Main.rockLayer)
            {
                var num4 = NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), x2 * 16 + 16, y2 * 16 + 32, -6);
                if (num4 > -1)
                {
                    Main.npc[num4].ai[1] = 75f;
                    Main.npc[num4].netUpdate = true;
                }
            }
            else
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 75);
            }

            return;
        }

        if (Main.remixWorld && i > Main.maxTilesX * 0.37 && i < Main.maxTilesX * 0.63 && j > Main.maxTilesY - 220)
        {
            var stack = Main.rand.Next(20, 41);
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 965, stack);
            return;
        }

        // TODO: POTION CASE
        if (WorldGen.genRand.NextBool(45) || (Main.rand.NextBool(45) && Main.expertMode))
        {
            if (j < Main.worldSurface)
            {
                var num5 = WorldGen.genRand.Next(10);
                switch (num5)
                {
                    case 0:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 292);
                        break;

                    case 1:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 298);
                        break;

                    case 2:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 299);
                        break;

                    case 3:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 290);
                        break;

                    case 4:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2322);
                        break;

                    case 5:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2324);
                        break;

                    case 6:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2325);
                        break;

                    case >= 7:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2350, WorldGen.genRand.Next(1, 3));
                        break;
                }
            }
            else if (aboveRockLayer)
            {
                var num6 = WorldGen.genRand.Next(11);
                switch (num6)
                {
                    case 0:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 289);
                        break;

                    case 1:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 298);
                        break;

                    case 2:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 299);
                        break;

                    case 3:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 290);
                        break;

                    case 4:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 303);
                        break;

                    case 5:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 291);
                        break;

                    case 6:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 304);
                        break;

                    case 7:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2322);
                        break;

                    case 8:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2329);
                        break;
                }

                if (num6 >= 7)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2350, WorldGen.genRand.Next(1, 3));
                }
            }
            else if (aboveUnderworldLayer)
            {
                var num7 = WorldGen.genRand.Next(15);
                switch (num7)
                {
                    case 0:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 296);
                        break;

                    case 1:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 295);
                        break;

                    case 2:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 299);
                        break;

                    case 3:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 302);
                        break;

                    case 4:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 303);
                        break;

                    case 5:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 305);
                        break;

                    case 6:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 301);
                        break;

                    case 7:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 302);
                        break;

                    case 8:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 297);
                        break;

                    case 9:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 304);
                        break;

                    case 10:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2322);
                        break;

                    case 11:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2323);
                        break;

                    case 12:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2327);
                        break;

                    case 13:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2329);
                        break;
                }

                if (num7 >= 7)
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2350, WorldGen.genRand.Next(1, 3));
                }
            }
            else
            {
                var num8 = WorldGen.genRand.Next(14);
                switch (num8)
                {
                    case 0:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 296);
                        break;

                    case 1:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 295);
                        break;

                    case 2:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 293);
                        break;

                    case 3:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 288);
                        break;

                    case 4:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 294);
                        break;

                    case 5:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 297);
                        break;

                    case 6:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 304);
                        break;

                    case 7:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 305);
                        break;

                    case 8:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 301);
                        break;

                    case 9:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 302);
                        break;

                    case 10:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 288);
                        break;

                    case 11:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 300);
                        break;

                    case 12:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2323);
                        break;

                    case 13:
                        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2326);
                        break;
                }

                if (WorldGen.genRand.NextBool(5))
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 4870);
                }
            }

            return;
        }

        if (Main.netMode == NetmodeID.Server && Main.rand.NextBool(30))
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 2997);
            return;
        }

        var num9 = Main.rand.Next(7);
        if (Main.expertMode)
        {
            num9--;
        }

        var player2 = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
        var num10 = 0;
        const int num11 = 20;
        for (var k = 0; k < 50; k++)
        {
            var item = player2.inventory[k];
            if (!item.IsAir && item.createTile == TileID.Torches)
            {
                num10 += item.stack;
                if (num10 >= num11)
                {
                    break;
                }
            }
        }

        var flag4 = num10 < num11;
        if (num9 == 0 && player2.statLife < player2.statLifeMax2)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
            if (Main.rand.NextBool(2))
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
            }

            if (Main.expertMode)
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
                }

                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 58);
                }
            }

            return;
        }

        if (num9 == 1 || (num9 == 0 && flag4))
        {
            var torchStack = Main.rand.Next(2, 7);
            if (Main.expertMode)
            {
                torchStack += Main.rand.Next(1, 7);
            }

            var torchType = 8;
            var glowstickType = 282;

            potBehavior.ModifyTorchType(i, j, style, player2, ref torchType, ref glowstickType, ref torchStack);

            if (Main.tile[i, j].liquid > 0)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, glowstickType, torchStack);
            }
            else
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, torchType, torchStack);
            }

            return;
        }

        switch (num9)
        {
            case 2:
            {
                var stack2 = Main.rand.Next(10, 21);
                var type4 = 40;
                if (aboveRockLayer && WorldGen.genRand.NextBool(2))
                {
                    type4 = !Main.hardMode ? 42 : 168;
                }

                if (j > Main.UnderworldLayer)
                {
                    type4 = 265;
                }
                else if (Main.hardMode)
                {
                    type4 = !Main.rand.NextBool(2) ? 47 : WorldGen.SavedOreTiers.Silver != 168 ? 278 : 4915;
                }

                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type4, stack2);
                return;
            }

            case 3:
            {
                var type5 = 28;
                if (j > Main.UnderworldLayer || Main.hardMode)
                {
                    type5 = 188;
                }

                var num14 = 1;
                if (Main.expertMode && !Main.rand.NextBool(3))
                {
                    num14++;
                }

                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, type5, num14);
                return;
            }

            case 4:
                /*if (isUndergroundDesertPot || aboveUnderworldLayer)
                {
                    var utilityType = 166;
                    if (isUndergroundDesertPot)
                    {
                        utilityType = 4423;
                    }

                    var utilityStack = Main.rand.Next(4) + 1;
                    if (Main.expertMode)
                    {
                        utilityStack += Main.rand.Next(4);
                    }

                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, utilityType, utilityStack);
                    return;
                }*/

                if (potBehavior.TryGetUtilityItem(i, j, style, aboveUnderworldLayer, out var utilityType, out var utilityStack))
                {
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, utilityType, utilityStack);
                }
                break;
        }

        if (num9 is 4 or 5 && j < Main.UnderworldLayer && !Main.hardMode)
        {
            var stack3 = Main.rand.Next(20, 41);
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 965, stack3);
            return;
        }

        float num15 = 200 + WorldGen.genRand.Next(-100, 101);
        if (j < Main.worldSurface)
        {
            num15 *= 0.5f;
        }
        else if (aboveRockLayer)
        {
            num15 *= 0.75f;
        }
        else if (j > Main.maxTilesY - 250)
        {
            num15 *= 1.25f;
        }

        num15 *= 1f + Main.rand.Next(-20, 21) * 0.01f;
        if (Main.rand.NextBool(4))
        {
            num15 *= 1f + Main.rand.Next(5, 11) * 0.01f;
        }

        if (Main.rand.NextBool(8))
        {
            num15 *= 1f + Main.rand.Next(10, 21) * 0.01f;
        }

        if (Main.rand.NextBool(12))
        {
            num15 *= 1f + Main.rand.Next(20, 41) * 0.01f;
        }

        if (Main.rand.NextBool(16))
        {
            num15 *= 1f + Main.rand.Next(40, 81) * 0.01f;
        }

        if (Main.rand.NextBool(20))
        {
            num15 *= 1f + Main.rand.Next(50, 101) * 0.01f;
        }

        if (Main.expertMode)
        {
            num15 *= 2.5f;
        }

        if (Main.expertMode && Main.rand.NextBool(2))
        {
            num15 *= 1.25f;
        }

        if (Main.expertMode && Main.rand.NextBool(3))
        {
            num15 *= 1.5f;
        }

        if (Main.expertMode && Main.rand.NextBool(4))
        {
            num15 *= 1.75f;
        }

        num15 *= coinMultiplier;
        if (NPC.downedBoss1)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedBoss2)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedBoss3)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedMechBoss1)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedMechBoss2)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedMechBoss3)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedPlantBoss)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedQueenBee)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedGolemBoss)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedPirates)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedGoblins)
        {
            num15 *= 1.1f;
        }

        if (NPC.downedFrost)
        {
            num15 *= 1.1f;
        }

        while ((int)num15 > 0)
        {
            switch (num15)
            {
                case > 1000000f:
                {
                    var num16 = (int)(num15 / 1000000f);
                    if (num16 > 50 && Main.rand.NextBool(2))
                    {
                        num16 /= Main.rand.Next(3) + 1;
                    }

                    if (Main.rand.NextBool(2))
                    {
                        num16 /= Main.rand.Next(3) + 1;
                    }

                    num15 -= 1000000 * num16;
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 74, num16);
                    continue;
                }

                case > 10000f:
                {
                    var num17 = (int)(num15 / 10000f);
                    if (num17 > 50 && Main.rand.NextBool(2))
                    {
                        num17 /= Main.rand.Next(3) + 1;
                    }

                    if (Main.rand.NextBool(2))
                    {
                        num17 /= Main.rand.Next(3) + 1;
                    }

                    num15 -= 10000 * num17;
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 73, num17);
                    continue;
                }

                case > 100f:
                {
                    var num18 = (int)(num15 / 100f);
                    if (num18 > 50 && Main.rand.NextBool(2))
                    {
                        num18 /= Main.rand.Next(3) + 1;
                    }

                    if (Main.rand.NextBool(2))
                    {
                        num18 /= Main.rand.Next(3) + 1;
                    }

                    num15 -= 100 * num18;
                    Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 72, num18);
                    continue;
                }
            }

            var num19 = (int)num15;
            if (num19 > 50 && Main.rand.NextBool(2))
            {
                num19 /= Main.rand.Next(3) + 1;
            }

            if (Main.rand.NextBool(2))
            {
                num19 /= Main.rand.Next(4) + 1;
            }

            if (num19 < 1)
            {
                num19 = 1;
            }

            num15 -= num19;
            Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 71, num19);
        }
    }
}