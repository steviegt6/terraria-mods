using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria.GameContent;

namespace Tomat.TML.Mod.Nightshade.Common.Features.AssetReplacement;

/// <summary>
///     Provides utility methods for replacing assets.
/// </summary>
internal static class AssetReplacer
{
    public static AssetReplacementHandle<Texture2D> Npc(int npcId, Asset<Texture2D> newAsset)
    {
        return new AssetReplacementHandle<Texture2D>(() => ref TextureAssets.Npc[npcId], newAsset);
    }
}