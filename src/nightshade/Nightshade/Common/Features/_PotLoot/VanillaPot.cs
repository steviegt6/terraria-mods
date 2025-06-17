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

    public override bool ShouldTryForLoot(int i, int j, int style)
    {
        return !echo;
    }

    public override void ModifyTorchType(
        int i,
        int j,
        int style,
        Player player,
        ref int torchType,
        ref int glowstickType,
        ref int itemStack
    )
    {
        if (player.ZoneHallow)
        {
            itemStack += Main.rand.Next(2, 7);
            torchType = 4387;
        }
        else if (style is >= 22 and <= 24 || player.ZoneCrimson)
        {
            itemStack += Main.rand.Next(2, 7);
            torchType = 4386;
        }
        else if (style is >= 16 and <= 18 || player.ZoneCorrupt)
        {
            itemStack += Main.rand.Next(2, 7);
            torchType = 4385;
        }
        else if (style is >= 7 and <= 9)
        {
            itemStack += Main.rand.Next(2, 7);
            itemStack = (int)(itemStack * 1.5f);
            torchType = 4388;
        }
        else if (style is >= 4 and <= 6)
        {
            torchType = 974;
            glowstickType = 286;
        }
        else if (style is >= 34 and <= 36)
        {
            itemStack += Main.rand.Next(2, 7);
            torchType = 4383;
        }
        else if (player.ZoneGlowshroom)
        {
            itemStack += Main.rand.Next(2, 7);
            torchType = 5293;
        }
    }

    public override bool TryGetUtilityItem(
        int i,
        int j,
        int style,
        bool aboveUnderworldLayer,
        out int utilityType,
        out int utilityStack
    )
    {
        var isUndergroundDesertPot = style is >= 34 and <= 36;
        if (!isUndergroundDesertPot && !aboveUnderworldLayer)
        {
            utilityType = 0;
            utilityStack = 0;
            return false;
        }

        utilityType = 166;
        if (isUndergroundDesertPot)
        {
            utilityType = 4423;
        }

        utilityStack = Main.rand.Next(4) + 1;
        if (Main.expertMode)
        {
            utilityStack += Main.rand.Next(4);
        }
    }

    public override void ModifyCoinMultiplier(int i, int j, int style, ref float multiplier)
    {
        switch (style)
        {
            case 4:
            case 5:
            case 6:
                multiplier = 1.25f;
                break;

            default:
                switch (style)
                {
                    case >= 7 and <= 9:
                        multiplier = 1.75f;
                        break;

                    case >= 10 and <= 12:
                        multiplier = 1.9f;
                        break;

                    case >= 13 and <= 15:
                        multiplier = 2.1f;
                        break;

                    case >= 16 and <= 18:
                        multiplier = 1.6f;
                        break;

                    case >= 19 and <= 21:
                        multiplier = 3.5f;
                        break;

                    case >= 22 and <= 24:
                        multiplier = 1.6f;
                        break;

                    case >= 25 and <= 27:
                        multiplier = 10f;
                        break;

                    case >= 28 and <= 30:
                    {
                        if (Main.hardMode)
                        {
                            multiplier = 4f;
                        }
                        break;
                    }

                    case >= 31 and <= 33:
                        multiplier = 2f;
                        break;

                    case >= 34 and <= 36:
                        multiplier = 1.25f;
                        break;
                }
                break;

            case 0:
            case 1:
            case 2:
            case 3:
                break;
        }
    }
}