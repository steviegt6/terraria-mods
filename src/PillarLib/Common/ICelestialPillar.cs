using Terraria.GameContent.UI.BigProgressBar;

namespace Tomat.TML.Mod.PillarLib.Common;

/// <summary>
///     The basic contract of a Celestial Pillar.
/// </summary>
public interface ICelestialPillar
{
    /// <summary>
    ///     The NPC ID of the Celestial Pillar.
    /// </summary>
    int CelestialPillarType { get; }

    /// <summary>
    ///     The singleton progress bar for this Celestial Pillar.
    /// </summary>
    LunarPillarBigProgessBar ProgressBar { get; }

    /// <summary>
    ///     Whether this pillar is currently active (alive) in the world.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    ///     The current shield strength of the spawned tower.
    /// </summary>
    int ShieldStrength { get; set; }
}