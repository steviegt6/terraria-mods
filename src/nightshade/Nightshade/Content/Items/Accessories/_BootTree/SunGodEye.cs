using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class SunGodEye : ModItem
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