using System;

using Microsoft.Xna.Framework;

using MonoMod.Cil;
using Nightshade.Common.Utilities;

using Terraria;
using Terraria.GameContent.Biomes.Desert;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

internal sealed class LivingTreeGen : ModSystem
{
    public override void Load()
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Micro Biomes"], GenLivingTrees);

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

                c.EmitDelegate((int x, int y) => WorldGen.PlaceOasis(x, y) || PlaceBallCactus(x, y));
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
                while (Main.tile[x, y - 1].HasTile)
                {
                    y--;

                    if (y < description.Desert.Y)
                    {
                        break;
                    }
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

    private static void GenLivingTrees(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        GenCacti(progress);

        orig(self, progress, configuration);
    }

    private static void GenCacti(GenerationProgress progress)
    {
        progress.Message = Lang.gen[76].Value + ".. More Living Trees";

        LivingCactusCount = WorldGen.genRand.Next(1, 5) + WorldGen.GetWorldSize() * 2;

        if (WorldGen.drunkWorldGen)
        {
            LivingCactusCount *= 22;
        }

        int fallback = 0;
        float currentCactusCount = 0;

        LivingCactusBiome cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        while (currentCactusCount < LivingCactusCount && fallback < 20000)
        {
            cactus.Round = WorldGen.genRand.NextBool();

            if (cactus.Place(WorldGen.RandomRectanglePoint(GenVars.desertHiveLeft + 25, GenVars.desertHiveHigh + 100, GenVars.desertHiveRight - 25, GenVars.desertHiveLow - 50), GenVars.structures))
            {
                currentCactusCount++;
                progress.Set((float)currentCactusCount / LivingCactusCount);
            }

            fallback++;
        }

        // if (WorldGen.genRand.NextBool(20))

        int locationX = WorldGen.genRand.NextBool() ? GenVars.desertHiveRight : GenVars.desertHiveLeft;
        cactus.Place(new Point(locationX, GenVars.desertHiveHigh), GenVars.structures);
    }

    private static bool PlaceBallCactus(int x, int y)
    {
        // Let's not spawn too many.
        if (!WorldGen.genRand.NextBool(3) || y > GenVars.desertHiveHigh + 50)
        {
            return false;
        }

        float terrainSlope = NightshadeGenUtil.GetAverageSurfaceSlope(x, y, 15);
        if (Math.Abs(terrainSlope) > 0.2f)
        {
            return false;
        }

        var cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        cactus.Round = true;
        cactus.WithWater = false;
        return cactus.Place(new Point(x, y), GenVars.structures);
    }
}