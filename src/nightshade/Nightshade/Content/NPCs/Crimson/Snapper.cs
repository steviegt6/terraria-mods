using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Content.Dusts;
using Nightshade.Content.Tiles;
using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Nightshade.Content.NPCs.Crimson;

internal sealed class Snapper : ModNPC
{
    public override string Texture => Assets.Images.NPCs.Crimson.Snapper.KEY;
    //string IHasMonsterBanner.ItemTexture => Assets.Images.NPCs.Crimson.Snapper_BannerItem.KEY;

    private ref float TimerForPlayersColliding => ref NPC.ai[0];
    private ref float PreviousTimerForPlayersColliding => ref NPC.ai[1];
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
    private bool ShouldMouthStayClosedForced => CooldownUntilCanBiteAgain > 30;
    private bool IsUnableToBite => CooldownUntilCanBiteAgain > 0;

    public override void Load()
    {
        base.Load();

        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, Texture + "_Gore1");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, Texture + "_Gore2");
    }

    public override bool IsLoadingEnabled(Mod mod)
    {
        int npcType = NPCLoader.NPCCount;
        int bannerType = MonsterBannerItem.RegisterMonsterBanner(
            mod,
            name: "SnapperBanner",
            npcType: npcType,
            itemTexture: Assets.Images.NPCs.Crimson.Snapper_BannerItem.KEY,
            tileTexture: Assets.Images.NPCs.Crimson.Snapper_BannerTile.KEY
        );
        Banner = npcType;
        BannerItem = bannerType;

        return base.IsLoadingEnabled(mod);
    }

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.npcFrameCount[NPC.type] = 2;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
        {
            Frame = 1,
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        (NPC.width, NPC.height) = (88, 24);

        NPC.damage = 40;
        NPC.defense = 6;
        NPC.lifeMax = 150;
        NPC.value = Item.buyPrice(silver: 20);
        NPC.knockBackResist = 0f;
        NPC.HitSound = SoundID.NPCHit20;
        NPC.DeathSound = SoundID.NPCDeath12;
        NPC.rarity = 1;

        NPC.alpha = NPC.IsABestiaryIconDummy ? 0 : 230;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        base.SetBestiary(database, bestiaryEntry);

        bestiaryEntry.Info.AddRange([
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
            new BestiaryPortraitBackgroundProviderPreferenceInfoElement(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson),
            new FlavorTextBestiaryInfoElement("Mods.Nightshade.NPCs.Snapper.Bestiary"),
        ]);
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

        if (TimerForPlayersColliding > 25 && PreviousTimerForPlayersColliding <= 25)
        {
            SoundEngine.PlaySound(SoundID.ChesterClose.WithPitchOffset(-0.5f), NPC.Center);
        }
        else if (TimerForPlayersColliding <= 25 && PreviousTimerForPlayersColliding > 25)
        {
            SoundEngine.PlaySound(SoundID.ChesterOpen.WithVolume(0.25f).WithPitchOffset(-0.5f), NPC.Center);
        }

        if (NPC.IsABestiaryIconDummy)
        {
            NPC.alpha = 0;
        }
        else
        {
            NPC.alpha += (TimerForPlayersColliding > 25) ? -25 : 25;
            NPC.alpha = MathHelper.Clamp(NPC.alpha, 0, 230);
        }
    }

    public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
    {
        npcHitbox = new Rectangle(npcHitbox.X + (NPC.direction == 1 ? npcHitbox.Width / 2 : 0), npcHitbox.Y, npcHitbox.Width / 2, npcHitbox.Height);
        if (!HasCollidedWithPlayerThisFrame && !ShouldMouthStayClosedForced)
        {
            HasCollidedWithPlayerThisFrame = true;
            PreviousTimerForPlayersColliding = TimerForPlayersColliding;
            TimerForPlayersColliding += victimHitbox.Intersects(npcHitbox) && !IsUnableToBite ? 1 : -1;
            TimerForPlayersColliding = MathHelper.Clamp(TimerForPlayersColliding, 0, 30);
        }
        return false;
    }

    public override bool CanHitPlayer(Player target, ref int cooldownSlot)
    {
        return TimerForPlayersColliding > 25 && TimerForPlayersColliding < 30 && !IsUnableToBite;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hitInfo)
    {
        base.OnHitPlayer(target, hitInfo);

        if (hitInfo.Damage > 0)
        {
            target.AddBuff(BuffID.Bleeding, 5 * 60, true);
            if (Main.expertMode)
            {
                target.AddBuff(BuffID.Rabies, 15 * 60, true); //lol
            }
            CooldownUntilCanBiteAgain = 5 * 60;
            PreviousTimerForPlayersColliding = TimerForPlayersColliding;
            NPC.netUpdate = true;

            Vector2 dustPosition = new Vector2(NPC.Hitbox.X + (NPC.direction == 1 ? NPC.Hitbox.Width / 2 : 0), NPC.Hitbox.Y + NPC.Hitbox.Height * 0.8f);
            for (int i = 0; i < 16; i++)
            {
                Dust.NewDust(dustPosition, NPC.Hitbox.Width / 2, 8, ModContent.DustType<CoolBloodDust>(), SpeedX: 1 * hitInfo.HitDirection, SpeedY: -0.2f, Scale: 0.8f);
            }
            SoundEngine.PlaySound(SoundID.NPCDeath23, NPC.Center);

        }
    }

    public override void HitEffect(NPC.HitInfo hitInfo)
    {
        for (int i = 0; i < (NPC.life <= 0 ? 32 : 12); i++)
        {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CoolBloodDust>(), SpeedX: 2f * hitInfo.HitDirection, SpeedY: -0.5f, Scale: 0.8f, Alpha: NPC.life <= 0 ? 0 : NPC.alpha);
        }
        if (NPC.life <= 0)
        {
            for (int i = 1; i <= 2; i++)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, Mod.Find<ModGore>($"{Name}_Gore{i}").Type, NPC.scale);
            }
        }
    }

    public override void FindFrame(int frameHeight)
    {
        base.FindFrame(frameHeight);

        NPC.spriteDirection = NPC.direction;
        NPC.frame.Y = frameHeight * (TimerForPlayersColliding > 25 || ShouldMouthStayClosedForced ? 1 : 0);
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.PlayerSafe || spawnInfo.Water)
        {
            return 0f;
        }
        return SpawnCondition.Crimson.Chance * 0.2f;
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