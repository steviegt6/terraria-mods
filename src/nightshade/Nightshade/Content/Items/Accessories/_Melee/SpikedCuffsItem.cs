using Nightshade.Common.Features;

using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories._Melee;

[AutoloadEquip(EquipType.HandsOn)]
internal sealed class SpikedCuffsItem : ModItem
{
    public override string Texture => Assets.Images.Items.Accessories.SpikedCuffs.KEY;

    private const int max_time_since_last_hit = 5 * 60; // 5 seconds

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (34, 48);

        Item.accessory = true;

        Item.SetShopValues(
            ItemRarityColor.Blue1,
            Item.buyPrice(gold: 1, silver: 30)
        );
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        if (player.TimeSinceLastHitEnemy() >= max_time_since_last_hit)
        {
            return;
        }

        player.statDefense                              -= 8;
        player.GetArmorPenetration(DamageClass.Generic) += 8f;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient(ItemID.Shackle)
           .AddIngredient(ItemID.SharkToothNecklace)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}