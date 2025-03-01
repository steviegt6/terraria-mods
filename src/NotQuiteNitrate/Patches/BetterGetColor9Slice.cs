using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class BetterGetColor9Slice : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_Lighting.GetColor9Slice_int_int_refVector3Array += GetColor9Slice;
        On_Lighting.GetColor9Slice_int_int_refColorArray   += GetColor9Slice;
    }

    private static void GetColor9Slice(
        On_Lighting.orig_GetColor9Slice_int_int_refVector3Array orig,
        int                                                     x,
        int                                                     y,
        ref Vector3[]                                           slices
    )
    {
        // Easy performance gains by just caching these results.
        var engine           = Lighting._activeEngine;
        var globalBrightness = Lighting.GlobalBrightness;

        // TODO: Write utility method to load all these values into a buffer?
        slices[0] = engine.GetColor(x - 1, y - 1)        * globalBrightness;
        slices[3] = engine.GetColor(x - 1, y)            * globalBrightness;
        slices[6] = engine.GetColor(x - 1, y + 1)        * globalBrightness;
        slices[1] = engine.GetColor(x,     y - 1)        * globalBrightness;
        slices[4] = engine.GetColor(x,     y)            * globalBrightness;
        slices[7] = engine.GetColor(x,     y + 1)        * globalBrightness;
        slices[2] = engine.GetColor(x        + 1, y - 1) * globalBrightness;
        slices[5] = engine.GetColor(x        + 1, y)     * globalBrightness;
        slices[8] = engine.GetColor(x        + 1, y + 1) * globalBrightness;
    }

    private static void GetColor9Slice(
        On_Lighting.orig_GetColor9Slice_int_int_refColorArray orig,
        int                                                   centerX,
        int                                                   centerY,
        ref Color[]                                           slices
    )
    {
        // Easy performance gains by just caching these results.
        var engine           = Lighting._activeEngine;
        var globalBrightness = Lighting.GlobalBrightness;

        var sliceIndex = 0;
        for (var i = centerX - 1; i <= centerX + 1; i++)
        {
            for (var j = centerY - 1; j <= centerY + 1; j++)
            {
                var color = engine.GetColor(i, j);

                var r = (int)(255f * color.X * globalBrightness);
                if (r > 255)
                {
                    r = 255;
                }

                var g = (int)(255f * color.Y * globalBrightness);
                if (g > 255)
                {
                    g = 255;
                }

                var b = (int)(255f * color.Z * globalBrightness);
                if (b > 255)
                {
                    b = 255;
                }

                b <<= 16;
                g <<= 8;
                {
                    slices[sliceIndex].PackedValue = (uint)(r | g | b) | 0xFF000000u;
                }

                sliceIndex += 3;
            }

            sliceIndex -= 8;
        }
    }
}