using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Nightshade.Content.Items;

internal sealed class PreDigester : ModItem
{
    private sealed class PreDigesterPlayer : ModPlayer
    {
        public override bool OnPickup(Item item)
        {
            if (!CanBeExtracted(item.type))
            {
                return base.OnPickup(item);
            }

            // Try to make every PreDigester in the inventory claim the item.
            for (var i = 0; i < Main.InventorySlotsTotal; i++)
            {
                if (Player.inventory[i].ModItem is not PreDigester preDigester)
                {
                    continue;
                }

                while (item.stack > 0 && preDigester.TryClaimSilt(Player, item.type))
                {
                    item.stack--;
                }

                if (item.stack == 0)
                {
                    break;
                }
            }

            // TODO: Any behavior to properly cancel this?
            if (item.stack == 0)
            {
                item.TurnToAir();
            }

            return true;
        }
    }

    private const int max_items = 200;

    public override string Texture => Assets.Images.Items.Misc.PreDigester.KEY;

    private List<(int itemType, int stack)> storedItems = [];

    private static PreDigester? instanceToSendItemsTo;
    private static bool wasFull;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (24, 12);

        Item.useStyle = ItemUseStyleID.Thrust;

		Item.rare = ItemRarityID.Blue;
        Item.value = Item.buyPrice(gold: 1, silver: 50);
	}

	public override void Load()
    {
        base.Load();

        On_Player.DropItemFromExtractinator += (orig, self, type, stack) =>
        {
            if (instanceToSendItemsTo is not null)
            {
                // Give coins directly to the player.
                if (ItemID.Sets.CommonCoin[type])
                {
                    self.QuickSpawnItem(self.GetSource_ItemUse(Item, "PreDigester"), type, stack);
                    return;
                }

                wasFull = !instanceToSendItemsTo.AddExtractinatorResult(type, stack, out var remaining);

                if (remaining > 0)
                {
                    self.QuickSpawnItem(self.GetSource_ItemUse(Item, "PreDigester"), type, remaining);
                }
                return;
            }

            orig(self, type, stack);
        };
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);

        // TODO: Sort tooltips properly

        /*var silt = new TooltipLine(Mod, "PreDigesterSiltCount", $"{GetItemTag(currentSilt, ItemID.SiltBlock)}/{GetItemTag(max_silt, ItemID.SiltBlock)}");
        tooltips.Add(silt);*/

        var items = "";
        foreach (var (itemType, stack) in storedItems)
        {
            if (stack <= 0)
            {
                continue;
            }

            items += $"{GetItemTag(stack, itemType)} ";
        }
        if (items.Length > 0)
        {
            items = items[..^1];
        }
        var itemsLine = new TooltipLine(Mod, "PreDigesterItems", items);
        tooltips.Add(itemsLine);

        return;

        static string GetItemTag(int amount, int type)
        {
            return $"[i/s{amount}:{type}]";
        }
    }

    public override bool? UseItem(Player player)
    {
        if (storedItems.Count == 0)
        {
            return base.UseItem(player);
        }

        foreach (var (itemType, stack) in storedItems)
        {
            if (stack <= 0)
            {
                continue;
            }

            player.QuickSpawnItem(player.GetSource_ItemUse(Item, "PreDigester"), itemType, stack);
        }

        storedItems.Clear();
        return true;
    }

    public override void NetSend(BinaryWriter writer)
    {
        base.NetSend(writer);

        writer.Write(storedItems.Count);
        foreach (var (itemType, stack) in storedItems)
        {
            writer.Write(itemType);
            writer.Write(stack);
        }
    }

    public override void NetReceive(BinaryReader reader)
    {
        base.NetReceive(reader);

        var count = reader.ReadInt32();
        storedItems.Clear();
        for (var i = 0; i < count; i++)
        {
            var itemType = reader.ReadInt32();
            var stack = reader.ReadInt32();
            storedItems.Add((itemType, stack));
        }
    }

    public override void SaveData(TagCompound tag)
    {
        base.SaveData(tag);

        tag["storedItems"] = storedItems.Select(x => $"{x.itemType}/{x.stack}").ToArray();
    }

    public override void LoadData(TagCompound tag)
    {
        base.LoadData(tag);

        var storedItems = tag.GetList<string>("storedItems");
        this.storedItems.Clear();
        foreach (var item in storedItems)
        {
            var parts = item.Split('/');
            if (parts.Length != 2)
            {
                continue;
            }

            var itemType = int.Parse(parts[0]);
            var stack = int.Parse(parts[1]);
            this.storedItems.Add((itemType, stack));
        }
    }

    /// <summary>
    ///     Attempts to claim an item of the given type.  If successful, it
    ///     should be considered extracted.
    /// </summary>
    public bool TryClaimSilt(Player player, int itemType)
    {
        // Means this item can't be extracted.
        if (!CanBeExtracted(itemType))
        {
            return false;
        }

        if (!IsAcceptingItems())
        {
            return false;
        }

        return ExtractItem(player, itemType);
    }

    private bool ExtractItem(Player player, int itemType)
    {
        instanceToSendItemsTo = this;
        player.ExtractinatorUse(itemType, TileID.Extractinator);
        instanceToSendItemsTo = null;

        if (!wasFull)
        {
            return true;
        }

        wasFull = false;
        return false;
    }

    private bool IsAcceptingItems()
    {
        return storedItems.Select(x => x.stack).Sum() < max_items;
    }

    private bool AddExtractinatorResult(int itemType, int stack, out int remaining)
    {
        var totalItems = storedItems.Select(x => x.stack).Sum();
        if (totalItems >= max_items)
        {
            // Carry over any that won't make it in so we can spit it out.
            remaining = totalItems + stack - max_items;
            stack -= remaining;
        }
        else
        {
            remaining = 0;
        }

        // Respect max stack sizes for items when adding.
        var maxStackSize = ContentSamples.ItemsByType[itemType].maxStack;
        for (var i = 0; i < storedItems.Count; i++)
        {
            var (t, s) = storedItems[i];

            if (itemType != t)
            {
                continue;
            }

            if (s >= maxStackSize)
            {
                continue;
            }

            var add = Math.Min(stack, maxStackSize - s);
            stack -= add;
            storedItems[i] = (t, s + add);

            if (stack <= 0)
            {
                return true;
            }
        }

        // If we still have some stack, add it as a new item.
        if (stack > 0)
        {
            storedItems.Add((itemType, stack));
        }

        return true;
    }

    private static bool CanBeExtracted(int itemType)
    {
        return ItemID.Sets.ExtractinatorMode[itemType] != -1;
    }
}