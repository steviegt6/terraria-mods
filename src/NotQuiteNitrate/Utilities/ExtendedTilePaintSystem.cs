using System.Collections.Generic;

using JetBrains.Annotations;

using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Utilities;

public sealed class ExtendedTilePaintSystem : TilePaintSystemV2
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
    private sealed class ExtendedTilePaintSystemImplementer : ILoadable
    {
        void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
        {
            On_TilePaintSystemV2.Reset += (orig, self) =>
            {
                orig(self);

                if (self is ExtendedTilePaintSystem extended)
                {
                    extended.OnReset();
                }
            };

            Main.instance.TilePaintSystem = new ExtendedTilePaintSystem(Main.instance.TilePaintSystem);
        }

        void ILoadable.Unload()
        {
            Main.instance.TilePaintSystem = Instance.original;
        }
    }

    private sealed class LiquidSlopeRenderTargetHolder : ARenderTargetHolder
    {
        public LiquidSlopeVariationKey Key { get; set; }

        public override void Prepare()
        {
            PrepareTextureIfNecessary(Main.Assets.Request<Texture2D>(TextureAssets.Tile[Key.TileType].Name,   AssetRequestMode.ImmediateLoad).Value);
            PrepareTextureIfNecessary(Main.Assets.Request<Texture2D>(TextureAssets.Liquid[Key.TileType].Name, AssetRequestMode.ImmediateLoad).Value);
        }

        public override void PrepareShader() { }
    }

    public readonly record struct LiquidSlopeVariationKey(
        int TileType,
        int TileStyle,
        int PaintColor,
        int LiquidType
    );

    public static ExtendedTilePaintSystem Instance => (ExtendedTilePaintSystem)Main.instance.TilePaintSystem;

    private Dictionary<LiquidSlopeVariationKey, LiquidSlopeRenderTargetHolder> liquidSlopeRenders = [];

    private readonly TilePaintSystemV2 original;

    private ExtendedTilePaintSystem(TilePaintSystemV2 original)
    {
        this.original = original;

        _tilesRenders      = original._tilesRenders;
        _wallsRenders      = original._wallsRenders;
        _treeTopRenders    = original._treeTopRenders;
        _treeBranchRenders = original._treeBranchRenders;

        _requests = original._requests;
    }

    private void OnReset() { }
}