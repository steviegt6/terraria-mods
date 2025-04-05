using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Nightshade.Common.Rendering;
using Nightshade.Common.Utilities;
using Nightshade.Core;
using Nightshade.Core.Attributes;

using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace Nightshade.Content.VisualTweaks;

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
                    shader.Parameters.uColorResolution = 4f;
                }
            }
        );

        managedRt = new ManagedRenderTarget(reinitOnResolutionChange: true);

        Main.QueueMainThreadAction(
            () =>
            {
                managedRt.Initialize(Main.screenWidth, Main.screenHeight);
            }
        );

        IL_EmpressBladeDrawer.Draw += WrapDraw;
        IL_FinalFractalHelper.Draw += WrapDraw;
        IL_FlameLashDrawer.Draw    += WrapDraw;
        IL_LightDiscDrawer.Draw    += WrapDraw;
        IL_MagicMissileDrawer.Draw += WrapDraw;
        IL_RainbowRodDrawer.Draw   += WrapDraw;
    }

    //private static void WrapDraw(Action<object, Projectile> orig, object self, Projectile proj)
    private static void WrapDraw(ILContext il)
    {
        var rtsIndex = il.AddVariable<RenderTargetBinding[]>();

        var c = new ILCursor(il);

        c.EmitDelegate(
            static () =>
            {
                Debug.Assert(managedRt is not null);

                var rts = Main.instance.GraphicsDevice.GetRenderTargets();

                Main.instance.GraphicsDevice.SetRenderTarget(managedRt.Value);
                Main.instance.GraphicsDevice.Clear(Color.Transparent);

                return rts;
            }
        );
        c.EmitStloc(rtsIndex);

        c.GotoNext(MoveType.After, x => x.MatchCallvirt<VertexStrip>(nameof(VertexStrip.DrawTrail)));

        c.EmitLdloc(rtsIndex);
        c.EmitDelegate(
            static (RenderTargetBinding[] rts) =>
            {
                Debug.Assert(shader is not null);
                Debug.Assert(managedRt is not null);

                Main.instance.GraphicsDevice.SetRenderTargets(rts);

                shader.Parameters.uImage0 = managedRt.Value;
                shader.Apply();

                var snapshot = new SpriteBatchSnapshot(Main.spriteBatch);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(managedRt.Value, Vector2.Zero, Color.White);

                Main.spriteBatch.End();
                snapshot.Apply(Main.spriteBatch);

                // Main.spriteBatch.Draw(
                //     TextureAssets.MagicPixel.Value,
                //     new Rectangle(0, 0, Main.screenWidth, Main.screenWidth),
                //     Color.White
                // );
            }
        );
    }
}