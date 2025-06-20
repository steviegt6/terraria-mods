using MonoMod.Cil;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content;

public abstract class PlatinumCritterCageItem<TTile>(string critterName) : ModItem
    where TTile : ModTile
{
    public override string Texture => MakeTexturePath(critterName);

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.CloneDefaults(ItemID.Search.GetId(PlatCritterHelpers.GetGoldName(critterName)));
        Item.createTile = ModContent.TileType<TTile>();
        Item.value = Item.sellPrice(platinum: 1);
        Item.rare = ItemRarityID.LightRed;
    }

    private static string MakeTexturePath(string name)
    {
        // Use a definite reference here so we fail to compile if we ever change
        // it.
        const string key = Assets.Images.Items.Misc.PlatinumCritters.Cages.Bird.KEY;

        // Get the actual directory.
        var basePath = key.Split('/')[..^1];

        return string.Join('/', basePath) + '/' + name;
    }
}

public sealed class PlatinumBirdCageItem() : PlatinumCritterCageItem<PlatinumBirdTile>("Bird");

public sealed class PlatinumBunnyCageItem() : PlatinumCritterCageItem<PlatinumBunnyTile>("Bunny");

public sealed class PlatinumButterflyCageItem() : PlatinumCritterCageItem<PlatinumButterflyTile>("Butterfly");

public sealed class PlatinumDragonflyCageItem() : PlatinumCritterCageItem<PlatinumDragonflyTile>("Dragonfly");

public sealed class PlatinumFrogCageItem() : PlatinumCritterCageItem<PlatinumFrogTile>("Frog");

[AutoloadEquip(EquipType.Head)]
public sealed class PlatinumGoldfishCageItem() : PlatinumCritterCageItem<PlatinumGoldfishTile>("Goldfish")
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        IL_Player.CheckDrowning += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(MoveType.Before, x => x.MatchLdcI4(ItemID.GoldGoldfishBowl));
            c.EmitDelegate(
                (int type) =>
                {
                    if (type == ModContent.ItemType<PlatinumGoldfishCageItem>())
                    {
                        return ItemID.GoldGoldfishBowl;
                    }

                    return type;
                }
            );
        };
    }
}

public sealed class PlatinumGrasshopperCageItem() : PlatinumCritterCageItem<PlatinumGrasshopperTile>("Grasshopper");

public sealed class PlatinumLadyBugCageItem() : PlatinumCritterCageItem<PlatinumLadyBugTile>("LadyBug");

public sealed class PlatinumMouseCageItem() : PlatinumCritterCageItem<PlatinumMouseTile>("Mouse");

public sealed class PlatinumSeahorseCageItem() : PlatinumCritterCageItem<PlatinumSeahorseTile>("Seahorse");

public sealed class PlatinumSquirrelCageItem() : PlatinumCritterCageItem<PlatinumSquirrelTile>("Squirrel");

public sealed class PlatinumWaterStriderCageItem() : PlatinumCritterCageItem<PlatinumWaterStriderTile>("WaterStrider");

public sealed class PlatinumWormCageItem() : PlatinumCritterCageItem<PlatinumWormTile>("Worm");