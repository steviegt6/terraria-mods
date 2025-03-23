using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.Nightshade.Core.Rendering;

public readonly struct SpriteBatchSnapshot(SpriteBatch spriteBatch)
{
    private readonly SpriteSortMode    sortMode          = spriteBatch.sortMode;
    private readonly BlendState        blendState        = spriteBatch.blendState;
    private readonly SamplerState      samplerState      = spriteBatch.samplerState;
    private readonly DepthStencilState depthStencilState = spriteBatch.depthStencilState;
    private readonly RasterizerState   rasterizerState   = spriteBatch.rasterizerState;
    private readonly Effect?           customEffect      = spriteBatch.customEffect;
    private readonly Matrix            transformMatrix   = spriteBatch.transformMatrix;

    public void Apply(SpriteBatch spriteBatch)
    {
        if (spriteBatch.beginCalled)
        {
            spriteBatch.End();
        }

        spriteBatch.Begin(
            sortMode,
            blendState,
            samplerState,
            depthStencilState,
            rasterizerState,
            customEffect,
            transformMatrix
        );
    }
}