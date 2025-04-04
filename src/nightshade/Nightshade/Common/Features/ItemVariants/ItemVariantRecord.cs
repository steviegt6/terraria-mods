using System.Collections.Generic;

namespace Nightshade.Common.Features.ItemVariants;

/// <summary>
///     Holds known variants.
/// </summary>
/// <param name="Variants">
///     Fully-qualified paths to known, non-NPC-specific variants.
/// </param>
/// <param name="NpcVariants">
///     NPC type-mapped collection of fully-qualified paths to known,
///     NPC-specific variants.
/// </param>
public readonly record struct ItemVariantRecord(
    string[]                  Variants,
    Dictionary<int, string[]> NpcVariants
);