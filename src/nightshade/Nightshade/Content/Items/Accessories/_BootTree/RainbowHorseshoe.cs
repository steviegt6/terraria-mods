using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Nightshade.Content.Items.Accessories;

internal sealed class RainbowHorseshoe : ModItem
{
    private sealed class LuckyCharmsCereal : GlobalItem
    {
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (item.type != ModContent.ItemType<RainbowHorseshoe>())
            {
                return base.PreDrawTooltipLine(item, line, ref yOffset);
            }

            if (line is not { Mod: "Terraria", Name: "Price" })
            {
                return base.PreDrawTooltipLine(item, line, ref yOffset);
            }

            var saleColor = new Color(109, 213, 140);
            var originalColor = new Color(153, 157, 169);

            const string sale_text = "$2.08";
            const string original_text = "(-30%)";
            // const string original_text = "$3";

            var originalOffset = line.Font.MeasureString(sale_text + "  ");

            var pos = new Vector2(line.OriginalX, line.OriginalY);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.Font, sale_text, pos, saleColor, Color.Black, 0f, Vector2.Zero, line.BaseScale);

            pos += Vector2.UnitX * originalOffset.X;
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.Font, original_text, pos, originalColor, Color.Black, 0f, Vector2.Zero, line.BaseScale);

            /*var originalMeasure = line.Font.MeasureString(original_text);
            var strikePosition = pos + Vector2.UnitY * (originalMeasure.Y / 3f);
            for (var i = 0; i < 5; i++)
            {
                var color = i == 4 ? originalColor : Color.Black;
                var offset = i == 4 ? Vector2.Zero : ChatManager.ShadowDirections[i] * 2f;

                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)(strikePosition.X + offset.X), (int)(strikePosition.Y + offset.Y), (int)originalMeasure.X, 2), color);
            }*/

            return false;
        }
    }

    public override string Texture => Assets.Images.Items.Accessories.RainbowHorseshoe.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (26, 26);

        Item.accessory = true;

        Item.SetShopValues(
            ItemRarityColor.Green2,
            Item.sellPrice(gold: 2, silver: 8)
        );
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