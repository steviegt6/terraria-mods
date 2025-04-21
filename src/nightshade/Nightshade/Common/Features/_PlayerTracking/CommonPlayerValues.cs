using System.Diagnostics.CodeAnalysis;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal static class CommonPlayerValues
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    private sealed class CommonValueTracker : ModPlayer
    {
        public int TimeSinceLastHitEnemy { get; private set; } = int.MaxValue;

        public override void PostUpdate()
        {
            base.PostUpdate();

            if (TimeSinceLastHitEnemy != int.MaxValue)
            {
                TimeSinceLastHitEnemy++;
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            base.OnHitAnything(x, y, victim);

            TimeSinceLastHitEnemy = 0;
        }
    }

    public static int TimeSinceLastHitEnemy(this Player player)
    {
        return player.Common()?.TimeSinceLastHitEnemy ?? int.MaxValue;
    }

    private static CommonValueTracker? Common(this Player player)
    {
        // Explicitly make null in case we somehow end up in a case where there
        // are no mod players loaded for a player?
        return player.TryGetModPlayer<CommonValueTracker>(out var common) ? common : null;
    }
}