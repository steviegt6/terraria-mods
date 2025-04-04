using System;

namespace Nightshade.Common.Features.ItemVariants;

/// <summary>
///     Describes the path to search for item variants in.
/// </summary>
/// <param name="path">The path.</param>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class ItemVariantsPathAttribute(string path) : Attribute
{
    internal string Path { get; } = path;
}