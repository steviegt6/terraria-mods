using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     A modded achievement.  Unrelated to the vanilla
///     <see cref="Terraria.Achievements.Achievement"/> type, which this
///     implementation wholly replaces.
/// </summary>
public abstract class Achievement : ModType
{
    protected sealed override void Register()
    {
        AchievementImpl.Register(this);
    }

    protected sealed override void InitTemplateInstance() { }
}