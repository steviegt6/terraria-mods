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
        WorldGen.DetourPass((PassLegacy)WorldGen.VanillaGenPasses["Living Trees"], GenLivingTrees);
    }

    public override void PostAddRecipes()
    {
        if (ModLoader.TryGetMod("BlockarozToolkit", out Mod toolkit))
            toolkit.Call("NewDebug", Mod, "Enable tree generation", ItemID.Acorn, () =>
            {
                Point point = Main.MouseWorld.ToTileCoordinates();

                Dust d = Dust.NewDustPerfect(point.ToWorldCoordinates(), DustID.t_Cactus, Main.rand.NextVector2Circular(1, 1), Scale: 0.5f);
                d.fadeIn = 1.5f;
                d.noGravity = true;

                if (Main.mouseRight && Main.mouseRightRelease && !Main.mapFullscreen)
                {
                    LivingCactusGen.GenerateLivingCactus(point.X, point.Y);

                    for (int i = 0; i < 15; i++)
                    {
                        Dust e = Dust.NewDustPerfect(point.ToWorldCoordinates(), DustID.t_Cactus, Main.rand.NextVector2Circular(4, 4), Scale: 2f);
                        e.fadeIn = 2f;
                        e.noGravity = true;
                    }
                }
            }, true);
    }

    public static int LivingCactusCount { get; private set; }

    public static void GenLivingTrees(WorldGen.orig_GenPassDetour orig, object self, GenerationProgress progress, GameConfiguration configuration)
    {
        LivingCactusCount = WorldGen.genRand.Next(3, 8);

        orig(self, progress, configuration);
    }
}
