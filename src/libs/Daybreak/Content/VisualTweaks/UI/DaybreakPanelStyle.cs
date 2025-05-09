using Daybreak.Common.Features.ModPanel;
using Daybreak.Common.Rendering;
using Daybreak.Core.Hooks;
using Daybreak.Core;

using System.Diagnostics;

using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ModLoader.UI;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Daybreak.Common.Features.ModPanel;
using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using ReLogic.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
internal sealed class DaybreakPanelStyle : ModPanelStyleExt
{
    private static WrapperShaderData<Assets.Shaders.UI.ModPanelShader.Parameters>? panelShaderData;
    private static WrapperShaderData<Assets.Shaders.UI.PowerfulSunIcon.Parameters>? whenDayBreaksShaderData;

    public override void Load()
    {
        base.Load();

        panelShaderData = Assets.Shaders.UI.ModPanelShader.CreatePanelShader();
        whenDayBreaksShaderData = Assets.Shaders.UI.PowerfulSunIcon.CreatePanelShader();
    }

    private sealed class ModIcon : UIImage
    {
        public ModIcon() : base(TextureAssets.MagicPixel)
        {
        }

        public override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.End(out var ss);
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Additive,
                SamplerState.PointClamp,
                DepthStencilState.None,
                ss.RasterizerState,
                null,
                Main.UIScaleMatrix
            );
            
            // const int offset = (80 - 46) / 2;
            var dims = GetDimensions().ToRectangle();

            Debug.Assert(whenDayBreaksShaderData is not null);
            whenDayBreaksShaderData.Parameters.uGrayness = 1f;
            whenDayBreaksShaderData.Parameters.uInColor = new Vector3(1f, 0f, 1f);
            whenDayBreaksShaderData.Parameters.uSpeed = 0.2f;
            whenDayBreaksShaderData.Parameters.uSource = new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y);
            whenDayBreaksShaderData.Parameters.uPixel = 2f;
            whenDayBreaksShaderData.Parameters.uColorResolution = 10f;
            whenDayBreaksShaderData.Apply();

            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                dims.TopLeft(),
                dims,
                Color.Red,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Restart(ss);
        }
    }

    public override UIImage ModifyModIcon(UIModItem element, UIImage modIcon, ref int modIconAdjust)
    {
        return new ModIcon()
        {
            Left = modIcon.Left,
            Top = modIcon.Top,
            Width = modIcon.Width,
            Height = modIcon.Height,
        };
    }

    public override bool PreDrawPanel(UIModItem element, SpriteBatch sb)
    {
        if (element._needsTextureLoading)
        {
            element._needsTextureLoading = false;
            element.LoadTextures();
        }

        // Render our cool custom panel with a shader.
        {
            sb.End(out var ss);
            sb.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                ss.RasterizerState,
                null,
                Main.UIScaleMatrix
            );
            {
                var dims = element.GetDimensions();
                Debug.Assert(panelShaderData is not null);
                panelShaderData.Parameters.uGrayness = 1f;
                panelShaderData.Parameters.uInColor = new Vector3(1f, 0f, 1f);
                panelShaderData.Parameters.uSpeed = 0.2f;
                panelShaderData.Parameters.uSource = new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y);
                panelShaderData.Parameters.uPixel = 2f;
                panelShaderData.Parameters.uColorResolution = 10f;
                panelShaderData.Apply();

                Debug.Assert(element._backgroundTexture is not null);
                element.DrawPanel(sb, element._backgroundTexture.Value, element.BackgroundColor);
            }
            sb.Restart(ss);
        }

        Debug.Assert(element._borderTexture is not null);
        element.DrawPanel(sb, element._borderTexture.Value, element.BorderColor);

        return false;
    }
}