namespace Nightshade.Common.Features;

/// <summary>
///     Denotes a tile as providing a pot behavior, since it's a pot.
/// </summary>
public interface IPot
{
    /// <summary>
    ///     The pot behavior that this tile provides, which should be a
    ///     singleton.
    /// </summary>
    PotBehavior Behavior { get; }
}