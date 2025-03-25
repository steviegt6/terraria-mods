using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Tomat.TML.Mod.Nightshade.Common.Hooks.StatPickups;

namespace Tomat.TML.Mod.Nightshade.Content.Items.Accessories;

// TODO: Player spritesheet.

[AutoloadEquip(EquipType.Neck)]
internal sealed class StarTalismanAccessory : ModItem
{
    public sealed class StarTalismanPlayer : ModPlayer, IModifyStatPickups
    {
        public bool IsEquipped { get; set; }
        
        public override void ResetEffects()
        {
            base.ResetEffects();

            IsEquipped = false;
        }

        void IModifyStatPickups.ModifyStatPickup(StatPickupKind kind, ref int amount)
        {
            if (!IsEquipped)
            {
                return;
            }
            
            switch (kind)
            {
                case StatPickupKind.Star:
                    amount += 20;
                    break;

                case StatPickupKind.ManaCloakStar:
                    amount += 10;
                    break;
            }
        }
    }
    
    public override string Texture => $"{Mod.Name}/Assets/Images/Items/Accessories/StarTalisman";

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (28, 38);

        Item.accessory = true;

        Item.rare  = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 20);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<StarTalismanPlayer>().IsEquipped = true;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient(ItemID.FallenStar, 3)
           .AddIngredient(ItemID.Cobweb,     20)
           .Register();
    }
}