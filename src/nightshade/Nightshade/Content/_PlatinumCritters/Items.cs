using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
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

    public override bool? CanConsumeBait(Player player)
    {
        return Item.bait > 0 ? false : base.CanConsumeBait(player);
    }

    private SpriteBatchSnapshot worldSs;

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        spriteBatch.End(out worldSs);
        spriteBatch.Begin(
            worldSs with { SortMode = SpriteSortMode.Immediate }
        );

        GameShaders.Armor.GetShaderFromItemId(ModContent.ItemType<ReflectivePlatinumDyeItem>())
                   .Apply(Item, new DrawData(TextureAssets.Item[Item.type].Value, Item.position, alphaColor));

        return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Restart(in worldSs);

        base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
    }

    private SpriteBatchSnapshot inventorySs;

    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.End(out inventorySs);
        spriteBatch.Begin(
            inventorySs with { SortMode = SpriteSortMode.Immediate }
        );

        GameShaders.Armor.GetShaderFromItemId(ModContent.ItemType<ReflectivePlatinumDyeItem>())
                   .Apply(null, new DrawData(TextureAssets.Item[Item.type].Value, Item.position, itemColor));

        return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Restart(in inventorySs);

        base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
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