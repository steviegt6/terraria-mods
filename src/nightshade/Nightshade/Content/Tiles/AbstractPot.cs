using System.Collections.Generic;

using Daybreak.Common.Features.PotLoot;

using Nightshade.Common.Features;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Nightshade.Content.Tiles;

internal abstract class AbstractPot : ModTile, IHasPotBehavior
{
    public abstract PotBehavior Behavior { get; }
    
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.tileFrameImportant[Type] = true;
        Main.tileLavaDeath[Type] = true;
        Main.tileWaterDeath[Type] = false;
        Main.tileOreFinderPriority[Type] = 100;
        Main.tileSpelunker[Type] = true;
        Main.tileCut[Type] = true;
        
        // Vanilla pots don't have TileObjectData, but we do.  This also means
        // we don't need to set TileID.Sets.IsMultitile
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.addTile(Type);
        
        HitSound = SoundID.Shatter;
    }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
    {
        WorldGen.CheckPot(i, j, Type);
        return false;
    }

    // Drops are handled separately.
    public override IEnumerable<Item> GetItemDrops(int i, int j)
    {
        return [];
    }
}