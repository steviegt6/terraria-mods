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
                GameShaders.Armor.GetShaderIdFromItemId(ItemID.AcidDye),
                GameShaders.Armor.GetShaderIdFromItemId(ItemID.GelDye),
                GameShaders.Armor.GetShaderIdFromItemId(ItemID.RedDye)
            );
        }
    }

    private static readonly Dictionary<int, int[]> dye_map = [];
    private static readonly Queue<RenderTarget2D> hanging_targets = [];

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
        On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += PropagateDrawDataChanges;

        Main.RunOnMainThread(() =>
            {
                immediateRenderer = new SpriteBatch(Main.instance.GraphicsDevice);
            }
        );
    }

    public override void PostUpdateEverything()
    {
        base.PostUpdateEverything();

        while (hanging_targets.TryDequeue(out var rt))
        {
            RenderTargetPool.Return(rt);
        }
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
        if (cdd.texture is not null && dye_map.TryGetValue(cdd.shader, out var shaders))
        {
            var texture = cdd.texture;

            var rts = Main.instance.GraphicsDevice.GetRenderTargets();
            RtContentPreserver.ApplyToBindings(rts);


            for (var i = 0; i < shaders.Length - 1; i++)
            {
                var rt = RenderTargetPool.Get(cdd.texture.Width, cdd.texture.Height);
                hanging_targets.Enqueue(rt);

                Main.instance.GraphicsDevice.SetRenderTarget(rt);
                Main.instance.GraphicsDevice.Clear(Color.Transparent);

                PlayerDrawHelper.UnpackShader(shaders[i], out var localShaderIndex, out var shaderType);
                Debug.Assert(shaderType == PlayerDrawHelper.ShaderConfiguration.ArmorShader);

                GameShaders.Hair.Apply(0, player, cdd);
                GameShaders.Armor.Apply(localShaderIndex, player, cdd);

                immediateRenderer?.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null);
                immediateRenderer?.Draw(texture, Vector2.Zero, Color.White);
                immediateRenderer?.End();

                texture = rt;
            }

            // Make it use the final shader.
            {
                cdd.shader = shaders[^1];
                cdd.texture = texture;
            }
            
            Main.instance.GraphicsDevice.SetRenderTargets(rts);
        }

        // Final render pass.
        orig(player, cHead, ref cdd);
    }

    // We regrettably rewrite this entire method for one minor edit.
    private static void PropagateDrawDataChanges(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo)
    {
        var drawDataCache = drawinfo.DrawDataCache.ToArray();
        if (PlayerDrawLayers.spriteBuffer == null)
        {
            PlayerDrawLayers.spriteBuffer = new SpriteDrawBuffer(Main.graphics.GraphicsDevice, 200);
        }
        else
        {
            PlayerDrawLayers.spriteBuffer.CheckGraphicsDevice(Main.graphics.GraphicsDevice);
        }

        for (var i = 0; i < drawDataCache.Length; i++)
        {
            PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref drawDataCache[i]);
            if (drawDataCache[i].texture != null)
            {
                drawDataCache[i].Draw(PlayerDrawLayers.spriteBuffer);
            }
        }

        PlayerDrawLayers.spriteBuffer.UploadAndBind();
        var cdd = default(DrawData);
        var num = 0;
        for (var i = 0; i <= drawDataCache.Length; i++)
        {
            if (drawinfo.projectileDrawPosition == i)
            {
                if (cdd.shader != 0)
                {
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                }

                PlayerDrawLayers.spriteBuffer.Unbind();
                PlayerDrawLayers.DrawHeldProj(drawinfo, Main.projectile[drawinfo.drawPlayer.heldProj]);
                PlayerDrawLayers.spriteBuffer.Bind();
            }

            if (i == drawDataCache.Length)
            {
                continue;
            }

            cdd = drawDataCache[i];
            cdd.sourceRect ??= cdd.texture.Frame();

            PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
            if (cdd.texture != null)
            {
                PlayerDrawLayers.spriteBuffer.DrawSingle(num++);
            }
        }

        PlayerDrawLayers.spriteBuffer.Unbind();
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }
}