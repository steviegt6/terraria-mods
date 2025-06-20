using Terraria.ModLoader;

namespace Nightshade.Content;

// TODO: everything

public abstract class PlatinumCritterTile<TCageItem>(string critterName) : ModTile
    where TCageItem : ModItem
{
    public override string Texture => MakeTexturePath(critterName);

    private static string MakeTexturePath(string name)
    {
        // Use a definite reference here so we fail to compile if we ever change
        // it.
        const string key = Assets.Images.Tiles.Misc.PlatinumCritters.Bird.KEY;

        // Get the actual directory.
        var basePath = key.Split('/')[..^1];

        return string.Join('/', basePath) + '/' + name;
    }
}

public sealed class PlatinumBirdTile() : PlatinumCritterTile<PlatinumBirdCageItem>("Bird");

public sealed class PlatinumBunnyTile() : PlatinumCritterTile<PlatinumBunnyCageItem>("Bunny");

public sealed class PlatinumButterflyTile() : PlatinumCritterTile<PlatinumButterflyCageItem>("Butterfly");

public sealed class PlatinumDragonflyTile() : PlatinumCritterTile<PlatinumDragonflyCageItem>("Dragonfly");

public sealed class PlatinumFrogTile() : PlatinumCritterTile<PlatinumFrogCageItem>("Frog");

public sealed class PlatinumGoldfishTile() : PlatinumCritterTile<PlatinumGoldfishCageItem>("Goldfish");

public sealed class PlatinumGrasshopperTile() : PlatinumCritterTile<PlatinumGrasshopperCageItem>("Grasshopper");

public sealed class PlatinumLadyBugTile() : PlatinumCritterTile<PlatinumLadyBugCageItem>("LadyBug");

public sealed class PlatinumMouseTile() : PlatinumCritterTile<PlatinumMouseCageItem>("Mouse");

public sealed class PlatinumSeahorseTile() : PlatinumCritterTile<PlatinumSeahorseCageItem>("Seahorse");

public sealed class PlatinumSquirrelTile() : PlatinumCritterTile<PlatinumSquirrelCageItem>("Squirrel");

public sealed class PlatinumWaterStriderTile() : PlatinumCritterTile<PlatinumWaterStriderCageItem>("WaterStrider");

public sealed class PlatinumWormTile() : PlatinumCritterTile<PlatinumWormCageItem>("Worm");