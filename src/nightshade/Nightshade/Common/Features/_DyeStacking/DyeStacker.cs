using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Rendering;

using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Common.Features._DyeStacking;

internal sealed class DyeStacker : ModSystem
{
    private sealed class Test : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);

            entity.dye = BindShader(
                GameShaders.Armor.GetShaderIdFromItemId(ItemID.RedDye)
            );
        }
    }

    private static readonly Dictionary<int, int[]> dye_map = [];

    private static int count;
    private static SpriteBatch? immediateRenderer;

    public static int BindShader(params int[] shaders)
    {
        var key = int.MaxValue - count++;
        dye_map[key] = shaders;
        return key;
    }

    public override void Load()
    {
        base.Load();

        On_PlayerDrawHelper.SetShaderForData += ManipulateDrawDataToOnlyUseFinalShader;

        Main.RunOnMainThread(() =>
            {
                immediateRenderer = new SpriteBatch(Main.instance.GraphicsDevice);
            }
        );
    }

    // Basically all uses of armor shaders are done with DrawData which routes
    // to SetShaderForData.
    // Concept: Hook into SetShaderForData, determine what shaders we're using,
    // and render them all in order until the final shader.  Our final render
    // target (minus the final shader) is what the DrawData will then use, which
    // will get rendered later with the final shader applied.
    // Very fragile!!!  May break in obscure cases and cases where the DrawData
    // is reused.
    private static void ManipulateDrawDataToOnlyUseFinalShader(On_PlayerDrawHelper.orig_SetShaderForData orig, Player player, int cHead, ref DrawData cdd)
    {
        // TODO: Should we handle cHead?
        if (dye_map.TryGetValue(cdd.shader, out var shaders))
        {
            var texture = cdd.texture;

            immediateRenderer?.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null);

            for (var i = 0; i < shaders.Length - 1; i++)
            {
                var oldRts = Main.instance.GraphicsDevice.GetRenderTargets();
                RtContentPreserver.ApplyToBindings(oldRts);

                var rt = RenderTargetPool.Get(cdd.texture.Width, cdd.texture.Height);
                Main.instance.GraphicsDevice.SetRenderTarget(rt);
                Main.instance.GraphicsDevice.Clear(Color.Transparent);

                PlayerDrawHelper.UnpackShader(shaders[i], out var localShaderIndex, out var shaderType);
                Debug.Assert(shaderType == PlayerDrawHelper.ShaderConfiguration.ArmorShader);

                GameShaders.Hair.Apply(0, player, cdd);
                GameShaders.Armor.Apply(localShaderIndex, player, cdd);

                immediateRenderer?.Draw(texture, Vector2.Zero, Color.White);

                Main.instance.GraphicsDevice.SetRenderTargets(oldRts);

                if (i != 0)
                {
                    if (texture is RenderTarget2D cddRt)
                    {
                        RenderTargetPool.Return(cddRt);
                    }
                }

                texture = rt;
            }

            immediateRenderer?.End();

            // Make it use the final shader.
            cdd.shader = 0;
            cdd.texture = texture;
        }

        // TODO: We leak a pooled Target at the end.

        // Final render pass.
        orig(player, cHead, ref cdd);
    }
}