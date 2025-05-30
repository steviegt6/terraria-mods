using System;
using System.Collections.Generic;

using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

internal sealed class ItemSlotLoader : ModSystem
{
    public static int Count { get; private set; } = ItemSlot.Context.Count;

    private static readonly List<CustomItemSlot> item_slots = [];
    
    public static void Register(CustomItemSlot itemSlot)
    {
        itemSlot.Type = Count++;
        item_slots.Add(itemSlot);
    }

    public override void ResizeArrays()
    {
        base.ResizeArrays();
        
        Array.Resize(ref ItemSlot.canFavoriteAt, Count);
        Array.Resize(ref ItemSlot.canShareAt, Count);
    }
}