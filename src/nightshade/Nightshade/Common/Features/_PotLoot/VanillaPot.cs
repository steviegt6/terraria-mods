using System;

using Microsoft.Xna.Framework;

using Terraria;
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

    public override void PlayBreakSound(int i, int j, int style)
    {
        switch (style)
        {
            case >= 7 and <= 9:
                SoundEngine.PlaySound(SoundID.Grass, i * 16, j * 16);
                break;

            case >= 16 and <= 24:
                SoundEngine.PlaySound(4, i * 16, j * 16);
                break;

            default:
                SoundEngine.PlaySound(SoundID.Shatter, i * 16, j * 16);
                break;
        }
    }

    public override void SpawnGore(int i, int j, int style)
    {
        switch (style)
        {
            case 0:
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 51);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 52);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 53);
                break;

            case 1:
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 166);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 167);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 168);
                break;

            case 2:
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 169);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 170);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 171);
                break;

            case 3:
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 172);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 173);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 174);
                break;

            case 4:
            case 5:
            case 6:
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 197);
                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 198);
                break;

            default:
                if (style is >= 7 and <= 9)
                {
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 199);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 200);
                }
                else if (style is >= 10 and <= 12)
                {
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 201);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 202);
                }
                else if (style is >= 13 and <= 15)
                {
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 203);
                    Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 204);
                }
                else
                {
                    if (style is >= 16 and <= 18 or >= 19 and <= 21 or >= 22 and <= 24)
                    {
                        break;
                    }

                    switch (style)
                    {
                        case >= 25 and <= 27:
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(217, 220));
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(217, 220));
                            break;

                        case >= 28 and <= 30:
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(315, 317));
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), WorldGen.genRand.Next(315, 317));
                            break;

                        case >= 31 and <= 33:
                        {
                            var num6 = WorldGen.genRand.Next(2, 5);
                            for (var num7 = 0; num7 < num6; num7++)
                            {
                                Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 698 + WorldGen.genRand.Next(6));
                            }
                            break;
                        }

                        case >= 34 and <= 36:
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 1122);
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 1123);
                            Gore.NewGore(new Vector2(i * 16, j * 16), default(Vector2), 1124);
                            break;
                    }
                }
                break;
        }
    }
}