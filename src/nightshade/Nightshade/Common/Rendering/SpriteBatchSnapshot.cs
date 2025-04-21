using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nightshade.Common.Rendering;

internal struct SpriteBatchSnapshot(SpriteBatch spriteBatch)
{
    public SpriteSortMode SortMode { get; set; } = spriteBatch.sortMode;

    public BlendState BlendState { get; set; } = spriteBatch.blendState;

    public SamplerState SamplerState { get; set; } = spriteBatch.samplerState;

    public DepthStencilState DepthStencilState { get; set; } = spriteBatch.depthStencilState;

    public RasterizerState RasterizerState { get; set; } = spriteBatch.rasterizerState;

    public Effect? CustomEffect { get; set; } = spriteBatch.customEffect;

    public Matrix TransformMatrix { get; set; } = spriteBatch.transformMatrix;
}

internal static class SpriteBatchSnapshotExtensions
{
    public static void End(this SpriteBatch @this, out SpriteBatchSnapshot ss)
    {
        ss = new SpriteBatchSnapshot(@this);
        @this.End();
    }

    public static void Begin(this SpriteBatch @this, in SpriteBatchSnapshot ss)
    {
        @this.Begin(
            ss.SortMode,
            ss.BlendState,
            ss.SamplerState,
            ss.DepthStencilState,
            ss.RasterizerState,
            ss.CustomEffect,
            ss.TransformMatrix
        );
    }

    public static void Restart(this SpriteBatch @this, in SpriteBatchSnapshot ss)
    {
        @this.End();
        @this.Begin(ss);
    }
}