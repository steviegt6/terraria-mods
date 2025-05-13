using Terraria.ModLoader.IO;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     An achievement condition, for which an achievement may have an arbitrary
///     number of.  These conditions may track arbitrary state and report back
///     their progress to the achievement system, determining the progress of an
///     achievement.  Achievements are expected to have all conditions completed
///     to be considered earned.
/// </summary>
public abstract class AchievementCondition
{
    public virtual void SaveData(TagCompound tag) { }

    public virtual void LoadData(TagCompound tag) { }
}