using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Content.Gores;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Nightshade.Content.Tiles;

public class LivingCactusPot : ModTile
{
    public override string Texture => Assets.Images.Tiles.Misc.LivingCactusPot.KEY;

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileWaterDeath[Type] = false;
        Main.tileOreFinderPriority[Type] = 100;
        Main.tileSpelunker[Type] = true;
        Main.tileCut[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.addTile(Type);
        AddMapEntry(new Color(47, 79, 79), Language.GetText("MapObject.Pot")); // dark slate gray
        DustType = 29;
        HitSound = SoundID.Shatter;
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        if (Main.netMode != NetmodeID.Server)
        {
            int goreAmt = Main.rand.Next(1, 2 + 1);
            for (int k = 0; k < goreAmt; k++)
            {
                for (int l = 1; l < 3; l++)
                {
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.NextVector2CircularEdge(3f, 3f), Mod.Find<ModGore>($"LivingCactusPotGore{l}").Type);
                }
            }
        }

        return base.GetItemDrops(i, j);
    }
}



public class LivingCactusPotPlacer : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactusPot>());

        Item.value = 100;
        Item.rare = ItemRarityID.Green;
    }
}

public class PotGoreLoader : ILoadable
{
    public void Load(Mod mod)
    {
        // Load the gore here
        mod.AddContent(new GenericGore("LivingCactusPotGore1", Assets.Images.Gores.Misc.CactusPotGore1.KEY));
        mod.AddContent(new GenericGore("LivingCactusPotGore2", Assets.Images.Gores.Misc.CactusPotGore2.KEY));
    }

    public void Unload()
    {

    }
}