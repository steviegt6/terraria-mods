using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

namespace Nightshade.Common.Features;

/// <summary>
///     Describes the behavior of a pot.
/// </summary>
public abstract class PotBehavior
{
#region Break actions
    // Used only for vanilla pots; modded tiles have better control over their
    // kill sound.
    internal virtual void PlayBreakSound(PotBreakContext ctx) { }

    /// <summary>
    ///     Responsible for spawning any gore upon break.  Ideally should not be
    ///     used for anything else.
    /// </summary>
    public abstract void SpawnGore(PotBreakContext ctx);

    /// <summary>
    ///     Whether this pot should follow through with its break and try to
    ///     spawn loot or anything else resulting from the pot.
    /// </summary>
    public virtual bool ShouldTryForLoot(PotBreakContext ctx)
    {
        return true;
    }
#endregion

#region Loot actions
    /// <summary>
    ///     Gets the initial value for the coin multiplier for this pot.
    /// </summary>
    public abstract float GetInitialCoinValue(PotLootContext ctx);

#region Coin portal
    public virtual bool ShouldSpawnCoinPortal(PotLootContextWithCoinMult ctx)
    {
        var coinPortalChance = (int)(500f / ((ctx.CoinMult + 1f) / 2f));
        return Player.GetClosestRollLuck(ctx.X, ctx.Y, coinPortalChance) == 0f;
    }

    public virtual void SpawnCoinPortal(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(
                WorldGen.GetProjectileSource_TileBreak(i, j),
                i * 16 + 16,
                j * 16 + 16,
                0f,
                -12f,
                ProjectileID.CoinPortal,
                0,
                0f,
                Main.myPlayer
            );
        }
    }
#endregion

#region Golden key
    public virtual bool ShouldSpawnGoldenKey(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        return WorldGen.genRand.NextBool(35) && Main.wallDungeon[Main.tile[i, j].wall] && j > Main.worldSurface;
    }

    public virtual void SpawnGoldenKey(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, 327);
    }
#endregion

#region For the Worthy bomb
    public virtual bool ShouldSpawnLitBomb(PotLootContextWithCoinMult ctx)
    {
        return Main.getGoodWorld && WorldGen.genRand.NextBool(6);
    }

    public virtual void SpawnLitBomb(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        Projectile.NewProjectile(
            WorldGen.GetProjectileSource_TileBreak(i, j),
            i * 16 + 16,
            j * 16 + 8,
            Main.rand.Next(-100, 101) * 0.002f,
            0f,
            ProjectileID.Bomb,
            0,
            0f,
            Main.myPlayer,
            16f,
            16f
        );
    }
#endregion

#region Don't Dig Up star
    public virtual bool ShouldSpawnDontDigUpStar(PotLootContextWithCoinMult ctx)
    {
        return Main.remixWorld && Main.netMode != NetmodeID.MultiplayerClient && WorldGen.genRand.NextBool(5);
    }

    public virtual void SpawnDontDigUpStar(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;
        var x2 = ctx.X2;
        var y2 = ctx.Y2;

        var player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];

        if (Main.rand.NextBool(2))
        {
            Item.NewItem(
                WorldGen.GetItemSource_FromTileBreak(i, j),
                i * 16,
                j * 16,
                16,
                16,
                ItemID.FallenStar
            );
        }
        else if (player.ZoneJungle)
        {
            var slime = NPC.NewNPC(
                NPC.GetSpawnSourceForNaturalSpawn(),
                x2 * 16 + 16,
                y2 * 16 + 32,
                NPCID.JungleSlime
            );

            if (slime < 0)
            {
                return;
            }

            Main.npc[slime].ai[1] = ItemID.FallenStar;
            Main.npc[slime].netUpdate = true;
        }
        else if (j > Main.rockLayer && j < Main.maxTilesY - 350)
        {
            var slimeType = Main.rand.NextBool(9)
                ? NPCID.PurpleSlime
                : Main.rand.NextBool(7)
                    ? NPCID.RedSlime
                    : Main.rand.NextBool(6)
                        ? NPCID.YellowSlime
                        : !Main.rand.NextBool(3)
                            ? NPCID.BlueSlime
                            : NPCID.GreenSlime;

            var slime = NPC.NewNPC(
                NPC.GetSpawnSourceForNaturalSpawn(),
                x2 * 16 + 16,
                y2 * 16 + 32,
                slimeType
            );

            if (slime < 0)
            {
                return;
            }

            Main.npc[slime].ai[1] = ItemID.FallenStar;
            Main.npc[slime].netUpdate = true;
        }
        else if (j > Main.worldSurface && j <= Main.rockLayer)
        {
            var slime = NPC.NewNPC(
                NPC.GetSpawnSourceForNaturalSpawn(),
                x2 * 16 + 16,
                y2 * 16 + 32,
                NPCID.BlackSlime
            );

            if (slime < 0)
            {
                return;
            }

            Main.npc[slime].ai[1] = ItemID.FallenStar;
            Main.npc[slime].netUpdate = true;
        }
        else
        {
            Item.NewItem(
                WorldGen.GetItemSource_FromTileBreak(i, j),
                i * 16,
                j * 16,
                16,
                16,
                ItemID.FallenStar
            );
        }
    }
