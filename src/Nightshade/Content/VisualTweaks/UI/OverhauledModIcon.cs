using System;
using System.Diagnostics;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria.ModLoader;
using Terraria.ModLoader.UI;

using Tomat.TML.Mod.Nightshade.Core.Attributes;

namespace Tomat.TML.Mod.Nightshade.Content.VisualTweaks.UI;

/// <summary>
///     Reworks the rendering of the mod's <see cref="UIModItem"/> instance.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class OverhauledModIcon : ILoadable
{
    [InitializedInLoad]
    private static Mod? theMod;

    private static bool renderingOurMod;

    void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
    {
        if (mod is not Mod nsMod)
        {
            throw new InvalidOperationException("The mod instance is not of the correct type.");
        }

        theMod = nsMod;

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

        orig(self);

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

        renderingOurMod = true;
        orig(self, spriteBatch);
        renderingOurMod = false;

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
                if (!renderingOurMod)
                {
                    return displayColor;
                }

                return self._enabled ? new Color(47, 199, 229) : new Color(124, 31, 221);
            }
        );
    }
}