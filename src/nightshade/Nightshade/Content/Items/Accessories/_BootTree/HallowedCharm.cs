using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class HallowedCharm : ModItem
{
    public override string Texture => "ModLoader/UnloadedItem";

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.accessory = true;
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.jumpBoost = true;
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