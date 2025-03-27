using System;

using Terraria.ModLoader;

namespace Tomat.TML.Mod.Nightshade.Core.Attributes;

// TODO: Use for smart checking with an analyzer.

/// <summary>
///     Indicates the decorated member will not be null after initialization
///     through an <see cref="ILoadable"/> implementation.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class InitializedInLoadAttribute : Attribute;