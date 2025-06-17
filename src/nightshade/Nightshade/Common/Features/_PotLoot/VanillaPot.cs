using System;
using System.Diagnostics.CodeAnalysis;

using Terraria.Audio;
using Terraria.ID;

namespace Nightshade.Common.Features;

/// <summary>
///     Provides implementations for vanilla pot types.
/// </summary>
internal sealed class VanillaPot(bool echo) : CustomPot
{
    public const int POT_0_FOREST = 0;
    public const int POT_1_FOREST = 1;
    public const int POT_2_FOREST = 2;
    public const int POT_3_FOREST = 3;

    public const int POT_4_TUNDRA = 4;
    public const int POT_5_TUNDRA = 5;
    public const int POT_6_TUNDRA = 6;

    public const int POT_7_JUNGLE = 7;
    public const int POT_8_JUNGLE = 8;
    public const int POT_9_JUNGLE = 9;

    public const int POT_10_DUNGEON = 10;
    public const int POT_11_DUNGEON = 11;
    public const int POT_12_DUNGEON = 12;

    public const int POT_13_UNDERWORLD = 13;
    public const int POT_14_UNDERWORLD = 14;
    public const int POT_15_UNDERWORLD = 15;

    public const int POT_16_CORRUPTION = 16;
    public const int POT_17_CORRUPTION = 17;
    public const int POT_18_CORRUPTION = 18;

    public const int POT_19_SPIDER_CAVE = 19;
    public const int POT_20_SPIDER_CAVE = 20;
    public const int POT_21_SPIDER_CAVE = 21;

    public const int POT_22_CRIMSON = 22;
    public const int POT_23_CRIMSON = 23;
    public const int POT_24_CRIMSON = 24;

    public const int POT_25_PYRAMID = 25;
    public const int POT_26_PYRAMID = 26;
    public const int POT_27_PYRAMID = 27;

    public const int POT_28_LIHZAHRD = 28;
    public const int POT_29_LIHZAHRD = 29;
    public const int POT_30_LIHZAHRD = 30;

    public const int POT_31_MARBLE = 31;
    public const int POT_32_MARBLE = 32;
    public const int POT_33_MARBLE = 33;

    public const int POT_34_UNDERGROUND_DESERT = 34;
    public const int POT_35_UNDERGROUND_DESERT = 35;
    public const int POT_36_UNDERGROUND_DESERT = 36;

    public override void PlayBreakSound()
    {
                
        if (style is >= 7 and <= 9)
        {
            SoundEngine.PlaySound(SoundID.Grass, i * 16, j * 16);
        }
        else if (styleIndex is >= 16 and <= 24)
        {
            SoundEngine.PlaySound(4, i * 16, j * 16);
        }
        else
        {
            SoundEngine.PlaySound(SoundID.Shatter, i * 16, j * 16);
        }
    }

    public override void SpawnGore()
    {
        throw new NotImplementedException();
    }

    public static bool IsVanillaPot(int type)
    {
        return type is TileID.Pots or TileID.PotsEcho;
    }

    public static bool TryGetVanillaPot(
        int type,
        [NotNullWhen(returnValue: true)] out CustomPot? pot
    )
    {
        // Originally, I was going to make it so, when providing a pot style to
        // the rewritten method handling pot loot, vanilla styles would be
        // denoted by negative values (stuck using the same parameter).  I
        // realized after that I can just not do that since vanilla pot styles
        // would only overlap existing vanilla tile IDs, so I can assume in good
        // faith when a pot style is vanilla or actually a new modded tile ID.
        /*if (type < 0)
        {
            type = Math.Abs(type);
        }*/

        if (!IsWithinPotBounds(type))
        {
            pot = null;
            return false;
        }

        pot = new VanillaPot(type);
        return true;
    }
    
    private static bool IsWithinPotBounds(int type)
    {
        return type is >= POT_0_FOREST and <= POT_36_UNDERGROUND_DESERT;
    }
}