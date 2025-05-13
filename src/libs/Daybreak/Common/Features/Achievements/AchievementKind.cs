namespace Daybreak.Common.Features.Achievements;

// TODO: PerPlayerPerWorld? lol

/// <summary>
///     The kind of achievement, clarifying how it's saved and what it pertains
///     to.
/// </summary>
public enum AchievementKind
{
    /// <summary>
    ///     Persistent in spite of any players or worlds.  Effectively like
    ///     regular vanilla achievements (though vanilla achievements are
    ///     re-assigned kinds).
    /// </summary>
    Persistent,
    
    /// <summary>
    ///     The achievement is saved per-player.  Use this for achievements such
    ///     as picking up items, crafting, killing certain kinds of enemies (but
    ///     not bosses [think collectors]), etc.
    /// </summary>
    PerPlayer,
    
    /// <summary>
    ///     The achievement is saved per-world.  Use this for achievements that
    ///     pertain to world events such as boss deaths, etc.
    /// </summary>
    PerWorld,
}