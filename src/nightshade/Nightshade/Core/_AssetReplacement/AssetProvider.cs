using ReLogic.Content;

namespace Nightshade.Core;

/// <summary>
///     Provides a mutable reference to an asset.
/// </summary>
/// <typeparam name="T">The asset type.</typeparam>
public delegate ref Asset<T> AssetProvider<T>() where T : class;