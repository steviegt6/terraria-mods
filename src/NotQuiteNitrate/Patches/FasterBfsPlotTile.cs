using System.Collections.Generic;
using System.Drawing;

using JetBrains.Annotations;

using Terraria;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class FasterBfsPlotTile : ModSystem
{
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

        var queue   = new Queue<Point>();
        var visited = new HashSet<Point>();

        queue.Enqueue(new Point(x, y));
        visited.Add(new Point(x,   y));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (!WorldGen.InWorld(current.X, current.Y, 1))
            {
                continue;
            }

            if (!plot(current.X, current.Y))
            {
                continue;
            }

            var neighbors = new[]
            {
                current with { X = current.X - 1 },
                current with { X = current.X + 1 },
                current with { Y = current.Y - 1 },
                current with { Y = current.Y + 1 },
            };

            foreach (var neighbor in neighbors)
            {
                if (WorldGen.InWorld(neighbor.X, neighbor.Y, 1) && visited.Add(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return true;
    }
}