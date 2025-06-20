using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content;

public abstract class PlatinumCritterItem<TNpc, TCage>(string critterName, int housingItem = ItemID.Terrarium) : ModItem
    where TNpc : ModNPC
    where TCage : ModItem
{
    public override string Texture => MakeTexturePath(critterName);

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.CloneDefaults(ItemID.Search.GetId(PlatCritterHelpers.GetGoldName(critterName)));
        Item.makeNPC = ModContent.NPCType<TNpc>();
        Item.value = Item.sellPrice(platinum: 1);
        Item.rare = ItemRarityID.LightRed;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        // Made by hand.
        Recipe.Create(ModContent.ItemType<TCage>())
              .AddIngredient(this)
              .AddIngredient(housingItem)
              .Register();
    }

    private static string MakeTexturePath(string name)
    {
        // Use a definite reference here so we fail to compile if we ever change
        // it.
        const string key = Assets.Images.Items.Misc.PlatinumCritters.Bird.KEY;

        // Get the actual directory.
        var basePath = key.Split('/')[..^1];

        return string.Join('/', basePath) + '/' + name;
    }
}

public sealed class PlatinumBirdItem() : PlatinumCritterItem<PlatinumBirdNpc, PlatinumBirdCageItem>("Bird");

public sealed class PlatinumBunnyItem() : PlatinumCritterItem<PlatinumBunnyNpc, PlatinumBunnyCageItem>("Bunny");

public sealed class PlatinumButterflyItem() : PlatinumCritterItem<PlatinumButterflyNpc, PlatinumButterflyCageItem>("Butterfly", ItemID.Bottle);

public sealed class PlatinumDragonflyItem() : PlatinumCritterItem<PlatinumDragonflyNpc, PlatinumDragonflyCageItem>("Dragonfly", ItemID.Bottle);

public sealed class PlatinumFrogItem() : PlatinumCritterItem<PlatinumFrogNpc, PlatinumFrogCageItem>("Frog");

public sealed class PlatinumGoldfishItem() : PlatinumCritterItem<PlatinumGoldfishNpc, PlatinumGoldfishCageItem>("Goldfish", ItemID.BottledWater);

public sealed class PlatinumGrasshopperItem() : PlatinumCritterItem<PlatinumGrasshopperNpc, PlatinumGrasshopperCageItem>("Grasshopper");

public sealed class PlatinumLadyBugItem() : PlatinumCritterItem<PlatinumLadyBugNpc, PlatinumLadyBugCageItem>("LadyBug");

public sealed class PlatinumMouseItem() : PlatinumCritterItem<PlatinumMouseNpc, PlatinumMouseCageItem>("Mouse");

public sealed class PlatinumSeahorseItem() : PlatinumCritterItem<PlatinumSeahorseNpc, PlatinumSeahorseCageItem>("Seahorse");

public sealed class PlatinumSquirrelItem() : PlatinumCritterItem<PlatinumSquirrelNpc, PlatinumSquirrelCageItem>("Squirrel");

public sealed class PlatinumWaterStriderItem() : PlatinumCritterItem<PlatinumWaterStriderNpc, PlatinumWaterStriderCageItem>("WaterStrider");

public sealed class PlatinumWormItem() : PlatinumCritterItem<PlatinumWormNpc, PlatinumWormCageItem>("Worm");