using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

using JetBrains.Annotations;

using Terraria;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class FasterBfsPlotTile : ModSystem
{
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct PackedPoint(int x, int y) : IEquatable<PackedPoint>
    {
        [FieldOffset(0)]
        private readonly ulong data;

        [FieldOffset(0)]
        public readonly int X = x;

        [FieldOffset(4)]
        public readonly int Y = y;

        public override int GetHashCode()
        {
            return HashCode.Combine(data, X, Y);
        }

        public bool Equals(PackedPoint other)
        {
            return data == other.data;
        }

        public override bool Equals(object? obj)
        {
            return obj is PackedPoint other && Equals(other);
        }
    }

    private static readonly ThreadLocal<Queue<PackedPoint>>   tl_queue   = new(() => new Queue<PackedPoint>(100));
    private static readonly ThreadLocal<HashSet<PackedPoint>> tl_visited = new(() => new HashSet<PackedPoint>(100));

    public override void Load()
    {
        base.Load();

        On_Utils.PlotTileArea += PlotTileArea;
    }

    private static bool PlotTileArea(
        On_Utils.orig_PlotTileArea orig,
        int                        x,
        int                        y,
        Utils.TileActionAttempt    plot
    )
    {
        if (!WorldGen.InWorld(x, y))
        {
            return false;
        }

        var queue = tl_queue.Value!;
        {
            queue.Enqueue(new PackedPoint(x, y));
        }

        var visited = tl_visited.Value!;
        {
            visited.Add(new PackedPoint(x, y));
        }

        while (queue.TryDequeue(out var current))
        {
            if (!WorldGen.InWorld(current.X, current.Y, 1))
            {
                continue;
            }

            if (!plot(current.X, current.Y))
            {
                continue;
            }

            var neighbors = (Span<PackedPoint>)
            [
                new PackedPoint(current.X - 1, current.Y),
                new PackedPoint(current.X + 1, current.Y),
                new PackedPoint(current.X,     current.Y - 1),
                new PackedPoint(current.X,     current.Y + 1),
            ];

            foreach (var neighbor in neighbors)
            {
                if (WorldGen.InWorld(neighbor.X, neighbor.Y, 1) && visited.Add(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        visited.Clear();
        return true;
    }
}