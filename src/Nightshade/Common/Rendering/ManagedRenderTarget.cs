using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Tomat.TML.Mod.Nightshade.Common.Rendering;

/// <summary>
///     A managed, intelligent wrapper around a <see cref="RenderTarget2D"/>.
///     <br />
///     It handles triggering re-initializations upon changes to the resolution.
///     <br />
///     API consumers should invoke <see cref="Initialize"/> upon creation with
///     their target size.
/// </summary>
internal sealed class ManagedRenderTarget : IDisposable
{
    /// <summary>
    ///     The wrapped render target.
    /// </summary>
    public RenderTarget2D? Value { get; private set; }

    private Func<int, int, RenderTarget2D> initFunc;

    /// <summary>
    ///     Creates a new wrapper which will initialize the render target to the
    ///     given width and height.
    /// </summary>
    public ManagedRenderTarget(int width, int height, bool reinitOnResolutionChange)
    {
        initFunc = (_, _) => DefaultInitializer(width, height);

        if (reinitOnResolutionChange)
        {
            Main.OnResolutionChanged += ReinitializeRenderTarget;
        }
    }

    /// <summary>
    ///     Creates a new wrapper which will initialize the render target given
    ///     the arbitrary initialization function.  This function takes the
    ///     contextual width and height as parameters (most commonly screen
    ///     width and height) as specified in <see cref="Initialize"/>.
    /// </summary>
    public ManagedRenderTarget(Func<int, int, RenderTarget2D> initFunc, bool reinitOnResolutionChange)
    {
        this.initFunc = initFunc;

        if (reinitOnResolutionChange)
        {
            Main.OnResolutionChanged += ReinitializeRenderTarget;
        }
    }

    /// <summary>
    ///     Creates a new wrapper which will initialize a render target to the
    ///     contextual width and height (most commonly screen width and height)
    ///     as specified in <see cref="Initialize"/>.
    /// </summary>
    public ManagedRenderTarget(bool reinitOnResolutionChange)
    {
        initFunc = DefaultInitializer;

        if (reinitOnResolutionChange)
        {
            Main.OnResolutionChanged += ReinitializeRenderTarget;
        }
    }

    /// <summary>
    ///     Initializes the render target.  Disposes of and reinitialized the
    ///     render target if it already exists.
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    public void Initialize(int screenWidth, int screenHeight)
    {
        Dispose(false);

        Value = initFunc(screenWidth, screenHeight);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool forRealsies)
    {
        Value?.Dispose();
        Value = null;

        if (!forRealsies)
        {
            return;
        }

        try
        {
            Main.OnResolutionChanged -= ReinitializeRenderTarget;
        }
        catch
        {
            // ignore
        }
    }

    private void ReinitializeRenderTarget(Vector2 size)
    {
        Initialize((int)size.X, (int)size.Y);
    }

    private static RenderTarget2D DefaultInitializer(int width, int height)
    {
        return new RenderTarget2D(Main.instance.GraphicsDevice, width, height);
    }
}