namespace Nightshade.Common.Features;

/// <summary>
///     A developer armor item model.
/// </summary>
/// <param name="ItemType">The item type.</param>
/// <param name="Stack">The number of this item.</param>
/// <param name="Hardmode">
///     Whether this item should only be granted from hardmode bags.
/// </param>
/// <remarks>
///     While the feature is referred to as &quot;Dev Armor&quot;, it can be any
///     item and not just armor.
/// </remarks>
public readonly record struct DevArmorItem(
    int ItemType,
    int Stack = 1,
    bool Hardmode = false
);

/// <summary>
///     The kind of &quot;dev armor&quot; this is.
/// </summary>
public enum DevArmorKind
{
    /// <summary>
    ///     A vanilla developer armor set.
    /// </summary>
    VanillaDev,
    
    /// <summary>
    ///     A tModLoader patreon armor set.
    /// </summary>
    ModLoaderPatreon,
    
    /// <summary>
    ///     A tModLoader developer armor set.
    /// </summary>
    ModLoaderDev,
    
    /// <summary>
    ///     A Nightshade developer armor set.
    /// </summary>
    NightshadeDev,
}

/// <summary>
///     A &quot;dev armor&quot; definition, providing all items that make up a
///     developer set.
/// </summary>
/// <param name="ArmorKind">The kind of set this is.</param>
/// <param name="Items">The items making up the set.</param>
public readonly record struct DevArmorDefinition(
    DevArmorKind ArmorKind,
    params DevArmorItem[] Items
);