using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

internal sealed class ItemSlotLoader : ModSystem
{
    public static int Count { get; private set; } = ItemSlot.Context.Count;

    private static readonly List<CustomItemSlot> item_slots = [];

    private static int originalContext = -1;

    public static void Register(CustomItemSlot itemSlot)
    {
        itemSlot.Type = Count++;
        item_slots.Add(itemSlot);
    }

    private static CustomItemSlot? GetItemSlot(int type)
    {
        if (type < ItemSlot.Context.Count || type >= Count)
        {
            return null;
        }

        return item_slots[type - ItemSlot.Context.Count];
    }

    //       Handle(Item[] inv, int context = 0, int slot = 0)
    // DONE: OverrideHover(Item[] inv, int context = 0, int slot = 0)
    //       OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
    //       IsAccessoryContext(int context)
    // DONE: LeftClick(Item[] inv, int context = 0, int slot = 0)
    //       LeftClick_SellOrTrash(Item[] inv, int context, int slot)
    //       SellOrTrash(Item[] inv, int context, int slot)
    //       GetOverrideInstructions(Item[] inv, int context, int slot)
    // DONE: PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
    //       RightClick(Item[] inv, int context = 0, int slot = 0)
    //       PickupItemIntoMouse(Item[] inv, int context, int slot, Player player)
    // DONE: SwapVanityEquip(Item[] inv, int context, int slot, Player player)
    //       TryPickupDyeToCursor(int context, Item[] inv, int slot, Player player)
    // DONE: Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
    //       GetColorByLoadout(int slot, int context)
    //       TryGetSlotColor(int loadoutIndex, int context, out Color color)
    //       DrawItemIcon(Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
    // DONE: MouseHover(Item[] inv, int context = 0, int slot = 0)
    // DONE: SwapEquip(Item[] inv, int context, int slot)
    //       Equippable(Item[] inv, int context, int slot)
    //       TryEnteringFastUseMode(Item[] inv, int context, int slot, Player player, ref string s)
    //       TryEnteringBuildingMode(Item[] inv, int context, int slot, Player player, ref string s)

    public override void ResizeArrays()
    {
        base.ResizeArrays();

        Array.Resize(ref ItemSlot.canFavoriteAt, Count);
        Array.Resize(ref ItemSlot.canShareAt, Count);
    }

    public override void Load()
    {
        base.Load();

        On_ItemSlot.OverrideHover_ItemArray_int_int += OverrideHover;
        On_ItemSlot.LeftClick_ItemArray_int_int += LeftClick;
        On_ItemSlot.GetOverrideInstructions += GetOverrideInstructions;
        On_ItemSlot.PickItemMovementAction += PickItemMovementAction;
        On_ItemSlot.SwapVanityEquip += SwapVanityEquip;
        On_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += Draw;
        On_ItemSlot.MouseHover_ItemArray_int_int += MouseHover;
        On_ItemSlot.SwapEquip_ItemArray_int_int += SwapEquip;

        IL_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += ModifyItemSlotIcon;
    }

    private static void ModifyItemSlotIcon(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(x => x.MatchCallvirt<AccessorySlotLoader>(nameof(AccessorySlotLoader.DrawSlotTexture)));
        c.GotoNext(MoveType.Before, x => x.MatchCallvirt<SpriteBatch>(nameof(SpriteBatch.Draw)));

        c.Remove();

        c.EmitDelegate(
            (
                SpriteBatch self,
                Texture2D texture,
                Vector2 position,
                Rectangle? sourceRectangle,
                Color color,
                float rotation,
                Vector2 origin,
                float scale,
                SpriteEffects effects,
                float layerDepth
            ) =>
            {
                if (GetItemSlot(originalContext) is not { } itemSlot)
                {
                    self.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
                    return;
                }

                if (itemSlot.ModifyIcon(self, ref texture, ref position, ref sourceRectangle, ref color, ref rotation, ref origin, ref scale, ref effects))
                {
                    self.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
                }
            }
        );
    }

