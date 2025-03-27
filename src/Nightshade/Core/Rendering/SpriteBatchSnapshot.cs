using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.Nightshade.Core.Rendering;

public struct SpriteBatchSnapshot(SpriteBatch spriteBatch)
{
    public SpriteSortMode SortMode { get; set; } = spriteBatch.sortMode;

    public BlendState BlendState { get; set; } = spriteBatch.blendState;

    public SamplerState SamplerState { get; set; } = spriteBatch.samplerState;

    public DepthStencilState DepthStencilState { get; set; } = spriteBatch.depthStencilState;

    public RasterizerState RasterizerState { get; set; } = spriteBatch.rasterizerState;

    public Effect? CustomEffect { get; set; } = spriteBatch.customEffect;

    public Matrix TransformMatrix { get; set; } = spriteBatch.transformMatrix;

    public void Apply(SpriteBatch spriteBatch)
    {
        if (spriteBatch.beginCalled)
        {
            spriteBatch.End();
        }

        spriteBatch.Begin(
            SortMode,
            BlendState,
            SamplerState,
            DepthStencilState,
            RasterizerState,
            CustomEffect,
            TransformMatrix
        );
    }
}