using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Nightshade.Content;

public abstract class PlatinumCritterTile<TCageItem>(string critterName) : ModTile
    where TCageItem : ModItem
{
    private sealed class FakeEntity : Entity;
    
    public override string Texture => MakeTexturePath(critterName);

    private int TileType => TileID.Search.GetId(PlatCritterHelpers.GetGoldName(critterName + "Cage"));

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.tileFrameImportant[Type] = true;

        var sourceData = TileObjectData.GetTileData(TileType, 0);
        TileObjectData.newTile.CopyFrom(sourceData);
        TileObjectData.addTile(Type);

        On_TileDrawing.GetTileDrawData += GetTileDrawData_StealDataOfVanillaTile;
    }

    private SpriteBatchSnapshot ss;

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        spriteBatch.End(out ss);
        spriteBatch.Begin(
            ss with { SortMode = SpriteSortMode.Immediate }
        );

        GameShaders.Armor.GetShaderFromItemId(ModContent.ItemType<ReflectivePlatinumDyeItem>())
                   .Apply(new FakeEntity
                    {
                        width = 16,
                        height = 16,
                        position = new Vector2(i, j),
                        direction = 0,
                    }, new DrawData(TextureAssets.Tile[Type].Value, new Vector2(i, j), Color.White));

        return base.PreDraw(i, j, spriteBatch);
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Restart(in ss);

        base.PostDraw(i, j, spriteBatch);
    }

    private void GetTileDrawData_StealDataOfVanillaTile(
        On_TileDrawing.orig_GetTileDrawData orig,
        TileDrawing self,
        int x,
        int y,
        Tile tileCache,
        ushort typeCache,
        ref short tileFrameX,
        ref short tileFrameY,
        out int tileWidth,
        out int tileHeight,
        out int tileTop,
        out int halfBrickHeight,
        out int addFrX,
        out int addFrY,
        out SpriteEffects tileSpriteEffect,
        out Texture2D glowTexture,
        out Rectangle glowSourceRect,
        out Color glowColor
    )
    {
        if (typeCache == Type)
        {
            typeCache = (ushort)TileType;
        }

        orig(
            self,
            x,
            y,
            tileCache,
            typeCache,
            ref tileFrameX,
            ref tileFrameY,
            out tileWidth,
            out tileHeight,
            out tileTop,
            out halfBrickHeight,
            out addFrX,
            out addFrY,
            out tileSpriteEffect,
            out glowTexture,
            out glowSourceRect,
            out glowColor
        );
    }

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