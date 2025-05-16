using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class RainbowHorseshoe : ModItem
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
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient(ItemID.PinkGel, 10)
           .AddIngredient(ItemID.ShinyRedBalloon)
           .AddIngredient(ItemID.LuckyHorseshoe)
           .AddIngredient<FourLeafClover>()
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}