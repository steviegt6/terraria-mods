using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Content.Items;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Tiles;

public sealed class CactusWood : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.CactusWood.KEY;

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;

        RegisterItemDrop(ModContent.ItemType<CactusWoodBlock>());

        Main.tileBrick[Type] = true;
        Main.tileMergeDirt[Type] = true;

        DustType = DustID.PalmWood;
        HitSound = SoundID.Dig;

        AddMapEntry(new Color(207, 167, 82));
    }
}