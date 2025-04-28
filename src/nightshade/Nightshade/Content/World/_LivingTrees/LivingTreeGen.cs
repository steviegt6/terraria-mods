using Microsoft.Xna.Framework;

using MonoMod.Cil;
using Nightshade.Content.Items;
using Terraria;
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
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Micro Biomes"], GenLivingTrees);

        WorldGen.ModifyPass(
            (PassLegacy)WorldGen.VanillaGenPasses["Oasis"],
            il =>
            {
                // Basically just run our code if PlaceOasis fails.

                var c = new ILCursor(il);

                c.GotoNext(MoveType.Before, x => x.MatchCall<WorldGen>(nameof(WorldGen.PlaceOasis)));
                c.Remove();

                c.EmitDelegate((int x, int y) => WorldGen.PlaceOasis(x, y) || PlaceBallCactus(x, y));
            }
        );
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
        if (WorldGen.genRand.NextBool())
        {
            return false;
        }
        
        
        
        var cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        cactus.Round = true;

        return cactus.Place(new Point(x, y), GenVars.structures);
    }
}