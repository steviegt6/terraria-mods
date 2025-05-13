using Terraria.ModLoader;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     An achievement category, which an achievement may belong to an arbitrary
///     number of.
/// </summary>
public abstract class AchievementCategory : ModType
{
    // TODO
    public virtual bool IsCompleted { get; set; } = false;

    protected sealed override void Register()
    {
        AchievementImpl.RegisterCategory(this);
    }

    protected sealed override void InitTemplateInstance() { }
}