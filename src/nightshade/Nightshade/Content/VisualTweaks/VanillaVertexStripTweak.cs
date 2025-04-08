using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Nightshade.Common.Rendering;
using Nightshade.Common.Utilities;
using Nightshade.Core;
using Nightshade.Core.Attributes;

using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace Nightshade.Content.VisualTweaks;

[Autoload(Side = ModSide.Client)]
internal sealed class VanillaVertexStripTweak : ModSystem
{
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.VanillaVertexStripShader.Parameters>? shader;

    [InitializedInLoad]
    private static ManagedRenderTarget? managedRt;

    public override void Load()
    {
        base.Load();

        Main.QueueMainThreadAction(
            () =>
            {
                shader = Assets.Shaders.Misc.VanillaVertexStripShader.CreateStripShader();
                {
                    shader.Parameters.uPixel           = 2f;
                    shader.Parameters.uColorResolution = 16f;
                }
            }
        );

        managedRt = new ManagedRenderTarget(reinitOnResolutionChange: true);

        Main.QueueMainThreadAction(
            () =>
            {
                managedRt.Initialize(Main.screenWidth, Main.screenHeight);

                Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
                Main.graphics.ApplyChanges();
            }
        );

        IL_EmpressBladeDrawer.Draw += WrapDraw;
        IL_FinalFractalHelper.Draw += WrapDraw;
        IL_FlameLashDrawer.Draw    += WrapDraw;
        IL_LightDiscDrawer.Draw    += WrapDraw;
        IL_MagicMissileDrawer.Draw += WrapDraw;
        IL_RainbowRodDrawer.Draw   += WrapDraw;
    }

    private static void WrapDraw(ILContext il)
    {
        var rtsIndex = il.AddVariable<RenderTargetBinding[]>();
        var snpIndex = il.AddVariable<SpriteBatchSnapshot>();

        var c = new ILCursor(il);

        c.EmitDelegate(
            static () => new SpriteBatchSnapshot(Main.spriteBatch)
        );
        c.EmitStloc(snpIndex);

        c.EmitLdloc(snpIndex);
        c.EmitDelegate(
            static (SpriteBatchSnapshot snapshot) =>
            {
                Debug.Assert(managedRt is not null);

                Main.spriteBatch.End();

                var rts = Main.instance.GraphicsDevice.GetRenderTargets();

                Main.instance.GraphicsDevice.SetRenderTarget(managedRt.Value);
                Main.instance.GraphicsDevice.Clear(Color.Transparent);

                snapshot.Apply(Main.spriteBatch);

                return rts;
            }
        );
        c.EmitStloc(rtsIndex);

        c.GotoNext(MoveType.After, x => x.MatchCallvirt<VertexStrip>(nameof(VertexStrip.DrawTrail)));

        c.EmitLdloc(rtsIndex);
        c.EmitLdloc(snpIndex);
        c.EmitDelegate(
            static (RenderTargetBinding[] rts, SpriteBatchSnapshot snapshot) =>
            {
                Debug.Assert(shader is not null);
                Debug.Assert(managedRt is not null);
                Debug.Assert(managedRt.Value is not null);

                Main.spriteBatch.End();
                Main.instance.GraphicsDevice.SetRenderTargets(rts);

                Main.spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    DepthStencilState.Default,
                    RasterizerState.CullNone,
                    null,
                    Main.Transform
                );

                // Main.graphics.GraphicsDevice.Textures[0]      = managedRt.Value;
                // Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                shader.Parameters.uSize = new Vector2(managedRt.Value.Width, managedRt.Value.Height);
                shader.Apply();

                Main.spriteBatch.Draw(managedRt.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

                Main.spriteBatch.End();
                snapshot.Apply(Main.spriteBatch);
            }
        );
    }
}