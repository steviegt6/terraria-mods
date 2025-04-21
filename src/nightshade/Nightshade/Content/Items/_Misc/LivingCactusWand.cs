using Nightshade.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

internal sealed class LivingCactusWand : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusWand.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (28, 30);

        Item.value = 100;
        Item.rare = ItemRarityID.Green;

        Item.tileWand = ItemID.Cactus;
        Item.createTile = ModContent.TileType<LivingCactus>();

        Item.autoReuse = true;
        Item.useAnimation = 20;
        Item.useTime = 15;
        Item.useStyle = ItemUseStyleID.Swing;
    }
}