using System.Diagnostics;

using Daybreak.Common.Features.Hooks;
using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Core;
using Nightshade.Core.Attributes;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Content.VisualTweaks.UI;

internal static class NightshadeBestiaryPanel
{
    private sealed class CustomPanel(
        Asset<Texture2D> customBackground,
        Asset<Texture2D>? customBorder,
        int customCornerSize = 12,
        int customBarSize = 4
    ) : UIPanel(customBackground, customBorder, customCornerSize, customBarSize)
    {
        protected override void DrawSelf(SpriteBatch sb)
        {
            if (_needsTextureLoading)
            {
                _needsTextureLoading = false;
                LoadTextures();
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
                    var dims = GetDimensions();

                    // hoverIntensity += (element.IsMouseHovering ? 1f : -1f) / 15f;
                    // hoverIntensity = Math.Clamp(hoverIntensity, 0f, 1f);
                    var hoverIntensity = 1f;

                    Debug.Assert(panelShaderData is not null);
                    /*panelShaderData.Parameters.uGrayness = 1f;
                    panelShaderData.Parameters.uInColor = new Vector3(1f, 0f, 1f);
                    panelShaderData.Parameters.uSpeed = 0.2f;
                    panelShaderData.Parameters.uSource = Transform(new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y));
                    panelShaderData.Parameters.uHoverIntensity = hoverIntensity;
                    panelShaderData.Parameters.uPixel = 2f;
                    panelShaderData.Parameters.uColorResolution = 10f;
                    panelShaderData.Apply();

                    Debug.Assert(_backgroundTexture is not null);*/
                    DrawPanel(sb, _backgroundTexture.Value, BackgroundColor);

                    sb.End();
                    sb.Begin(
                        SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        DepthStencilState.None,
                        ss.RasterizerState,
                        null,
                        Main.UIScaleMatrix
                    );

                    dims.X -= dims.Width / 2f;
                    dims.X += 20f;

                    Debug.Assert(flowerShaderData is not null);
                    flowerShaderData.Parameters.uSource = Transform(new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y));
                    flowerShaderData.Parameters.uPixel = 2f;
                    flowerShaderData.Apply();
                    DrawPanel(sb, _backgroundTexture.Value, BackgroundColor);
                }
                sb.Restart(ss);
            }

            if (_borderTexture is not null)
            {
                DrawPanel(sb, _borderTexture.Value, BorderColor);
            }
        }
    }

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.UI.ModPanelShader.Parameters>? panelShaderData;

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.UI.CoolFlowerShader.Parameters>? flowerShaderData;

    [OnLoad]
    private static void OnLoad()
    {
        On_ModBestiaryInfoElement.ProvideUIElement += ProvideCustomElementForOurMod;

        Main.QueueMainThreadAction(
            () =>
            {
                panelShaderData = Assets.Shaders.UI.ModPanelShader.CreatePanelShader();
                flowerShaderData = Assets.Shaders.UI.CoolFlowerShader.CreateFlowerShader();
            }
        );
    }

    private static UIElement? ProvideCustomElementForOurMod(On_ModBestiaryInfoElement.orig_ProvideUIElement orig, ModBestiaryInfoElement self, BestiaryUICollectionInfo info)
    {
        if (self != ModContent.GetInstance<ModImpl>().ModSourceBestiaryInfoElement)
        {
            return orig(self, info);
        }

        if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
        {
            return null;
        }

        UIElement uIElement = new CustomPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel"), null, 12, 7)
        {
            Width = new StyleDimension(-14f, 1f),
            Height = new StyleDimension(34f, 0f),
            BackgroundColor = new Color(43, 56, 101),
            BorderColor = Color.Transparent,
            Left = new StyleDimension(5f, 0f),
        };
        {
            uIElement.SetPadding(0f);
            uIElement.PaddingRight = 5f;

            var filterImage = self.GetFilterImage();
            {
                filterImage.HAlign = 0f;
                filterImage.Left = new StyleDimension(5f, 0f);
            }
            {
                uIElement.Append(filterImage);
            }

            var element = new NightshadePanelStyle.ModName(Mods.Nightshade.UI.ModIcon.ModName.GetTextValue(), 0.8f)
            {
                HAlign = 0f,
                Left = new StyleDimension(38f, 0f),
                TextOriginX = 0f,
                VAlign = 0.5f,
                DynamicallyScaleDownToWidth = true,
            };
            {
                uIElement.Append(element);
            }

            self.AddOnHover(uIElement);
        }
        return uIElement;
    }
    
    private static Vector4 Transform(Vector4 vector)
    {
        var vec1 = Vector2.Transform(new Vector2(vector.X, vector.Y), Main.UIScaleMatrix);
        var vec2 = Vector2.Transform(new Vector2(vector.Z, vector.W), Main.UIScaleMatrix);
        return new Vector4(vec1, vec2.X, vec2.Y);
    }
}