using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;

namespace Nightshade.Common.Features;

public static class CustomItemSlot
{
    public static CustomItemSlotContext? CurrentContext { get; private set; }

    public static void Handle(ref Item inv, CustomItemSlotContext context)
    {
        try
        {
            CurrentContext = context;
            ItemSlot.Handle(ref inv, context.VanillaContext);
        }
        finally
        {
            CurrentContext = null;
        }
    }

    public static void Draw(SpriteBatch spriteBatch, ref Item inv, CustomItemSlotContext context, Vector2 position, Color lightColor = default)
    {
        try
        {
            CurrentContext = context;
            ItemSlot.Draw(spriteBatch, ref inv, context.VanillaContext, position, lightColor);
        }
        finally
        {
            CurrentContext = null;
        }
    }
}