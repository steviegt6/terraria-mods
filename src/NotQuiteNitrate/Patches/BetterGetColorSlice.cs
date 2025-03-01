using System;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

using Tomat.TML.Mod.NotQuiteNitrate.Utilities;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class BetterGetColorSlice : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_Lighting.GetColor4Slice_int_int_refColorArray   += GetColor4Slice;
        On_Lighting.GetColor4Slice_int_int_refVector3Array += GetColor4Slice;

        On_Lighting.GetColor9Slice_int_int_refColorArray   += GetColor9Slice;
        On_Lighting.GetColor9Slice_int_int_refVector3Array += GetColor9Slice;
    }

    private static void GetColor4Slice(
        On_Lighting.orig_GetColor4Slice_int_int_refColorArray orig,
        int                                                   centerX,
        int                                                   centerY,
        ref Color[]                                           slices
    )
    {
        var globalBrightness = Lighting.GlobalBrightness;

        var colors = (Span<Vector3>)stackalloc Vector3[4];
        ColorBuffer.GetPlus(Lighting._activeEngine, centerX, centerY, colors);

        var color  = colors[0];
        var color2 = colors[1];
        var color3 = colors[2];
        var color4 = colors[3];

        var total1 = color.X  + color.Y  + color.Z;
        var total2 = color2.X + color2.Y + color2.Z;
        var total3 = color4.X + color4.Y + color4.Z;
        var total4 = color3.X + color3.Y + color3.Z;

        ColorBuffer.FastVector3ToColor(ref slices[0], total1 >= total4 ? color3 : color,  globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[1], total1 >= total3 ? color4 : color,  globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[2], total2 >= total4 ? color3 : color2, globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[3], total2 >= total3 ? color4 : color2, globalBrightness);
    }

    private static void GetColor4Slice(
        On_Lighting.orig_GetColor4Slice_int_int_refVector3Array orig,
        int                                                     x,
        int                                                     y,
        ref Vector3[]                                           slices
    )
    {
        var globalBrightness = Lighting.GlobalBrightness;

        var colors = (Span<Vector3>)stackalloc Vector3[4];
        ColorBuffer.GetPlus(Lighting._activeEngine, x, y, colors);

        var color  = colors[0];
        var color2 = colors[1];
        var color3 = colors[2];
        var color4 = colors[3];

        var total1 = color.X  + color.Y  + color.Z;
        var total2 = color2.X + color2.Y + color2.Z;
        var total3 = color4.X + color4.Y + color4.Z;
        var total4 = color3.X + color3.Y + color3.Z;

        slices[0] = (total1 >= total4 ? color3 : color)  * globalBrightness;
        slices[1] = (total1 >= total3 ? color4 : color)  * globalBrightness;
        slices[2] = (total2 >= total4 ? color3 : color2) * globalBrightness;
        slices[3] = (total2 >= total3 ? color4 : color2) * globalBrightness;
    }

    private static void GetColor9Slice(
        On_Lighting.orig_GetColor9Slice_int_int_refColorArray orig,
        int                                                   centerX,
        int                                                   centerY,
        ref Color[]                                           slices
    )
    {
        var globalBrightness = Lighting.GlobalBrightness;

        var colors = (Span<Vector3>)stackalloc Vector3[9];
        ColorBuffer.GetSquare(Lighting._activeEngine, centerX, centerY, colors);

        ColorBuffer.FastVector3ToColor(ref slices[0], colors[0], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[1], colors[1], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[2], colors[2], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[3], colors[3], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[4], colors[4], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[5], colors[5], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[6], colors[6], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[7], colors[7], globalBrightness);
        ColorBuffer.FastVector3ToColor(ref slices[8], colors[8], globalBrightness);
    }

    private static void GetColor9Slice(
        On_Lighting.orig_GetColor9Slice_int_int_refVector3Array orig,
        int                                                     x,
        int                                                     y,
        ref Vector3[]                                           slices
    )
    {
        var globalBrightness = Lighting.GlobalBrightness;

        var colors = (Span<Vector3>)stackalloc Vector3[9];
        ColorBuffer.GetSquare(Lighting._activeEngine, x, y, colors);

        slices[0] = colors[0] * globalBrightness;
        slices[1] = colors[1] * globalBrightness;
        slices[2] = colors[2] * globalBrightness;
        slices[3] = colors[3] * globalBrightness;
        slices[4] = colors[4] * globalBrightness;
        slices[5] = colors[5] * globalBrightness;
        slices[6] = colors[6] * globalBrightness;
        slices[7] = colors[7] * globalBrightness;
        slices[8] = colors[8] * globalBrightness;
    }
}