    private static void LeftClick(On_ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        try
        {
            originalContext = context;

            var itemSlot = GetItemSlot(context);
            if (itemSlot is null)
            {
                orig(inv, context, slot);
                return;
            }

            if (!itemSlot.PreLeftClick(inv[slot], ref context))
            {
                return;
            }

            orig(inv, context, slot);
            itemSlot.PostLeftClick(inv[slot], context);
        }
        finally
        {
            originalContext = -1;
        }
    }

    private static int PickItemMovementAction(On_ItemSlot.orig_PickItemMovementAction orig, Item[] inv, int context, int slot, Item checkItem)
    {
        context = originalContext == -1 ? context : originalContext;

        var itemSlot = GetItemSlot(context);
        if (itemSlot is null)
        {
            return orig(inv, context, slot, checkItem);
        }

        var result = itemSlot.PrePickItemMovementAction(inv[slot], ref context, checkItem);
        if (result.HasValue)
        {
            return result.Value;
        }

        result = orig(inv, context, slot, checkItem);
        itemSlot.PostPickItemMovementAction(inv[slot], context, checkItem);
        return result.Value;
    }

    private static string GetOverrideInstructions(On_ItemSlot.orig_GetOverrideInstructions orig, Item[] inv, int context, int slot)
    {
        var itemSlot = GetItemSlot(context);
        if (itemSlot is null)
        {
            return orig(inv, context, slot);
        }

        var result = itemSlot.PreGetOverrideInstructions(inv[slot], ref context);
        if (result is not null)
        {
            return result;
        }

        result = orig(inv, context, slot);
        itemSlot.PostGetOverrideInstructions(inv[slot], context);
        return result;
    }

    private static void SwapVanityEquip(On_ItemSlot.orig_SwapVanityEquip orig, Item[] inv, int context, int slot, Player player)
    {
        var itemSlot = GetItemSlot(context);
        if (itemSlot is null)
        {
            orig(inv, context, slot, player);
            return;
        }

        if (!itemSlot.PreSwapVanityEquip(inv[slot], ref context, player))
        {
            return;
        }

        orig(inv, context, slot, player);
        itemSlot.PostSwapVanityEquip(inv[slot], context, player);
    }

    private static void Draw(On_ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
    {
        try
        {
            originalContext = context;

            var itemSlot = GetItemSlot(context);
            if (itemSlot is null)
            {
                orig(spriteBatch, inv, context, slot, position, lightColor);
                return;
            }

            if (!itemSlot.PreDraw(spriteBatch, inv[slot], ref context, position, lightColor))
            {
                return;
            }

            orig(spriteBatch, inv, context, slot, position, lightColor);
            itemSlot.PostDraw(spriteBatch, inv[slot], context, position, lightColor);
        }
        finally
        {
            originalContext = -1;
        }
    }

    private static void MouseHover(On_ItemSlot.orig_MouseHover_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        var itemSlot = GetItemSlot(context);
        if (itemSlot is null)
        {
            orig(inv, context, slot);
            return;
        }

        if (!itemSlot.PreMouseHover(inv[slot], ref context))
        {
            return;
        }

        orig(inv, context, slot);
        itemSlot.PostMouseHover(inv[slot], context);
    }

    private static void SwapEquip(On_ItemSlot.orig_SwapEquip_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        var itemSlot = GetItemSlot(context);
        if (itemSlot is null)
        {
            orig(inv, context, slot);
            return;
        }

        if (!itemSlot.PreSwapEquip(inv[slot], ref context))
        {
            return;
        }

        orig(inv, context, slot);
        itemSlot.PostSwapEquip(inv[slot], context);
    }

    private static void OverrideHover(On_ItemSlot.orig_OverrideHover_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        var itemSlot = GetItemSlot(context);
        if (itemSlot is null)
        {
            orig(inv, context, slot);
            return;
        }

        if (!itemSlot.PreOverrideHover(inv[slot], ref context))
        {
            return;
        }

        orig(inv, context, slot);
        itemSlot.PostOverrideHover(inv[slot], context);
    }
}