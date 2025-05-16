using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class SunGodEye : ModItem
{
    public override string Texture => Assets.Images.Items.Accessories.SunGodEye.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (42, 36);

        Item.accessory = true;
        
        Item.SetShopValues(
            ItemRarityColor.LightRed4,
            Item.sellPrice(gold: 5, silver: 29)
        );
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        // TODO: actual feature
    }

    private static readonly int[] bars = [ItemID.AdamantiteBar, ItemID.TitaniumBar];
    private static readonly int[] evils = [ItemID.CursedFlame, ItemID.Ichor];

    public override void AddRecipes()
    {
        base.AddRecipes();

        foreach (var bar in bars)
        foreach (var evil in evils)
        {
            CreateRecipe()
               .AddIngredient(ItemID.AncientBattleArmorMaterial)
               .AddIngredient(bar, 8)
               .AddIngredient(evil, 6)
               .AddTile(TileID.MythrilAnvil)
               .Register();
        }
    }
}