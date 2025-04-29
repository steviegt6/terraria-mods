using Nightshade.Content.Tiles;

using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

public sealed class LivingCactusBlock : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingCactusBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactus>());
    }
}