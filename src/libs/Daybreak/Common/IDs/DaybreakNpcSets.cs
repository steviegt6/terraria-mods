using JetBrains.Annotations;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace Daybreak.Common.IDs;

// TODO: NPCSmartInteractCandidateProvider.ProvideCandidate, CanChat
// TODO: ShopHelper.GetNearbyResidentNPCs - what does it care about?
// TODO: Support custom hook for EmoteBubble.CheckForNPCsToReactToEmoteBubble
// TODO: Party of Doom Main.DoUpdateInWorld

[PublicAPI]
public sealed class DaybreakNpcSets : ModSystem
{
    public static bool?[] CanParty = [];

    public static bool?[] ContributesToEocSpawn = [];

    public static bool?[] ContributesToTravelingMerchantSpawn = [];

    public static bool?[] TravelingMerchantCanSpawnNear = [];

    public static bool?[] VulnerableToAfterPartyOfDoom = [];

    public static bool[] TownNpcContributesToTownNpcSlots = [];

    public static bool?[] ContributesToTownNpcSpawnCount = [];

    public override void ResizeArrays()
    {
        base.ResizeArrays();

        CanParty = CreateSet<bool?>(nameof(CanParty), null);
        ContributesToEocSpawn = CreateSet<bool?>(nameof(ContributesToEocSpawn), null);
        ContributesToTravelingMerchantSpawn = CreateSet<bool?>(nameof(ContributesToTravelingMerchantSpawn), null);
        TravelingMerchantCanSpawnNear = CreateSet<bool?>(nameof(TravelingMerchantCanSpawnNear), null);
        VulnerableToAfterPartyOfDoom = CreateSet<bool?>(nameof(VulnerableToAfterPartyOfDoom), null);
        TownNpcContributesToTownNpcSlots = CreateSet(nameof(TownNpcContributesToTownNpcSlots), true);
        ContributesToTownNpcSpawnCount = CreateSet<bool?>(nameof(ContributesToTownNpcSpawnCount), true);

        return;

        T[] CreateSet<T>(string name, T defaultState)
        {
            return NPCID.Sets.Factory.CreateNamedSet(Mod, name)
                        .RegisterCustomSet(defaultState);
        }
    }

    public override void Load()
    {
        base.Load();

        On_BirthdayParty.CanNPCParty += ConsiderWhetherNpcCanParty;
        IL_Main.UpdateTime_StartNight += ConsiderWhetherNpcLetsEocSpawn;
        IL_Main.UpdateTime += ConsiderWhetherNpcLetsTravelingMerchantSpawn;
        IL_WorldGen.SpawnTravelNPC += ConsiderWhetherTravelingMerchantCanSpawnNearNpc;
        IL_Main.DoUpdateInWorld += ConsiderVulnerabilityToAfterPartyOfDoom;
        On_NPC.AddIntoPlayersTownNPCSlots += ConsiderWhetherTownNpcActuallyCountsForSlots;
        IL_Main.UpdateTime_SpawnTownNPCs += ConsiderWhetherTownNpcShouldBeConsideredForSpawnCounts;
    }

    private static bool ConsiderWhetherNpcCanParty(On_BirthdayParty.orig_CanNPCParty orig, NPC n)
    {
        return CanParty[n.type] ?? orig(n);
    }

    private static void ConsiderWhetherNpcLetsEocSpawn(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdfld<NPC>(nameof(NPC.townNPC)));
        c.Remove();
        c.EmitDelegate(static (NPC npc) => ContributesToEocSpawn[npc.type] ?? npc.townNPC);
    }

    private static void ConsiderWhetherNpcLetsTravelingMerchantSpawn(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdfld<NPC>(nameof(NPC.townNPC)));
        c.Remove();
        c.EmitDelegate(static (NPC npc) => ContributesToTravelingMerchantSpawn[npc.type] ?? npc.townNPC);
    }

    private static void ConsiderWhetherTravelingMerchantCanSpawnNearNpc(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdfld<NPC>(nameof(NPC.townNPC)));
        c.Remove();
        c.EmitDelegate(static (NPC npc) => TravelingMerchantCanSpawnNear[npc.type] ?? npc.townNPC);
    }

    private static void ConsiderVulnerabilityToAfterPartyOfDoom(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdfld<NPC>(nameof(NPC.townNPC)));
        c.Remove();
        c.EmitDelegate(static (NPC npc) => VulnerableToAfterPartyOfDoom[npc.type] ?? npc.townNPC);
    }

    private static void ConsiderWhetherTownNpcActuallyCountsForSlots(On_NPC.orig_AddIntoPlayersTownNPCSlots orig, NPC self)
    {
        if (!TownNpcContributesToTownNpcSlots[self.type])
        {
            return;
        }

        orig(self);
    }

    private static void ConsiderWhetherTownNpcShouldBeConsideredForSpawnCounts(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchLdfld<NPC>(nameof(NPC.townNPC)));
        c.Remove();
        c.EmitDelegate(static (NPC npc) => ContributesToTownNpcSpawnCount[npc.type] ?? npc.townNPC);
    }
}