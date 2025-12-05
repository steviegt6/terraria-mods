using System;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

using MonoMod.Cil;

using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SlightlyBetterTextRendering;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class BetterTextStroke : ModSystem
{
    public override void Load()
    {
        base.Load();

        IL_ChatManager.DrawColorCodedStringWithShadow_SpriteBatch_DynamicSpriteFont_string_Vector2_Color_float_Vector2_Vector2_float_float += DarkenStrokeIfDefault;
        IL_ChatManager.DrawColorCodedStringWithShadow_SpriteBatch_DynamicSpriteFont_string_Vector2_Color_Color_float_Vector2_Vector2_float_float += DarkenStrokeIfDefault;
        IL_ChatManager.DrawColorCodedStringWithShadow_SpriteBatch_DynamicSpriteFont_TextSnippetArray_Vector2_float_Vector2_Vector2_refInt32_float_float += DarkenStrokeIfDefault;
        IL_ChatManager.DrawColorCodedStringWithShadow_SpriteBatch_DynamicSpriteFont_TextSnippetArray_Vector2_float_Color_Vector2_Vector2_refInt32_float_float += DarkenStrokeIfDefault;
        IL_ChatManager.DrawColorCodedStringWithShadow_SpriteBatch_DynamicSpriteFont_TextSnippetArray_Vector2_float_Color_Color_Vector2_Vector2_refInt32_float_float += DarkenStrokeIfDefault;
    }

    private static void DarkenStrokeIfDefault(ILContext il)
    {
        var c = new ILCursor(il);

        var color = il.Method.Parameters.FirstOrDefault(x => string.Equals(x.Name, "color", StringComparison.InvariantCultureIgnoreCase))
                 ?? il.Method.Parameters.FirstOrDefault(x => string.Equals(x.Name, "baseColor", StringComparison.InvariantCultureIgnoreCase));
        var shadowColor = il.Method.Parameters.FirstOrDefault(x => string.Equals(x.Name, "shadowColor", StringComparison.InvariantCultureIgnoreCase));

        if (shadowColor is null)
        {
            // Simpler case: if there is no shadow color, search and replace
            // Color::get_Black or Color::.ctor(int32, int32, int32, int32).

            if (!c.TryGotoNext(MoveType.After, x => x.MatchCall<Color>("get_Black"))
             && !c.TryGotoNext(MoveType.After, x => x.MatchNewobj(typeof(Color).GetConstructor([typeof(int), typeof(int), typeof(int), typeof(int)])!)))
            {
                throw new InvalidOperationException("Could not find Color::get_Black or Color::.ctor(int32, int32, int32, int32) in simple case");
            }
            c.EmitPop();

            EmitColor(c, color);
            c.EmitDelegate(static (Color color) => DarkenColor(color)
            );
        }
        else
        {
            // Complex case: modify incoming value of shadowColor iff
            // shadowColor is equal to Color::Black.
            // For simplicity's sake, we'll just modify the value at the start
            // of the method instead of trying to modify it when it's being
            // consumed by an invocation.

            EmitColor(c, color);
            c.EmitLdarg(shadowColor.Index);
            c.EmitDelegate(static (Color color, Color shadowColor) => shadowColor == Color.Black ? DarkenColor(color) : color
            );
            c.EmitStarg(shadowColor.Index);
        }

        return;

        // Helper to emit Color::get_White when there is no color parameter
        // (single case, currently).
        static void EmitColor(ILCursor c, ParameterDefinition? color)
        {
            if (color is null)
            {
                c.EmitDelegate(() => Color.White);
            }
            else
            {
                c.EmitLdarg(color.Index);
            }
        }

        static Color DarkenColor(Color color)
        {
            if (color == Color.White && ModContent.GetInstance<BtsConfig>().OverrideWhiteToBlack)
            {
                return Color.Black;
            }

            var factor = ModContent.GetInstance<BtsConfig>().Factor;

            return new Color(
                Math.Clamp((byte)(color.R * factor), (byte)0, (byte)255),
                Math.Clamp((byte)(color.G * factor), (byte)0, (byte)255),
                Math.Clamp((byte)(color.B * factor), (byte)0, (byte)255),
                color.A
            );
        }
    }
}