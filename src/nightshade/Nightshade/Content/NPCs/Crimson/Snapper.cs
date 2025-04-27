using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Nightshade.Content.NPCs.Crimson;

internal sealed class Snapper : ModNPC
{
    public override string Texture => Assets.Images.NPCs.Crimson.Snapper.KEY;

    private ref float TimerForPlayersColliding => ref NPC.ai[0];
    //To prevent the enemy from snapping faster the more players are touching it
    private bool HasCollidedWithPlayerThisFrame
    {
        get => NPC.localAI[0] == 1;
        set
        {
            NPC.localAI[0] = value ? 1 : 0;
        }
    }
    private ref float CooldownUntilCanBiteAgain => ref NPC.ai[2];
    private bool IsUnableToBite => CooldownUntilCanBiteAgain > 0;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.npcFrameCount[NPC.type] = 2;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        (NPC.width, NPC.height) = (88, 48);

        NPC.damage = 10;
        NPC.defense = 5;
        NPC.lifeMax = 60;
        NPC.value = Item.buyPrice(silver: 20);
        NPC.knockBackResist = 0f;
        NPC.HitSound = SoundID.NPCHit20;
        NPC.DeathSound = SoundID.NPCDeath12;

        NPC.alpha = 230;

        Banner = Type;
        BannerItem = ItemID.FaceMonsterBanner;
    }

    public override void AI()
    {
        base.AI();

        HasCollidedWithPlayerThisFrame = false;
        if (IsUnableToBite)
        {
            CooldownUntilCanBiteAgain -= 1;
            CooldownUntilCanBiteAgain = MathF.Max(CooldownUntilCanBiteAgain, 0);
        }

        NPC.alpha += IsUnableToBite ? -25 : 25;
        NPC.alpha = MathHelper.Clamp(NPC.alpha, 0, 230);
    }

    public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
    {
        npcHitbox = new Rectangle(npcHitbox.X + (NPC.direction == 1 ? npcHitbox.Width / 2 : 0), npcHitbox.Y, npcHitbox.Width / 2, npcHitbox.Height);
        if (!HasCollidedWithPlayerThisFrame && !IsUnableToBite)
        {
            HasCollidedWithPlayerThisFrame = true;
            TimerForPlayersColliding += victimHitbox.Intersects(npcHitbox) ? 2 : -5;
            TimerForPlayersColliding = MathHelper.Clamp(TimerForPlayersColliding, 0, 30);
        }
        return false;
    }

    public override bool CanHitPlayer(Player target, ref int cooldownSlot)
    {
        return TimerForPlayersColliding == 30;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        base.OnHitPlayer(target, hurtInfo);

        if (hurtInfo.Damage > 0)
        {
            target.AddBuff(BuffID.Bleeding, 5 * 60, true);
            if (Main.expertMode)
            {
                target.AddBuff(BuffID.Rabies, 15 * 60, true); //lol
            }
            CooldownUntilCanBiteAgain = 5 * 60;
            TimerForPlayersColliding = 0;
            NPC.netUpdate = true;
        }
    }

    public override void FindFrame(int frameHeight)
    {
        base.FindFrame(frameHeight);

        NPC.spriteDirection = NPC.direction;
        NPC.frame.Y = frameHeight * (TimerForPlayersColliding > 25 || IsUnableToBite ? 1 : 0);
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return SpawnCondition.Crimson.Chance * 0.1f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        base.ModifyNPCLoot(npcLoot);

        npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 3));
        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.DontStarveIsUp(), ItemID.TentacleSpike, 100));
        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.DontStarveIsNotUp(), ItemID.TentacleSpike, 525));
        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.DontStarveIsUp(), ItemID.PigPetItem, 500));
        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.DontStarveIsNotUp(), ItemID.PigPetItem, 1500));
    }
}