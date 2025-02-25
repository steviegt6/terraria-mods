using System;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Tomat.TML.Mod.CrowsWhoMow;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class CrowThatMows : ModNPC
{
    public override string Texture => "CrowsWhoMow/CrowThatMows";

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.npcFrameCount[Type] = 4;

        NPCID.Sets.CountsAsCritter[Type]                             = true;
        NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
        NPCID.Sets.TownCritter[Type]                                 = true;

        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.CloneDefaults(NPCID.Bunny);
        AIType = NPCID.Bunny;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        base.SetBestiary(database, bestiaryEntry);

        bestiaryEntry.AddTags(
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
            new FlavorTextBestiaryInfoElement("those who know vs crows who mow")
        );
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return Math.Max(SpawnCondition.OverworldDay.Chance * 0.1f, Main.dayTime ? SpawnCondition.OverworldHallow.Chance * 0.1f : 0f);
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        base.HitEffect(hit);

        if (NPC.life > 0)
        {
            return;
        }

        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 438, NPC.scale);
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 439, NPC.scale);
    }

    public override bool PreAI()
    {
        MowGrassTile(NPC.position with { Y = NPC.position.Y + NPC.height });

        NPC.direction = NPC.spriteDirection = NPC.velocity.X > 0f ? 1 : -1;

        return base.PreAI();
    }

    public override void FindFrame(int frameHeight)
    {
        base.FindFrame(frameHeight);

        NPC.frameCounter++;
        if (!(NPC.frameCounter >= 8))
        {
            return;
        }

        NPC.frameCounter = 0;
        NPC.frame.Y      = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        base.ModifyNPCLoot(npcLoot);

        npcLoot.Add(ItemDropRule.Common(ItemID.LawnMower));
    }

    private static void MowGrassTile(Vector2 pos)
    {
        var point = pos.ToTileCoordinates();
        var tile  = Framing.GetTileSafely(point);
        if (!WorldGen.CanKillTile(point.X, point.Y, WorldGen.SpecialKillTileContext.MowingTheGrass))
        {
            return;
        }

        var resType = tile.TileType switch
        {
            TileID.Grass         => TileID.GolfGrass,
            TileID.HallowedGrass => TileID.GolfGrassHallowed,
            _                    => (ushort)0,
        };

        if (resType == 0)
        {
            return;
        }

        var dustAmount = WorldGen.KillTile_GetTileDustAmount(fail: true, tile, point.X, point.Y);
        for (var i = 0; i < dustAmount; i++)
        {
            WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile);
        }
        tile.TileType = resType;

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            NetMessage.SendTileSquare(-1, point.X, point.Y);
        }
    }
}