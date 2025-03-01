using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Light;

namespace Tomat.TML.Mod.NotQuiteNitrate.Utilities;

public static class ColorBuffer
{
    /// <summary>
    ///     Reads a square section to a buffer.
    /// </summary>
    /// <param name="engine">The engine to get the color from.</param>
    /// <param name="x">The center X position.</param>
    /// <param name="y">The center Y position.</param>
    /// <param name="padding">
    ///     Padding around the center to make the square
    /// .</param>
    /// <param name="colors">
    ///     The buffer to write to.  Expected to be an ample size to write all
    ///     the colors to.
    /// </param>
    public static void GetSquare(
        ILightingEngine engine,
        int             x,
        int             y,
        int             padding,
        Span<Vector3>   colors
    )
    {
        var width = 1 + padding * 2;

        var offset = (width * width - 1) / 2;

        switch (engine)
        {
            case LegacyLighting legacy:
            {
                var realX = x - legacy._requestedRectLeft + Lighting.OffScreenTiles;
                var realY = y - legacy._requestedRectTop  + Lighting.OffScreenTiles;

                // TODO: Squeeze out nanoseconds by not duplicating OffScreenTiles?
                var unscaledSize = legacy._camera.UnscaledSize;
                var unscaledX    = unscaledSize.X / 16f + Lighting.OffScreenTiles * 2 + 10;
                var unscaledY    = unscaledSize.Y / 16f + Lighting.OffScreenTiles * 2;

                var xMin = realX - padding;
                var xMax = realX + padding;
                var yMin = realY - padding;
                var yMax = realY + padding;
                for (var localX = xMin; localX <= xMax; localX++)
                for (var localY = yMin; localY <= yMax; localY++)
                {
                    var idx = (localY - realY) * width + (localX - realX) + offset;

                    if (localX < 0 || localY < 0 || localX > unscaledX || localY > unscaledY)
                    {
                        colors[idx] = Vector3.Zero;
                    }
                    else
                    {
                        var state = legacy._states[localX][localY];
                        colors[idx] = new Vector3(state.R, state.G, state.B);
                    }
                }
                break;
            }

            case LightingEngine modern:
            {
                var xMin = x - padding;
                var xMax = x + padding;
                var yMin = y - padding;
                var yMax = y + padding;
                for (var localX = xMin; localX <= xMax; localX++)
                for (var localY = yMin; localY <= yMax; localY++)
                {
                    var idx = (localY - y) * width + (localX - x) + offset;

                    if (!modern._activeProcessedArea.Contains(localX, localY))
                    {
                        colors[idx] = Vector3.Zero;
                    }
                    else
                    {
                        colors[idx] = modern._activeLightMap[
                            localX - modern._activeProcessedArea.X,
                            localY - modern._activeProcessedArea.Y
                        ];
                    }
                }

                break;
            }

            default:
                throw new InvalidOperationException("The engine is not supported.");
        }
    }

    /*/// <summary>
    ///     Gets the <see cref="colors"/> at the given <see cref="positions"/>.
    /// </summary>
    /// <param name="engine">The lighting engine in use.</param>
    /// <param name="positions">The requested positions.</param>
    /// <param name="colors">A buffer to write the colors to.</param>
    public static void GetSpan(ILightingEngine engine, Span<int> positions, Span<Vector3> colors)
    {
        Debug.Assert(positions.Length == colors.Length);

        if (engine is LegacyLighting legacy)
        {
            var realX =
        }
        else if (engine is LightingEngine modern) { }
        else
        {
            throw new InvalidOperationException("The engine is not supported.");
        }
    }*/

    // TODO: I'm not confident this is a meaningful performance gain?
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void FastVector3ToColor(ref Color color, Vector3 vector, float brightness)
    {
        var r = (int)(255f * vector.X * brightness);
        if (r > 255)
        {
            r = 255;
        }

        var g = (int)(255f * vector.Y * brightness);
        if (g > 255)
        {
            g = 255;
        }

        var b = (int)(255f * vector.Z * brightness);
        if (b > 255)
        {
            b = 255;
        }

        b <<= 16;
        g <<= 8;
        {
            color.PackedValue = (uint)(r | g | b) | 0xFF000000u;
        }
    }
}