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

        if (self.SpawnedFromStatue
         || NPCID.Sets.ShimmerTransformToNPC[self.type] >= 0
         || NPCID.Sets.ShimmerTransformToItem[self.type] >= 0
         || !NPCID.Sets.ShimmerTownTransform[self.type])
        {
            return;
        }

        var behavior = shimmerHandler.GetShimmered();

        if (behavior == INpcCustomShimmerAi.Behavior.None)
        {
            return;
        }

        if (behavior.HasFlag(INpcCustomShimmerAi.Behavior.ResetAi))
        {
            self.ai[0] = 25f;
            self.ai[1] = 0f;
            self.ai[2] = 0f;
            self.ai[3] = 0f;
        }

        if (behavior.HasFlag(INpcCustomShimmerAi.Behavior.NetUpdate))
        {
            self.netUpdate = true;
        }

        if (behavior.HasFlag(INpcCustomShimmerAi.Behavior.ShimmerTransparency))
        {
            self.shimmerTransparency = 0.89f;
        }

        if (behavior.HasFlag(INpcCustomShimmerAi.Behavior.RemoveShimmerDebuff))
        {
            var idx = self.FindBuffIndex(BuffID.Shimmer);
            if (idx != -1)
            {
                self.DelBuff(idx);
            }
        }
    }
}