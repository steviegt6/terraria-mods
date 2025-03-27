using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

using Tomat.TML.Mod.Nightshade.Core.Attributes;
using Tomat.TML.Mod.Nightshade.Core.Rendering;

namespace Tomat.TML.Mod.Nightshade.Content.VisualTweaks.UI;

/// <summary>
///     Reworks the rendering of the mod's <see cref="UIModItem"/> instance.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class OverhauledModIcon : ILoadable
{
    private sealed class PulsatingAndAwesomeText : UIText
    {
        private readonly string originalText;

        public PulsatingAndAwesomeText(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            if (ChatManager.Regexes.Format.Matches(text).Count != 0)
            {
                throw new InvalidOperationException("The text cannot contain formatting.");
            }

            originalText = text;
        }

        [Obsolete("Unsupported", error: true)]
        public PulsatingAndAwesomeText(LocalizedText text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            throw new NotSupportedException("Localized text is not supported.");
        }

        public override void DrawSelf(SpriteBatch spriteBatch)
        {
            var formattedText = GetPulsatingText(originalText, Main.GlobalTimeWrappedHourly);
            SetText(formattedText);

            base.DrawSelf(spriteBatch);
        }

        private static string GetPulsatingText(string text, float time)
        {
            var lightPurple = light_pink;
            var darkPurple  = dark_pink;

            const float speed  = 3f;
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

    private sealed class PetalImage : UIImage
    {
        private readonly Asset<Texture2D> icon;
        private readonly Asset<Texture2D> iconDots;

        public PetalImage(Mod mod) : base(TextureAssets.MagicPixel)
        {
            const string path = "Assets/Images/UI/ModIcon/";

            icon     = mod.Assets.Request<Texture2D>(path + "Icon");
            iconDots = mod.Assets.Request<Texture2D>(path + "Icon_Dots");

            SetImage(icon);
        }

        public override void DrawSelf(SpriteBatch spriteBatch)
        {
            // const int offset = (80 - 46) / 2;
            const int offset = 40;

            var dims = GetDimensions();
            dims.X += offset;
            dims.Y += offset;

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

    [InitializedInLoad]
    private static Mod? theMod;

    [InitializedInLoad]
    private static MiscShaderData? panelShaderData;

    [InitializedInLoad]
    private static MiscShaderData? flowerShaderData;

    private static bool  isCurrentlyHandlingOurMod;
    private static float hoverIntensity;

    private const string panel_shader_path  = "Assets/Shaders/UI/ModPanelShader";
    private const string flower_shader_path = "Assets/Shaders/UI/CoolFlowerShader";

    // C50084 CE008C dark
    // E600E6        light
    private static readonly Color dark_pink  = new(197, 0, 132);
    private static readonly Color light_pink = new(230, 0, 230);

    // In no particular order of importance.  We could order based on whether
    // they have icons?
    // These names are used as unique keys for both getting icons and for
    // getting localized names.
    private static readonly string[] authors =
    [
        "Tomat", "Triangle", "Sixtydegrees", "Math2", "OneThree", "Tyeski",
        "Wymsical", "Taco", "DylanDoe21",
    ];

    void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
    {
        if (mod is not Mod nsMod)
        {
            throw new InvalidOperationException("The mod instance is not of the correct type.");
        }

        theMod = nsMod;

        var panelShader = nsMod.Assets.Request<Effect>(panel_shader_path);
        panelShaderData = new MiscShaderData(panelShader, "PanelShader");

        var flowerShader = nsMod.Assets.Request<Effect>(flower_shader_path);
        flowerShaderData = new MiscShaderData(flowerShader, "FlowerShader");

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.OnInitialize)),
            OnInitialize
        );

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.SetHoverColors)),
            SetHoverColors
        );

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.Draw)),
            Draw
        );

        MonoModHooks.Modify(
            typeof(UIModStateText).GetMethod("DrawEnabledText", BindingFlags.NonPublic | BindingFlags.Instance),
            DrawCustomColoredEnabledText
        );

        MonoModHooks.Modify(
            GetMethod("OnInitialize"),
            ModifyAppendedFields
        );

        MonoModHooks.Add(
            typeof(UIPanel).GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance),
            OverrideRegularPanelDrawing
        );

        return;

        static MethodInfo GetMethod(string name)
        {
            return typeof(UIModItem).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)!;
        }
    }

    void ILoadable.Unload() { }

    private static void OnInitialize(Action<UIModItem> orig, UIModItem self)
    {
        Debug.Assert(theMod is not null);

        if (self._mod.Name != theMod.Name)
        {
            orig(self);
            return;
        }

        self.BorderColor = Color.Black;

        // We can't use our ARH system because these are properties.

        var modInfoTextureOrig = UICommon.ButtonModInfoTexture;
        {
            var modInfoTextureNew = theMod.Assets.Request<Texture2D>("Assets/Images/UI/ModLoader/ButtonModInfo");
            UICommon.ButtonModInfoTexture = modInfoTextureNew;
        }

        var modConfigTextureOrig = UICommon.ButtonModConfigTexture;
        {
            var modConfigTextureNew = theMod.Assets.Request<Texture2D>("Assets/Images/UI/ModLoader/ButtonModConfig");
            UICommon.ButtonModConfigTexture = modConfigTextureNew;
        }

        isCurrentlyHandlingOurMod = true;
        orig(self);
        isCurrentlyHandlingOurMod = false;

        UICommon.ButtonModInfoTexture   = modInfoTextureOrig;
        UICommon.ButtonModConfigTexture = modConfigTextureOrig;
    }

    private static void SetHoverColors(
        Action<UIModItem, bool> orig,
        UIModItem               self,
        bool                    hovered
    )
    {
        Debug.Assert(theMod is not null);

        if (self._mod.Name != theMod.Name)
        {
            orig(self, hovered);
            return;
        }

        // Always set to black, we have our own effect for hovering.
        self.BorderColor     = Color.Black;
        self.BackgroundColor = new Color(20, 20, 20);

        // TODO: Glow ring around the panel upon hovering?
    }

    private static void Draw(
        Action<UIModItem, SpriteBatch> orig,
        UIModItem                      self,
        SpriteBatch                    spriteBatch
    )
    {
        Debug.Assert(theMod is not null);

        if (self._mod.Name != theMod.Name)
        {
            orig(self, spriteBatch);
            return;
        }

        // We can't use our ARH system because this is a property.

        var innerPanelTextureOrig = UICommon.InnerPanelTexture;
        {
            var innerPanelTextureNew = theMod.Assets.Request<Texture2D>("Assets/Images/UI/ModLoader/InnerPanelBackground");
            UICommon.InnerPanelTexture = innerPanelTextureNew;
        }

        isCurrentlyHandlingOurMod = true;
        orig(self, spriteBatch);
        isCurrentlyHandlingOurMod = false;

        UICommon.InnerPanelTexture = innerPanelTextureOrig;

        // Override the author tooltip with our own awesome one.
        if (self._tooltip != Language.GetTextValue("tModLoader.ModsByline", self._mod.properties.author))
        {
            return;
        }

        UICommon.TooltipMouseText(GetAuthorTooltip());
        // Main.HoverItem.TurnToAir();
        // Main.mouseText = false;
        // Main.instance.MouseText(GetAuthorTooltip());

        return;

        static string GetAuthorTooltip()
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetText("UI.ModIcon.AuthorHeader"));

            foreach (var authorName in authors)
            {
                sb.Append($"[nsa:{authorName}]");
                sb.Append(' ');
                sb.AppendLine(GetText($"UI.ModIcon.Authors.{authorName}"));
            }

            return sb.ToString();
        }

        static string GetText(string key)
        {
            return Language.GetTextValue($"Mods.Nightshade.{key}");
        }
    }

    private static void DrawCustomColoredEnabledText(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchCall<UIModStateText>("get_DisplayColor"));

        c.EmitLdarg0(); // this
        c.EmitDelegate(
            static (Color displayColor, UIModStateText self) =>
            {
                if (!isCurrentlyHandlingOurMod)
                {
                    return displayColor;
                }

                return self._enabled ? light_pink : dark_pink;
            }
        );
    }

    private static void ModifyAppendedFields(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modIcon)));
        c.EmitDelegate(
            (UIImage originalImage) =>
            {
                if (!isCurrentlyHandlingOurMod || theMod is null)
                {
                    return originalImage;
                }

                return new PetalImage(theMod)
                {
                    Left   = originalImage.Left,
                    Top    = originalImage.Top,
                    Width  = originalImage.Width,
                    Height = originalImage.Height,
                };
            }
        );

        c.GotoNext(MoveType.Before, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modName)));
        c.EmitLdarg0(); // this
        c.EmitDelegate(
            (UIText originalText, UIModItem self) =>
            {
                if (!isCurrentlyHandlingOurMod)
                {
                    return originalText;
                }

                var name = Language.GetTextValue("Mods.Nightshade.UI.ModIcon.ModName");
                return new PulsatingAndAwesomeText(name + $" v{self._mod.Version}")
                {
                    Left = originalText.Left,
                    Top  = originalText.Top,
                };
            }
        );
    }

    private static void OverrideRegularPanelDrawing(
        Action<UIPanel, SpriteBatch> orig,
        UIPanel                      self,
        SpriteBatch                  spriteBatch
    )
    {
        if (!isCurrentlyHandlingOurMod || self is not UIModItem uiModItem)
        {
            orig(self, spriteBatch);
            return;
        }

        if (uiModItem._needsTextureLoading)
        {
            uiModItem._needsTextureLoading = false;
            uiModItem.LoadTextures();
        }

        // Render our cool custom panel with a shader.
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

            var dims = uiModItem.GetDimensions();

            hoverIntensity += (self.IsMouseHovering ? 1f : -1f) / 15f;
            hoverIntensity =  Math.Clamp(hoverIntensity, 0f, 1f);

            Debug.Assert(panelShaderData is not null);
            panelShaderData.Shader.Parameters["grayness"].SetValue(1f);
            panelShaderData.Shader.Parameters["inColor"].SetValue(new Vector3(1f, 0f, 1f));
            panelShaderData.Shader.Parameters["speed"].SetValue(0.2f);
            panelShaderData.Shader.Parameters["uSource"].SetValue(new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y));
            panelShaderData.Shader.Parameters["uHoverIntensity"].SetValue(hoverIntensity);
            panelShaderData.Shader.Parameters["uPixel"].SetValue(2f);
            panelShaderData.Shader.Parameters["uColorResolution"].SetValue(new Vector3(10f));
            panelShaderData.Apply();

            Debug.Assert(uiModItem._backgroundTexture is not null);
            uiModItem.DrawPanel(spriteBatch, uiModItem._backgroundTexture.Value, uiModItem.BackgroundColor);

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

            dims.X -= dims.Width / 2f;
            dims.X += 40f; // 80 / 2

            Debug.Assert(flowerShaderData is not null);
            flowerShaderData.Shader.Parameters["uSource"].SetValue(new Vector4(dims.Width, dims.Height - 2f, dims.X, dims.Y));
            flowerShaderData.Shader.Parameters["uPixel"].SetValue(2f);
            flowerShaderData.Apply();
            uiModItem.DrawPanel(spriteBatch, uiModItem._backgroundTexture.Value, uiModItem.BackgroundColor);
            // Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, flowerDimensions, Color.White);
            // Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            spriteBatch.End();
            snapshot.Apply(spriteBatch);
        }

        Debug.Assert(uiModItem._borderTexture is not null);
        uiModItem.DrawPanel(spriteBatch, uiModItem._borderTexture.Value, uiModItem.BorderColor);
    }
}