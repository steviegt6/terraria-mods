using System;

using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria.GameContent;

namespace Nightshade.Core;

internal static class AssetReplacer
{
    public readonly struct Handle<T> : IDisposable
        where T : class
    {
        private readonly Asset<T> source;
        private readonly T original;

        internal Handle(Asset<T> source, T target)
        {
            this.source = source;

            original = source.ownValue;
            source.ownValue = target;
        }

        public void Dispose()
        {
            source.ownValue = original;
        }
    }

    public static Handle<T> Replace<T>(Asset<T> oldAsset, T newAsset)
        where T : class
    {
        return new Handle<T>(oldAsset, newAsset);
    }
    
    public static Handle<Texture2D> Npc(int value, Texture2D newAsset)
    {
        return Replace(TextureAssets.Npc[value], newAsset);
    }
    
    public static Handle<Texture2D> Extra(int value, Texture2D newAsset)
    {
        return Replace(TextureAssets.Extra[value], newAsset);
    }
}