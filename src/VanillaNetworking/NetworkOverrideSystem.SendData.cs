using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;

namespace Tomat.Terraria.TML.VanillaNetworking;

partial class NetworkOverrideSystem
{
    private static void NetMessage_SendData(
        On_NetMessage.orig_SendData orig,
        int                         msgType,
        int                         remoteClient,
        int                         ignoreClient,
        NetworkText?                text,
        int                         number,
        float                       number2,
        float                       number3,
        float                       number4,
        int                         number5,
        int                         number6,
        int                         number7
    )
    {
        if (Main.netMode == 0)
        {
            return;
        }
        if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
        {
            msgType = 145;
        }
        int num = 256;
        if (text == null)
        {
            text = NetworkText.Empty;
        }
        if (Main.netMode == 2 && remoteClient >= 0)
        {
            num = remoteClient;
        }
        if (ModNet.HijackSendData(num, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7))
        {
            return;
        }
        lock (NetMessage.buffer[num])
        {
            BinaryWriter writer = NetMessage.buffer[num].writer;
            if (writer == null)
            {
                NetMessage.buffer[num].ResetWriter();
                writer = NetMessage.buffer[num].writer;
            }
            writer.BaseStream.Position = 0L;
            long position = writer.BaseStream.Position;
            writer.BaseStream.Position += 2L;
            writer.Write((byte)msgType);
            switch (msgType)
            {
                case 1:
                    // writer.Write(ModNet.NetVersionString);
                    writer.Write("Terraria" + 279);
                    break;

                case 2:
                    text.Serialize(writer);
                    if (Main.dedServ)
                    {
                        Logging.ServerConsoleLine(Language.GetTextValue("CLI.ClientWasBooted", Netplay.Clients[num].Socket.GetRemoteAddress().ToString(), text));
                    }
                    break;

                case 3:
                    writer.Write((byte)remoteClient);
                    writer.Write(value: false);
                    break;

                case 4:
                {
                    Player player4 = Main.player[number];
                    writer.Write((byte)number);
                    writer.Write((byte)player4.skinVariant);
                    writer.Write((byte)player4.hair);
                    writer.Write(player4.name);
                    // writer.Write7BitEncodedInt(player4.hairDye);
                    writer.Write(player4.hairDye);
                    NetMessage.WriteAccessoryVisibility(writer, player4.hideVisibleAccessory);
                    writer.Write(player4.hideMisc);
                    writer.WriteRGB(player4.hairColor);
                    writer.WriteRGB(player4.skinColor);
                    writer.WriteRGB(player4.eyeColor);
                    writer.WriteRGB(player4.shirtColor);
                    writer.WriteRGB(player4.underShirtColor);
                    writer.WriteRGB(player4.pantsColor);
                    writer.WriteRGB(player4.shoeColor);
                    BitsByte bitsByte16 = (byte)0;
                    if (player4.difficulty == 1)
                    {
                        bitsByte16[0] = true;
                    }
                    else if (player4.difficulty == 2)
                    {
                        bitsByte16[1] = true;
                    }
                    else if (player4.difficulty == 3)
                    {
                        bitsByte16[3] = true;
                    }
                    bitsByte16[2] = player4.extraAccessory;
                    writer.Write(bitsByte16);
                    BitsByte bitsByte17 = (byte)0;
                    bitsByte17[0] = player4.UsingBiomeTorches;
                    bitsByte17[1] = player4.happyFunTorchTime;
                    bitsByte17[2] = player4.unlockedBiomeTorches;
                    bitsByte17[3] = player4.unlockedSuperCart;
                    bitsByte17[4] = player4.enabledSuperCart;
                    writer.Write(bitsByte17);
                    BitsByte bitsByte18 = (byte)0;
                    bitsByte18[0] = player4.usedAegisCrystal;
                    bitsByte18[1] = player4.usedAegisFruit;
                    bitsByte18[2] = player4.usedArcaneCrystal;
                    bitsByte18[3] = player4.usedGalaxyPearl;
                    bitsByte18[4] = player4.usedGummyWorm;
                    bitsByte18[5] = player4.usedAmbrosia;
                    bitsByte18[6] = player4.ateArtisanBread;
                    writer.Write(bitsByte18);
                    break;
                }

                case 5:
                {
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    Player player5 = Main.player[number];
                    Item   item6   = null;
                    item6 = ((number2 >= (float)PlayerItemSlotID.Loadout3_Dye_0)
                        ? player5.Loadouts[2].Dye[(int)number2 - PlayerItemSlotID.Loadout3_Dye_0]
                        : ((number2 >= (float)PlayerItemSlotID.Loadout3_Armor_0)
                            ? player5.Loadouts[2].Armor[(int)number2 - PlayerItemSlotID.Loadout3_Armor_0]
                            : ((number2 >= (float)PlayerItemSlotID.Loadout2_Dye_0)
                                ? player5.Loadouts[1].Dye[(int)number2 - PlayerItemSlotID.Loadout2_Dye_0]
                                : ((number2 >= (float)PlayerItemSlotID.Loadout2_Armor_0)
                                    ? player5.Loadouts[1].Armor[(int)number2 - PlayerItemSlotID.Loadout2_Armor_0]
                                    : ((number2 >= (float)PlayerItemSlotID.Loadout1_Dye_0)
                                        ? player5.Loadouts[0].Dye[(int)number2 - PlayerItemSlotID.Loadout1_Dye_0]
                                        : ((number2 >= (float)PlayerItemSlotID.Loadout1_Armor_0)
                                            ? player5.Loadouts[0].Armor[(int)number2 - PlayerItemSlotID.Loadout1_Armor_0]
                                            : ((number2 >= (float)PlayerItemSlotID.Bank4_0)
                                                ? player5.bank4.item[(int)number2 - PlayerItemSlotID.Bank4_0]
                                                : ((number2 >= (float)PlayerItemSlotID.Bank3_0) ? player5.bank3.item[(int)number2 - PlayerItemSlotID.Bank3_0] : ((number2 >= (float)PlayerItemSlotID.TrashItem) ? player5.trashItem : ((number2 >= (float)PlayerItemSlotID.Bank2_0) ? player5.bank2.item[(int)number2 - PlayerItemSlotID.Bank2_0] : ((number2 >= (float)PlayerItemSlotID.Bank1_0) ? player5.bank.item[(int)number2 - PlayerItemSlotID.Bank1_0] : ((number2 >= (float)PlayerItemSlotID.MiscDye0) ? player5.miscDyes[(int)number2 - PlayerItemSlotID.MiscDye0] : ((number2 >= (float)PlayerItemSlotID.Misc0) ? player5.miscEquips[(int)number2 - PlayerItemSlotID.Misc0] : ((number2 >= (float)PlayerItemSlotID.Dye0) ? player5.dye[(int)number2 - PlayerItemSlotID.Dye0] : ((!(number2 >= (float)PlayerItemSlotID.Armor0)) ? player5.inventory[(int)number2 - PlayerItemSlotID.Inventory0] : player5.armor[(int)number2 - PlayerItemSlotID.Armor0])))))))))))))));
                    if (item6.Name == "" || item6.stack == 0 || item6.type == 0)
                    {
                        item6.SetDefaults(0, noMatCheck: true);
                    }
                    _ = item6.stack;
                    _ = item6.netID;
                    _ = 0;
                    // ItemIO.Send(item6, writer, writeStack: true);
                    var stack = item6.stack > 0 ? 0 : item6.stack;
                    var netId = item6.netID;
                    writer.Write((short)stack);
                    writer.Write((byte)number3);
                    writer.Write((short)netId);
                    break;
                }

                case 7:
                {
                    writer.Write((int)Main.time);
                    BitsByte bitsByte5 = (byte)0;
                    bitsByte5[0] = Main.dayTime;
                    bitsByte5[1] = Main.bloodMoon;
                    bitsByte5[2] = Main.eclipse;
                    writer.Write(bitsByte5);
                    writer.Write((byte)Main.moonPhase);
                    writer.Write((short)Main.maxTilesX);
                    writer.Write((short)Main.maxTilesY);
                    writer.Write((short)Main.spawnTileX);
                    writer.Write((short)Main.spawnTileY);
                    writer.Write((short)Main.worldSurface);
                    writer.Write((short)Main.rockLayer);
                    writer.Write(Main.worldID);
                    writer.Write(Main.worldName);
                    writer.Write((byte)Main.GameMode);
                    writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
                    writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
                    writer.Write((byte)Main.moonType);
                    writer.Write((byte)WorldGen.treeBG1);
                    writer.Write((byte)WorldGen.treeBG2);
                    writer.Write((byte)WorldGen.treeBG3);
                    writer.Write((byte)WorldGen.treeBG4);
                    writer.Write((byte)WorldGen.corruptBG);
                    writer.Write((byte)WorldGen.jungleBG);
                    writer.Write((byte)WorldGen.snowBG);
                    writer.Write((byte)WorldGen.hallowBG);
                    writer.Write((byte)WorldGen.crimsonBG);
                    writer.Write((byte)WorldGen.desertBG);
                    writer.Write((byte)WorldGen.oceanBG);
                    writer.Write((byte)WorldGen.mushroomBG);
                    writer.Write((byte)WorldGen.underworldBG);
                    writer.Write((byte)Main.iceBackStyle);
                    writer.Write((byte)Main.jungleBackStyle);
                    writer.Write((byte)Main.hellBackStyle);
                    writer.Write(Main.windSpeedTarget);
                    writer.Write((byte)Main.numClouds);
                    for (int n = 0; n < 3; n++)
                    {
                        writer.Write(Main.treeX[n]);
                    }
                    for (int num8 = 0; num8 < 4; num8++)
                    {
                        writer.Write((byte)Main.treeStyle[num8]);
                    }
                    for (int num9 = 0; num9 < 3; num9++)
                    {
                        writer.Write(Main.caveBackX[num9]);
                    }
                    for (int num10 = 0; num10 < 4; num10++)
                    {
                        writer.Write((byte)Main.caveBackStyle[num10]);
                    }
                    WorldGen.TreeTops.SyncSend(writer);
                    if (!Main.raining)
                    {
                        Main.maxRaining = 0f;
                    }
                    writer.Write(Main.maxRaining);
                    BitsByte bitsByte6 = (byte)0;
                    bitsByte6[0] = WorldGen.shadowOrbSmashed;
                    bitsByte6[1] = NPC.downedBoss1;
                    bitsByte6[2] = NPC.downedBoss2;
                    bitsByte6[3] = NPC.downedBoss3;
                    bitsByte6[4] = Main.hardMode;
                    bitsByte6[5] = NPC.downedClown;
                    bitsByte6[7] = NPC.downedPlantBoss;
                    writer.Write(bitsByte6);
                    BitsByte bitsByte7 = (byte)0;
                    bitsByte7[0] = NPC.downedMechBoss1;
                    bitsByte7[1] = NPC.downedMechBoss2;
                    bitsByte7[2] = NPC.downedMechBoss3;
                    bitsByte7[3] = NPC.downedMechBossAny;
                    bitsByte7[4] = Main.cloudBGActive >= 1f;
                    bitsByte7[5] = WorldGen.crimson;
                    bitsByte7[6] = Main.pumpkinMoon;
                    bitsByte7[7] = Main.snowMoon;
                    writer.Write(bitsByte7);
                    BitsByte bitsByte8 = (byte)0;
                    bitsByte8[1] = Main.fastForwardTimeToDawn;
                    bitsByte8[2] = Main.slimeRain;
                    bitsByte8[3] = NPC.downedSlimeKing;
                    bitsByte8[4] = NPC.downedQueenBee;
                    bitsByte8[5] = NPC.downedFishron;
                    bitsByte8[6] = NPC.downedMartians;
                    bitsByte8[7] = NPC.downedAncientCultist;
                    writer.Write(bitsByte8);
                    BitsByte bitsByte9 = (byte)0;
                    bitsByte9[0] = NPC.downedMoonlord;
                    bitsByte9[1] = NPC.downedHalloweenKing;
                    bitsByte9[2] = NPC.downedHalloweenTree;
                    bitsByte9[3] = NPC.downedChristmasIceQueen;
                    bitsByte9[4] = NPC.downedChristmasSantank;
                    bitsByte9[5] = NPC.downedChristmasTree;
                    bitsByte9[6] = NPC.downedGolemBoss;
                    bitsByte9[7] = BirthdayParty.PartyIsUp;
                    writer.Write(bitsByte9);
                    BitsByte bitsByte10 = (byte)0;
                    bitsByte10[0] = NPC.downedPirates;
                    bitsByte10[1] = NPC.downedFrost;
                    bitsByte10[2] = NPC.downedGoblins;
                    bitsByte10[3] = Sandstorm.Happening;
                    bitsByte10[4] = DD2Event.Ongoing;
                    bitsByte10[5] = DD2Event.DownedInvasionT1;
                    bitsByte10[6] = DD2Event.DownedInvasionT2;
                    bitsByte10[7] = DD2Event.DownedInvasionT3;
                    writer.Write(bitsByte10);
                    BitsByte bitsByte11 = (byte)0;
                    bitsByte11[0] = NPC.combatBookWasUsed;
                    bitsByte11[1] = LanternNight.LanternsUp;
                    bitsByte11[2] = NPC.downedTowerSolar;
                    bitsByte11[3] = NPC.downedTowerVortex;
                    bitsByte11[4] = NPC.downedTowerNebula;
                    bitsByte11[5] = NPC.downedTowerStardust;
                    bitsByte11[6] = Main.forceHalloweenForToday;
                    bitsByte11[7] = Main.forceXMasForToday;
                    writer.Write(bitsByte11);
                    BitsByte bitsByte12 = (byte)0;
                    bitsByte12[0] = NPC.boughtCat;
                    bitsByte12[1] = NPC.boughtDog;
                    bitsByte12[2] = NPC.boughtBunny;
                    bitsByte12[3] = NPC.freeCake;
                    bitsByte12[4] = Main.drunkWorld;
                    bitsByte12[5] = NPC.downedEmpressOfLight;
                    bitsByte12[6] = NPC.downedQueenSlime;
                    bitsByte12[7] = Main.getGoodWorld;
                    writer.Write(bitsByte12);
                    BitsByte bitsByte13 = (byte)0;
                    bitsByte13[0] = Main.tenthAnniversaryWorld;
                    bitsByte13[1] = Main.dontStarveWorld;
                    bitsByte13[2] = NPC.downedDeerclops;
                    bitsByte13[3] = Main.notTheBeesWorld;
                    bitsByte13[4] = Main.remixWorld;
                    bitsByte13[5] = NPC.unlockedSlimeBlueSpawn;
                    bitsByte13[6] = NPC.combatBookVolumeTwoWasUsed;
                    bitsByte13[7] = NPC.peddlersSatchelWasUsed;
                    writer.Write(bitsByte13);
                    BitsByte bitsByte14 = (byte)0;
                    bitsByte14[0] = NPC.unlockedSlimeGreenSpawn;
                    bitsByte14[1] = NPC.unlockedSlimeOldSpawn;
                    bitsByte14[2] = NPC.unlockedSlimePurpleSpawn;
                    bitsByte14[3] = NPC.unlockedSlimeRainbowSpawn;
                    bitsByte14[4] = NPC.unlockedSlimeRedSpawn;
                    bitsByte14[5] = NPC.unlockedSlimeYellowSpawn;
                    bitsByte14[6] = NPC.unlockedSlimeCopperSpawn;
                    bitsByte14[7] = Main.fastForwardTimeToDusk;
                    writer.Write(bitsByte14);
                    BitsByte bitsByte15 = (byte)0;
                    bitsByte15[0] = Main.noTrapsWorld;
                    bitsByte15[1] = Main.zenithWorld;
                    bitsByte15[2] = NPC.unlockedTruffleSpawn;
                    writer.Write(bitsByte15);
                    writer.Write((byte)Main.sundialCooldown);
                    writer.Write((byte)Main.moondialCooldown);
                    writer.Write((short)WorldGen.SavedOreTiers.Copper);
                    writer.Write((short)WorldGen.SavedOreTiers.Iron);
                    writer.Write((short)WorldGen.SavedOreTiers.Silver);
                    writer.Write((short)WorldGen.SavedOreTiers.Gold);
                    writer.Write((short)WorldGen.SavedOreTiers.Cobalt);
                    writer.Write((short)WorldGen.SavedOreTiers.Mythril);
                    writer.Write((short)WorldGen.SavedOreTiers.Adamantite);
                    writer.Write((sbyte)Main.invasionType);
                    if (SocialAPI.Network != null)
                    {
                        writer.Write(SocialAPI.Network.GetLobbyId());
                    }
                    else
                    {
                        writer.Write(0uL);
                    }
                    writer.Write(Sandstorm.IntendedSeverity);
                    WorldIO.SendModData(writer);
                    break;
                }

                case 8:
                    writer.Write(number);
                    writer.Write((int)number2);
                    break;

                case 9:
                {
                    writer.Write(number);
                    text.Serialize(writer);
                    BitsByte bitsByte22 = (byte)number2;
                    writer.Write(bitsByte22);
                    break;
                }

                case 10:
                    NetMessage.CompressTileBlock(number, (int)number2, (short)number3, (short)number4, writer.BaseStream);
                    break;

                case 11:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    break;

                case 12:
                {
                    Player player6 = Main.player[number];
                    writer.Write((byte)number);
                    writer.Write((short)player6.SpawnX);
                    writer.Write((short)player6.SpawnY);
                    writer.Write(player6.respawnTimer);
                    writer.Write((short)player6.numberOfDeathsPVE);
                    writer.Write((short)player6.numberOfDeathsPVP);
                    writer.Write((byte)number2);
                    break;
                }

                case 13:
                {
                    Player player7 = Main.player[number];
                    writer.Write((byte)number);
                    BitsByte bitsByte25 = (byte)0;
                    bitsByte25[0] = player7.controlUp;
                    bitsByte25[1] = player7.controlDown;
                    bitsByte25[2] = player7.controlLeft;
                    bitsByte25[3] = player7.controlRight;
                    bitsByte25[4] = player7.controlJump;
                    bitsByte25[5] = player7.controlUseItem;
                    bitsByte25[6] = player7.direction == 1;
                    writer.Write(bitsByte25);
                    BitsByte bitsByte26 = (byte)0;
                    bitsByte26[0] = player7.pulley;
                    bitsByte26[1] = player7.pulley && player7.pulleyDir == 2;
                    bitsByte26[2] = player7.velocity != Vector2.Zero;
                    bitsByte26[3] = player7.vortexStealthActive;
                    bitsByte26[4] = player7.gravDir == 1f;
                    bitsByte26[5] = player7.shieldRaised;
                    bitsByte26[6] = player7.ghost;
                    writer.Write(bitsByte26);
                    BitsByte bitsByte27 = (byte)0;
                    bitsByte27[0] = player7.tryKeepingHoveringUp;
                    bitsByte27[1] = player7.IsVoidVaultEnabled;
                    bitsByte27[2] = player7.sitting.isSitting;
                    bitsByte27[3] = player7.downedDD2EventAnyDifficulty;
                    bitsByte27[4] = player7.isPettingAnimal;
                    bitsByte27[5] = player7.isTheAnimalBeingPetSmall;
                    bitsByte27[6] = player7.PotionOfReturnOriginalUsePosition.HasValue;
                    bitsByte27[7] = player7.tryKeepingHoveringDown;
                    writer.Write(bitsByte27);
                    BitsByte bitsByte28 = (byte)0;
                    bitsByte28[0] = player7.sleeping.isSleeping;
                    bitsByte28[1] = player7.autoReuseAllWeapons;
                    bitsByte28[2] = player7.controlDownHold;
                    bitsByte28[3] = player7.isOperatingAnotherEntity;
                    bitsByte28[4] = player7.controlUseTile;
                    writer.Write(bitsByte28);
                    writer.Write((byte)player7.selectedItem);
                    writer.WriteVector2(player7.position);
                    if (bitsByte26[2])
                    {
                        writer.WriteVector2(player7.velocity);
                    }
                    if (bitsByte27[6])
                    {
                        writer.WriteVector2(player7.PotionOfReturnOriginalUsePosition.Value);
                        writer.WriteVector2(player7.PotionOfReturnHomePosition.Value);
                    }
                    break;
                }

                case 14:
                    writer.Write((byte)number);
                    writer.Write((byte)number2);
                    break;

                case 16:
                    writer.Write((byte)number);
                    writer.Write((short)Main.player[number].statLife);
                    writer.Write((short)Main.player[number].statLifeMax);
                    break;

                case 17:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    writer.Write((byte)number5);
                    break;

                case 18:
                    writer.Write(Main.dayTime ? ((byte)1) : ((byte)0));
                    writer.Write((int)Main.time);
                    writer.Write(Main.sunModY);
                    writer.Write(Main.moonModY);
                    break;

                case 19:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((number4 == 1f) ? ((byte)1) : ((byte)0));
                    break;

                case 20:
                {
                    int num13 = number;
                    int num14 = (int)number2;
                    int num15 = (int)number3;
                    if (num15 < 0)
                    {
                        num15 = 0;
                    }
                    int num16 = (int)number4;
                    if (num16 < 0)
                    {
                        num16 = 0;
                    }
                    if (num13 < num15)
                    {
                        num13 = num15;
                    }
                    if (num13 >= Main.maxTilesX + num15)
                    {
                        num13 = Main.maxTilesX - num15 - 1;
                    }
                    if (num14 < num16)
                    {
                        num14 = num16;
                    }
                    if (num14 >= Main.maxTilesY + num16)
                    {
                        num14 = Main.maxTilesY - num16 - 1;
                    }
                    writer.Write((short)num13);
                    writer.Write((short)num14);
                    writer.Write((byte)num15);
                    writer.Write((byte)num16);
                    writer.Write((byte)number5);
                    for (int num17 = num13; num17 < num13 + num15; num17++)
                    {
                        for (int num18 = num14; num18 < num14 + num16; num18++)
                        {
                            BitsByte bitsByte19 = (byte)0;
                            BitsByte bitsByte20 = (byte)0;
                            BitsByte bitsByte21 = (byte)0;
                            byte     b3         = 0;
                            byte     b4         = 0;
                            Tile     tile2      = Main.tile[num17, num18];
                            bitsByte19[0] = tile2.active();
                            bitsByte19[2] = tile2.wall > 0;
                            bitsByte19[3] = tile2.liquid > 0 && Main.netMode == 2;
                            bitsByte19[4] = tile2.wire();
                            bitsByte19[5] = tile2.halfBrick();
                            bitsByte19[6] = tile2.actuator();
                            bitsByte19[7] = tile2.inActive();
                            bitsByte20[0] = tile2.wire2();
                            bitsByte20[1] = tile2.wire3();
                            if (tile2.active() && tile2.color() > 0)
                            {
                                bitsByte20[2] = true;
                                b3            = tile2.color();
                            }
                            if (tile2.wall > 0 && tile2.wallColor() > 0)
                            {
                                bitsByte20[3] = true;
                                b4            = tile2.wallColor();
                            }
                            bitsByte20    = (byte)((byte)bitsByte20 + (byte)(tile2.slope() << 4));
                            bitsByte20[7] = tile2.wire4();
                            bitsByte21[0] = tile2.fullbrightBlock();
                            bitsByte21[1] = tile2.fullbrightWall();
                            bitsByte21[2] = tile2.invisibleBlock();
                            bitsByte21[3] = tile2.invisibleWall();
                            writer.Write(bitsByte19);
                            writer.Write(bitsByte20);
                            writer.Write(bitsByte21);
                            if (b3 > 0)
                            {
                                writer.Write(b3);
                            }
                            if (b4 > 0)
                            {
                                writer.Write(b4);
                            }
                            if (tile2.active())
                            {
                                writer.Write(tile2.type);
                                if (Main.tileFrameImportant[tile2.type])
                                {
                                    writer.Write(tile2.frameX);
                                    writer.Write(tile2.frameY);
                                }
                            }
                            if (tile2.wall > 0)
                            {
                                writer.Write(tile2.wall);
                            }
                            if (tile2.liquid > 0 && Main.netMode == 2)
                            {
                                writer.Write(tile2.liquid);
                                writer.Write(tile2.liquidType());
                            }
                        }
                    }
                    break;
                }

                case 21:
                case 90:
                case 145:
                case 148:
                {
                    Item item3 = Main.item[number];
                    writer.Write((short)number);
                    writer.WriteVector2(item3.position);
                    writer.WriteVector2(item3.velocity);
                    // writer.Write7BitEncodedInt(item3.stack);
                    // writer.Write7BitEncodedInt(item3.prefix);
                    writer.Write((short)item3.stack);
                    writer.Write((byte)item3.prefix);
                    writer.Write((byte)number2);
                    short value2 = 0;
                    if (item3.active && item3.stack > 0)
                    {
                        value2 = (short)item3.netID;
                    }
                    writer.Write(value2);
                    if (msgType == 145)
                    {
                        writer.Write(item3.shimmered);
                        writer.Write(item3.shimmerTime);
                    }
                    if (msgType == 148)
                    {
                        writer.Write((byte)MathHelper.Clamp(item3.timeLeftInWhichTheItemCannotBeTakenByEnemies, 0f, 255f));
                    }
                    // ItemIO.SendModData(item3, writer);
                    break;
                }

                case 22:
                    writer.Write((short)number);
                    writer.Write((byte)Main.item[number].playerIndexTheItemIsReservedFor);
                    break;

                case 23:
                {
                    NPC nPC2 = Main.npc[number];
                    writer.Write((short)number);
                    writer.WriteVector2(nPC2.position);
                    writer.WriteVector2(nPC2.velocity);
                    writer.Write((ushort)nPC2.target);
                    int num4 = nPC2.life;
                    if (!nPC2.active)
                    {
                        num4 = 0;
                    }
                    if (!nPC2.active || nPC2.life <= 0)
                    {
                        nPC2.netSkip = 0;
                    }
                    short    value3    = (short)nPC2.netID;
                    bool[]   array     = new bool[4];
                    BitsByte bitsByte3 = (byte)0;
                    bitsByte3[0] = nPC2.direction  > 0;
                    bitsByte3[1] = nPC2.directionY > 0;
                    bitsByte3[2] = (array[0] = nPC2.ai[0] != 0f);
                    bitsByte3[3] = (array[1] = nPC2.ai[1] != 0f);
                    bitsByte3[4] = (array[2] = nPC2.ai[2] != 0f);
                    bitsByte3[5] = (array[3] = nPC2.ai[3] != 0f);
                    bitsByte3[6] = nPC2.spriteDirection > 0;
                    bitsByte3[7] = num4                 == nPC2.lifeMax;
                    writer.Write(bitsByte3);
                    BitsByte bitsByte4 = (byte)0;
                    bitsByte4[0] = nPC2.statsAreScaledForThisManyPlayers > 1;
                    bitsByte4[1] = nPC2.SpawnedFromStatue;
                    bitsByte4[2] = nPC2.strengthMultiplier != 1f;

                    // byte[] extraAI    = NPCLoader.WriteExtraAI(nPC2);
                    // bool   hasExtraAI = extraAI?.Length > 0;
                    // bitsByte4[3] = hasExtraAI; // This bit is unused by vanilla

                    writer.Write(bitsByte4);
                    for (int m = 0; m < NPC.maxAI; m++)
                    {
                        if (array[m])
                        {
                            writer.Write(nPC2.ai[m]);
                        }
                    }
                    writer.Write(value3);
                    if (bitsByte4[0])
                    {
                        writer.Write((byte)nPC2.statsAreScaledForThisManyPlayers);
                    }
                    if (bitsByte4[2])
                    {
                        writer.Write(nPC2.strengthMultiplier);
                    }
                    if (!bitsByte3[7])
                    {
                        byte b2 = 1;
                        if (nPC2.lifeMax > 32767)
                        {
                            b2 = 4;
                        }
                        else if (nPC2.lifeMax > 127)
                        {
                            b2 = 2;
                        }
                        writer.Write(b2);
                        switch (b2)
                        {
                            case 2:
                                writer.Write((short)num4);
                                break;

                            case 4:
                                writer.Write(num4);
                                break;

                            default:
                                writer.Write((sbyte)num4);
                                break;
                        }
                    }
                    if (nPC2.type >= 0 && nPC2.type < NPCID.Count && Main.npcCatchable[nPC2.type])
                    {
                        writer.Write((byte)nPC2.releaseOwner);
                    }

                    // if (hasExtraAI)
                    // {
                    //     NPCLoader.SendExtraAI(writer, extraAI);
                    // }
                    break;
                }

                case 24:
                    writer.Write((short)number);
                    writer.Write((byte)number2);
                    break;

                case 107:
                    writer.Write((byte)number2);
                    writer.Write((byte)number3);
                    writer.Write((byte)number4);
                    text.Serialize(writer);
                    writer.Write((short)number5);
                    break;

                case 27:
                {
                    Projectile projectile = Main.projectile[number];
                    writer.Write((short)projectile.identity);
                    writer.WriteVector2(projectile.position);
                    writer.WriteVector2(projectile.velocity);
                    writer.Write((byte)projectile.owner);
                    writer.Write((short)projectile.type);
                    BitsByte bitsByte23 = (byte)0;
                    BitsByte bitsByte24 = (byte)0;
                    bitsByte23[0] = projectile.ai[0] != 0f;
                    bitsByte23[1] = projectile.ai[1] != 0f;
                    bitsByte24[0] = projectile.ai[2] != 0f;
                    if (projectile.bannerIdToRespondTo != 0)
                    {
                        bitsByte23[3] = true;
                    }
                    if (projectile.damage != 0)
                    {
                        bitsByte23[4] = true;
                    }
                    if (projectile.knockBack != 0f)
                    {
                        bitsByte23[5] = true;
                    }
                    if (projectile.type > 0 && projectile.type < ProjectileID.Count && ProjectileID.Sets.NeedsUUID[projectile.type])
                    {
                        bitsByte23[7] = true;
                    }
                    if (projectile.originalDamage != 0)
                    {
                        bitsByte23[6] = true;
                    }
                    // byte[] extraAI    = ProjectileLoader.WriteExtraAI(projectile);
                    // bool   hasExtraAI = (bitsByte24[1] = extraAI != null && extraAI.Length != 0);
                    if ((byte)bitsByte24 != 0)
                    {
                        bitsByte23[2] = true;
                    }
                    writer.Write(bitsByte23);
                    if (bitsByte23[2])
                    {
                        writer.Write(bitsByte24);
                    }
                    if (bitsByte23[0])
                    {
                        writer.Write(projectile.ai[0]);
                    }
                    if (bitsByte23[1])
                    {
                        writer.Write(projectile.ai[1]);
                    }
                    if (bitsByte23[3])
                    {
                        writer.Write((ushort)projectile.bannerIdToRespondTo);
                    }
                    if (bitsByte23[4])
                    {
                        writer.Write((short)projectile.damage);
                    }
                    if (bitsByte23[5])
                    {
                        writer.Write(projectile.knockBack);
                    }
                    if (bitsByte23[6])
                    {
                        writer.Write((short)projectile.originalDamage);
                    }
                    if (bitsByte23[7])
                    {
                        writer.Write((short)projectile.projUUID);
                    }
                    if (bitsByte24[0])
                    {
                        writer.Write(projectile.ai[2]);
                    }
                    // if (hasExtraAI)
                    // {
                    //     ProjectileLoader.SendExtraAI(writer, extraAI);
                    // }
                    break;
                }

                case 28:
                {
                    writer.Write((short)number);
                    // if (number2 < 0f)
                    // {
                    //     if (Main.netMode != 2)
                    //     {
                    //         throw new ArgumentException("Packet 28 (StrikeNPC) can only be sent with negative damage (silent insta-kill) from the server.");
                    //     }
                    //     writer.Write7BitEncodedInt((int)number2);
                    //     break;
                    // }
                    // NPC.HitInfo hit = ((number7 == 1) ? NetMessage._currentStrike : NetMessage._lastLegacyStrike);
                    // writer.Write7BitEncodedInt(hit.Damage);
                    // writer.Write7BitEncodedInt(hit.SourceDamage);
                    // writer.Write7BitEncodedInt(hit.DamageType.Type);
                    // writer.Write((sbyte)hit.HitDirection);
                    // writer.Write(hit.Knockback);
                    // BitsByte flags = new BitsByte(hit.Crit, hit.InstantKill, hit.HideCombatText);
                    // writer.Write(flags);
                    writer.Write((short)number2);
                    writer.Write(number3);
                    writer.Write((byte)(number4 + 1f));
                    writer.Write((byte)number5);
                    break;
                }

                case 29:
                    writer.Write((short)number);
                    writer.Write((byte)number2);
                    break;

                case 30:
                    writer.Write((byte)number);
                    writer.Write(Main.player[number].hostile);
                    break;

                case 31:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    break;

                case 32:
                {
                    Item item7 = Main.chest[number].item[(byte)number2];
                    writer.Write((short)number);
                    writer.Write((byte)number2);
                    // ItemIO.Send(item7, writer, writeStack: true);

                    short value4 = (short)item7.netID;
                    if (item7.Name == null)
                        value4 = 0;

                    writer.Write((short)item7.stack);
                    writer.Write((byte)item7.prefix);
                    writer.Write(value4);
                    break;
                }

                case 33:
                {
                    int    num5  = 0;
                    int    num6  = 0;
                    int    num7  = 0;
                    string text2 = null;
                    if (number > -1)
                    {
                        num5 = Main.chest[number].x;
                        num6 = Main.chest[number].y;
                    }
                    if (number2 == 1f)
                    {
                        string text3 = text.ToString();
                        num7 = (byte)text3.Length;
                        if (num7 == 0 || num7 > /*63*/ 20)
                        {
                            num7 = 255;
                        }
                        else
                        {
                            text2 = text3;
                        }
                    }
                    writer.Write((short)number);
                    writer.Write((short)num5);
                    writer.Write((short)num6);
                    writer.Write((byte)num7);
                    if (text2 != null)
                    {
                        writer.Write(text2);
                    }
                    break;
                }

                case 34:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    if (Main.netMode == 2)
                    {
                        Netplay.GetSectionX((int)number2);
                        Netplay.GetSectionY((int)number3);
                        writer.Write((short)number5);
                    }
                    else
                    {
                        writer.Write((short)0);
                    }
                    // if (number >= 100)
                    // {
                    //     writer.Write((ushort)number6);
                    // }
                    break;

                case 35:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    break;

                case 36:
                {
                    Player player3 = Main.player[number];
                    writer.Write((byte)number);
                    writer.Write(player3.zone1);
                    writer.Write(player3.zone2);
                    writer.Write(player3.zone3);
                    writer.Write(player3.zone4);
                    writer.Write(player3.zone5);
                    // BiomeLoader.SendCustomBiomes(player3, writer);
                    break;
                }

                case 38:
                    writer.Write(Netplay.ServerPassword);
                    break;

                case 39:
                    writer.Write((short)number);
                    break;

                case 40:
                    writer.Write((byte)number);
                    writer.Write((short)Main.player[number].talkNPC);
                    break;

                case 41:
                    writer.Write((byte)number);
                    writer.Write(Main.player[number].itemRotation);
                    writer.Write((short)Main.player[number].itemAnimation);
                    break;

                case 42:
                    writer.Write((byte)number);
                    writer.Write((short)Main.player[number].statMana);
                    writer.Write((short)Main.player[number].statManaMax);
                    break;

                case 43:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    break;

                case 45:
                    writer.Write((byte)number);
                    writer.Write((byte)Main.player[number].team);
                    break;

                case 46:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    break;

                case 47:
                    writer.Write((short)number);
                    writer.Write((short)Main.sign[number].x);
                    writer.Write((short)Main.sign[number].y);
                    writer.Write(Main.sign[number].text);
                    writer.Write((byte)number2);
                    writer.Write((byte)number3);
                    break;

                case 48:
                {
                    Tile tile = Main.tile[number, (int)number2];
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write(tile.liquid);
                    writer.Write(tile.liquidType());
                    break;
                }

                case 50:
                {
                    writer.Write((byte)number);
                    for (int l = 0; l < Player.maxBuffs; l++)
                    {
                        writer.Write((ushort)Main.player[number].buffType[l]);
                    }
                    break;
                }

                case 51:
                    writer.Write((byte)number);
                    writer.Write((byte)number2);
                    break;

                case 52:
                    writer.Write((byte)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    break;

                case 53:
                    writer.Write((short)number);
                    writer.Write((ushort)number2);
                    writer.Write((short)number3);
                    break;

                case 54:
                {
                    writer.Write((short)number);
                    for (int k = 0; k < NPC.maxBuffs; k++)
                    {
                        writer.Write((ushort)Main.npc[number].buffType[k]);
                        writer.Write((short)Main.npc[number].buffTime[k]);
                    }
                    break;
                }

                case 55:
                    writer.Write((byte)number);
                    writer.Write((ushort)number2);
                    writer.Write((int)number3);
                    break;

                case 56:
                    writer.Write((short)number);
                    if (Main.netMode == 2)
                    {
                        string givenName = Main.npc[number].GivenName;
                        writer.Write(givenName);
                        writer.Write(Main.npc[number].townNpcVariationIndex);
                    }
                    break;

                case 57:
                    writer.Write(WorldGen.tGood);
                    writer.Write(WorldGen.tEvil);
                    writer.Write(WorldGen.tBlood);
                    break;

                case 58:
                    writer.Write((byte)number);
                    writer.Write(number2);
                    break;

                case 59:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    break;

                case 60:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((byte)number4);
                    break;

                case 61:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    break;

                case 62:
                    writer.Write((byte)number);
                    writer.Write((byte)number2);
                    break;

                case 63:
                case 64:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((byte)number3);
                    writer.Write((byte)number4);
                    break;

                case 65:
                {
                    BitsByte bitsByte29 = (byte)0;
                    bitsByte29[0] = (number & 1) == 1;
                    bitsByte29[1] = (number & 2) == 2;
                    bitsByte29[2] = number6      == 1;
                    bitsByte29[3] = number7      != 0;
                    writer.Write(bitsByte29);
                    writer.Write((short)number2);
                    writer.Write(number3);
                    writer.Write(number4);
                    writer.Write((byte)number5);
                    if (bitsByte29[3])
                    {
                        writer.Write(number7);
                    }
                    break;
                }

                case 66:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    break;

                case 68:
                    writer.Write(Main.clientUUID);
                    break;

                case 69:
                    Netplay.GetSectionX((int)number2);
                    Netplay.GetSectionY((int)number3);
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write(Main.chest[(short)number].name);
                    break;

                case 70:
                    writer.Write((short)number);
                    writer.Write((byte)number2);
                    break;

                case 71:
                    writer.Write(number);
                    writer.Write((int)number2);
                    writer.Write((short)number3);
                    writer.Write((byte)number4);
                    break;

                case 72:
                {
                    for (int num20 = 0; num20 < 40; num20++)
                    {
                        writer.Write((short)Main.travelShop[num20]);
                    }
                    break;
                }

                case 73:
                    writer.Write((byte)number);
                    break;

                case 74:
                {
                    writer.Write((byte)Main.anglerQuest);
                    bool value7 = Main.anglerWhoFinishedToday.Contains(text.ToString());
                    writer.Write(value7);
                    break;
                }

                case 76:
                    writer.Write((byte)number);
                    writer.Write(Main.player[number].anglerQuestsFinished);
                    writer.Write(Main.player[number].golferScoreAccumulated);
                    break;

                case 77:
                    writer.Write((short)number);
                    writer.Write((ushort)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    break;

                case 78:
                    writer.Write(number);
                    writer.Write((int)number2);
                    writer.Write((sbyte)number3);
                    writer.Write((sbyte)number4);
                    break;

                case 79:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    writer.Write((byte)number5);
                    writer.Write((sbyte)number6);
                    writer.Write(number7 == 1);
                    break;

                case 80:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    break;

                case 81:
                {
                    writer.Write(number2);
                    writer.Write(number3);
                    Color c2 = default(Color);
                    c2.PackedValue = (uint)number;
                    writer.WriteRGB(c2);
                    writer.Write((int)number4);
                    break;
                }

                case 119:
                {
                    writer.Write(number2);
                    writer.Write(number3);
                    Color c = default(Color);
                    c.PackedValue = (uint)number;
                    writer.WriteRGB(c);
                    text.Serialize(writer);
                    break;
                }

                case 83:
                {
                    int num19 = number;
                    if (num19 < 0 && num19 >= 290)
                    {
                        num19 = 1;
                    }
                    int value6 = NPC.killCount[num19];
                    writer.Write((short)num19);
                    writer.Write(value6);
                    break;
                }

                case 84:
                {
                    byte  b5      = (byte)number;
                    float stealth = Main.player[b5].stealth;
                    writer.Write(b5);
                    writer.Write(stealth);
                    break;
                }

                case 85:
                {
                    short value5 = (short)number;
                    writer.Write(value5);
                    break;
                }

                case 86:
                {
                    writer.Write(number);
                    bool flag3 = TileEntity.ByID.ContainsKey(number);
                    writer.Write(flag3);
                    if (flag3)
                    {
                        TileEntity.Write(writer, TileEntity.ByID[number], networkSend: true /*, lightSend: true*/);
                    }
                    break;
                }

                case 87:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((byte)number3);
                    break;

                case 88:
                {
                    BitsByte bitsByte  = (byte)number2;
                    BitsByte bitsByte2 = (byte)number3;
                    writer.Write((short)number);
                    writer.Write(bitsByte);
                    Item item5 = Main.item[number];
                    if (bitsByte[0])
                    {
                        writer.Write(item5.color.PackedValue);
                    }
                    if (bitsByte[1])
                    {
                        writer.Write((ushort)item5.damage);
                    }
                    if (bitsByte[2])
                    {
                        writer.Write(item5.knockBack);
                    }
                    if (bitsByte[3])
                    {
                        writer.Write((ushort)item5.useAnimation);
                    }
                    if (bitsByte[4])
                    {
                        writer.Write((ushort)item5.useTime);
                    }
                    if (bitsByte[5])
                    {
                        writer.Write((short)item5.shoot);
                    }
                    if (bitsByte[6])
                    {
                        writer.Write(item5.shootSpeed);
                    }
                    if (bitsByte[7])
                    {
                        writer.Write(bitsByte2);
                        if (bitsByte2[0])
                        {
                            writer.Write((ushort)item5.width);
                        }
                        if (bitsByte2[1])
                        {
                            writer.Write((ushort)item5.height);
                        }
                        if (bitsByte2[2])
                        {
                            writer.Write(item5.scale);
                        }
                        if (bitsByte2[3])
                        {
                            writer.Write((short)item5.ammo);
                        }
                        if (bitsByte2[4])
                        {
                            writer.Write((short)item5.useAmmo);
                        }
                        if (bitsByte2[5])
                        {
                            writer.Write(item5.notAmmo);
                        }
                    }
                    break;
                }

                case 89:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    // ItemIO.Send(Main.player[(int)number4].inventory[(int)number3], writer);
                    // writer.Write7BitEncodedInt(number5);

                    Item item4 = Main.player[(int)number4].inventory[(int)number3];
                    writer.Write((short)item4.netID);
                    writer.Write((byte)item4.prefix);
                    writer.Write((short)number5);
                    break;

                case 91:
                    writer.Write(number);
                    writer.Write((byte)number2);
                    // if ((byte)number2 == 2)
                    // {
                    //     writer.Write((byte)((int)number2 >> 8));
                    // }
                    if (number2 != 255f)
                    {
                        writer.Write((ushort)number3);
                        writer.Write((ushort)number4);
                        writer.Write((byte)number5);
                        if (number5 < 0)
                        {
                            writer.Write((short)number6);
                        }
                    }
                    break;

                case 92:
                    writer.Write((short)number);
                    writer.Write((int)number2);
                    writer.Write(number3);
                    writer.Write(number4);
                    break;

                case 95:
                    writer.Write((ushort)number);
                    writer.Write((byte)number2);
                    break;

                case 96:
                {
                    writer.Write((byte)number);
                    Player player2 = Main.player[number];
                    writer.Write((short)number4);
                    writer.Write(number2);
                    writer.Write(number3);
                    writer.WriteVector2(player2.velocity);
                    break;
                }

                case 97:
                    writer.Write((short)number);
                    break;

                case 98:
                    writer.Write((short)number);
                    break;

                case 99:
                    writer.Write((byte)number);
                    writer.WriteVector2(Main.player[number].MinionRestTargetPoint);
                    break;

                case 115:
                    writer.Write((byte)number);
                    writer.Write((short)Main.player[number].MinionAttackTargetNPC);
                    break;

                case 100:
                {
                    writer.Write((ushort)number);
                    NPC nPC = Main.npc[number];
                    writer.Write((short)number4);
                    writer.Write(number2);
                    writer.Write(number3);
                    writer.WriteVector2(nPC.velocity);
                    break;
                }

                case 101:
                    writer.Write((ushort)NPC.ShieldStrengthTowerSolar);
                    writer.Write((ushort)NPC.ShieldStrengthTowerVortex);
                    writer.Write((ushort)NPC.ShieldStrengthTowerNebula);
                    writer.Write((ushort)NPC.ShieldStrengthTowerStardust);
                    break;

                case 102:
                    writer.Write((byte)number);
                    writer.Write((ushort)number2);
                    writer.Write(number3);
                    writer.Write(number4);
                    break;

                case 103:
                    writer.Write(NPC.MaxMoonLordCountdown);
                    writer.Write(NPC.MoonLordCountdown);
                    break;

                case 104:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    writer.Write(((short)number3 < 0) ? 0f : number3);
                    writer.Write((byte)number4);
                    writer.Write(number5);
                    writer.Write((byte)number6);
                    break;

                case 105:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write(number3 == 1f);
                    break;

                case 106:
                    writer.Write(new HalfVector2(number, number2).PackedValue);
                    break;

                case 108:
                    writer.Write((short)number);
                    writer.Write(number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    writer.Write((short)number5);
                    writer.Write((short)number6);
                    writer.Write((byte)number7);
                    break;

                case 109:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((short)number4);
                    writer.Write((byte)number5);
                    break;

                case 110:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    writer.Write((byte)number3);
                    break;

                case 112:
                    writer.Write((byte)number);
                    writer.Write((int)number2);
                    writer.Write((int)number3);
                    writer.Write((byte)number4);
                    writer.Write((short)number5);
                    break;

                case 113:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    break;

                case 116:
                    writer.Write(number);
                    break;

                case 117:
                    if ( /*number7 == 1*/ false)
                    {
                        writer.Write(byte.MaxValue);
                        writer.Write((byte)number);
                        Player.HurtInfo args = NetMessage._currentPlayerHurtInfo;
                        BitsByte        pack = new BitsByte(args.PvP, args.Dodgeable, args.DustDisabled, args.SoundDisabled);
                        writer.Write(pack);
                        args.DamageSource.WriteSelfTo(writer);
                        writer.Write((sbyte)args.CooldownCounter);
                        writer.Write7BitEncodedInt(args.SourceDamage);
                        writer.Write7BitEncodedInt(args.Damage);
                        writer.Write((sbyte)args.HitDirection);
                        writer.Write(args.Knockback);
                    }
                    else
                    {
                        writer.Write((byte)number);
                        NetMessage._currentPlayerDeathReason.WriteSelfTo(writer);
                        writer.Write((short)number2);
                        writer.Write((byte)(number3 + 1f));
                        writer.Write((byte)number4);
                        writer.Write((sbyte)number5);
                    }
                    break;

                case 118:
                    writer.Write((byte)number);
                    NetMessage._currentPlayerDeathReason.WriteSelfTo(writer);
                    writer.Write((short)number2);
                    writer.Write((byte)(number3 + 1f));
                    writer.Write((byte)number4);
                    break;

                case 120:
                    writer.Write((byte)number);
                    writer.Write((byte)number2);
                    break;

                case 121:
                {
                    int  num3  = (int)number3;
                    bool flag2 = number4 == 1f;
                    if (flag2)
                    {
                        num3 += 8;
                    }
                    writer.Write((byte)number);
                    writer.Write((int)number2);
                    writer.Write((byte)num3);
                    if (TileEntity.ByID[(int)number2] is TEDisplayDoll tEDisplayDoll)
                    {
                        tEDisplayDoll.WriteItem((int)number3, writer, flag2);
                        break;
                    }
                    writer.Write(0);
                    writer.Write((byte)0);
                    break;
                }

                case 122:
                    writer.Write(number);
                    writer.Write((byte)number2);
                    break;

                case 123:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    // ItemIO.Send(Main.player[(int)number4].inventory[(int)number3], writer, writeStack: true);

                    Item item2 = Main.player[(int)number4].inventory[(int)number3];
                    writer.Write((short)item2.netID);
                    writer.Write((byte)item2.prefix);
                    writer.Write((short)number5);
                    break;

                case 124:
                {
                    int  num2 = (int)number3;
                    bool flag = number4 == 1f;
                    if (flag)
                    {
                        num2 += 2;
                    }
                    writer.Write((byte)number);
                    writer.Write((int)number2);
                    writer.Write((byte)num2);
                    if (TileEntity.ByID[(int)number2] is TEHatRack tEHatRack)
                    {
                        tEHatRack.WriteItem((int)number3, writer, flag);
                        break;
                    }
                    writer.Write(0);
                    writer.Write((byte)0);
                    break;
                }

                case 125:
                    writer.Write((byte)number);
                    writer.Write((short)number2);
                    writer.Write((short)number3);
                    writer.Write((byte)number4);
                    break;

                case 126:
                    NetMessage._currentRevengeMarker.WriteSelfTo(writer);
                    break;

                case 127:
                    writer.Write(number);
                    break;

                case 128:
                    writer.Write((byte)number);
                    writer.Write((ushort)number5);
                    writer.Write((ushort)number6);
                    writer.Write((ushort)number2);
                    writer.Write((ushort)number3);
                    break;

                case 130:
                    writer.Write((ushort)number);
                    writer.Write((ushort)number2);
                    writer.Write((short)number3);
                    break;

                case 131:
                    writer.Write((ushort)number);
                    writer.Write((byte)number2);
                    if ((byte)number2 == 1)
                    {
                        writer.Write((int)number3);
                        writer.Write((short)number4);
                    }
                    break;

                case 132:
                    NetMessage._currentNetSoundInfo.WriteSelfTo(writer);
                    break;

                case 133:
                    writer.Write((short)number);
                    writer.Write((short)number2);
                    // ItemIO.Send(Main.player[(int)number4].inventory[(int)number3], writer, writeStack: true);

                    Item item = Main.player[(int)number4].inventory[(int)number3];
                    writer.Write((short)item.netID);
                    writer.Write((byte)item.prefix);
                    writer.Write((short)number5);
                    break;

                case 134:
                {
                    writer.Write((byte)number);
                    Player player = Main.player[number];
                    writer.Write(player.ladyBugLuckTimeLeft);
                    writer.Write(player.torchLuck);
                    writer.Write(player.luckPotion);
                    writer.Write(player.HasGardenGnomeNearby);
                    writer.Write(player.equipmentBasedLuckBonus);
                    writer.Write(player.coinLuck);
                    break;
                }

                case 135:
                    writer.Write((byte)number);
                    break;

                case 136:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            writer.Write((ushort)NPC.cavernMonsterType[i, j]);
                        }
                    }
                    break;
                }

                case 137:
                    writer.Write((short)number);
                    writer.Write((ushort)number2);
                    break;

                case 139:
                {
                    writer.Write((byte)number);
                    bool value = number2 == 1f;
                    writer.Write(value);
                    break;
                }

                case 140:
                    writer.Write((byte)number);
                    writer.Write((int)number2);
                    break;

                case 141:
                    writer.Write((byte)number);
                    writer.Write((byte)number2);
                    writer.Write(number3);
                    writer.Write(number4);
                    writer.Write(number5);
                    writer.Write(number6);
                    break;

                case 142:
                {
                    writer.Write((byte)number);
                    Player obj = Main.player[number];
                    obj.piggyBankProjTracker.Write(writer);
                    obj.voidLensChest.Write(writer);
                    break;
                }

                case 146:
                    writer.Write((byte)number);
                    switch (number)
                    {
                        case 0:
                            writer.WriteVector2(new Vector2((int)number2, (int)number3));
                            break;

                        case 1:
                            writer.WriteVector2(new Vector2((int)number2, (int)number3));
                            writer.Write((int)number4);
                            break;

                        case 2:
                            writer.Write((int)number2);
                            break;
                    }
                    break;

                case 147:
                    writer.Write((byte)number);
                    writer.Write((byte)number2);
                    NetMessage.WriteAccessoryVisibility(writer, Main.player[number].hideVisibleAccessory);
                    break;
            }
            int num21 = (int)writer.BaseStream.Position;
            if (num21 > 65535)
            {
                throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + num21);
            }
            writer.BaseStream.Position = position;
            if (num21 > 65535)
            {
                throw new IndexOutOfRangeException($"Maximum packet length exceeded {num21} > {65535}");
            }
            if (ModNet.DetailedLogging)
            {
                ModNet.LogSend(remoteClient, ignoreClient, $"SendData {MessageID.GetName(msgType)}({msgType})", num21);
            }
            writer.Write((ushort)num21);
            writer.BaseStream.Position = num21;
            if (Main.netMode == 1)
            {
                if (Netplay.Connection.Socket.IsConnected())
                {
                    try
                    {
                        NetMessage.buffer[num].spamCount++;
                        Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                        Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Connection.ClientWriteCallBack);
                    }
                    catch { }
                }
            }
            else if (remoteClient == -1)
            {
                switch (msgType)
                {
                    case 34:
                    case 69:
                    {
                        for (int num23 = 0; num23 < 256; num23++)
                        {
                            if (num23 != ignoreClient && NetMessage.buffer[num23].broadcast && Netplay.Clients[num23].IsConnected())
                            {
                                try
                                {
                                    NetMessage.buffer[num23].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num23].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num23].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        break;
                    }

                    case 20:
                    {
                        for (int num27 = 0; num27 < 256; num27++)
                        {
                            if (num27 != ignoreClient && NetMessage.buffer[num27].broadcast && Netplay.Clients[num27].IsConnected() && Netplay.Clients[num27].SectionRange((int)Math.Max(number3, number4), number, (int)number2))
                            {
                                try
                                {
                                    NetMessage.buffer[num27].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num27].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num27].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        break;
                    }

                    case 23:
                    {
                        NPC nPC4 = Main.npc[number];
                        for (int num28 = 0; num28 < 256; num28++)
                        {
                            if (num28 == ignoreClient || !NetMessage.buffer[num28].broadcast || !Netplay.Clients[num28].IsConnected())
                            {
                                continue;
                            }
                            bool flag6 = false;
                            if (nPC4.boss || nPC4.netAlways || nPC4.townNPC || !nPC4.active)
                            {
                                flag6 = true;
                            }
                            else if (nPC4.netSkip <= 0)
                            {
                                Rectangle rect5 = Main.player[num28].getRect();
                                Rectangle rect6 = nPC4.getRect();
                                rect6.X      -= 2500;
                                rect6.Y      -= 2500;
                                rect6.Width  += 5000;
                                rect6.Height += 5000;
                                if (rect5.Intersects(rect6))
                                {
                                    flag6 = true;
                                }
                            }
                            else
                            {
                                flag6 = true;
                            }
                            if (flag6)
                            {
                                try
                                {
                                    NetMessage.buffer[num28].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num28].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num28].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        nPC4.netSkip++;
                        if (nPC4.netSkip > 4)
                        {
                            nPC4.netSkip = 0;
                        }
                        break;
                    }

                    case 28:
                    {
                        NPC nPC3 = Main.npc[number];
                        for (int num25 = 0; num25 < 256; num25++)
                        {
                            if (num25 == ignoreClient || !NetMessage.buffer[num25].broadcast || !Netplay.Clients[num25].IsConnected())
                            {
                                continue;
                            }
                            bool flag5 = false;
                            if (nPC3.life <= 0)
                            {
                                flag5 = true;
                            }
                            else
                            {
                                Rectangle rect3 = Main.player[num25].getRect();
                                Rectangle rect4 = nPC3.getRect();
                                rect4.X      -= 3000;
                                rect4.Y      -= 3000;
                                rect4.Width  += 6000;
                                rect4.Height += 6000;
                                if (rect3.Intersects(rect4))
                                {
                                    flag5 = true;
                                }
                            }
                            if (flag5)
                            {
                                try
                                {
                                    NetMessage.buffer[num25].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num25].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num25].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        break;
                    }

                    case 13:
                    {
                        for (int num26 = 0; num26 < 256; num26++)
                        {
                            if (num26 != ignoreClient && NetMessage.buffer[num26].broadcast && Netplay.Clients[num26].IsConnected())
                            {
                                try
                                {
                                    NetMessage.buffer[num26].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num26].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num26].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        Main.player[number].netSkip++;
                        if (Main.player[number].netSkip > 2)
                        {
                            Main.player[number].netSkip = 0;
                        }
                        break;
                    }

                    case 27:
                    {
                        Projectile projectile2 = Main.projectile[number];
                        for (int num24 = 0; num24 < 256; num24++)
                        {
                            if (num24 == ignoreClient || !NetMessage.buffer[num24].broadcast || !Netplay.Clients[num24].IsConnected())
                            {
                                continue;
                            }
                            bool flag4 = false;
                            if (projectile2.type == 12 || Main.projPet[projectile2.type] || projectile2.aiStyle == 11 || projectile2.netImportant)
                            {
                                flag4 = true;
                            }
                            else
                            {
                                Rectangle rect  = Main.player[num24].getRect();
                                Rectangle rect2 = projectile2.getRect();
                                rect2.X      -= 5000;
                                rect2.Y      -= 5000;
                                rect2.Width  += 10000;
                                rect2.Height += 10000;
                                if (rect.Intersects(rect2))
                                {
                                    flag4 = true;
                                }
                            }
                            if (flag4)
                            {
                                try
                                {
                                    NetMessage.buffer[num24].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num24].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num24].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        break;
                    }

                    default:
                    {
                        for (int num22 = 0; num22 < 256; num22++)
                        {
                            if (num22 != ignoreClient && (NetMessage.buffer[num22].broadcast || (Netplay.Clients[num22].State >= 3 && msgType == 10)) && Netplay.Clients[num22].IsConnected())
                            {
                                try
                                {
                                    NetMessage.buffer[num22].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                                    Netplay.Clients[num22].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[num22].ServerWriteCallBack);
                                }
                                catch { }
                            }
                        }
                        break;
                    }
                }
            }
            else if (Netplay.Clients[remoteClient].IsConnected())
            {
                try
                {
                    NetMessage.buffer[remoteClient].spamCount++;
                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num21);
                    Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num21, Netplay.Clients[remoteClient].ServerWriteCallBack);
                }
                catch { }
            }
            if (Main.verboseNetplay)
            {
                for (int num29 = 0; num29 < num21; num29++) { }
                for (int num30 = 0; num30 < num21; num30++)
                {
                    _ = NetMessage.buffer[num].writeBuffer[num30];
                }
            }
            NetMessage.buffer[num].writeLocked = false;
            if (msgType == 2 && Main.netMode == 2)
            {
                Netplay.Clients[num].SetPendingTermination("Kicked");
                Netplay.Clients[num].PendingTerminationApproved = true;
            }
        }
    }

    private static void NetMessage_DecompressTileBlock_Inner(
        On_NetMessage.orig_DecompressTileBlock_Inner orig,
        BinaryReader                                 reader,
        int                                          xStart,
        int                                          yStart,
        int                                          width,
        int                                          height
    )
    {
        Tile tile = default(Tile);
        int  num  = 0;
        for (int i = yStart; i < yStart + height; i++)
        {
            for (int j = xStart; j < xStart + width; j++)
            {
                if (num != 0)
                {
                    num--;
                    Main.tile[j, i].CopyFrom(tile);
                    continue;
                }
                byte b2;
                byte b;
                byte b3 = (b2 = (b = 0));
                tile = Main.tile[j, i];
                if (tile == null)
                {
                    tile = (Main.tile[j, i] = default(Tile));
                }
                else
                {
                    tile.ClearEverything();
                }
                byte b4   = reader.ReadByte();
                bool flag = false;
                if ((b4 & 1) == 1)
                {
                    flag = true;
                    b3   = reader.ReadByte();
                }
                bool flag2 = false;
                if (flag && (b3 & 1) == 1)
                {
                    flag2 = true;
                    b2    = reader.ReadByte();
                }
                if (flag2 && (b2 & 1) == 1)
                {
                    b = reader.ReadByte();
                }
                bool flag3 = tile.active();
                byte b5;
                if ((b4 & 2) == 2)
                {
                    tile.active(active: true);
                    ushort type = tile.type;
                    int    num2;
                    if ((b4 & 0x20) == 32)
                    {
                        b5   = reader.ReadByte();
                        num2 = reader.ReadByte();
                        num2 = (num2 << 8) | b5;
                    }
                    else
                    {
                        num2 = reader.ReadByte();
                    }
                    tile.type = (ushort)num2;
                    if (Main.tileFrameImportant[num2])
                    {
                        tile.frameX = reader.ReadInt16();
                        tile.frameY = reader.ReadInt16();
                    }
                    else if (!flag3 || tile.type != type)
                    {
                        tile.frameX = -1;
                        tile.frameY = -1;
                    }
                    if ((b2 & 8) == 8)
                    {
                        tile.color(reader.ReadByte());
                    }
                }
                if ((b4 & 4) == 4)
                {
                    tile.wall = reader.ReadByte();
                    if ((b2 & 0x10) == 16)
                    {
                        tile.wallColor(reader.ReadByte());
                    }
                }
                b5 = (byte)((b4 & 0x18) >> 3);
                if (b5 != 0)
                {
                    tile.liquid = reader.ReadByte();
                    if ((b2 & 0x80) == 128)
                    {
                        tile.shimmer(shimmer: true);
                    }
                    else if (b5 > 1)
                    {
                        if (b5 == 2)
                        {
                            tile.lava(lava: true);
                        }
                        else
                        {
                            tile.honey(honey: true);
                        }
                    }
                }
                if (b3 > 1)
                {
                    if ((b3 & 2) == 2)
                    {
                        tile.wire(wire: true);
                    }
                    if ((b3 & 4) == 4)
                    {
                        tile.wire2(wire2: true);
                    }
                    if ((b3 & 8) == 8)
                    {
                        tile.wire3(wire3: true);
                    }
                    b5 = (byte)((b3 & 0x70) >> 4);
                    if (b5 != 0 && Main.tileSolid[tile.type])
                    {
                        if (b5 == 1)
                        {
                            tile.halfBrick(halfBrick: true);
                        }
                        else
                        {
                            tile.slope((byte)(b5 - 1));
                        }
                    }
                }
                if (b2 > 1)
                {
                    if ((b2 & 2) == 2)
                    {
                        tile.actuator(actuator: true);
                    }
                    if ((b2 & 4) == 4)
                    {
                        tile.inActive(inActive: true);
                    }
                    if ((b2 & 0x20) == 32)
                    {
                        tile.wire4(wire4: true);
                    }
                    if ((b2 & 0x40) == 64)
                    {
                        b5        = reader.ReadByte();
                        tile.wall = (ushort)((b5 << 8) | tile.wall);
                    }
                }
                if (b > 1)
                {
                    if ((b & 2) == 2)
                    {
                        tile.invisibleBlock(invisibleBlock: true);
                    }
                    if ((b & 4) == 4)
                    {
                        tile.invisibleWall(invisibleWall: true);
                    }
                    if ((b & 8) == 8)
                    {
                        tile.fullbrightBlock(fullbrightBlock: true);
                    }
                    if ((b & 0x10) == 16)
                    {
                        tile.fullbrightWall(fullbrightWall: true);
                    }
                }
                num = (byte)((b4 & 0xC0) >> 6) switch
                {
                    0 => 0,
                    1 => reader.ReadByte(),
                    _ => reader.ReadInt16(),
                };
            }
        }
        short num3 = reader.ReadInt16();
        for (int k = 0; k < num3; k++)
        {
            short  num4 = reader.ReadInt16();
            short  x    = reader.ReadInt16();
            short  y    = reader.ReadInt16();
            string name = reader.ReadString();
            if (num4 >= 0 && num4 < 8000)
            {
                if (Main.chest[num4] == null)
                {
                    Main.chest[num4] = new Chest();
                }
                Main.chest[num4].name = name;
                Main.chest[num4].x    = x;
                Main.chest[num4].y    = y;
            }
        }
        num3 = reader.ReadInt16();
        for (int l = 0; l < num3; l++)
        {
            short  num5 = reader.ReadInt16();
            short  x2   = reader.ReadInt16();
            short  y2   = reader.ReadInt16();
            string text = reader.ReadString();
            if (num5 >= 0 && num5 < 1000)
            {
                if (Main.sign[num5] == null)
                {
                    Main.sign[num5] = new Sign();
                }
                Main.sign[num5].text = text;
                Main.sign[num5].x    = x2;
                Main.sign[num5].y    = y2;
            }
        }
        num3 = reader.ReadInt16();
        for (int m = 0; m < num3; m++)
        {
            TileEntity tileEntity = TileEntity.Read(reader /*, networkSend: true*/);
            TileEntity.ByID[tileEntity.ID]             = tileEntity;
            TileEntity.ByPosition[tileEntity.Position] = tileEntity;
        }
        Main.sectionManager.SetTilesLoaded(xStart, yStart, xStart + width - 1, yStart + height - 1);
    }
}