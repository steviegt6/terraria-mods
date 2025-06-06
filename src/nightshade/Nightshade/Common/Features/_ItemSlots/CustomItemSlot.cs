using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal abstract class CustomItemSlot : ModType
{
    public int Type { get; internal set; }

    protected sealed override void Register()
    {
        ItemSlotLoader.Register(this);
    }

    protected sealed override void InitTemplateInstance()
    {
        base.InitTemplateInstance();
    }

    public sealed override void SetupContent()
    {
        base.SetupContent();

        SetStaticDefaults();
    }

    public virtual bool PreLeftClick(Item item, ref int context)
    {
        return true;
    }

    public virtual void PostLeftClick(Item item, int context) { }

    public virtual bool PreRightClick(Item item, ref int context)
    {
        return true;
    }
    
    public virtual void PostRightClick(Item item, int context) { }

    public virtual int? PrePickItemMovementAction(Item item, ref int context, Item checkItem)
    {
        return null;
    }

    public virtual void PostPickItemMovementAction(Item item, int context, Item checkItem) { }

    public virtual string? PreGetOverrideInstructions(Item item, ref int context)
    {
        return null;
    }

    public virtual void PostGetOverrideInstructions(Item item, int context) { }

    public virtual bool PreSwapVanityEquip(Item item, ref int context, Player player)
    {
        return true;
    }

    public virtual void PostSwapVanityEquip(Item item, int context, Player player) { }

    public virtual bool PreDraw(SpriteBatch spriteBatch, Item item, ref int context, Vector2 position, Color lightColor)
    {
        return true;
    }

    public virtual void PostDraw(SpriteBatch spriteBatch, Item item, int context, Vector2 position, Color lightColor) { }

    public virtual bool ModifyIcon(
        SpriteBatch spriteBatch,
        ref Texture2D texture,
        ref Vector2 position,
        ref Rectangle? sourceRectangle,
        ref Color color,
        ref float rotation,
        ref Vector2 origin,
        ref float scale,
        ref SpriteEffects effects
    )
    {
        return true;
    }

    public virtual bool PreMouseHover(Item item, ref int context)
    {
        return true;
    }

    public virtual void PostMouseHover(Item item, int context) { }

    public virtual bool PreSwapEquip(Item item, ref int context)
    {
        return true;
    }

    public virtual void PostSwapEquip(Item item, int context) { }

    public virtual bool PreOverrideHover(Item item, ref int context)
    {
        return true;
    }

    public virtual void PostOverrideHover(Item item, int context) { }

    public virtual bool TryHandleSwap(ref Item item, int incomingContext, Player player)
    {
        return false;
    }
}