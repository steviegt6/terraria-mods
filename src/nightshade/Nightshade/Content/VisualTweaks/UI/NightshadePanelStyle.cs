using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Daybreak.Common.Features.ModPanel;
using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Core;
using Nightshade.Core.Attributes;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

namespace Nightshade.Content.VisualTweaks.UI;

internal sealed class NightshadePanelStyle : ModPanelStyleExt
{
    private sealed class ModName : UIText
    {
        private readonly string originalText;

        public ModName(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            if (ChatManager.Regexes.Format.Matches(text).Count != 0)
            {
                throw new InvalidOperationException("The text cannot contain formatting.");
            }

            originalText = text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var formattedText = GetPulsatingText(originalText, Main.GlobalTimeWrappedHourly);
            SetText(formattedText);

            base.DrawSelf(spriteBatch);
        }

        private static string GetPulsatingText(string text, float time)
        {
            var lightPurple = light_pink;
            var darkPurple = dark_pink;

            const float speed = 3f;
            const float offset = 0.3f;

            // [c/______:x]
            const int character_length = 12;

            var sb = new StringBuilder(character_length * text.Length);
            for (var i = 0; i < text.Length; i++)
            {
                var wave = MathF.Sin(time * speed + i * offset);

                // Factor normalized 0-1.
                var color = Color.Lerp(lightPurple, darkPurple, (wave + 1f) / 2f);

                sb.Append($"[c/{color.Hex3()}:{text[i]}]");
            }
            return sb.ToString();
        }
    }

    private sealed class ModIcon : UIImage
    {
        private readonly bool useCensor;
        private readonly Asset<Texture2D> icon;
        private readonly Asset<Texture2D> iconDots;

        public ModIcon(bool useCensor) : base(TextureAssets.MagicPixel)
        {
            this.useCensor = useCensor;
            icon = Assets.Images.UI.ModIcon.Icon.Asset;
            iconDots = Assets.Images.UI.ModIcon.Icon_Dots.Asset;

            SetImage(icon);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // const int offset = (80 - 46) / 2;
            const int offset = 40;

            var dims = GetDimensions();
            dims.X += offset;
            dims.Y += offset;

            if (useCensor)
            {
                spriteBatch.Draw(
                    TextureAssets.MagicPixel.Value,
                    new Rectangle((int)dims.X - 25, (int)dims.Y - 25, 50, 50),
                    null,
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f
                );
                return;
            }

            var rotation = Main.GlobalTimeWrappedHourly / 10f;

            var origin = icon.Size() / 2f;

            spriteBatch.Draw(
                icon.Value,
                dims.Position(),
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
                dims.Position(),
                null,
                Color.White,
                rotation,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }

    // C50084 CE008C dark
    // E600E6        light
    private static readonly Color dark_pink = new(197, 0, 132);
    private static readonly Color light_pink = new(230, 0, 230);

    // In no particular order of importance.  We could order based on whether
    // they have icons?
    // These names are used as unique keys for both getting icons and for
    // getting localized names.
    private static readonly string[] authors =
    [
        "Tomat", "Triangle", "Sixtydegrees", "Math2", "OneThree", "Tyeski",
        "Wymsical", "Ebonfly", "Citrus", "BabyBlueSheep",
    ];

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.UI.ModPanelShader.Parameters>? panelShaderData;

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.UI.CoolFlowerShader.Parameters>? flowerShaderData;

    private static float hoverIntensity;

    private static bool AprilFools => DateTime.Now.Month == 4 && DateTime.Now.Day == 1;

    public override Dictionary<TextureKind, Asset<Texture2D>> TextureOverrides { get; } = new()
    {
        { TextureKind.ModInfo, Assets.Images.UI.ModLoader.ButtonModInfo.Asset },
        { TextureKind.ModConfig, Assets.Images.UI.ModLoader.ButtonModConfig.Asset },
        { TextureKind.Deps, Assets.Images.UI.ModLoader.ButtonDeps.Asset },
        { TextureKind.InnerPanel, Assets.Images.UI.ModLoader.InnerPanelBackground.Asset },
    };

    public override void Load()
    {
        base.Load();

        Main.QueueMainThreadAction(() =>
            {
                panelShaderData = Assets.Shaders.UI.ModPanelShader.CreatePanelShader();
                flowerShaderData = Assets.Shaders.UI.CoolFlowerShader.CreateFlowerShader();

                Assets.Images.UI.ModLoader.ButtonModInfo.Asset.Wait();
                Assets.Images.UI.ModLoader.ButtonModConfig.Asset.Wait();
                Assets.Images.UI.ModLoader.InnerPanelBackground.Asset.Wait();
            }
        );
    }

    public override bool PreInitialize(UIModItem element)
    {
        element.BorderColor = Color.Black;

        return base.PreInitialize(element);
    }

    public override void PostInitialize(UIModItem element)
    {
        base.PostInitialize(element);

        if (ModLoader.HasMod("ConciseModList") && element._configButton is not null)
        {
            element._configButton._texture = Assets.Images.UI.ModLoader.ButtonModConfig_ConciseModsList.Asset;
        }
    }

    public override UIImage ModifyModIcon(UIModItem element, UIImage modIcon, ref int modIconAdjust)
    {
        return new ModIcon(AprilFools)
        {
            Left = modIcon.Left,
            Top = modIcon.Top,
            Width = modIcon.Width,
            Height = modIcon.Height,
        };
    }

    public override UIText ModifyModName(UIModItem element, UIText modName)
    {
        var name = AprilFools
            ? Mods.Nightshade.UI.ModIcon.AprilFools.ModName.GetTextValue()
            : Mods.Nightshade.UI.ModIcon.ModName.GetTextValue();
        return new ModName(name + $" v{element._mod.Version}")
        {
            Left = modName.Left,
            Top = modName.Top,
        };
    }

    public override bool PreSetHoverColors(UIModItem element, bool hovered)
    {
        // Always set to black, we have our own effect for hovering.
        element.BorderColor = Color.Black;
        element.BackgroundColor = new Color(20, 20, 20);

        return false;
    }

    public override void PostDraw(UIModItem element, SpriteBatch sb)
    {
        base.PostDraw(element, sb);

        // Override the author tooltip with our own awesome one.
        if (element._tooltip != Language.GetTextValue("tModLoader.ModsByline", element._mod.properties.author))
        {
            return;
        }

        UICommon.TooltipMouseText(GetAuthorTooltip());

        return;

        static string GetAuthorTooltip()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Mods.Nightshade.UI.ModIcon.AuthorHeader.GetTextValue());

            foreach (var authorName in authors)
            {
                sb.Append($"[nsa:{authorName}]");
                sb.Append(' ');
                sb.AppendLine(Mods.Nightshade.UI.ModIcon.Authors.GetChildTextValue(authorName));
            }

            return sb.ToString();
        }
    }

