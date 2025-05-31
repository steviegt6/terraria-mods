using Daybreak.Common.Features.Hooks;

using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Nightshade.Common.Rendering;

internal static class RtContentPreserver
{
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

    [OnLoad]
    private static void Load()
    {
        Main.RunOnMainThread(
            () =>
            {
                Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
                Main.graphics.ApplyChanges();
            }
        );
    }
}