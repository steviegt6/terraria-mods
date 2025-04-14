using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
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
}

public class LivingCactusPotPlacer : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactusPot>());

        Item.value   = 100;
        Item.rare    = ItemRarityID.Green;
    }
}