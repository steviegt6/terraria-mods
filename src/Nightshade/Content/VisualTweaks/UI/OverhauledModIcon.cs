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
            var lightPurple = new Color(200, 100, 255);
            var darkPurple  = new Color(100, 50,  150);

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

    [InitializedInLoad]
    private static Mod? theMod;

    [InitializedInLoad]
    private static MiscShaderData? panelShaderData;

    private static bool  isCurrentlyHandlingOurMod;
    private static float hoverIntensity;

    private const string panel_shader_path = "Assets/Shaders/UI/ModPanelShader";

    void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
    {
        if (mod is not Mod nsMod)
        {
            throw new InvalidOperationException("The mod instance is not of the correct type.");
        }

        theMod = nsMod;

        var shader = nsMod.Assets.Request<Effect>(panel_shader_path);
        panelShaderData = new MiscShaderData(shader, "PanelShader");

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

        // self.OverflowHidden = false;
        // // test
        // var dims = self.GetDimensions();
        // var rect = dims.ToRectangle();
        // rect.Inflate(2, 2);
        // spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.Red);

        isCurrentlyHandlingOurMod = true;
        orig(self, spriteBatch);
        isCurrentlyHandlingOurMod = false;

        UICommon.InnerPanelTexture = innerPanelTextureOrig;
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

                return self._enabled ? new Color(47, 199, 229) : new Color(124, 31, 221);
            }
        );
    }

    private static void ModifyAppendedFields(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modName)));
        c.EmitDelegate(
            (UIText originalText) =>
            {
                if (!isCurrentlyHandlingOurMod)
                {
                    return originalText;
                }

                return new PulsatingAndAwesomeText(originalText.Text)
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

            if (self.IsMouseHovering)
            {
                hoverIntensity += 1f / 25f;
            }
            else
            {
                hoverIntensity -= 1f / 25f;
            }

            hoverIntensity = Math.Clamp(hoverIntensity, 0f, 1f);
            
            var color = 0.1f + (0.8f * hoverIntensity);

            Debug.Assert(panelShaderData is not null);
            panelShaderData.Shader.Parameters["grayness"].SetValue(-1f + hoverIntensity * 2f);
            panelShaderData.Shader.Parameters["inColor"].SetValue(new Vector3(color, 0f, color));
            panelShaderData.Shader.Parameters["speed"].SetValue(0.2f);
            panelShaderData.Shader.Parameters["uResolution"].SetValue(new Vector2(dims.Width, dims.Height));
            panelShaderData.Apply();

            Debug.Assert(uiModItem._backgroundTexture is not null);
            uiModItem.DrawPanel(spriteBatch, uiModItem._backgroundTexture.Value, uiModItem.BackgroundColor);

            spriteBatch.End();
            snapshot.Apply(spriteBatch);
        }

        Debug.Assert(uiModItem._borderTexture is not null);
        uiModItem.DrawPanel(spriteBatch, uiModItem._borderTexture.Value, uiModItem.BorderColor);
    }
}