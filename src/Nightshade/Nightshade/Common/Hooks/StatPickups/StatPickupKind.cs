namespace Nightshade.Common.Hooks.StatPickups;

public enum StatPickupKind
{
    // We can also include Nebula pickups later.

    /// <summary>
    ///     Heals 20 health.
    /// </summary>
    Heart,
    
    /// <summary>
    ///     Restores 100 mana.
    /// </summary>
    Star,
    
    /// <summary>
    ///     Generated from the Mana Cloak accessory, restores 50 mana.
    /// </summary>
    ManaCloakStar,
}