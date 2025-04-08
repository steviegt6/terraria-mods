using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories._Melee;

[AutoloadEquip(EquipType.HandsOn)]
internal sealed class BarbaricCuffsItem : ModItem
{
    private sealed class BarbaricCuffsPlayer : ModPlayer
    {
        public bool CancelRegen { get; set; }

        public override void ResetEffects()
        {
            base.ResetEffects();

            CancelRegen = false;
        }

        public override void UpdateLifeRegen()
        {
            base.UpdateLifeRegen();

            if (!CancelRegen)
            {
                return;
            }

            if (Player.lifeRegen > 0)
            {
                Player.lifeRegen = 0;
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            base.NaturalLifeRegen(ref regen);

            if (!CancelRegen)
            {
                return;
            }

            if (regen > 0f)
            {
                regen = 0f;
            }
        }
    }

    public override string Texture => Assets.Images.Items.Accessories.BarbaricCuffs.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (46, 52);

        Item.accessory = true;

        Item.defense = 8;

        Item.SetShopValues(
            ItemRarityColor.Pink5,
            Item.sellPrice(gold: 9, silver: 50)
        );
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetArmorPenetration(DamageClass.Generic)        += 8f;
        player.aggro                                           += 400;
        player.GetModPlayer<BarbaricCuffsPlayer>().CancelRegen =  true;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient(ItemID.FleshKnuckles)
           .AddIngredient<SpikedCuffsItem>()
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}