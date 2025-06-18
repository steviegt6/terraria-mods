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
                int frameXOffset;
                for (frameXOffset = Main.tile[x, y].frameX / 18; frameXOffset > 1; frameXOffset -= 2) { }

                if (!Main.tile[x, y].active() || Main.tile[x, y].type != type || frameXOffset != x - startX || Main.tile[x, y].frameY != (y - startY) * 18 + style * 36)
                {
                    dontHandleBreak = true;
                }
            }

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

        var ctx = new PotBreakContext(i, j, style);

        pot.PlayBreakSound(ctx);

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
            pot.SpawnGore(ctx);

            if (Main.netMode != NetmodeID.MultiplayerClient && pot.ShouldTryForLoot(ctx))
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

        var ctx = new PotLootContext(i, j, x2, y2, style, aboveRockLayer, aboveUnderworldLayer);

        var coinValue = potBehavior.GetInitialCoinValue(ctx);
        {
            coinValue = (coinValue * 2f + 1f) / 3f;
        }

        var ctxWithCoinValue = new PotLootContextWithCoinMult(
            ctx.X,
            ctx.Y,
            ctx.X2,
            ctx.Y2,
            ctx.Style,
            ctx.AboveRockLayer,
            ctx.AboveUnderworldLayer,
            coinValue
        );

        if (potBehavior.ShouldSpawnCoinPortal(ctxWithCoinValue))
        {
            potBehavior.SpawnCoinPortal(ctxWithCoinValue);
            return;
        }

        if (potBehavior.ShouldSpawnGoldenKey(ctxWithCoinValue))
        {
            potBehavior.SpawnGoldenKey(ctxWithCoinValue);
            return;
        }

        if (potBehavior.ShouldSpawnLitBomb(ctxWithCoinValue))
        {
            potBehavior.SpawnLitBomb(ctxWithCoinValue);
            return;
        }

        if (potBehavior.ShouldSpawnDontDigUpStar(ctxWithCoinValue))
        {
            potBehavior.SpawnDontDigUpStar(ctxWithCoinValue);
            return;
        }

        if (potBehavior.ShouldSpawnDontDigUpRope(ctxWithCoinValue))
        {
            potBehavior.SpawnDontDigUpRope(ctxWithCoinValue);
            return;
        }

        if (potBehavior.ShouldSpawnPotion(ctxWithCoinValue))
        {
            var potions = potBehavior.GetPotions(ctxWithCoinValue);
            potBehavior.SpawnPotions(ctxWithCoinValue, potions);
            return;
        }

        if (potBehavior.ShouldSpawnWormholePotion(ctxWithCoinValue))
        {
            potBehavior.SpawnWormholePotion(ctxWithCoinValue);
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

        num15 *= coinValue;
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