    public override bool PreDrawPanel(UIModItem element, SpriteBatch sb, ref bool shouldDrawDivider)
    {
        shouldDrawDivider = true;
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

                hoverIntensity += (element.IsMouseHovering ? 1f : -1f) / 15f;
                hoverIntensity = Math.Clamp(hoverIntensity, 0f, 1f);

                Debug.Assert(panelShaderData is not null);
                panelShaderData.Parameters.uGrayness = 1f;
                panelShaderData.Parameters.uInColor = new Vector3(1f, 0f, 1f);
                panelShaderData.Parameters.uSpeed = 0.2f;
                panelShaderData.Parameters.uSource = Transform(new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y));
                panelShaderData.Parameters.uHoverIntensity = hoverIntensity;
                panelShaderData.Parameters.uPixel = 2f;
                panelShaderData.Parameters.uColorResolution = 10f;
                panelShaderData.Apply();

                Debug.Assert(element._backgroundTexture is not null);
                element.DrawPanel(sb, element._backgroundTexture.Value, element.BackgroundColor);

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
                dims.X += 40f; // 80 / 2

                Debug.Assert(flowerShaderData is not null);
                flowerShaderData.Parameters.uSource = Transform(new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y));
                flowerShaderData.Parameters.uPixel = 2f;
                flowerShaderData.Apply();
                element.DrawPanel(sb, element._backgroundTexture.Value, element.BackgroundColor);
            }
            sb.Restart(ss);
        }

        Debug.Assert(element._borderTexture is not null);
        element.DrawPanel(sb, element._borderTexture.Value, element.BorderColor);

        return false;
    }

    public override Color ModifyEnabledTextColor(bool enabled, Color color)
    {
        return enabled ? light_pink : dark_pink;
    }
    
    private static Vector4 Transform(Vector4 vector)
    {
        var vec1 = Vector2.Transform(new Vector2(vector.X, vector.Y), Main.UIScaleMatrix);
        var vec2 = Vector2.Transform(new Vector2(vector.Z, vector.W), Main.UIScaleMatrix);
        return new Vector4(vec1, vec2.X, vec2.Y);
    }
}