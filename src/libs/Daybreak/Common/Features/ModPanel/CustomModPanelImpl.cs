using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

internal sealed class CustomModPanelImpl : ILoad, IUnload
{
    // The current mod whose panel style to use.
    private static Mod? currentMod;

    private static readonly Dictionary<Mod, List<ModPanelStyle>> panel_styles = [];

    public static void AddPanelStyle(Mod mod, ModPanelStyle style)
    {
        if (panel_styles.TryGetValue(mod, out var styles))
        {
            // TODO: We should support multiple styles.  Issue is handling
            //       reinitialization.
            ModContent.GetInstance<ModImpl>().Logger.Warn($"Mod \"${mod.Name}\" has already registered a ModPanelStyle and is attempting to add another one; only the first one loaded will be displayed.");
        }
        else
        {
            panel_styles[mod] = styles = [];
        }

        styles.Add(style);
    }

    public static bool TryGetPanelStyle(Mod? mod, [NotNullWhen(returnValue: true)] out ModPanelStyle? style)
    {
        if (mod is null)
        {
            style = null;
            return false;
        }

        if (panel_styles.TryGetValue(mod, out var styles))
        {
            // We should always have at least one style, but let's be safe.
            style = styles.FirstOrDefault();
            return style is not null;
        }

        style = null;
        return false;
    }

    /*private static bool TryGetPanelStyle(string modName, [NotNullWhen(returnValue: true)] out ModPanelStyle? style)
    {
        if (!ModLoader.TryGetMod(modName, out var mod))
        {
            style = null;
            return false;
        }

        return TryGetPanelStyle(mod, out style);
    }*/

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

    void IUnload.Unload()
    {
        currentMod = null;
        panel_styles.Clear();
    }

    private static void OnInitialize(Action<UIModItem> orig, UIModItem self)
    {
        if (!ModLoader.TryGetMod(self._mod.Name, out currentMod) || !TryGetPanelStyle(currentMod, out var style))
        {
            orig(self);
            return;
        }

        using (style.OverrideTextures())
        {
            if (style.PreInitialize(self))
            {
                orig(self);
            }
            style.PostInitialize(self);
        }

        currentMod = null;
    }

    private static void ModifyAppendedFields(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.Before, x => x.MatchStfld<UIModItem>(nameof(UIModItem._modIcon)));
        c.EmitLdarg0();
        c.EmitDelegate((UIImage originalImage, UIModItem self) => !TryGetPanelStyle(currentMod, out var style)
            ? originalImage
            : style.ModifyModIcon(self, originalImage, ref self._modIconAdjust)
        );

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
                if (!TryGetPanelStyle(currentMod, out var style))
                {
                    return originalText;
                }

                return style.ModifyModName(self, originalText);
            }
        );
    }

    // TODO: Don't remember if we can use currentMod here, but I'd rather
    // minimize its usage in cases where we actually can determine the context.
    private static void SetHoverColors(
        Action<UIModItem, bool> orig,
        UIModItem self,
        bool hovered
    )
    {
        if (!ModLoader.TryGetMod(self._mod.Name, out var mod) || !TryGetPanelStyle(mod, out var style))
        {
            orig(self, hovered);
            return;
        }

        if (style.PreSetHoverColors(self, hovered))
        {
            orig(self, hovered);
        }
        style.PostSetHoverColors(self, hovered);
    }

    private static void Draw(
        Action<UIModItem, SpriteBatch> orig,
        UIModItem self,
        SpriteBatch spriteBatch
    )
    {
        if (!ModLoader.TryGetMod(self._mod.Name, out var mod) || !TryGetPanelStyle(mod, out var style))
        {
            orig(self, spriteBatch);
            return;
        }

        using (style.OverrideTextures())
        {
            currentMod = mod;
            if (style.PreDraw(self, spriteBatch))
            {
                orig(self, spriteBatch);
            }
            style.PostDraw(self, spriteBatch);
            currentMod = null;
        }
    }

    private static void OverrideRegularPanelDrawing(
        Action<UIPanel, SpriteBatch> orig,
        UIPanel self,
        SpriteBatch spriteBatch
    )
    {
        if (!TryGetPanelStyle(currentMod, out var style) || self is not UIModItem uiModItem)
        {
            orig(self, spriteBatch);
            return;
        }

        if (style.PreDrawPanel(uiModItem, spriteBatch))
        {
            orig(self, spriteBatch);
        }
        style.PostDrawPanel(uiModItem, spriteBatch);
    }

    private static void DrawCustomColoredEnabledText(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchCall<UIModStateText>("get_DisplayColor"));

        c.EmitLdarg0(); // this
        c.EmitDelegate(static (Color displayColor, UIModStateText self) =>
            {
                if (!TryGetPanelStyle(currentMod, out var style))
                {
                    return displayColor;
                }

                return style.ModifyEnabledTextColor(self._enabled, displayColor);
            }
        );
    }
}