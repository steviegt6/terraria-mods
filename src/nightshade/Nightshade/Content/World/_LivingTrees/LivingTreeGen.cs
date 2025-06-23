using System;

using Microsoft.Xna.Framework;

using MonoMod.Cil;

using Nightshade.Common.Utilities;
using Nightshade.Content.World._LivingTrees;
using Terraria;
using Terraria.GameContent.Biomes.Desert;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

internal sealed class LivingTreeGen : ModSystem
{
    public override void Load()
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Living Trees"], GenBigLivingTrees);
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Micro Biomes"], GenSmallLivingTrees);

        // When attempting to place oases, place a ball cactus instead if it
        // fails.  These spawn outside of the desert (instead, in dunes), so it
        // adds some nice variety.
        WorldGen.ModifyPass(
            (PassLegacy)WorldGen.VanillaGenPasses["Oasis"],
            il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(MoveType.Before, x => x.MatchCall<WorldGen>(nameof(WorldGen.PlaceOasis)));
                c.Remove();

                c.EmitDelegate((int x, int y) => WorldGen.PlaceOasis(x, y) || PlaceSurfaceBallCactus_FromSpace(x));
            }
        );

        // Try to place ball cacti on the surface desert similarly to the desert
        // entrances.
        On_DesertHive.Place += (orig, description) =>
        {
            var cactusTryCount = WorldGen.genRand.Next(0, 2);
            var range = new Point(description.Desert.Left, description.Desert.Right);
            for (var i = 0; i < cactusTryCount;)
            {
                var x = WorldGen.genRand.Next(range.X, range.Y);
                var y = description.Surface[x];

                // if below ground, travel up
                int airThreshold = 5;
                while (Main.tile[x, y - 1].HasTile && airThreshold > 0)
                {
                    y--;
                    airThreshold--;
                }

                i++;

                var cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
                cactus.Round = true;
                cactus.WithWater = false;
                cactus.Place(new Point(x, y), GenVars.structures);
            }

            orig(description);
        };
    }

	private static int LivingCactusCount { get; set; }
	private static int LivingPalmCount { get; set; }
	private static int LivingBorealCount { get; set; }

    private static void GenSmallLivingTrees(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
	{
		orig(self, progress, configuration);

		progress.Message = Lang.gen[76].Value + ".. More Living Trees";

		GenCacti(progress);
        GenPalms(progress);
    }

    private static void GenCacti(GenerationProgress progress)
    {
        LivingCactusCount = WorldGen.genRand.Next(1, 5) + WorldGen.GetWorldSize() * 2;

        if (WorldGen.drunkWorldGen)
        {
            LivingCactusCount *= 22;
        }

        var fallback = 0;
        float currentCactusCount = 0;

        var cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        while (currentCactusCount < LivingCactusCount && fallback < 20000)
        {
            cactus.Round = WorldGen.genRand.NextBool();

            if (cactus.Place(WorldGen.RandomRectanglePoint(GenVars.desertHiveLeft + 25, GenVars.desertHiveHigh + 100, GenVars.desertHiveRight - 25, GenVars.desertHiveLow - 50), GenVars.structures))
            {
                currentCactusCount++;
                progress.Set(currentCactusCount / LivingCactusCount);
            }

            fallback++;
        }

        // if (WorldGen.genRand.NextBool(20))

        var locationX = WorldGen.genRand.NextBool() ? GenVars.desertHiveRight : GenVars.desertHiveLeft;
        cactus.Place(new Point(locationX, GenVars.desertHiveHigh), GenVars.structures);
    }

    private static bool PlaceSurfaceBallCactus_FromSpace(int x)
    {
	    for (var y = 0; y < Main.worldSurface; y++)
	    {
		    var tile = Main.tile[x, y];
		    if (!tile.HasTile)
		    {
			    continue;
		    }

		    if (Main.tile[x, y].TileType != TileID.Sand)
		    {
			    break;
		    }

		    return PlaceSurfaceBallCactus(x, y);
	    }

	    return false;
    }

    private static bool PlaceSurfaceBallCactus(int x, int y)
    {
		if (x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance || y > Main.worldSurface - 10)
			return false;

		// Let's not spawn too many.
		if (!WorldGen.genRand.NextBool(3) || y > GenVars.desertHiveHigh + 50)
			return false;

		var terrainSlope = NightshadeGenUtil.GetAverageSurfaceSlope(x, y, 15);
        if (Math.Abs(terrainSlope) > 0.15f)
			return false;

		var cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        cactus.Round = true;
        cactus.WithWater = false;
        return cactus.Place(new Point(x, y), GenVars.structures);
    }

    private static void GenPalms(GenerationProgress progress)
    {
        LivingPalmCount = (!WorldGen.genRand.NextBool(4)).ToInt() + WorldGen.genRand.NextBool().ToInt();

        if (Main.drunkWorld)
			LivingPalmCount = 10;

		float count = 0;
        int fallback = 20000;
        LivingPalmBiome palm = GenVars.configuration.CreateBiome<LivingPalmBiome>();

        bool flipSide = WorldGen.genRand.NextBool();
		while (count < LivingPalmCount && fallback > 0)
		{
            int direction = WorldGen.genRand.NextBool().ToDirectionInt();
			palm.StartCurl = WorldGen.genRand.NextFloat(0.2f) * -direction;
			palm.CurlStrength = WorldGen.genRand.NextFloat(0.5f, 1f) * direction;

            int left = flipSide ? Main.maxTilesX - WorldGen.beachDistance : (int)(WorldGen.beachDistance / 1.5f);
            int right = flipSide ? Main.maxTilesX - (int)(WorldGen.beachDistance / 1.5f) : WorldGen.beachDistance;

            if (palm.Place(new Point(WorldGen.genRand.Next(left, right), (int)(Main.worldSurface) - 300), GenVars.structures))
            {
                count++;
			    progress.Set(count / LivingPalmCount);
            }

            flipSide = !flipSide;
			fallback--;
        }
	}

    private static void GenBigLivingTrees(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
		orig(self, progress, configuration);

        LivingBorealCount = WorldGen.genRand.NextBool().ToInt();

		if (Main.drunkWorld)
			LivingBorealCount = 14;

		int fallback = 2000;
        int count = 0;

        LivingBorealBiome boreal = GenVars.configuration.CreateBiome<LivingBorealBiome>();

		while (count < LivingBorealCount && fallback > 0)
        {
            Point placePoint = new Point(
                WorldGen.genRand.Next(GenVars.snowOriginLeft + 60, GenVars.snowOriginRight - 60), 
                WorldGen.genRand.Next(GenVars.snowTop - 220, GenVars.snowTop - 180));

			if (boreal.Place(placePoint, GenVars.structures))
            {
                count++;
				progress.Set(count / LivingBorealCount);
			}

			fallback--;
		}
	}
}