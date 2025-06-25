using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
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

        Item.CloneDefaults(ItemID.Search.GetId(PlatCritterHelpers.GetGoldName(critterName + "Cage")));
        Item.createTile = ModContent.TileType<TTile>();
        Item.value = Item.sellPrice(platinum: 1);
        Item.rare = ItemRarityID.LightRed;
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

    public override void SetDefaults()
    {
        var origHead = Item.headSlot;
        base.SetDefaults();
        Item.headSlot = origHead;
    }
}

public sealed class PlatinumGrasshopperCageItem() : PlatinumCritterCageItem<PlatinumGrasshopperTile>("Grasshopper");

public sealed class PlatinumLadyBugCageItem() : PlatinumCritterCageItem<PlatinumLadyBugTile>("LadyBug");

public sealed class PlatinumMouseCageItem() : PlatinumCritterCageItem<PlatinumMouseTile>("Mouse");

public sealed class PlatinumSeahorseCageItem() : PlatinumCritterCageItem<PlatinumSeahorseTile>("Seahorse");

public sealed class PlatinumSquirrelCageItem() : PlatinumCritterCageItem<PlatinumSquirrelTile>("Squirrel");

public sealed class PlatinumWaterStriderCageItem() : PlatinumCritterCageItem<PlatinumWaterStriderTile>("WaterStrider");

public sealed class PlatinumWormCageItem() : PlatinumCritterCageItem<PlatinumWormTile>("Worm");