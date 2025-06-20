using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class HallowedCharm : ModItem
{
    public override string Texture => Assets.Images.Items.Accessories.HallowedCharm.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        (Item.width, Item.height) = (32, 32);

        Item.accessory = true;
        
        Item.SetShopValues(
            ItemRarityColor.LightRed4,
            Item.sellPrice(gold: 3, silver: 50)
        );
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.jumpBoost = true;
        player.moveSpeed += 0.04f;
        player.noFallDmg = true;
        player.hasLuck_LuckyHorseshoe = true;
        player.GetModPlayer<FourLeafClover.FlcPlayer>().HasLuck = true;

        // TODO: Additional benefits like charging at full speed.
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient<RainbowHorseshoe>()
           .AddIngredient(ItemID.UnicornHorn, 3)
           .AddIngredient(ItemID.PixieDust, 10)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}