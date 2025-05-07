using System.Diagnostics;

using Daybreak.Common.Rendering;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Rendering;
using Nightshade.Core;
using Nightshade.Core.Attributes;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Nightshade.Content.Menus;

// very simple btw
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
internal sealed class SimpleModMenu : ModMenu
{
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.UI.ModPanelShader.Parameters>? panelShaderData;

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.UI.CoolFlowerShader.Parameters>? flowerShaderData;

    [InitializedInLoad]
    private static Asset<Texture2D>? icon;

    [InitializedInLoad]
    private static Asset<Texture2D>? iconDots;

    [InitializedInLoad]
    private static ManagedRenderTarget? managedRt;

    public override int Music => MusicLoader.GetMusicSlot("Nightshade/Assets/Music/NIGHTSHADE_-_track_0");

    public override void Load()
    {
        base.Load();

        icon     = Assets.Images.UI.ModIcon.Icon.Asset;
        iconDots = Assets.Images.UI.ModIcon.Icon_Dots.Asset;

        Main.QueueMainThreadAction(
            () =>
            {
                panelShaderData  = Assets.Shaders.UI.ModPanelShader.CreatePanelShader();
                flowerShaderData = Assets.Shaders.UI.CoolFlowerShader.CreateFlowerShader();
            }
        );

        managedRt = new ManagedRenderTarget((width, height) => new RenderTarget2D(Main.instance.GraphicsDevice, width / 2, height / 2), true);

        Main.QueueMainThreadAction(
            () =>
            {
                managedRt.Initialize(Main.screenWidth, Main.screenHeight);
            }
        );
    }

    public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
    {
        Debug.Assert(managedRt is not null);
        Debug.Assert(panelShaderData is not null);
        Debug.Assert(flowerShaderData is not null);

        // Background rendering.
        {
            var dims = new Rectangle(0, 0, Main.screenWidth / 2, Main.screenHeight / 2);
            
            spriteBatch.End(out var ss);

            var oldRts = Main.instance.GraphicsDevice.GetRenderTargets();

            Main.instance.GraphicsDevice.SetRenderTarget(managedRt.Value);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.UIScaleMatrix
            );
            {
                panelShaderData.Parameters.uGrayness        = 1f;
                panelShaderData.Parameters.uGrayness        = 1f;
                panelShaderData.Parameters.uInColor         = new Vector3(1f, 0f, 1f);
                panelShaderData.Parameters.uSpeed           = 0.2f;
                panelShaderData.Parameters.uSource          = new Vector4(dims.Width, dims.Height, dims.X, dims.Y);
                panelShaderData.Parameters.uHoverIntensity  = 1f;
                panelShaderData.Parameters.uPixel           = 1f;
                panelShaderData.Parameters.uColorResolution = 16f;
                panelShaderData.Apply();
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, dims, Color.White);

                spriteBatch.End();
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    null,
                    Main.UIScaleMatrix
                );

                flowerShaderData.Parameters.uSource = new Vector4(dims.Width, dims.Height, dims.X, dims.Y);
                flowerShaderData.Parameters.uPixel  = 1f;
                flowerShaderData.Apply();
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, dims, Color.White);
            }
            spriteBatch.End();

            Main.instance.GraphicsDevice.SetRenderTargets(oldRts);

            spriteBatch.Begin(ss with { SamplerState = SamplerState.PointClamp });
            {
                spriteBatch.Draw(managedRt.Value, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }
            spriteBatch.Restart(in ss);
        }

        // Draw logo flower.
        {
            var rotation = Main.GlobalTimeWrappedHourly / 10f;
            var origin   = icon.Size()                  / 2f;

            Debug.Assert(icon is not null);
            Debug.Assert(iconDots is not null);

            spriteBatch.Draw(
                icon.Value,
                logoDrawCenter,
                null,
                Color.White,
                rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Draw(
                iconDots.Value,
                logoDrawCenter,
                null,
                Color.White,
                rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        return false;
    }
}