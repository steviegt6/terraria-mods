using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Loading;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Rendering;

internal sealed class RtContentPreserver : ILoad
{
    void ILoad.Load()
    {
        Main.RunOnMainThread(
            () =>
            {
                Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
                Main.graphics.ApplyChanges();
            }
        );
    }

    public static void ApplyToBindings(RenderTargetBinding[] bindings)
    {
        foreach (var binding in bindings)
        {
            if (binding.RenderTarget is not RenderTarget2D rt)
            {
                continue;
            }

            rt.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }
    }
}