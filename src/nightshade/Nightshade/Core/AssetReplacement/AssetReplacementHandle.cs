using System;

using ReLogic.Content;

namespace Nightshade.Core.AssetReplacement;

/// <summary>
///     A handle to an asset replacement.  Upon creation, the original asset is
///     replaced with the new asset.  Upon disposal, the original asset is
///     restored.
/// </summary>
/// <typeparam name="T">The asset type.</typeparam>
public readonly struct AssetReplacementHandle<T> : IDisposable
    where T : class
{
    private readonly AssetProvider<T> assetProvider;
    private readonly Asset<T>         originalAsset;

    public AssetReplacementHandle(
        AssetProvider<T> assetProvider,
        Asset<T>         newAsset
    )
    {
        this.assetProvider = assetProvider;
        originalAsset      = assetProvider();

        // This is the important part.  Set the value.
        assetProvider() = newAsset;
    }

    /// <summary>
    ///     Resets the replaced asset with the original asset once again.
    /// </summary>
    public void Dispose()
    {
        assetProvider() = originalAsset;
    }
}