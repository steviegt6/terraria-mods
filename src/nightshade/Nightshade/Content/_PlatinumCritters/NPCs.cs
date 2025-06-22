using System.Collections.Generic;
using System.Linq;

using Daybreak.Common.Features.Hooks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;
using Nightshade.Common.Features;
using Nightshade.Content.Particles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Content;

// TODO: Hit sounds, hit effect, gore/kill effect

internal static class PlatinumCritterNpcHandler
{
    public static readonly List<int> COLLECTION = [];

    private static readonly Color platinum_color_one = Colors.CoinPlatinum;
    private static readonly Color platinum_color_two = Color.LightPink;

    [OnLoad]
    private static void OnLoad()
    {
        On_Main.DrawInfoAccs_AdjustInfoTextColorsForNPC += ColorPlatinumNpcNamesInInfoAccessories;
    }

    private static void ColorPlatinumNpcNamesInInfoAccessories(
        On_Main.orig_DrawInfoAccs_AdjustInfoTextColorsForNPC orig,
        Main self,
        NPC npc,
        ref Color infoTextColor,
        ref Color infoTextShadowColor
    )
    {
        orig(self, npc, ref infoTextColor, ref infoTextShadowColor);

        if (!COLLECTION.Contains(npc.type))
        {
            return;
        }

        infoTextColor = Color.Lerp(
            platinum_color_one,
            platinum_color_two,
            Utils.PingPongFrom01To010(Main.GlobalTimeWrappedHourly)
        );
        infoTextColor.A = Main.mouseTextColor;

        infoTextShadowColor = infoTextColor * 0.1f;
        infoTextShadowColor.A = Main.mouseTextColor;
    }
}

internal sealed class PlatinumCritterCollectionInfoProvider(string[] allowedPersistentIds, string platinumPersistentId) : IBestiaryUICollectionInfoProvider
{
    public BestiaryUICollectionInfo GetEntryUICollectionInfo()
    {
        var ourState = GetUnlockStateForCritter(platinumPersistentId);
        var stateToUse = BestiaryEntryUnlockState.NotKnownAtAll_0;

        if (ourState > stateToUse)
        {
            stateToUse = ourState;
        }

        foreach (var otherId in allowedPersistentIds)
        {
            var otherState = GetUnlockStateForCritter(otherId);
            if (otherState > stateToUse)
            {
                stateToUse = otherState;
            }
        }

        if (stateToUse == BestiaryEntryUnlockState.NotKnownAtAll_0)
        {
            return new BestiaryUICollectionInfo
            {
                UnlockState = stateToUse,
            };
        }

        if (!TryFindingOnePlatinumCritterThatIsAlreadyUnlocked())
        {
            return new BestiaryUICollectionInfo
            {
                UnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0,
            };
        }

        return new BestiaryUICollectionInfo
        {
            UnlockState = stateToUse,
        };
    }

    private static bool TryFindingOnePlatinumCritterThatIsAlreadyUnlocked()
    {
        return PlatinumCritterNpcHandler.COLLECTION.Any(
            x => GetUnlockStateForCritter(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[x]) > BestiaryEntryUnlockState.NotKnownAtAll_0
        );
    }

    private static BestiaryEntryUnlockState GetUnlockStateForCritter(string persistentId)
    {
        return !Main.BestiaryTracker.Sights.GetWasNearbyBefore(persistentId)
            ? BestiaryEntryUnlockState.NotKnownAtAll_0
            : BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
    }
}

