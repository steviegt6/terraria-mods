using System.Diagnostics;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

using Tomat.TML.Mod.Nightshade.Core.Attributes;
using Tomat.TML.Mod.Nightshade.Core.Rendering;

namespace Tomat.TML.Mod.Nightshade.Content.Menus;

// very simple btw
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
internal sealed class SimpleModMenu : ModMenu
{
    [InitializedInLoad]
    private static MiscShaderData? panelShaderData;

    [InitializedInLoad]
    private static MiscShaderData? flowerShaderData;

    [InitializedInLoad]
    private static Asset<Texture2D>? icon;

    [InitializedInLoad]
    private static Asset<Texture2D>? iconDots;

    public override void Load()
    {
        base.Load();

        const string path = "Assets/Images/UI/ModIcon/";

        icon     = Mod.Assets.Request<Texture2D>(path + "Icon");
        iconDots = Mod.Assets.Request<Texture2D>(path + "Icon_Dots");

        const string panel_shader_path  = "Assets/Shaders/UI/ModPanelShader";
        const string flower_shader_path = "Assets/Shaders/UI/CoolFlowerShaderBackground";

        var panelShader = Mod.Assets.Request<Effect>(panel_shader_path);
        panelShaderData = new MiscShaderData(panelShader, "PanelShader");

        var flowerShader = Mod.Assets.Request<Effect>(flower_shader_path);
        flowerShaderData = new MiscShaderData(flowerShader, "FlowerShader");
    }

    public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
    {
        {
            var snapshot = new SpriteBatchSnapshot(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.UIScaleMatrix
            );

            var dims = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            
            Debug.Assert(panelShaderData is not null);
            panelShaderData.Shader.Parameters["grayness"].SetValue(1f);
            panelShaderData.Shader.Parameters["inColor"].SetValue(new Vector3(1f, 0f, 1f));
            panelShaderData.Shader.Parameters["speed"].SetValue(0.2f);
            panelShaderData.Shader.Parameters["uSource"].SetValue(new Vector4(dims.Width, dims.Height, dims.X, dims.Y));
            panelShaderData.Shader.Parameters["uHoverIntensity"].SetValue(1f);
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

            // dims = dims;
            // dims.Y = (int)logoDrawCenter.Y - (Main.screenHeight / 2); // + (Main.screenHeight / 2);
            
            // half res for testing... someone do it better!!
            // dims = new Rectangle(dims.X, dims.Y, dims.Width / 2, dims.Height / 2);

            Debug.Assert(flowerShaderData is not null);
            flowerShaderData.Shader.Parameters["uSource"].SetValue(new Vector4(dims.Width, dims.Height, dims.X, dims.Y));
            flowerShaderData.Apply();
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, dims, Color.White);

            spriteBatch.End();
            snapshot.Apply(spriteBatch);
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