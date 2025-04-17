using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Rendering;
using Nightshade.Core;
using Nightshade.Core.Attributes;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

using static Terraria.Main;

namespace Nightshade.Content.VisualTweaks.VanillaOreArmors;

internal static class TungstenRT
{
    [InitializedInLoad]
    private static ManagedRenderTarget? _mRT;

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.TungstenArmorMoltenShader.Parameters>? shader;

    private static bool _shouldGammaBoost = false;
    public static void Initialize()
    {
        QueueMainThreadAction(() =>
        {
            _mRT = new ManagedRenderTarget(screenWidth, screenHeight, true);
            _mRT.Initialize(screenWidth, screenHeight);

            shader = AssetReferences.Assets.Shaders.Misc.TungstenArmorMoltenShader.CreateStripShader();
            shader.Parameters.uPixel = 2f;
            shader.Parameters.uSize = new Vector2(_mRT.Value.Width, _mRT.Value.Height);
        });
        Terraria.On_Lighting.GetColorClamped += (orig, x, y, c) =>
        {
            if (_shouldGammaBoost)
            {
                var color = orig.Invoke(x, y, c);
                var thresh = (byte)(0.05 * 255);

                if (color.R < thresh) color.R = thresh;
                if (color.G < thresh) color.G = thresh;
                if (color.B < thresh) color.B = thresh;

                return color;
            }

            return orig.Invoke(x, y, c);
        };

        Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayer += (orig, self, camera, player, position, rotation, origin, shadow, scale) =>
        {
            Debug.Assert(_mRT != null);
            Debug.Assert(_mRT.Value != null);
            Debug.Assert(shader != null);

            var sbsn = new SpriteBatchSnapshot(Main.spriteBatch);

            var rts = instance.GraphicsDevice.GetRenderTargets();
            RtContentPreserver.ApplyToBindings(rts);

            graphics.graphicsDevice.SetRenderTarget(_mRT.Value);
            graphics.GraphicsDevice.Clear(Color.Transparent);
            _shouldGammaBoost = true;
            orig.Invoke(self, camera, player, position, rotation, origin, shadow, scale);
            _shouldGammaBoost = false;
            Main.spriteBatch.End();

            graphics.graphicsDevice.SetRenderTargets(rts);

            Main.spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.Default,
            RasterizerState.CullNone,
            null,
            GameViewMatrix.EffectMatrix
            );

            shader.Parameters.uPixel = 2f * Main.GameViewMatrix.Zoom.X;
            shader.Parameters.uSize = new Vector2(1600, 900);
            shader.Apply();

            spriteBatch.Draw(_mRT.Value, Vector2.Zero, Color.Red);
            Main.spriteBatch.End();

            sbsn.Apply(Main.spriteBatch);
        };
    }

    public static void Deinitialize()
    {
        if (_mRT != null)
        {
            _mRT.Dispose();
            _mRT = null;
        }
    }


    internal static void RenderAPlayer(Player player)
    {

        if (_mRT == null)
        {
            return;
        }

        var rts = instance.GraphicsDevice.GetRenderTargets();
        graphics.graphicsDevice.SetRenderTarget(_mRT.Value);

        graphics.GraphicsDevice.Clear(Color.Transparent);
        PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin);

        graphics.graphicsDevice.SetRenderTargets(rts);
    }
}

public class TungstenRTLoader : ILoadable
{
    public void Load(Mod mod)
    {
        TungstenRT.Initialize();
    }

    public void Unload()
    {
        TungstenRT.Deinitialize();
    }
}

public class TungstenArmorLighting : ModPlayer
{
    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

        double luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;

        if (luminance < 0.3f)
        {
            (r, g, b) = (0.3f * 0.2126f, 0.3f * 0.7152f, 0.3f * 0.0722f);
        }
    }
}