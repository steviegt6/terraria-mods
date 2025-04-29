using System.Collections.Generic;
using System.IO;

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
    
    private const int max_items = 20;

    public override string Texture => Assets.Images.Items.Misc.PreDigester.KEY;

    private Dictionary<int, int> storedItems = [];

    private static PreDigester? instanceToSendItemsTo;
    
    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (24, 12);

        Item.useStyle = ItemUseStyleID.Thrust;
    }

    public override void Load()
    {
        base.Load();

        On_Player.DropItemFromExtractinator += (orig, self, type, stack) =>
        {
            // Give coins directly to the player.
            if (instanceToSendItemsTo is not null && !ItemID.Sets.CommonCoin[type])
            {
                instanceToSendItemsTo.AddExtractinatorResult(type, stack);
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
        foreach (var kvp in storedItems)
        {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value);
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
            storedItems[itemType] = stack;
        }
    }

    public override void SaveData(TagCompound tag)
    {
        base.SaveData(tag);
        
        tag["storedItems"] = storedItems;
    }
    
    public override void LoadData(TagCompound tag)
    {
        base.LoadData(tag);
        
        storedItems = tag.Get<Dictionary<int, int>>("storedItems");
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

        ExtractItem(player, itemType);
        return true;
    }

    private void ExtractItem(Player player, int itemType)
    {
        instanceToSendItemsTo = this;
        player.ExtractinatorUse(itemType, TileID.Extractinator);
        instanceToSendItemsTo = null;
    }

    private void AddExtractinatorResult(int itemType, int stack)
    {
        if (!storedItems.TryAdd(itemType, stack))
        {
            storedItems[itemType] += stack;
        }
    }
    
    private static bool CanBeExtracted(int itemType)
    {
        return ItemID.Sets.ExtractinatorMode[itemType] != -1;
    }
}