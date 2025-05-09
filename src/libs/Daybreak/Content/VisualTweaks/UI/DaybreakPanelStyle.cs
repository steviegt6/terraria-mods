using Daybreak.Common.Features.ModPanel;
using Daybreak.Common.Rendering;
using Daybreak.Core.Hooks;
using Daybreak.Core;

using System.Diagnostics;

using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ModLoader.UI;
using Microsoft.Xna.Framework;

internal sealed class DaybreakPanelStyle : ModPanelStyleExt
{
    private static WrapperShaderData<Assets.Shaders.UI.ModPanelShader.Parameters>? panelShaderData;

    public override void Load()
    {
        base.Load();
        
        panelShaderData = Assets.Shaders.UI.ModPanelShader.CreatePanelShader();
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
                RasterizerState.CullNone,
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