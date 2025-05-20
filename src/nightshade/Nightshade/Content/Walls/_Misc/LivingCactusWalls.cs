using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Content.Items;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Walls;

public sealed class LivingCactusWoodWall : ModWall
{
    public override string Texture => Assets.Images.Walls.Misc.LivingCactusWoodWall.KEY;

    public override void SetStaticDefaults()
    {
        DustType = DustID.t_Cactus;
        HitSound = SoundID.Dig;

		AddMapEntry(new Color(84, 67, 33));
    }
}