#endregion

#region Don't Dig Up rope
    public virtual bool ShouldSpawnDontDigUpRope(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        return Main.remixWorld && i > Main.maxTilesX * 0.37 && i < Main.maxTilesX * 0.63 && j > Main.maxTilesY - 220;
    }

    public virtual void SpawnDontDigUpRope(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        var stack = Main.rand.Next(20, 41);
        Item.NewItem(
            WorldGen.GetItemSource_FromTileBreak(i, j),
            i * 16,
            j * 16,
            16,
            16,
            ItemID.Rope,
            stack
        );
    }
#endregion

#region Potion drops
    public virtual bool ShouldSpawnPotion(PotLootContextWithCoinMult ctx)
    {
        return WorldGen.genRand.NextBool(45) || (Main.rand.NextBool(45) && Main.expertMode);
    }

    public virtual IEnumerable<PotLootPotion> GetPotions(PotLootContextWithCoinMult ctx)
    {
        if (ctx.Y < Main.worldSurface)
        {
            switch (WorldGen.genRand.Next(10))
            {
                case 0:
                    yield return new PotLootPotion(292);
                    break;

                case 1:
                    yield return new PotLootPotion(298);
                    break;

                case 2:
                    yield return new PotLootPotion(299);
                    break;

                case 3:
                    yield return new PotLootPotion(290);
                    break;

                case 4:
                    yield return new PotLootPotion(2322);
                    break;

                case 5:
                    yield return new PotLootPotion(2324);
                    break;

                case 6:
                    yield return new PotLootPotion(2325);
                    break;

                case >= 7:
                    yield return new PotLootPotion(2350, WorldGen.genRand.Next(1, 3));
                    break;
            }
        }
        else if (ctx.AboveRockLayer)
        {
            var roll = WorldGen.genRand.Next(11);
            switch (roll)
            {
                case 0:
                    yield return new PotLootPotion(289);
                    break;

                case 1:
                    yield return new PotLootPotion(298);
                    break;

                case 2:
                    yield return new PotLootPotion(299);
                    break;

                case 3:
                    yield return new PotLootPotion(290);
                    break;

                case 4:
                    yield return new PotLootPotion(303);
                    break;

                case 5:
                    yield return new PotLootPotion(291);
                    break;

                case 6:
                    yield return new PotLootPotion(304);
                    break;

                case 7:
                    yield return new PotLootPotion(2322);
                    break;

                case 8:
                    yield return new PotLootPotion(2329);
                    break;
            }

            if (roll >= 7)
            {
                yield return new PotLootPotion(2350, WorldGen.genRand.Next(1, 3));
            }
        }
        else if (ctx.AboveUnderworldLayer)
        {
            var roll = WorldGen.genRand.Next(15);
            switch (roll)
            {
                case 0:
                    yield return new PotLootPotion(296);
                    break;

                case 1:
                    yield return new PotLootPotion(295);
                    break;

                case 2:
                    yield return new PotLootPotion(299);
                    break;

                case 3:
                    yield return new PotLootPotion(302);
                    break;

                case 4:
                    yield return new PotLootPotion(303);
                    break;

                case 5:
                    yield return new PotLootPotion(305);
                    break;

                case 6:
                    yield return new PotLootPotion(301);
                    break;

                case 7:
                    yield return new PotLootPotion(302);
                    break;

                case 8:
                    yield return new PotLootPotion(297);
                    break;

                case 9:
                    yield return new PotLootPotion(304);
                    break;

                case 10:
                    yield return new PotLootPotion(2322);
                    break;

                case 11:
                    yield return new PotLootPotion(2323);
                    break;

                case 12:
                    yield return new PotLootPotion(2327);
                    break;

                case 13:
                    yield return new PotLootPotion(2329);
                    break;
            }

            if (roll >= 7)
            {
                yield return new PotLootPotion(2350, WorldGen.genRand.Next(1, 3));
            }
        }
        else
        {
            var roll = WorldGen.genRand.Next(14);
            switch (roll)
            {
                case 0:
                    yield return new PotLootPotion(296);
                    break;

                case 1:
                    yield return new PotLootPotion(295);
                    break;

                case 2:
                    yield return new PotLootPotion(293);
                    break;

                case 3:
                    yield return new PotLootPotion(288);
                    break;

                case 4:
                    yield return new PotLootPotion(294);
                    break;

                case 5:
                    yield return new PotLootPotion(297);
                    break;

                case 6:
                    yield return new PotLootPotion(304);
                    break;

                case 7:
                    yield return new PotLootPotion(305);
                    break;

                case 8:
                    yield return new PotLootPotion(301);
                    break;

                case 9:
                    yield return new PotLootPotion(302);
                    break;

                case 10:
                    yield return new PotLootPotion(288);
                    break;

                case 11:
                    yield return new PotLootPotion(300);
                    break;

                case 12:
                    yield return new PotLootPotion(2323);
                    break;

                case 13:
                    yield return new PotLootPotion(2326);
                    break;
            }

            if (WorldGen.genRand.NextBool(5))
            {
                yield return new PotLootPotion(ItemID.PotionOfReturn);
            }
        }
    }

    public virtual void SpawnPotions(PotLootContextWithCoinMult ctx, IEnumerable<PotLootPotion> potions)
    {
        var i = ctx.X;
        var j = ctx.Y;

        foreach (var potion in potions)
        {
            Item.NewItem(
                WorldGen.GetItemSource_FromTileBreak(i, j),
                i * 16,
                j * 16,
                16,
                16,
                potion.ItemType,
                potion.Stack
            );
        }
    }
#endregion

#region Wormhole potion
    public virtual bool ShouldSpawnWormholePotion(PotLootContextWithCoinMult ctx)
    {
        return Main.netMode == NetmodeID.Server && Main.rand.NextBool(30);
    }

    public virtual void SpawnWormholePotion(PotLootContextWithCoinMult ctx)
    {
        var i = ctx.X;
        var j = ctx.Y;

        Item.NewItem(
            WorldGen.GetItemSource_FromTileBreak(i, j),
            i * 16,
            j * 16,
            16,
            16,
            ItemID.WormholePotion
        );
    }
#endregion

    public abstract void ModifyTorchType(
        int i,
        int j,
        int style,
        Player player,
        ref int torchType,
        ref int glowstickType,
        ref int itemStack
    );

    // Ropes or bombs (or scarab bombs)
    public abstract bool TryGetUtilityItem(
        int i,
        int j,
        int style,
        bool aboveUnderworldLayer,
        out int utilityType,
        out int utilityStack
    );
#endregion
}