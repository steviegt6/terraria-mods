using Daybreak.Core.Hooks;

using Terraria;
using Terraria.ID;

namespace Daybreak.Common.Features.NPCs;

internal sealed class NpcCustomShimmerAiImpl : ILoad
{
    void ILoad.Load()
    {
        On_NPC.GetShimmered += HandleCustomShimmer;
    }

    private static void HandleCustomShimmer(On_NPC.orig_GetShimmered orig, NPC self)
    {
        if (self.ModNPC is not INpcCustomShimmerAi shimmerHandler)
        {
            orig(self);
            return;
        }

        if (!self.SpawnedFromStatue
         && NPCID.Sets.ShimmerTransformToNPC[self.type] < 0
         && NPCID.Sets.ShimmerTransformToItem[self.type] < 0
         && NPCID.Sets.ShimmerTownTransform[self.type])
        {
            shimmerHandler.GetShimmered();
        }
    }
}