public abstract class PlatinumCritterNpc<TItem>(string critterName) : ModNPC
    where TItem : ModItem
{
    public override string Texture => MakeTexturePath(critterName);

    protected int NpcType => NPCID.Search.GetId(PlatCritterHelpers.GetGoldName(critterName));

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[NpcType];
        NPCID.Sets.DebuffImmunitySets[Type] = NPCID.Sets.DebuffImmunitySets[NpcType];
        NPCID.Sets.CountsAsCritter[Type] = NPCID.Sets.CountsAsCritter[NpcType];
        NPCID.Sets.TownCritter[Type] = NPCID.Sets.TownCritter[NpcType];

        // TODO: ShimmerTransformToNpc

        Main.npcCatchable[Type] = Main.npcCatchable[NpcType];
        Main.npcFrameCount[Type] = Main.npcFrameCount[NpcType];

        PlatinumCritterNpcHandler.COLLECTION.Add(Type);

        var priorityIdx = NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(NpcType);
        if (priorityIdx >= 0)
        {
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Insert(priorityIdx + 1, Type);
        }

        if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(NpcType, out var offset))
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, offset);
        }

        On_NPC.UpdateNPC_Inner += UpdateNPC_Inner_ApplyFunctionalType;
        On_NPC.UpdateNPC_CritterSounds += UpdateNpc_CritterSounds_ApplyFunctionalType;

        On_UnlockableNPCEntryIcon.AdjustSpecialSpawnRulesForVisuals += AdjustSpecialSpawnRulesForVisuals_ApplyFunctionalType;

        On_BestiaryDatabaseNPCsPopulator.AddNPCBiomeRelationships_AddDecorations_Automated += CloneBestiaryInfos;
        On_BestiaryDatabaseNPCsPopulator.ModifyEntriesThatNeedIt += CloneBestiaryInfoProviders;

        IL_NPC.SpawnNPC += SpawnNpc_SpawnPlatinumCritterSometimes;
    }

    private void UpdateNPC_Inner_ApplyFunctionalType(On_NPC.orig_UpdateNPC_Inner orig, NPC self, int i)
    {
        if (self.type != Type)
        {
            orig(self, i);
            return;
        }

        var oldType = self.type;
        self.type = NpcType;
        orig(self, i);
        self.type = oldType;
    }

    private void UpdateNpc_CritterSounds_ApplyFunctionalType(On_NPC.orig_UpdateNPC_CritterSounds orig, NPC self)
    {
        if (self.type != Type)
        {
            orig(self);
            return;
        }

        var oldType = self.type;
        self.type = NpcType;
        orig(self);
        self.type = oldType;
    }

    private void AdjustSpecialSpawnRulesForVisuals_ApplyFunctionalType(On_UnlockableNPCEntryIcon.orig_AdjustSpecialSpawnRulesForVisuals orig, UnlockableNPCEntryIcon self, EntryIconDrawSettings settings)
    {
        if (self._npcNetId != Type)
        {
            orig(self, settings);
            return;
        }

        var oldType = self._npcNetId;
        self._npcNetId = NpcType;
        orig(self, settings);
        self._npcNetId = oldType;
    }

    private void CloneBestiaryInfos(On_BestiaryDatabaseNPCsPopulator.orig_AddNPCBiomeRelationships_AddDecorations_Automated orig, BestiaryDatabaseNPCsPopulator self)
    {
        var bestiaryEntry = BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(Type);
        if (bestiaryEntry.Info.Count != 0)
        {
            bestiaryEntry.Info.AddRange(
                BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(NpcType).Info.Where(x => x is SpawnConditionBestiaryInfoElement)
            );
        }

        orig(self);
    }

    private void CloneBestiaryInfoProviders(
        On_BestiaryDatabaseNPCsPopulator.orig_ModifyEntriesThatNeedIt orig,
        BestiaryDatabaseNPCsPopulator self
    )
    {
        orig(self);

        var bestiaryEntry = BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(Type);

        if (BestiaryDatabaseNPCsPopulator.FindEntryByNPCID(NpcType).UIInfoProvider is GoldCritterUICollectionInfoProvider goldProvider)
        {
            bestiaryEntry.UIInfoProvider = new PlatinumCritterCollectionInfoProvider(
                goldProvider._normalCritterPersistentId.Concat([goldProvider._goldCritterPersistentId]).ToArray(),
                ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type]
            );
        }
    }

    private void SpawnNpc_SpawnPlatinumCritterSometimes(ILContext il)
    {
        var c = new ILCursor(il);

        while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(NpcType)))
        {
            c.EmitDelegate(
                (int goldCritter) => Main.rand.NextBool(10) ? Type : goldCritter
            );
        }
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        base.SetBestiary(database, bestiaryEntry);

        bestiaryEntry.Info.AddRange(
            [
                new FlavorTextBestiaryInfoElement(Mods.Nightshade.Bestiary.PlatinumCritterText.KEY),
            ]
        );
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.CloneDefaults(NpcType);
        NPC.catchItem = ModContent.ItemType<TItem>();

        AIType = NpcType;
        AnimationType = NpcType;
    }

    public override void PostAI()
    {
        base.PostAI();

        NPC.position += NPC.netOffset;
        var color = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
        if (color.R > 20 || color.B > 20 || color.G > 20)
        {
            int red = color.R;
            if (color.G > red)
            {
                red = color.G;
            }

            if (color.B > red)
            {
                red = color.B;
            }

            red /= 30;
            if (Main.rand.Next(500) < red)
            {
                var dust = Dust.NewDust(
                    NPC.position,
                    NPC.width,
                    NPC.height,
                    DustID.TintableDustLighted,
                    0f,
                    0f,
                    254,
                    Color.SlateBlue * 2.65f,
                    0.5f
                );
                Main.dust[dust].velocity *= 0f;
            }
        }

        NPC.position -= NPC.netOffset;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (!NPC.IsABestiaryIconDummy)
        {
            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Main.Transform
            );
        }

        GameShaders.Armor.GetShaderFromItemId(ModContent.ItemType<ReflectivePlatinumDyeItem>())
                   .Apply(NPC, new DrawData(TextureAssets.Npc[Type].Value, NPC.position, drawColor));

        return base.PreDraw(spriteBatch, screenPos, drawColor);
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        if (!NPC.IsABestiaryIconDummy)
        {
            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.Transform
            );
        }

        base.PostDraw(spriteBatch, screenPos, drawColor);
    }

    public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitByItem(player, item, ref modifiers);

        modifiers.SetMaxDamage(1);
    }

    public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitByProjectile(projectile, ref modifiers);

        modifiers.SetMaxDamage(1);
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        base.HitEffect(hit);

        ParticleOrchestrator.RequestParticleSpawn(
            clientOnly: true,
            ParticleOrchestraType.PaladinsHammer,
            new ParticleOrchestraSettings
            {
                PositionInWorld = NPC.Center,
            }
        );
    }

    public override void OnKill()
    {
        base.OnKill();

		ParticleOrchestrator.RequestParticleSpawn(
			clientOnly: true,
			ParticleOrchestraType.PaladinsHammer,
			new ParticleOrchestraSettings
			{
				PositionInWorld = NPC.Center,
			}
		);

		for (int i = 0; i < 10; i++)
		{
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Platinum, 0, -1);
		}

		for (int i = 0; i < 20; i++)
        {
            TrailingSparkleParticle particle = TrailingSparkleParticle.pool.RequestParticle();
            particle.Prepare(NPC.Center, Main.rand.NextVector2Circular(2, 2), Color.LightSlateGray with { A = 10 }, Main.rand.Next(10, 80), Main.rand.NextFloat(0.3f, 1.5f));
            ParticleEngine.Particles.Add(particle);
        }
	}

    private static string MakeTexturePath(string name)
    {
        // Use a definite reference here so we fail to compile if we ever change
        // it.
        const string key = Assets.Images.NPCs.PlatinumCritters.Bird.KEY;

        // Get the actual directory.
        var basePath = key.Split('/')[..^1];

        return string.Join('/', basePath) + '/' + name;
    }
}

