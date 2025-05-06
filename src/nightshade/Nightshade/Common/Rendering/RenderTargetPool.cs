using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Rendering;

/// <summary>
///     Pools RenderTarget2Ds of desired widths and heights.
/// </summary>
public class RenderTargetPool : ModSystem
{
    private static readonly Dictionary<(int, int), Queue<RenderTarget2D>> targets = [];

    public override void Unload()
    {
        base.Unload();

        foreach (var rt in targets.Values.SelectMany(x => x))
        {
            rt.Dispose();
        }

        foreach (var queue in targets.Values)
        {
            queue.Clear();
        }

        targets.Clear();
    }

    /// <summary>
    ///     Gets or initializes a pooled render target.
    /// </summary>
    /// <remarks>
    ///     RTs are not guaranteed to be cleared and should be first cleared
    ///     upon use.
    /// </remarks>
    public static RenderTarget2D Get(int width, int height)
    {
        var key = (width, height);
        if (!targets.TryGetValue(key, out var queue))
        {
            targets[key] = queue = [];
        }

        return queue.TryDequeue(out var rt) ? rt : new RenderTarget2D(Main.instance.GraphicsDevice, width, height);
    }

    /// <summary>
    ///     Returns a render target to the pool.
    /// </summary>
    public static void Return(RenderTarget2D target)
    {
        var key = (target.Width, target.Height);
        if (!targets.TryGetValue(key, out var queue))
        {
            targets[key] = queue = [];
        }

        queue.Enqueue(target);
    }
}