using ReLogic.Content;

namespace Tomat.TML.Mod.Nightshade.Common.Features.AssetReplacement;

/// <summary>
///     Provides a mutable reference to an asset.
/// </summary>
/// <typeparam name="T">The asset type.</typeparam>
public delegate ref Asset<T> AssetProvider<T>() where T : class;