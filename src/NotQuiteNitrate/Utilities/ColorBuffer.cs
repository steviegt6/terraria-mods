using System;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Light;

namespace Tomat.TML.Mod.NotQuiteNitrate.Utilities;

public static class ColorBuffer
{
    private static readonly (int x, int y)[] plus_offsets =
    [
        (+0, -1),
        (+0, +1),
        (-1, +0),
        (+1, +0),
    ];

    private static readonly (int x, int y)[] square_offsets =
    [
        (-1, -1),
        (-1, +0),
        (-1, +1),

        (+0, -1),
        (+0, +0),
        (+0, +1),

        (+1, -1),
        (+1, +0),
        (+1, +1),
    ];

    /// <summary>
    ///     Reads a "plus" section to a buffer.
    /// </summary>
    /// <param name="engine">The engine to get the color from.</param>
    /// <param name="x">The center X position.</param>
    /// <param name="y">The center Y position.</param>
    /// <param name="colors">
    ///     The buffer to write to.  Expected to be an ample size to write all
    ///     the colors to.
    /// </param>
    public static void GetPlus(
        ILightingEngine engine,
        int             x,
        int             y,
        Span<Vector3>   colors
    )
    {
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

                for (var i = 0; i < plus_offsets.Length; i++)
                {
                    var offset = plus_offsets[i];
                    var localX = realX + offset.x;
                    var localY = realY + offset.y;

                    if (localX < 0 || localY < 0 || localX > unscaledX || localY > unscaledY)
                    {
                        colors[i] = Vector3.Zero;
                    }
                    else
                    {
                        var state = legacy._states[localX][localY];
                        colors[i] = new Vector3(state.R, state.G, state.B);
                    }
                }
                break;
            }

            case LightingEngine modern:
            {
                for (var i = 0; i < plus_offsets.Length; i++)
                {
                    var offset = plus_offsets[i];
                    var localX = x + offset.x;
                    var localY = y + offset.y;

                    if (!modern._activeProcessedArea.Contains(localX, localY))
                    {
                        colors[i] = Vector3.Zero;
                    }
                    else
                    {
                        colors[i] = modern._activeLightMap[
                            localX - modern._activeProcessedArea.X,
                            localY - modern._activeProcessedArea.Y
                        ];
                    }
                }
                break;
            }

            // default:
            //     throw new InvalidOperationException("The engine is not supported.");
        }
    }

    /// <summary>
    ///     Reads a 3x3 square section to a buffer.
    /// </summary>
    /// <param name="engine">The engine to get the color from.</param>
    /// <param name="x">The center X position.</param>
    /// <param name="y">The center Y position.</param>
    /// <param name="colors">
    ///     The buffer to write to.  Expected to be an ample size to write all
    ///     the colors to.
    /// </param>
    public static void GetSquare(
        ILightingEngine engine,
        int             x,
        int             y,
        Span<Vector3>   colors
    )
    {
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

                for (var i = 0; i < square_offsets.Length; i++)
                {
                    var offset = square_offsets[i];
                    var localX = realX + offset.x;
                    var localY = realY + offset.y;

                    if (localX < 0 || localY < 0 || localX > unscaledX || localY > unscaledY)
                    {
                        colors[i] = Vector3.Zero;
                    }
                    else
                    {
                        var state = legacy._states[localX][localY];
                        colors[i] = new Vector3(state.R, state.G, state.B);
                    }
                }
                break;
            }

            case LightingEngine modern:
            {
                for (var i = 0; i < square_offsets.Length; i++)
                {
                    var offset = square_offsets[i];
                    var localX = x + offset.x;
                    var localY = y + offset.y;

                    if (!modern._activeProcessedArea.Contains(localX, localY))
                    {
                        colors[i] = Vector3.Zero;
                    }
                    else
                    {
                        colors[i] = modern._activeLightMap[
                            localX - modern._activeProcessedArea.X,
                            localY - modern._activeProcessedArea.Y
                        ];
                    }
                }
                break;
            }

            // default:
            //     throw new InvalidOperationException("The engine is not supported.");
        }
    }

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