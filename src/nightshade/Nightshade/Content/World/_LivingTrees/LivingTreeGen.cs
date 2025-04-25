using Microsoft.Xna.Framework;

using MonoMod.Cil;

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
        GenCactus(progress);

        orig(self, progress, configuration);
    }

    private static void GenCactus(GenerationProgress progress)
    {
        progress.Message = Lang.gen[76].Value + ".. More Living Trees";

        LivingCactusCount = WorldGen.genRand.Next(1, 5) + WorldGen.GetWorldSize() * 2;

        if (WorldGen.drunkWorldGen)
        {
            LivingCactusCount *= 20;
        }

        var fallback = 0;
        var currentCactusCount = 0;

        var cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        while (currentCactusCount < LivingCactusCount && fallback < 20000)
        {
            if (cactus.Place(WorldGen.RandomRectanglePoint(GenVars.desertHiveLeft + 25, GenVars.desertHiveHigh + 100, GenVars.desertHiveRight - 25, GenVars.desertHiveLow - 50), GenVars.structures))
            {
                currentCactusCount++;
                progress.Set((float)currentCactusCount / LivingCactusCount);
            }

            fallback++;
        }

        // if (WorldGen.genRand.NextBool(20))

        var locationX = WorldGen.genRand.NextBool() ? GenVars.desertHiveRight : GenVars.desertHiveLeft;
        cactus.Place(new Point(locationX, GenVars.desertHiveHigh), GenVars.structures);
    }

    private static bool PlaceBallCactus(int x, int y)
    {
        WorldGen.PlaceTile(x, y, TileID.DiamondGemspark);
        
        // test
        return true;
    }
}