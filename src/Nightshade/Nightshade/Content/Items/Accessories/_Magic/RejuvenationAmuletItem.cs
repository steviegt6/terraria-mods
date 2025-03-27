using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.Nightshade.Content.Items.Accessories;

[AutoloadEquip(EquipType.Neck)]
internal sealed class RejuvenationAmuletItem : ModItem
{
    public sealed class RejuvenationBandPlayer : ModPlayer
    {
        private const int regen_boost_time = 5 * 60;

        public bool IsEquipped { get; set; }

        private int regenBoostTime;

        public override void ResetEffects()
        {
            base.ResetEffects();

            IsEquipped = false;
        }

        public override bool OnPickup(Item item)
        {
            // TODO: Star ItemID set?
            if (IsEquipped && item.type is ItemID.Star or ItemID.SoulCake or ItemID.SugarPlum or ItemID.ManaCloakStar)
            {
                regenBoostTime = regen_boost_time;
            }

            return base.OnPickup(item);
        }

        public override void UpdateEquips()
        {
            base.UpdateEquips();

            if (!IsEquipped)
            {
                regenBoostTime = 0;
                return;
            }

            if (--regenBoostTime <= 0)
            {
                regenBoostTime = 0;
                return;
            }

            Player.manaRegenDelayBonus += 1f;
            Player.manaRegenBonus      += 25;
        }
    }

    public override string Texture => $"{Mod.Name}/Assets/Images/Items/Accessories/RejuvenationAmulet";

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (36, 34);

        Item.accessory = true;

        Item.rare  = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1, silver: 30);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<StarTalismanItem.StarTalismanPlayer>().IsEquipped = true;
        player.GetModPlayer<RejuvenationBandPlayer>().IsEquipped              = true;

        player.statManaMax2 += 20;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient(ItemID.ManaRegenerationBand)
           .AddIngredient<StarTalismanItem>()
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}