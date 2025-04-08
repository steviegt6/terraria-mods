using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace Nightshade.Common.Rendering;

internal static class RtContentsPreserver
{
    private static bool done;

    public static void Request()
    {
        if (done)
        {
            return;
        }

        done = true;

        // Assume some other mod got to it first.
        if (Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage == RenderTargetUsage.PreserveContents)
        {
            return;
        }

        Main.RunOnMainThread(
            () =>
            {
                Main.graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
                Main.graphics.ApplyChanges();
            }
        );
    }
}