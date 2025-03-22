using System;

using ReLogic.Content;

namespace Tomat.TML.Mod.Nightshade.Common.Features.AssetReplacement;

/// <summary>
///     A handle to an asset replacement.  Upon creation, the original asset is
///     replaced with the new asset.  Upon disposal, the original asset is
///     restored.
/// </summary>
/// <typeparam name="T">The asset type.</typeparam>
public sealed class AssetReplacementHandle<T> : IDisposable
    where T : class
{
    /// <summary>
    ///     The original asset to be replaced.
    /// </summary>
    public Asset<T> OriginalAsset { get; }

    /// <summary>
    ///     The new asset to replace the original asset.
    /// </summary>
    public Asset<T> NewAsset { get; }

    private readonly AssetProvider<T> assetProvider;

    public AssetReplacementHandle(
        AssetProvider<T> assetProvider,
        Asset<T>         newAsset
    )
    {
        OriginalAsset      = assetProvider();
        NewAsset           = newAsset;
        this.assetProvider = assetProvider;

        // This is the important part.  Set the value.
        assetProvider() = newAsset;
    }

    /// <summary>
    ///     Resets the replaced asset with the original asset once again.
    /// </summary>
    public void Dispose()
    {
        assetProvider() = OriginalAsset;
    }
}