public sealed class PlatinumBirdNpc() : PlatinumCritterNpc<PlatinumBirdItem>("Bird");

public sealed class PlatinumBunnyNpc() : PlatinumCritterNpc<PlatinumBunnyItem>("Bunny");

public sealed class PlatinumButterflyNpc() : PlatinumCritterNpc<PlatinumButterflyItem>("Butterfly");

public sealed class PlatinumDragonflyNpc() : PlatinumCritterNpc<PlatinumDragonflyItem>("Dragonfly");

public sealed class PlatinumFrogNpc() : PlatinumCritterNpc<PlatinumFrogItem>("Frog");

public sealed class PlatinumGoldfishNpc() : PlatinumCritterNpc<PlatinumGoldfishItem>("Goldfish")
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        On_NPC.AttemptToConvertNPCToEvil += ConvertGoldfishToEvil;
        On_NPC.FishTransformationDuringRain += HandleRainTransformation;
    }

    private void ConvertGoldfishToEvil(On_NPC.orig_AttemptToConvertNPCToEvil orig, NPC self, bool crimson)
    {
        orig(self, crimson);

        if (self.type == Type)
        {
            self.Transform(crimson ? 465 : 57);
        }
    }

    private void HandleRainTransformation(On_NPC.orig_FishTransformationDuringRain orig, NPC self)
    {
        orig(self);

        if (self.type != Type || self.wet || !Main.raining)
        {
            return;
        }

        var dir = self.direction;
        var vel = self.velocity;
        self.Transform(ModContent.NPCType<PlatinumWalkerGoldfishNpc>());
        self.directionY = dir;
        self.velocity = vel;
        self.UpdateHomeTileState(
            self.homeless,
            (int)(self.position.X / 16f) + 10 * self.direction,
            self.homeTileY
        );
    }
}

