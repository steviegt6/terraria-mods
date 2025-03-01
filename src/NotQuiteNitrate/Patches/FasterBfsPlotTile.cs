using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

using JetBrains.Annotations;

using Terraria;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class FasterBfsPlotTile : ModSystem
{
    private static readonly ThreadLocal<Queue<Point>>   tl_queue   = new(() => new Queue<Point>(100));
    private static readonly ThreadLocal<HashSet<Point>> tl_visited = new(() => new HashSet<Point>(100));

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
            queue.Enqueue(new Point(x, y));
        }

        var visited = tl_visited.Value!;
        {
            visited.Add(new Point(x, y));
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

            var neighbors = (Span<Point>)
            [
                current with { X = current.X - 1 },
                current with { X = current.X + 1 },
                current with { Y = current.Y - 1 },
                current with { Y = current.Y + 1 },
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