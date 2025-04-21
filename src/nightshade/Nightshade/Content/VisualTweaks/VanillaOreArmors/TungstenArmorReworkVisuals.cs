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
    private static ManagedRenderTarget? _mRT_bloom;
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.TungstenArmorMoltenShader.Parameters>? _immolationShader;
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.GaussianBloom.Parameters>? _bloomShader;

    private static bool _shouldGammaBoost = false;
    public static void Initialize()
    {
        QueueMainThreadAction(() =>
        {
            _mRT = new ManagedRenderTarget(screenWidth, screenHeight, true);
            _mRT.Initialize(screenWidth, screenHeight);

            _mRT_bloom = new ManagedRenderTarget(screenWidth, screenHeight, true);
            _mRT_bloom.Initialize(screenWidth, screenHeight);

            _immolationShader = AssetReferences.Assets.Shaders.Misc.TungstenArmorMoltenShader.CreateStripShader();
            _immolationShader.Parameters.uSize = new Vector2(_mRT.Value.Width, _mRT.Value.Height);
            _immolationShader.Parameters.uScale = 1f;

            _bloomShader = AssetReferences.Assets.Shaders.Misc.GaussianBloom.CreateStripShader();
            _bloomShader.Parameters.uSize = new Vector2(_mRT.Value.Width, _mRT.Value.Height);
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

        Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayer += static (orig, self, camera, player, position, rotation, origin, shadow, scale) =>
        {
            Debug.Assert(_mRT != null);
            Debug.Assert(_mRT.Value != null);
            Debug.Assert(_immolationShader != null);

            Debug.Assert(_mRT_bloom != null);
            Debug.Assert(_mRT_bloom.Value != null);
            Debug.Assert(_bloomShader != null);

            var sbsn = new SpriteBatchSnapshot(Main.spriteBatch);

            var rts = instance.GraphicsDevice.GetRenderTargets();
            RtContentPreserver.ApplyToBindings(rts);

            graphics.graphicsDevice.SetRenderTarget(_mRT.Value);
            graphics.GraphicsDevice.Clear(Color.Transparent);
            _shouldGammaBoost = true;
            orig.Invoke(self, camera, player, position, rotation, origin, shadow, scale);
            _shouldGammaBoost = false;
            Main.spriteBatch.End();

            graphics.graphicsDevice.SetRenderTarget(_mRT_bloom.Value);

            Main.spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.Default,
            RasterizerState.CullNone,
            null,
            GameViewMatrix.EffectMatrix
            );

            _immolationShader.Parameters.uSize = _mRT.Value.Bounds.Size();
            _immolationShader.Parameters.uScale = 1f;
            _immolationShader.Apply();

            spriteBatch.Draw(_mRT.Value, Vector2.Zero, Color.Red);
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

            _immolationShader.Parameters.uSize = _mRT.Value.Bounds.Size();
            _immolationShader.Parameters.uScale = 1f;
            _immolationShader.Apply();

            spriteBatch.Draw(_mRT.Value, Vector2.Zero, Color.Red);
            Main.spriteBatch.End();

            Main.spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.Additive,
            SamplerState.PointClamp,
            DepthStencilState.Default,
            RasterizerState.CullNone,
            null,
            GameViewMatrix.EffectMatrix
            );

            _bloomShader.Parameters.uSize = _mRT.Value.Bounds.Size();
            _bloomShader.Apply();

            spriteBatch.Draw(_mRT_bloom.Value, Vector2.Zero, Color.Red);
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