public sealed class PlatinumWalkerGoldfishNpc() : PlatinumCritterNpc<PlatinumGoldfishItem>("GoldfishWalker")
{
    public override string Texture => Assets.Images.NPCs.PlatinumCritters.GoldfishWalk.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        On_NPC.AttemptToConvertNPCToEvil += ConvertGoldfishToEvil;
        On_NPC.FishTransformationDuringRain += HandleRainTransformation;
    }

    private void ConvertGoldfishToEvil(On_NPC.orig_AttemptToConvertNPCToEvil orig, NPC self, bool crimson)
    {
        orig(self, crimson);

        if (self.type == Type)
        {
            self.Transform(crimson ? 465 : 57);
        }
    }

    private void HandleRainTransformation(On_NPC.orig_FishTransformationDuringRain orig, NPC self)
    {
        orig(self);

        if (self.type != Type || !self.wet)
        {
            return;
        }

        var dir = self.direction;
        var vel = self.velocity;
        self.Transform(ModContent.NPCType<PlatinumGoldfishNpc>());
        self.directionY = dir;
        self.velocity = vel;
        self.wet = true;
        if (self.velocity.Y < 0f)
        {
            self.velocity.Y = 0f;
        }
    }
}

public sealed class PlatinumGrasshopperNpc() : PlatinumCritterNpc<PlatinumGrasshopperItem>("Grasshopper");

public sealed class PlatinumLadyBugNpc() : PlatinumCritterNpc<PlatinumLadyBugItem>("LadyBug")
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        On_NPC.CheckActive += CheckActive_ApplyLadybugLuck;
    }

    public override void OnKill()
    {
        base.OnKill();

        NPC.LadyBugKilled(NPC.Center, true);
    }

    private void CheckActive_ApplyLadybugLuck(On_NPC.orig_CheckActive orig, NPC self)
    {
        if (self.type != Type)
        {
            orig(self);
            return;
        }

        var oldType = self.type;
        self.type = NpcType;
        orig(self);
        self.type = oldType;
    }
}

public sealed class PlatinumMouseNpc() : PlatinumCritterNpc<PlatinumMouseItem>("Mouse");

public sealed class PlatinumSeahorseNpc() : PlatinumCritterNpc<PlatinumSeahorseItem>("Seahorse");

public sealed class PlatinumSquirrelNpc() : PlatinumCritterNpc<PlatinumSquirrelItem>("Squirrel");

public sealed class PlatinumWaterStriderNpc() : PlatinumCritterNpc<PlatinumWaterStriderItem>("WaterStrider");

public sealed class PlatinumWormNpc() : PlatinumCritterNpc<PlatinumWormItem>("Worm");