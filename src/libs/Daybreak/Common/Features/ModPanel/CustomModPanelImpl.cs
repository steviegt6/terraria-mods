using System;
using System.Reflection;

using Daybreak.Core.Hooks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Daybreak.Common.Features.ModPanel;

internal sealed class CustomModPanelImpl : ILoad
{
    // The current mod whose panel style to use.
    private static Mod? currentMod;

    void ILoad.Load()
    {
        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.OnInitialize)),
            OnInitialize
        );

        MonoModHooks.Modify(
            GetMethod(nameof(UIModItem.OnInitialize)),
            ModifyAppendedFields
        );

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.SetHoverColors)),
            SetHoverColors
        );

        MonoModHooks.Add(
            GetMethod(nameof(UIModItem.Draw)),
            Draw
        );
        
        MonoModHooks.Add(
            typeof(UIPanel).GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance),
            OverrideRegularPanelDrawing
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

    private static void OnInitialize(Action<UIModItem> orig, UIModItem self)
    {
        if (!ModLoader.TryGetMod(self._mod.Name, out var mod))
        {
            orig(self);
            return;
        }

        if (mod is not IHasModPanelStyle styleProvider)
        {
            orig(self);
            return;
        }

        currentMod = mod;
        var style = styleProvider.PanelStyle;

        var modInfoTextureOrig = UICommon.ButtonModInfoTexture;
        {
            var modInfoTextureNew = style.ModInfoTexture;
            UICommon.ButtonModInfoTexture = modInfoTextureNew ?? UICommon.ButtonModInfoTexture;
        }

        var modConfigTextureOrig = UICommon.ButtonModConfigTexture;
        {
            var modConfigTextureNew = style.ModConfigTexture;
            UICommon.ButtonModConfigTexture = modConfigTextureNew ?? UICommon.ButtonModConfigTexture;
        }

        try
        {
            if (style.PreInitialize(self))
            {
                orig(self);
            }
            style.PostInitialize(self);
        }
        finally
        {
            currentMod = null;
        
            UICommon.ButtonModInfoTexture = modInfoTextureOrig;
            UICommon.ButtonModConfigTexture = modConfigTextureOrig;   
        }
    }

    private static void ModifyAppendedFields(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modIcon)));
        c.EmitLdarg0();
        c.EmitDelegate((UIImage originalImage, UIModItem self) =>
            {
                if (currentMod is not IHasModPanelStyle styleProvider)
                {
                    return originalImage;
                }

                return styleProvider.PanelStyle.ModifyModIcon(self, originalImage);
            }
        );

        c.EmitLdarg0();
        c.EmitDelegate((UIImage? modIcon, UIModItem self) =>
        {
            if (modIcon is null)
            {
                self._modIconAdjust = 0;
            }

            return modIcon;
        });
        
        c.GotoNext(MoveType.After, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modIcon)));

        var skipAppend = c.DefineLabel();
        c.EmitLdarg0();
        c.EmitLdfld(typeof(UIModItem).GetField(nameof(UIModItem._modIcon), BindingFlags.NonPublic | BindingFlags.Instance)!);
        c.EmitLdcI4(0); // null
        c.EmitBeq(skipAppend);

        c.GotoNext(MoveType.After, x => x.MatchCall<UIElement>(nameof(UIElement.Append)));
        c.MarkLabel(skipAppend);

        c.GotoNext(MoveType.Before, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modName)));
        c.EmitLdarg0();
        c.EmitDelegate((UIText originalText, UIModItem self) =>
            {
                if (currentMod is not IHasModPanelStyle styleProvider)
                {
                    return originalText;
                }

                return styleProvider.PanelStyle.ModifyModName(self, originalText);
            }
        );
    }

    private static void SetHoverColors(
        Action<UIModItem, bool> orig,
        UIModItem self,
        bool hovered
    )
    {
        if (!ModLoader.TryGetMod(self._mod.Name, out var mod))
        {
            orig(self, hovered);
            return;
        }

        if (mod is not IHasModPanelStyle styleProvider)
        {
            orig(self, hovered);
            return;
        }

        if (styleProvider.PanelStyle.PreSetHoverColors(self, hovered))
        {
            orig(self, hovered);
        }
        styleProvider.PanelStyle.PostSetHoverColors(self, hovered);
    }

    private static void Draw(
        Action<UIModItem, SpriteBatch> orig,
        UIModItem self,
        SpriteBatch spriteBatch
    )
    {
        if (!ModLoader.TryGetMod(self._mod.Name, out var mod))
        {
            orig(self, spriteBatch);
            return;
        }

        if (mod is not IHasModPanelStyle styleProvider)
        {
            orig(self, spriteBatch);
            return;
        }

        var innerPanelTextureOrig = UICommon.InnerPanelTexture;
        {
            var innerPanelTextureNew = styleProvider.PanelStyle.InnerPanelTexture;
            UICommon.InnerPanelTexture = innerPanelTextureNew ?? UICommon.InnerPanelTexture;
        }

        currentMod = mod;
        try
        {
            if (styleProvider.PanelStyle.PreDraw(self, spriteBatch))
            {
                orig(self, spriteBatch);
            }
            styleProvider.PanelStyle.PostDraw(self, spriteBatch);
        }
        finally
        {
            currentMod = null;
        
            UICommon.InnerPanelTexture = innerPanelTextureOrig;   
        }
    }

    private static void OverrideRegularPanelDrawing(
        Action<UIPanel, SpriteBatch> orig,
        UIPanel self,
        SpriteBatch spriteBatch
    )
    {
        if (currentMod is not IHasModPanelStyle styleProvider || self is not UIModItem uiModItem)
        {
            orig(self, spriteBatch);
            return;
        }

        if (styleProvider.PanelStyle.PreDrawPanel(uiModItem, spriteBatch))
        {
            orig(self, spriteBatch);
        }
        styleProvider.PanelStyle.PostDrawPanel(uiModItem, spriteBatch);
    }

    private static void DrawCustomColoredEnabledText(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchCall<UIModStateText>("get_DisplayColor"));

        c.EmitLdarg0(); // this
        c.EmitDelegate(static (Color displayColor, UIModStateText self) =>
            {
                if (currentMod is not IHasModPanelStyle styleProvider)
                {
                    return displayColor;
                }

                return styleProvider.PanelStyle.ModifyEnabledTextColor(self._enabled, displayColor);
            }
        );
    }
}