using Microsoft.Xna.Framework;
using Nightshade.Content.Items;
using Nightshade.Content.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

public class LivingTreeGen : ModSystem
{
    public override void Load()
    {
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Micro Biomes"], GenLivingTrees);
    }

    public static int LivingCactusCount { get; private set; }

    public static void GenLivingTrees(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        GenCactus(progress, configuration);

        orig(self, progress, configuration);
    }

    private static void GenCactus(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = Lang.gen[76].Value + ".. More Living Trees";

        LivingCactusCount = WorldGen.genRand.Next(1, 5) + WorldGen.GetWorldSize() * 2;

        if (WorldGen.drunkWorldGen)
            LivingCactusCount *= 20;

        int fallback = 0;
        int currentCactusCount = 0;

        LivingCactusBiome cactus = GenVars.configuration.CreateBiome<LivingCactusBiome>();
        while (currentCactusCount < LivingCactusCount && fallback < 20000)
        {
            if (cactus.Place(WorldGen.RandomRectanglePoint(GenVars.desertHiveLeft + 25, GenVars.desertHiveHigh + 100, GenVars.desertHiveRight - 25, GenVars.desertHiveLow - 50), GenVars.structures))
            {
                currentCactusCount++;
                progress.Set(currentCactusCount / LivingCactusCount);
            }

            fallback++;
        }

        if (true)//WorldGen.genRand.NextBool(20))
        {
            int locationX = WorldGen.genRand.NextBool() ? GenVars.desertHiveRight : GenVars.desertHiveLeft;
            cactus.Place(new Point(locationX, GenVars.desertHiveHigh), GenVars.structures);
        }
    }
}
