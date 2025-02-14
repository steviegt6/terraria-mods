using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.Net;
using Terraria.UI;

namespace Tomat.Terraria.TML.VanillaNetworking;

partial class NetworkOverrideSystem
{
    private static readonly FieldInfo message_buffer_on_tile_change_received_event = typeof(MessageBuffer).GetField("OnTileChangeReceived", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField)!;

    private static void MessageBuffer_GetData(
        On_MessageBuffer.orig_GetData orig,
        MessageBuffer                 self,
        int                           start,
        int                           length,
        out int                       messageType
    )
    {
        if (self.whoAmI < 256)
        {
            Netplay.Clients[self.whoAmI].TimeOutTimer = 0;
        }
        else
        {
            Netplay.Connection.TimeOutTimer = 0;
        }
        byte b   = 0;
        int  num = 0;
        num = start + 1;
        int interactingPlayer = (messageType = self.readBuffer[start]);
        b = (byte)interactingPlayer;
        if (ModNet.DetailedLogging)
        {
            ModNet.Debug(self.whoAmI, $"GetData {MessageID.GetName(b)}({b}), {length + 2}");
        }
        Main.ActiveNetDiagnosticsUI.CountReadMessage(b, length);
        if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
        {
            Netplay.Connection.StatusCount++;
        }
        if (Main.verboseNetplay)
        {
            for (int i = start; i < start + length; i++) { }
            for (int j = start; j < start + length; j++)
            {
                _ = self.readBuffer[j];
            }
        }
        if (Main.netMode == 2 && b != 38 && Netplay.Clients[self.whoAmI].State == -1)
        {
            NetMessage.TrySendData(2, self.whoAmI, -1, Lang.mp[1].ToNetworkText());
            return;
        }
        if (Main.netMode == 2)
        {
            if (Netplay.Clients[self.whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68 && b != 147 /*&& b < 250*/)
            {
                NetMessage.BootPlayer(self.whoAmI, Lang.mp[2].ToNetworkText());
            }
            if (Netplay.Clients[self.whoAmI].State == 0 && b != 1)
            {
                NetMessage.BootPlayer(self.whoAmI, Lang.mp[2].ToNetworkText());
            }
        }
        if (self.reader == null)
        {
            self.ResetReader();
        }
        self.reader.BaseStream.Position = num;
        if (ModNet.HijackGetData(ref b, ref self.reader, self.whoAmI))
        {
            return;
        }
        switch (b)
        {
            case 1:
                if (Main.netMode != 2)
                {
                    break;
                }
                if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[self.whoAmI].Socket.GetRemoteAddress()))
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, Lang.mp[3].ToNetworkText());
                }
                else
                {
                    if (Netplay.Clients[self.whoAmI].State != 0)
                    {
                        break;
                    }

                    if (self.reader.ReadString() == "Terraria" + 279 /*ModNet.IsClientCompatible(self.reader.ReadString(), out ModNet.isModdedClient[self.whoAmI], out var kickMsg)*/)
                    {
                        if (string.IsNullOrEmpty(Netplay.ServerPassword))
                        {
                            Netplay.Clients[self.whoAmI].State = 1;
                            if (false /*ModNet.isModdedClient[self.whoAmI]*/)
                            {
                                ModNet.SyncMods(self.whoAmI);
                            }
                            else
                            {
                                NetMessage.TrySendData(3, self.whoAmI);
                            }
                        }
                        else
                        {
                            Netplay.Clients[self.whoAmI].State = -1;
                            NetMessage.TrySendData(37, self.whoAmI);
                        }
                    }
                    else
                    {
                        // NetMessage.TrySendData(2, self.whoAmI, -1, NetworkText.FromLiteral(Lang.mp[4].Value + "\n(" + kickMsg + ")"));
                        NetMessage.TrySendData(2, self.whoAmI, -1, Lang.mp[4].ToNetworkText());
                    }
                }
                break;

            case 2:
                if (Main.netMode == 1)
                {
                    Netplay.Disconnect = true;
                    Main.statusText    = NetworkText.Deserialize(self.reader).ToString();
                    Main.menuMode      = 14;
                }
                break;

            case 3:
                if (Main.netMode == 1)
                {
                    if (Netplay.Connection.State == 1)
                    {
                        Netplay.Connection.State = 2;
                    }
                    int  num142 = self.reader.ReadByte();
                    bool value5 = self.reader.ReadBoolean();
                    Netplay.Connection.ServerSpecialFlags[2] = value5;
                    if (num142 != Main.myPlayer)
                    {
                        Main.player[num142]        = Main.ActivePlayerFileData.Player;
                        Main.player[Main.myPlayer] = new Player();
                    }
                    Main.player[num142].whoAmI = num142;
                    Main.myPlayer              = num142;
                    Player player12 = Main.player[num142];
                    NetMessage.TrySendData(4,   -1, -1, null, num142);
                    NetMessage.TrySendData(68,  -1, -1, null, num142);
                    NetMessage.TrySendData(16,  -1, -1, null, num142);
                    NetMessage.TrySendData(42,  -1, -1, null, num142);
                    NetMessage.TrySendData(50,  -1, -1, null, num142);
                    NetMessage.TrySendData(147, -1, -1, null, num142, player12.CurrentLoadoutIndex);
                    for (int num143 = 0; num143 < 59; num143++)
                    {
                        NetMessage.TrySendData(5, -1, -1, null, num142, PlayerItemSlotID.Inventory0 + num143, player12.inventory[num143].prefix);
                    }
                    MessageBuffer.TrySendingItemArray(num142, player12.armor,      PlayerItemSlotID.Armor0);
                    MessageBuffer.TrySendingItemArray(num142, player12.dye,        PlayerItemSlotID.Dye0);
                    MessageBuffer.TrySendingItemArray(num142, player12.miscEquips, PlayerItemSlotID.Misc0);
                    MessageBuffer.TrySendingItemArray(num142, player12.miscDyes,   PlayerItemSlotID.MiscDye0);
                    MessageBuffer.TrySendingItemArray(num142, player12.bank.item,  PlayerItemSlotID.Bank1_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.bank2.item, PlayerItemSlotID.Bank2_0);
                    NetMessage.TrySendData(5, -1, -1, null, num142, PlayerItemSlotID.TrashItem, player12.trashItem.prefix);
                    MessageBuffer.TrySendingItemArray(num142, player12.bank3.item,        PlayerItemSlotID.Bank3_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.bank4.item,        PlayerItemSlotID.Bank4_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.Loadouts[0].Dye,   PlayerItemSlotID.Loadout1_Dye_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.Loadouts[1].Dye,   PlayerItemSlotID.Loadout2_Dye_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
                    MessageBuffer.TrySendingItemArray(num142, player12.Loadouts[2].Dye,   PlayerItemSlotID.Loadout3_Dye_0);
                    // PlayerLoader.SyncPlayer(player12, -1, -1, newPlayer: true);
                    NetMessage.TrySendData(6);
                    if (Netplay.Connection.State == 2)
                    {
                        Netplay.Connection.State = 3;
                    }
                }
                break;

            case 4:
            {
                int num211 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num211 = self.whoAmI;
                }
                if (num211 == Main.myPlayer && !Main.ServerSideCharacter)
                {
                    break;
                }
                Player player14 = Main.player[num211];
                player14.whoAmI      = num211;
                player14.skinVariant = self.reader.ReadByte();
                player14.skinVariant = (int)MathHelper.Clamp(player14.skinVariant, 0f, PlayerVariantID.Count - 1);
                player14.hair        = self.reader.ReadByte();
                if (player14.hair >= HairID.Count)
                {
                    player14.hair = 0;
                }
                player14.name    = self.reader.ReadString().Trim().Trim();
                player14.hairDye = self.reader.ReadByte();
                MessageBuffer.ReadAccessoryVisibility(self.reader, player14.hideVisibleAccessory);
                player14.hideMisc        = self.reader.ReadByte();
                player14.hairColor       = self.reader.ReadRGB();
                player14.skinColor       = self.reader.ReadRGB();
                player14.eyeColor        = self.reader.ReadRGB();
                player14.shirtColor      = self.reader.ReadRGB();
                player14.underShirtColor = self.reader.ReadRGB();
                player14.pantsColor      = self.reader.ReadRGB();
                player14.shoeColor       = self.reader.ReadRGB();
                BitsByte bitsByte24 = self.reader.ReadByte();
                player14.difficulty = 0;
                if (bitsByte24[0])
                {
                    player14.difficulty = 1;
                }
                if (bitsByte24[1])
                {
                    player14.difficulty = 2;
                }
                if (bitsByte24[3])
                {
                    player14.difficulty = 3;
                }
                if (player14.difficulty > 3)
                {
                    player14.difficulty = 3;
                }
                player14.extraAccessory = bitsByte24[2];
                BitsByte bitsByte25 = self.reader.ReadByte();
                player14.UsingBiomeTorches    = bitsByte25[0];
                player14.happyFunTorchTime    = bitsByte25[1];
                player14.unlockedBiomeTorches = bitsByte25[2];
                player14.unlockedSuperCart    = bitsByte25[3];
                player14.enabledSuperCart     = bitsByte25[4];
                BitsByte bitsByte26 = self.reader.ReadByte();
                player14.usedAegisCrystal  = bitsByte26[0];
                player14.usedAegisFruit    = bitsByte26[1];
                player14.usedArcaneCrystal = bitsByte26[2];
                player14.usedGalaxyPearl   = bitsByte26[3];
                player14.usedGummyWorm     = bitsByte26[4];
                player14.usedAmbrosia      = bitsByte26[5];
                player14.ateArtisanBread   = bitsByte26[6];
                if (Main.netMode != 2)
                {
                    break;
                }
                bool flag12 = false;
                if (Netplay.Clients[self.whoAmI].State < 10)
                {
                    for (int num212 = 0; num212 < 255; num212++)
                    {
                        if (num212 != num211 && player14.name == Main.player[num212].name && Netplay.Clients[num212].IsActive)
                        {
                            flag12 = true;
                        }
                    }
                }
                if (flag12)
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player14.name));
                }
                else if (player14.name.Length > Player.nameLen)
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
                }
                else if (player14.name == "")
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
                }
                else if (player14.difficulty == 3 && !Main.GameModeInfo.IsJourneyMode)
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
                }
                else if (player14.difficulty != 3 && Main.GameModeInfo.IsJourneyMode)
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
                }
                else
                {
                    Netplay.Clients[self.whoAmI].Name = player14.name;
                    Netplay.Clients[self.whoAmI].Name = player14.name;
                    NetMessage.TrySendData(4, -1, self.whoAmI, null, num211);
                }
                break;
            }

            case 5:
            {
                int num258 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num258 = self.whoAmI;
                }
                if (num258 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num258].HasLockedInventory())
                {
                    break;
                }
                Player player17 = Main.player[num258];
                lock (player17)
                {
                    /*int    num259       = self.reader.ReadInt16();
                    int    num260       = -1;
                    Item[] array3       = null;
                    Item[] array4       = null;
                    int    num261       = 0;
                    bool   flag16       = false;
                    Player clientPlayer = Main.clientPlayer;*/
                    int    num259       = self.reader.ReadInt16();
                    int    stack7       = self.reader.ReadInt16();
                    int    num260       = self.reader.ReadByte();
                    int    type16       = self.reader.ReadInt16();
                    Item[] array3       = null;
                    Item[] array4       = null;
                    int    num261       = 0;
                    bool   flag16       = false;
                    Player clientPlayer = Main.clientPlayer;
                    if (num259 >= PlayerItemSlotID.Loadout3_Dye_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Loadout3_Dye_0;
                        array3 = player17.Loadouts[2].Dye;
                        array4 = clientPlayer.Loadouts[2].Dye;
                    }
                    else if (num259 >= PlayerItemSlotID.Loadout3_Armor_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Loadout3_Armor_0;
                        array3 = player17.Loadouts[2].Armor;
                        array4 = clientPlayer.Loadouts[2].Armor;
                    }
                    else if (num259 >= PlayerItemSlotID.Loadout2_Dye_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Loadout2_Dye_0;
                        array3 = player17.Loadouts[1].Dye;
                        array4 = clientPlayer.Loadouts[1].Dye;
                    }
                    else if (num259 >= PlayerItemSlotID.Loadout2_Armor_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Loadout2_Armor_0;
                        array3 = player17.Loadouts[1].Armor;
                        array4 = clientPlayer.Loadouts[1].Armor;
                    }
                    else if (num259 >= PlayerItemSlotID.Loadout1_Dye_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Loadout1_Dye_0;
                        array3 = player17.Loadouts[0].Dye;
                        array4 = clientPlayer.Loadouts[0].Dye;
                    }
                    else if (num259 >= PlayerItemSlotID.Loadout1_Armor_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Loadout1_Armor_0;
                        array3 = player17.Loadouts[0].Armor;
                        array4 = clientPlayer.Loadouts[0].Armor;
                    }
                    else if (num259 >= PlayerItemSlotID.Bank4_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Bank4_0;
                        array3 = player17.bank4.item;
                        array4 = clientPlayer.bank4.item;
                        if (Main.netMode == 1 && player17.disableVoidBag == num261)
                        {
                            player17.disableVoidBag = -1;
                            Recipe.FindRecipes(canDelayCheck: true);
                        }
                    }
                    else if (num259 >= PlayerItemSlotID.Bank3_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Bank3_0;
                        array3 = player17.bank3.item;
                        array4 = clientPlayer.bank3.item;
                    }
                    else if (num259 >= PlayerItemSlotID.TrashItem)
                    {
                        flag16 = true;
                    }
                    else if (num259 >= PlayerItemSlotID.Bank2_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Bank2_0;
                        array3 = player17.bank2.item;
                        array4 = clientPlayer.bank2.item;
                    }
                    else if (num259 >= PlayerItemSlotID.Bank1_0)
                    {
                        num261 = num259 - PlayerItemSlotID.Bank1_0;
                        array3 = player17.bank.item;
                        array4 = clientPlayer.bank.item;
                    }
                    else if (num259 >= PlayerItemSlotID.MiscDye0)
                    {
                        num261 = num259 - PlayerItemSlotID.MiscDye0;
                        array3 = player17.miscDyes;
                        array4 = clientPlayer.miscDyes;
                    }
                    else if (num259 >= PlayerItemSlotID.Misc0)
                    {
                        num261 = num259 - PlayerItemSlotID.Misc0;
                        array3 = player17.miscEquips;
                        array4 = clientPlayer.miscEquips;
                    }
                    else if (num259 >= PlayerItemSlotID.Dye0)
                    {
                        num261 = num259 - PlayerItemSlotID.Dye0;
                        array3 = player17.dye;
                        array4 = clientPlayer.dye;
                    }
                    else if (num259 >= PlayerItemSlotID.Armor0)
                    {
                        num261 = num259 - PlayerItemSlotID.Armor0;
                        array3 = player17.armor;
                        array4 = clientPlayer.armor;
                    }
                    else
                    {
                        num261 = num259 - PlayerItemSlotID.Inventory0;
                        array3 = player17.inventory;
                        array4 = clientPlayer.inventory;
                    }
                    if (flag16)
                    {
                        // player17.trashItem = ItemIO.Receive(self.reader, readStack: true);
                        player17.trashItem = new Item();
                        player17.trashItem.netDefaults(type16);
                        player17.trashItem.stack = stack7;
                        player17.trashItem.Prefix(num260);
                        if (num258 == Main.myPlayer && !Main.ServerSideCharacter)
                        {
                            clientPlayer.trashItem = player17.trashItem.Clone();
                        }
                    }
                    else if (num259 <= 58)
                    {
                        int type17 = array3[num261].type;
                        int stack8 = array3[num261].stack;
                        // array3[num261] = ItemIO.Receive(self.reader, readStack: true);
                        array3[num261] = new Item();
                        array3[num261].netDefaults(type16);
                        array3[num261].stack = stack7;
                        array3[num261].Prefix(num260);
                        if (num258 == Main.myPlayer && !Main.ServerSideCharacter)
                        {
                            array4[num261] = array3[num261].Clone();
                        }
                        if (num258 == Main.myPlayer && num261 == 58)
                        {
                            Main.mouseItem = array3[num261].Clone();
                        }
                        if (num258 == Main.myPlayer && Main.netMode == 1)
                        {
                            Main.player[num258].inventoryChestStack[num259] = false;
                            if (array3[num261].stack != stack8 || array3[num261].type != type17)
                            {
                                Recipe.FindRecipes(canDelayCheck: true);
                            }
                        }
                    }
                    else
                    {
                        // array3[num261] = ItemIO.Receive(self.reader, readStack: true);
                        array3[num261] = new Item();
                        array3[num261].netDefaults(type16);
                        array3[num261].stack = stack7;
                        array3[num261].Prefix(num260);
                        if (num258 == Main.myPlayer && !Main.ServerSideCharacter)
                        {
                            array4[num261] = array3[num261].Clone();
                        }
                    }
                    bool[] canRelay = PlayerItemSlotID.CanRelay;
                    if (Main.netMode == 2 && num258 == self.whoAmI && canRelay.IndexInRange(num259) && canRelay[num259])
                    {
                        NetMessage.TrySendData(5, -1, self.whoAmI, null, num258, num259, num260);
                    }
                    break;
                }
            }

            case 6:
                if (Main.netMode == 2)
                {
                    if (Netplay.Clients[self.whoAmI].State == 1)
                    {
                        Netplay.Clients[self.whoAmI].State = 2;
                    }
                    NetMessage.TrySendData(7, self.whoAmI);
                    Main.SyncAnInvasion(self.whoAmI);
                }
                break;

            case 7:
                if (Main.netMode == 1)
                {
                    Main.time = self.reader.ReadInt32();
                    BitsByte bitsByte13 = self.reader.ReadByte();
                    Main.dayTime                                   = bitsByte13[0];
                    Main.bloodMoon                                 = bitsByte13[1];
                    Main.eclipse                                   = bitsByte13[2];
                    Main.moonPhase                                 = self.reader.ReadByte();
                    Main.maxTilesX                                 = self.reader.ReadInt16();
                    Main.maxTilesY                                 = self.reader.ReadInt16();
                    Main.spawnTileX                                = self.reader.ReadInt16();
                    Main.spawnTileY                                = self.reader.ReadInt16();
                    Main.worldSurface                              = self.reader.ReadInt16();
                    Main.rockLayer                                 = self.reader.ReadInt16();
                    Main.worldID                                   = self.reader.ReadInt32();
                    Main.worldName                                 = self.reader.ReadString();
                    Main.GameMode                                  = self.reader.ReadByte();
                    Main.ActiveWorldFileData.UniqueId              = new Guid(self.reader.ReadBytes(16));
                    Main.ActiveWorldFileData.WorldGeneratorVersion = self.reader.ReadUInt64();
                    Main.moonType                                  = self.reader.ReadByte();
                    WorldGen.setBG(0,  self.reader.ReadByte());
                    WorldGen.setBG(10, self.reader.ReadByte());
                    WorldGen.setBG(11, self.reader.ReadByte());
                    WorldGen.setBG(12, self.reader.ReadByte());
                    WorldGen.setBG(1,  self.reader.ReadByte());
                    WorldGen.setBG(2,  self.reader.ReadByte());
                    WorldGen.setBG(3,  self.reader.ReadByte());
                    WorldGen.setBG(4,  self.reader.ReadByte());
                    WorldGen.setBG(5,  self.reader.ReadByte());
                    WorldGen.setBG(6,  self.reader.ReadByte());
                    WorldGen.setBG(7,  self.reader.ReadByte());
                    WorldGen.setBG(8,  self.reader.ReadByte());
                    WorldGen.setBG(9,  self.reader.ReadByte());
                    Main.iceBackStyle    = self.reader.ReadByte();
                    Main.jungleBackStyle = self.reader.ReadByte();
                    Main.hellBackStyle   = self.reader.ReadByte();
                    Main.windSpeedTarget = self.reader.ReadSingle();
                    Main.numClouds       = self.reader.ReadByte();
                    for (int num194 = 0; num194 < 3; num194++)
                    {
                        Main.treeX[num194] = self.reader.ReadInt32();
                    }
                    for (int num195 = 0; num195 < 4; num195++)
                    {
                        Main.treeStyle[num195] = self.reader.ReadByte();
                    }
                    for (int num196 = 0; num196 < 3; num196++)
                    {
                        Main.caveBackX[num196] = self.reader.ReadInt32();
                    }
                    for (int num197 = 0; num197 < 4; num197++)
                    {
                        Main.caveBackStyle[num197] = self.reader.ReadByte();
                    }
                    WorldGen.TreeTops.SyncReceive(self.reader);
                    WorldGen.BackgroundsCache.UpdateCache();
                    Main.maxRaining = self.reader.ReadSingle();
                    Main.raining    = Main.maxRaining > 0f;
                    BitsByte bitsByte14 = self.reader.ReadByte();
                    WorldGen.shadowOrbSmashed = bitsByte14[0];
                    NPC.downedBoss1           = bitsByte14[1];
                    NPC.downedBoss2           = bitsByte14[2];
                    NPC.downedBoss3           = bitsByte14[3];
                    Main.hardMode             = bitsByte14[4];
                    NPC.downedClown           = bitsByte14[5];
                    Main.ServerSideCharacter  = bitsByte14[6];
                    NPC.downedPlantBoss       = bitsByte14[7];
                    if (Main.ServerSideCharacter)
                    {
                        Main.ActivePlayerFileData.MarkAsServerSide();
                    }
                    BitsByte bitsByte15 = self.reader.ReadByte();
                    NPC.downedMechBoss1   = bitsByte15[0];
                    NPC.downedMechBoss2   = bitsByte15[1];
                    NPC.downedMechBoss3   = bitsByte15[2];
                    NPC.downedMechBossAny = bitsByte15[3];
                    Main.cloudBGActive    = (bitsByte15[4] ? 1 : 0);
                    WorldGen.crimson      = bitsByte15[5];
                    Main.pumpkinMoon      = bitsByte15[6];
                    Main.snowMoon         = bitsByte15[7];
                    BitsByte bitsByte16 = self.reader.ReadByte();
                    Main.fastForwardTimeToDawn = bitsByte16[1];
                    Main.UpdateTimeRate();
                    bool num263 = bitsByte16[2];
                    NPC.downedSlimeKing      = bitsByte16[3];
                    NPC.downedQueenBee       = bitsByte16[4];
                    NPC.downedFishron        = bitsByte16[5];
                    NPC.downedMartians       = bitsByte16[6];
                    NPC.downedAncientCultist = bitsByte16[7];
                    BitsByte bitsByte17 = self.reader.ReadByte();
                    NPC.downedMoonlord          = bitsByte17[0];
                    NPC.downedHalloweenKing     = bitsByte17[1];
                    NPC.downedHalloweenTree     = bitsByte17[2];
                    NPC.downedChristmasIceQueen = bitsByte17[3];
                    NPC.downedChristmasSantank  = bitsByte17[4];
                    NPC.downedChristmasTree     = bitsByte17[5];
                    NPC.downedGolemBoss         = bitsByte17[6];
                    BirthdayParty.ManualParty   = bitsByte17[7];
                    BitsByte bitsByte18 = self.reader.ReadByte();
                    NPC.downedPirates         = bitsByte18[0];
                    NPC.downedFrost           = bitsByte18[1];
                    NPC.downedGoblins         = bitsByte18[2];
                    Sandstorm.Happening       = bitsByte18[3];
                    DD2Event.Ongoing          = bitsByte18[4];
                    DD2Event.DownedInvasionT1 = bitsByte18[5];
                    DD2Event.DownedInvasionT2 = bitsByte18[6];
                    DD2Event.DownedInvasionT3 = bitsByte18[7];
                    BitsByte bitsByte19 = self.reader.ReadByte();
                    NPC.combatBookWasUsed       = bitsByte19[0];
                    LanternNight.ManualLanterns = bitsByte19[1];
                    NPC.downedTowerSolar        = bitsByte19[2];
                    NPC.downedTowerVortex       = bitsByte19[3];
                    NPC.downedTowerNebula       = bitsByte19[4];
                    NPC.downedTowerStardust     = bitsByte19[5];
                    Main.forceHalloweenForToday = bitsByte19[6];
                    Main.forceXMasForToday      = bitsByte19[7];
                    BitsByte bitsByte20 = self.reader.ReadByte();
                    NPC.boughtCat            = bitsByte20[0];
                    NPC.boughtDog            = bitsByte20[1];
                    NPC.boughtBunny          = bitsByte20[2];
                    NPC.freeCake             = bitsByte20[3];
                    Main.drunkWorld          = bitsByte20[4];
                    NPC.downedEmpressOfLight = bitsByte20[5];
                    NPC.downedQueenSlime     = bitsByte20[6];
                    Main.getGoodWorld        = bitsByte20[7];
                    BitsByte bitsByte21 = self.reader.ReadByte();
                    Main.tenthAnniversaryWorld     = bitsByte21[0];
                    Main.dontStarveWorld           = bitsByte21[1];
                    NPC.downedDeerclops            = bitsByte21[2];
                    Main.notTheBeesWorld           = bitsByte21[3];
                    Main.remixWorld                = bitsByte21[4];
                    NPC.unlockedSlimeBlueSpawn     = bitsByte21[5];
                    NPC.combatBookVolumeTwoWasUsed = bitsByte21[6];
                    NPC.peddlersSatchelWasUsed     = bitsByte21[7];
                    BitsByte bitsByte22 = self.reader.ReadByte();
                    NPC.unlockedSlimeGreenSpawn   = bitsByte22[0];
                    NPC.unlockedSlimeOldSpawn     = bitsByte22[1];
                    NPC.unlockedSlimePurpleSpawn  = bitsByte22[2];
                    NPC.unlockedSlimeRainbowSpawn = bitsByte22[3];
                    NPC.unlockedSlimeRedSpawn     = bitsByte22[4];
                    NPC.unlockedSlimeYellowSpawn  = bitsByte22[5];
                    NPC.unlockedSlimeCopperSpawn  = bitsByte22[6];
                    Main.fastForwardTimeToDusk    = bitsByte22[7];
                    BitsByte bitsByte23 = self.reader.ReadByte();
                    Main.noTrapsWorld                 = bitsByte23[0];
                    Main.zenithWorld                  = bitsByte23[1];
                    NPC.unlockedTruffleSpawn          = bitsByte23[2];
                    Main.sundialCooldown              = self.reader.ReadByte();
                    Main.moondialCooldown             = self.reader.ReadByte();
                    WorldGen.SavedOreTiers.Copper     = self.reader.ReadInt16();
                    WorldGen.SavedOreTiers.Iron       = self.reader.ReadInt16();
                    WorldGen.SavedOreTiers.Silver     = self.reader.ReadInt16();
                    WorldGen.SavedOreTiers.Gold       = self.reader.ReadInt16();
                    WorldGen.SavedOreTiers.Cobalt     = self.reader.ReadInt16();
                    WorldGen.SavedOreTiers.Mythril    = self.reader.ReadInt16();
                    WorldGen.SavedOreTiers.Adamantite = self.reader.ReadInt16();
                    if (num263)
                    {
                        Main.StartSlimeRain();
                    }
                    else
                    {
                        Main.StopSlimeRain();
                    }
                    Main.invasionType          = self.reader.ReadSByte();
                    Main.LobbyId               = self.reader.ReadUInt64();
                    Sandstorm.IntendedSeverity = self.reader.ReadSingle();
                    // if (Netplay.Connection.State > 4)
                    // {
                    //     WorldIO.ReceiveModData(self.reader);
                    // }
                    if (Netplay.Connection.State == 3)
                    {
                        Main.windSpeedCurrent    = Main.windSpeedTarget;
                        Netplay.Connection.State = 4;
                    }
                    Main.checkHalloween();
                    Main.checkXMas();
                }
                break;

            case 8:
            {
                if (Main.netMode != 2)
                {
                    break;
                }
                NetMessage.TrySendData(7, self.whoAmI);
                int  num70 = self.reader.ReadInt32();
                int  num71 = self.reader.ReadInt32();
                bool flag3 = true;
                if (num70 == -1 || num71 == -1)
                {
                    flag3 = false;
                }
                else if (num70 < 10 || num70 > Main.maxTilesX - 10)
                {
                    flag3 = false;
                }
                else if (num71 < 10 || num71 > Main.maxTilesY - 10)
                {
                    flag3 = false;
                }
                int num72 = Netplay.GetSectionX(Main.spawnTileX) - 2;
                int num73 = Netplay.GetSectionY(Main.spawnTileY) - 1;
                int num74 = num72                                + 5;
                int num75 = num73                                + 3;
                if (num72 < 0)
                {
                    num72 = 0;
                }
                if (num74 >= Main.maxSectionsX)
                {
                    num74 = Main.maxSectionsX;
                }
                if (num73 < 0)
                {
                    num73 = 0;
                }
                if (num75 >= Main.maxSectionsY)
                {
                    num75 = Main.maxSectionsY;
                }
                int         num76 = (num74 - num72) * (num75 - num73);
                List<Point> list  = new List<Point>();
                for (int num77 = num72; num77 < num74; num77++)
                {
                    for (int num78 = num73; num78 < num75; num78++)
                    {
                        list.Add(new Point(num77, num78));
                    }
                }
                int num79 = -1;
                int num80 = -1;
                if (flag3)
                {
                    num70 = Netplay.GetSectionX(num70) - 2;
                    num71 = Netplay.GetSectionY(num71) - 1;
                    num79 = num70                      + 5;
                    num80 = num71                      + 3;
                    if (num70 < 0)
                    {
                        num70 = 0;
                    }
                    if (num79 >= Main.maxSectionsX)
                    {
                        num79 = Main.maxSectionsX - 1;
                    }
                    if (num71 < 0)
                    {
                        num71 = 0;
                    }
                    if (num80 >= Main.maxSectionsY)
                    {
                        num80 = Main.maxSectionsY - 1;
                    }
                    for (int num81 = num70; num81 <= num79; num81++)
                    {
                        for (int num82 = num71; num82 <= num80; num82++)
                        {
                            if (num81 < num72 || num81 >= num74 || num82 < num73 || num82 >= num75)
                            {
                                list.Add(new Point(num81, num82));
                                num76++;
                            }
                        }
                    }
                }
                PortalHelper.SyncPortalsOnPlayerJoin(self.whoAmI, 1, list, out var portalSections);
                num76 += portalSections.Count;
                if (Netplay.Clients[self.whoAmI].State == 2)
                {
                    Netplay.Clients[self.whoAmI].State = 3;
                }
                NetMessage.TrySendData(9, self.whoAmI, -1, Lang.inter[44].ToNetworkText(), num76);
                Netplay.Clients[self.whoAmI].StatusText2 =  Language.GetTextValue("Net.IsReceivingTileData");
                Netplay.Clients[self.whoAmI].StatusMax   += num76;
                for (int num83 = num72; num83 < num74; num83++)
                {
                    for (int num84 = num73; num84 < num75; num84++)
                    {
                        NetMessage.SendSection(self.whoAmI, num83, num84);
                    }
                }
                if (flag3)
                {
                    for (int num85 = num70; num85 <= num79; num85++)
                    {
                        for (int num86 = num71; num86 <= num80; num86++)
                        {
                            NetMessage.SendSection(self.whoAmI, num85, num86);
                        }
                    }
                }
                for (int num87 = 0; num87 < portalSections.Count; num87++)
                {
                    NetMessage.SendSection(self.whoAmI, portalSections[num87].X, portalSections[num87].Y);
                }
                for (int num88 = 0; num88 < 400; num88++)
                {
                    if (Main.item[num88].active)
                    {
                        NetMessage.TrySendData(21, self.whoAmI, -1, null, num88);
                        NetMessage.TrySendData(22, self.whoAmI, -1, null, num88);
                    }
                }
                for (int num89 = 0; num89 < 200; num89++)
                {
                    if (Main.npc[num89].active)
                    {
                        NetMessage.TrySendData(23, self.whoAmI, -1, null, num89);
                    }
                }
                for (int num90 = 0; num90 < 1000; num90++)
                {
                    if (Main.projectile[num90].active && (Main.projPet[Main.projectile[num90].type] || Main.projectile[num90].netImportant))
                    {
                        NetMessage.TrySendData(27, self.whoAmI, -1, null, num90);
                    }
                }
                for (int num91 = 0; num91 < NPCID.Count /*NPCLoader.NPCCount*/; num91++)
                {
                    NetMessage.TrySendData(83, self.whoAmI, -1, null, num91);
                }
                NetMessage.TrySendData(57, self.whoAmI);
                NetMessage.TrySendData(103);
                NetMessage.TrySendData(101, self.whoAmI);
                NetMessage.TrySendData(136, self.whoAmI);
                NetMessage.TrySendData(49,  self.whoAmI);
                Main.BestiaryTracker.OnPlayerJoining(self.whoAmI);
                CreativePowerManager.Instance.SyncThingsToJoiningPlayer(self.whoAmI);
                Main.PylonSystem.OnPlayerJoining(self.whoAmI);
                break;
            }

            case 9:
                if (Main.netMode == 1)
                {
                    Netplay.Connection.StatusMax  += self.reader.ReadInt32();
                    Netplay.Connection.StatusText =  NetworkText.Deserialize(self.reader).ToString();
                    BitsByte bitsByte32         = self.reader.ReadByte();
                    BitsByte serverSpecialFlags = Netplay.Connection.ServerSpecialFlags;
                    serverSpecialFlags[0]                 = bitsByte32[0];
                    serverSpecialFlags[1]                 = bitsByte32[1];
                    Netplay.Connection.ServerSpecialFlags = serverSpecialFlags;
                }
                break;

            case 10:
                if (Main.netMode == 1)
                {
                    NetMessage.DecompressTileBlock(self.reader.BaseStream);
                }
                break;

            case 11:
                if (Main.netMode == 1)
                {
                    WorldGen.SectionTileFrame(self.reader.ReadInt16(), self.reader.ReadInt16(), self.reader.ReadInt16(), self.reader.ReadInt16());
                }
                break;

            case 12:
            {
                int num92 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num92 = self.whoAmI;
                }
                Player player6 = Main.player[num92];
                player6.SpawnX            = self.reader.ReadInt16();
                player6.SpawnY            = self.reader.ReadInt16();
                player6.respawnTimer      = self.reader.ReadInt32();
                player6.numberOfDeathsPVE = self.reader.ReadInt16();
                player6.numberOfDeathsPVP = self.reader.ReadInt16();
                if (player6.respawnTimer > 0)
                {
                    player6.dead = true;
                }
                PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)self.reader.ReadByte();
                player6.Spawn(playerSpawnContext);
                if (Main.netMode != 2 || Netplay.Clients[self.whoAmI].State < 3)
                {
                    break;
                }
                if (Netplay.Clients[self.whoAmI].State == 3)
                {
                    Netplay.Clients[self.whoAmI].State       = 10;
                    NetMessage.buffer[self.whoAmI].broadcast = true;
                    NetMessage.SyncConnectedPlayer(self.whoAmI);
                    bool flag4 = NetMessage.DoesPlayerSlotCountAsAHost(self.whoAmI);
                    Main.countsAsHostForGameplay[self.whoAmI] = flag4;
                    if (NetMessage.DoesPlayerSlotCountAsAHost(self.whoAmI))
                    {
                        NetMessage.TrySendData(139, self.whoAmI, -1, null, self.whoAmI, flag4.ToInt());
                    }
                    NetMessage.TrySendData(12,  -1,          self.whoAmI, null,                                                   self.whoAmI, (int)(byte)playerSpawnContext);
                    NetMessage.TrySendData(74,  self.whoAmI, -1,          NetworkText.FromLiteral(Main.player[self.whoAmI].name), Main.anglerQuest);
                    NetMessage.TrySendData(129, self.whoAmI);
                    NetMessage.greetPlayer(self.whoAmI);
                    if (Main.ActiveWorldFileData.ModSaveErrors.Any())
                    {
                        ChatHelper.SendChatMessageToClient(Utils.CreateSaveErrorMessage("tModLoader.WorldCustomDataSaveFail", Main.ActiveWorldFileData.ModSaveErrors), Color.OrangeRed, self.whoAmI);
                    }
                    if (Main.player[num92].unlockedBiomeTorches)
                    {
                        NPC nPC2 = new NPC();
                        nPC2.SetDefaults(664);
                        Main.BestiaryTracker.Kills.RegisterKill(nPC2);
                    }
                }
                else
                {
                    NetMessage.TrySendData(12, -1, self.whoAmI, null, self.whoAmI, (int)(byte)playerSpawnContext);
                }
                break;
            }

            case 13:
            {
                int num138 = self.reader.ReadByte();
                if (num138 != Main.myPlayer || Main.ServerSideCharacter)
                {
                    if (Main.netMode == 2)
                    {
                        num138 = self.whoAmI;
                    }
                    Player   player10   = Main.player[num138];
                    BitsByte bitsByte7  = self.reader.ReadByte();
                    BitsByte bitsByte8  = self.reader.ReadByte();
                    BitsByte bitsByte9  = self.reader.ReadByte();
                    BitsByte bitsByte10 = self.reader.ReadByte();
                    player10.controlUp      = bitsByte7[0];
                    player10.controlDown    = bitsByte7[1];
                    player10.controlLeft    = bitsByte7[2];
                    player10.controlRight   = bitsByte7[3];
                    player10.controlJump    = bitsByte7[4];
                    player10.controlUseItem = bitsByte7[5];
                    player10.direction      = (bitsByte7[6] ? 1 : (-1));
                    if (bitsByte8[0])
                    {
                        player10.pulley    = true;
                        player10.pulleyDir = (byte)((!bitsByte8[1]) ? 1u : 2u);
                    }
                    else
                    {
                        player10.pulley = false;
                    }
                    player10.vortexStealthActive = bitsByte8[3];
                    player10.gravDir             = (bitsByte8[4] ? 1 : (-1));
                    player10.TryTogglingShield(bitsByte8[5]);
                    player10.ghost        = bitsByte8[6];
                    player10.selectedItem = self.reader.ReadByte();
                    player10.position     = self.reader.ReadVector2();
                    if (bitsByte8[2])
                    {
                        player10.velocity = self.reader.ReadVector2();
                    }
                    else
                    {
                        player10.velocity = Vector2.Zero;
                    }
                    if (bitsByte9[6])
                    {
                        player10.PotionOfReturnOriginalUsePosition = self.reader.ReadVector2();
                        player10.PotionOfReturnHomePosition        = self.reader.ReadVector2();
                    }
                    else
                    {
                        player10.PotionOfReturnOriginalUsePosition = null;
                        player10.PotionOfReturnHomePosition        = null;
                    }
                    player10.tryKeepingHoveringUp        = bitsByte9[0];
                    player10.IsVoidVaultEnabled          = bitsByte9[1];
                    player10.sitting.isSitting           = bitsByte9[2];
                    player10.downedDD2EventAnyDifficulty = bitsByte9[3];
                    player10.isPettingAnimal             = bitsByte9[4];
                    player10.isTheAnimalBeingPetSmall    = bitsByte9[5];
                    player10.tryKeepingHoveringDown      = bitsByte9[7];
                    player10.sleeping.SetIsSleepingAndAdjustPlayerRotation(player10, bitsByte10[0]);
                    player10.autoReuseAllWeapons      = bitsByte10[1];
                    player10.controlDownHold          = bitsByte10[2];
                    player10.isOperatingAnotherEntity = bitsByte10[3];
                    player10.controlUseTile           = bitsByte10[4];
                    if (Main.netMode == 2 && Netplay.Clients[self.whoAmI].State == 10)
                    {
                        NetMessage.TrySendData(13, -1, self.whoAmI, null, num138);
                    }
                }
                break;
            }

            case 14:
            {
                int num253 = self.reader.ReadByte();
                int num254 = self.reader.ReadByte();
                if (Main.netMode != 1)
                {
                    break;
                }
                bool active = Main.player[num253].active;
                if (num254 == 1)
                {
                    if (!Main.player[num253].active)
                    {
                        Main.player[num253] = new Player();
                    }
                    Main.player[num253].active = true;
                }
                else
                {
                    Main.player[num253].active = false;
                }
                if (active != Main.player[num253].active)
                {
                    if (Main.player[num253].active)
                    {
                        Player.Hooks.PlayerConnect(num253);
                    }
                    else
                    {
                        Player.Hooks.PlayerDisconnect(num253);
                    }
                }
                break;
            }

            case 16:
            {
                int num139 = self.reader.ReadByte();
                if (num139 != Main.myPlayer || Main.ServerSideCharacter)
                {
                    if (Main.netMode == 2)
                    {
                        num139 = self.whoAmI;
                    }
                    Player obj6 = Main.player[num139];
                    obj6.statLife    = self.reader.ReadInt16();
                    obj6.statLifeMax = self.reader.ReadInt16();
                    if (obj6.statLifeMax < 100)
                        obj6.statLifeMax = 100;
                    SyncStatLifeMax(obj6);
                    obj6.dead = obj6.statLife <= 0;
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(16, -1, self.whoAmI, null, num139);
                    }
                }
                break;
            }

            case 17:
            {
                byte  b7     = self.reader.ReadByte();
                int   num176 = self.reader.ReadInt16();
                int   num177 = self.reader.ReadInt16();
                short num178 = self.reader.ReadInt16();
                int   num179 = self.reader.ReadByte();
                bool  flag9  = num178 == 1;
                if (!WorldGen.InWorld(num176, num177, 3))
                {
                    break;
                }
                if (Main.tile[num176, num177] == null)
                {
                    Main.tile[num176, num177] = default(Tile);
                }
                if (Main.netMode == 2)
                {
                    if (!flag9)
                    {
                        if (b7 == 0 || b7 == 2 || b7 == 4)
                        {
                            Netplay.Clients[self.whoAmI].SpamDeleteBlock += 1f;
                        }
                        if (b7 == 1 || b7 == 3)
                        {
                            Netplay.Clients[self.whoAmI].SpamAddBlock += 1f;
                        }
                    }
                    if (!Netplay.Clients[self.whoAmI].TileSections[Netplay.GetSectionX(num176), Netplay.GetSectionY(num177)])
                    {
                        flag9 = true;
                    }
                }
                if (b7 == 0)
                {
                    WorldGen.KillTile(num176, num177, flag9);
                    if (Main.netMode == 1 && !flag9)
                    {
                        HitTile.ClearAllTilesAtThisLocation(num176, num177);
                    }
                }
                bool flag10 = false;
                if (b7 == 1)
                {
                    bool forced = true;
                    if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(num176, num177))
                    {
                        flag10 = true;
                        forced = false;
                    }
                    WorldGen.PlaceTile(num176, num177, num178, mute: false, forced, -1, num179);
                }
                if (b7 == 2)
                {
                    WorldGen.KillWall(num176, num177, flag9);
                }
                if (b7 == 3)
                {
                    WorldGen.PlaceWall(num176, num177, num178);
                }
                if (b7 == 4)
                {
                    WorldGen.KillTile(num176, num177, flag9, effectOnly: false, noItem: true);
                }
                if (b7 == 5)
                {
                    WorldGen.PlaceWire(num176, num177);
                }
                if (b7 == 6)
                {
                    WorldGen.KillWire(num176, num177);
                }
                if (b7 == 7)
                {
                    WorldGen.PoundTile(num176, num177);
                }
                if (b7 == 8)
                {
                    WorldGen.PlaceActuator(num176, num177);
                }
                if (b7 == 9)
                {
                    WorldGen.KillActuator(num176, num177);
                }
                if (b7 == 10)
                {
                    WorldGen.PlaceWire2(num176, num177);
                }
                if (b7 == 11)
                {
                    WorldGen.KillWire2(num176, num177);
                }
                if (b7 == 12)
                {
                    WorldGen.PlaceWire3(num176, num177);
                }
                if (b7 == 13)
                {
                    WorldGen.KillWire3(num176, num177);
                }
                if (b7 == 14)
                {
                    WorldGen.SlopeTile(num176, num177, num178);
                }
                if (b7 == 15)
                {
                    Minecart.FrameTrack(num176, num177, pound: true);
                }
                if (b7 == 16)
                {
                    WorldGen.PlaceWire4(num176, num177);
                }
                if (b7 == 17)
                {
                    WorldGen.KillWire4(num176, num177);
                }
                switch (b7)
                {
                    case 18:
                        Wiring.SetCurrentUser(self.whoAmI);
                        Wiring.PokeLogicGate(num176, num177);
                        Wiring.SetCurrentUser();
                        return;

                    case 19:
                        Wiring.SetCurrentUser(self.whoAmI);
                        Wiring.Actuate(num176, num177);
                        Wiring.SetCurrentUser();
                        return;

                    case 20:
                        if (WorldGen.InWorld(num176, num177, 2))
                        {
                            int type10 = Main.tile[num176, num177].type;
                            WorldGen.KillTile(num176, num177, flag9);
                            num178 = (short)((Main.tile[num176, num177].active() && Main.tile[num176, num177].type == type10) ? 1 : 0);
                            if (Main.netMode == 2)
                            {
                                NetMessage.TrySendData(17, -1, -1, null, b7, num176, num177, num178, num179);
                            }
                        }
                        return;

                    case 21:
                        WorldGen.ReplaceTile(num176, num177, (ushort)num178, num179);
                        break;
                }
                if (b7 == 22)
                {
                    WorldGen.ReplaceWall(num176, num177, (ushort)num178);
                }
                if (b7 == 23)
                {
                    WorldGen.SlopeTile(num176, num177, num178);
                    WorldGen.PoundTile(num176, num177);
                }
                if (Main.netMode == 2)
                {
                    if (flag10)
                    {
                        NetMessage.SendTileSquare(-1, num176, num177, 5);
                    }
                    else if ((b7 != 1 && b7 != 21) || !TileID.Sets.Falling[num178] || Main.tile[num176, num177].active())
                    {
                        NetMessage.TrySendData(17, -1, self.whoAmI, null, b7, num176, num177, num178, num179);
                    }
                }
                break;
            }

            case 18:
                if (Main.netMode == 1)
                {
                    Main.dayTime  = self.reader.ReadByte() == 1;
                    Main.time     = self.reader.ReadInt32();
                    Main.sunModY  = self.reader.ReadInt16();
                    Main.moonModY = self.reader.ReadInt16();
                }
                break;

            case 19:
            {
                byte b11    = self.reader.ReadByte();
                int  num199 = self.reader.ReadInt16();
                int  num200 = self.reader.ReadInt16();
                if (WorldGen.InWorld(num199, num200, 3))
                {
                    int num201 = ((self.reader.ReadByte() != 0) ? 1 : (-1));
                    switch (b11)
                    {
                        case 0:
                            WorldGen.OpenDoor(num199, num200, num201);
                            break;

                        case 1:
                            WorldGen.CloseDoor(num199, num200, forced: true);
                            break;

                        case 2:
                            WorldGen.ShiftTrapdoor(num199, num200, num201 == 1, 1);
                            break;

                        case 3:
                            WorldGen.ShiftTrapdoor(num199, num200, num201 == 1, 0);
                            break;

                        case 4:
                            WorldGen.ShiftTallGate(num199, num200, closing: false, forced: true);
                            break;

                        case 5:
                            WorldGen.ShiftTallGate(num199, num200, closing: true, forced: true);
                            break;
                    }
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(19, -1, self.whoAmI, null, b11, num199, num200, (num201 == 1) ? 1 : 0);
                    }
                }
                break;
            }

            case 20:
            {
                int    num228 = self.reader.ReadInt16();
                int    num229 = self.reader.ReadInt16();
                ushort num230 = self.reader.ReadByte();
                ushort num231 = self.reader.ReadByte();
                byte   b13    = self.reader.ReadByte();
                if (!WorldGen.InWorld(num228, num229, 3))
                {
                    break;
                }
                TileChangeType type12 = TileChangeType.None;
                if (Enum.IsDefined(typeof(TileChangeType), b13))
                {
                    type12 = (TileChangeType)b13;
                }
                // if (MessageBuffer.OnTileChangeReceived != null)
                // {
                //     MessageBuffer.OnTileChangeReceived(num228, num229, Math.Max(num230, num231), type12);
                // }
                if (message_buffer_on_tile_change_received_event.GetValue(null)! is Delegate @delegate)
                {
                    @delegate.DynamicInvoke(num228, num229, Math.Max(num230, num231), type12);
                }
                BitsByte bitsByte28 = (byte)0;
                BitsByte bitsByte29 = (byte)0;
                BitsByte bitsByte30 = (byte)0;
                Tile     tile4      = default(Tile);
                for (int num232 = num228; num232 < num228 + num230; num232++)
                {
                    for (int num233 = num229; num233 < num229 + num231; num233++)
                    {
                        if (Main.tile[num232, num233] == null)
                        {
                            Main.tile[num232, num233] = default(Tile);
                        }
                        tile4 = Main.tile[num232, num233];
                        bool flag13 = tile4.active();
                        bitsByte28 = self.reader.ReadByte();
                        bitsByte29 = self.reader.ReadByte();
                        bitsByte30 = self.reader.ReadByte();
                        tile4.active(bitsByte28[0]);
                        tile4.wall = (bitsByte28[2] ? ((ushort)1) : ((ushort)0));
                        bool flag14 = bitsByte28[3];
                        if (Main.netMode != 2)
                        {
                            tile4.liquid = (flag14 ? ((byte)1) : ((byte)0));
                        }
                        tile4.wire(bitsByte28[4]);
                        tile4.halfBrick(bitsByte28[5]);
                        tile4.actuator(bitsByte28[6]);
                        tile4.inActive(bitsByte28[7]);
                        tile4.wire2(bitsByte29[0]);
                        tile4.wire3(bitsByte29[1]);
                        if (bitsByte29[2])
                        {
                            tile4.color(self.reader.ReadByte());
                        }
                        if (bitsByte29[3])
                        {
                            tile4.wallColor(self.reader.ReadByte());
                        }
                        if (tile4.active())
                        {
                            int type13 = tile4.type;
                            tile4.type = self.reader.ReadUInt16();
                            if (Main.tileFrameImportant[tile4.type])
                            {
                                tile4.frameX = self.reader.ReadInt16();
                                tile4.frameY = self.reader.ReadInt16();
                            }
                            else if (!flag13 || tile4.type != type13)
                            {
                                tile4.frameX = -1;
                                tile4.frameY = -1;
                            }
                            byte b14 = 0;
                            if (bitsByte29[4])
                            {
                                b14++;
                            }
                            if (bitsByte29[5])
                            {
                                b14 += 2;
                            }
                            if (bitsByte29[6])
                            {
                                b14 += 4;
                            }
                            tile4.slope(b14);
                        }
                        tile4.wire4(bitsByte29[7]);
                        tile4.fullbrightBlock(bitsByte30[0]);
                        tile4.fullbrightWall(bitsByte30[1]);
                        tile4.invisibleBlock(bitsByte30[2]);
                        tile4.invisibleWall(bitsByte30[3]);
                        if (tile4.wall > 0)
                        {
                            tile4.wall = self.reader.ReadUInt16();
                        }
                        if (flag14)
                        {
                            tile4.liquid = self.reader.ReadByte();
                            tile4.liquidType(self.reader.ReadByte());
                        }
                    }
                }
                WorldGen.RangeFrame(num228, num229, num228 + num230, num229 + num231);
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(b, -1, self.whoAmI, null, num228, num229, (int)num230, (int)num231, b13);
                }
                break;
            }

            case 21:
            case 90:
            case 145:
            case 148:
            {
                int     num105    = self.reader.ReadInt16();
                Vector2 position3 = self.reader.ReadVector2();
                Vector2 velocity3 = self.reader.ReadVector2();
                // int     stack3                                       = self.reader.Read7BitEncodedInt();
                // int     prefixWeWant2                                = self.reader.Read7BitEncodedInt();
                int   stack3                                       = self.reader.ReadInt16();
                int   prefixWeWant2                                = self.reader.ReadByte();
                int   num106                                       = self.reader.ReadByte();
                int   num107                                       = self.reader.ReadInt16();
                bool  shimmered                                    = false;
                float shimmerTime                                  = 0f;
                int   timeLeftInWhichTheItemCannotBeTakenByEnemies = 0;
                if (b == 145)
                {
                    shimmered   = self.reader.ReadBoolean();
                    shimmerTime = self.reader.ReadSingle();
                }
                if (b == 148)
                {
                    timeLeftInWhichTheItemCannotBeTakenByEnemies = self.reader.ReadByte();
                }
                if (Main.netMode == 1)
                {
                    if (num107 == 0)
                    {
                        Main.item[num105].active = false;
                        break;
                    }
                    int                     num108                  = num105;
                    Item                    item2                   = Main.item[num108];
                    ItemSyncPersistentStats itemSyncPersistentStats = default(ItemSyncPersistentStats);
                    itemSyncPersistentStats.CopyFrom(item2);
                    bool newAndShiny = (item2.newAndShiny || item2.netID != num107) && ItemSlot.Options.HighlightNewItems && (num107 < 0 || !ItemID.Sets.NeverAppearsAsNewInInventory[num107]);
                    item2.netDefaults(num107);
                    item2.newAndShiny = newAndShiny;
                    item2.Prefix(prefixWeWant2);
                    item2.stack = stack3;
                    // ItemIO.ReceiveModData(item2, self.reader);
                    item2.position    = position3;
                    item2.velocity    = velocity3;
                    item2.active      = true;
                    item2.shimmered   = shimmered;
                    item2.shimmerTime = shimmerTime;
                    if (b == 90)
                    {
                        item2.instanced                       = true;
                        item2.playerIndexTheItemIsReservedFor = Main.myPlayer;
                        item2.keepTime                        = 600;
                    }
                    item2.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
                    item2.wet                                          = Collision.WetCollision(item2.position, item2.width, item2.height);
                    itemSyncPersistentStats.PasteInto(item2);
                }
                else
                {
                    if (Main.timeItemSlotCannotBeReusedFor[num105] > 0)
                    {
                        break;
                    }
                    if (num107 == 0)
                    {
                        if (num105 < 400)
                        {
                            Main.item[num105].active = false;
                            NetMessage.TrySendData(21, -1, -1, null, num105);
                        }
                        break;
                    }
                    bool flag6 = false;
                    if (num105 == 400)
                    {
                        flag6 = true;
                    }
                    if (flag6)
                    {
                        Item item3 = new Item();
                        item3.netDefaults(num107);
                        num105 = Item.NewItem(new EntitySource_Sync(), (int)position3.X, (int)position3.Y, item3.width, item3.height, item3.type, stack3, noBroadcast: true);
                    }
                    Item item4 = Main.item[num105];
                    item4.netDefaults(num107);
                    item4.Prefix(prefixWeWant2);
                    item4.stack = stack3;
                    // ItemIO.ReceiveModData(item4, self.reader);
                    item4.position                                     = position3;
                    item4.velocity                                     = velocity3;
                    item4.active                                       = true;
                    item4.playerIndexTheItemIsReservedFor              = Main.myPlayer;
                    item4.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
                    if (b == 145)
                    {
                        item4.shimmered   = shimmered;
                        item4.shimmerTime = shimmerTime;
                    }
                    if (flag6)
                    {
                        NetMessage.TrySendData(b, -1, -1, null, num105);
                        if (num106 == 0)
                        {
                            Main.item[num105].ownIgnore = self.whoAmI;
                            Main.item[num105].ownTime   = 100;
                        }
                        Main.item[num105].FindOwner(num105);
                    }
                    else
                    {
                        NetMessage.TrySendData(b, -1, self.whoAmI, null, num105);
                    }
                }
                break;
            }

            case 22:
            {
                int num42 = self.reader.ReadInt16();
                int num43 = self.reader.ReadByte();
                if (Main.netMode != 2 || Main.item[num42].playerIndexTheItemIsReservedFor == self.whoAmI)
                {
                    Main.item[num42].playerIndexTheItemIsReservedFor = num43;
                    if (num43 == Main.myPlayer)
                    {
                        Main.item[num42].keepTime = 15;
                    }
                    else
                    {
                        Main.item[num42].keepTime = 0;
                    }
                    if (Main.netMode == 2)
                    {
                        Main.item[num42].playerIndexTheItemIsReservedFor = 255;
                        Main.item[num42].keepTime                        = 15;
                        NetMessage.TrySendData(22, -1, -1, null, num42);
                    }
                }
                break;
            }

            case 23:
            {
                if (Main.netMode != 1)
                {
                    break;
                }
                int     num155    = self.reader.ReadInt16();
                Vector2 vector5   = self.reader.ReadVector2();
                Vector2 velocity5 = self.reader.ReadVector2();
                int     num156    = self.reader.ReadUInt16();
                if (num156 == 65535)
                {
                    num156 = 0;
                }
                BitsByte bitsByte11 = self.reader.ReadByte();
                BitsByte bitsByte12 = self.reader.ReadByte();
                float[]  array2     = self.ReUseTemporaryNPCAI();
                for (int num157 = 0; num157 < NPC.maxAI; num157++)
                {
                    if (bitsByte11[num157 + 2])
                    {
                        array2[num157] = self.reader.ReadSingle();
                    }
                    else
                    {
                        array2[num157] = 0f;
                    }
                }
                int  num158                                      = self.reader.ReadInt16();
                int? playerCountForMultiplayerDifficultyOverride = 1;
                if (bitsByte12[0])
                {
                    playerCountForMultiplayerDifficultyOverride = self.reader.ReadByte();
                }
                float value8 = 1f;
                if (bitsByte12[2])
                {
                    value8 = self.reader.ReadSingle();
                }
                int num159 = 0;
                if (!bitsByte11[7])
                {
                    num159 = self.reader.ReadByte() switch
                    {
                        2 => self.reader.ReadInt16(),
                        4 => self.reader.ReadInt32(),
                        _ => self.reader.ReadSByte(),
                    };
                }
                int num160 = -1;
                NPC nPC5   = Main.npc[num155];
                if (nPC5.active && Main.multiplayerNPCSmoothingRange > 0 && Vector2.DistanceSquared(nPC5.position, vector5) < 640000f)
                {
                    nPC5.netOffset += nPC5.position - vector5;
                }
                if (!nPC5.active || nPC5.netID != num158)
                {
                    nPC5.netOffset *= 0f;
                    if (nPC5.active)
                    {
                        num160 = nPC5.type;
                    }
                    nPC5.active = true;
                    nPC5.SetDefaults(
                        num158,
                        new NPCSpawnParams
                        {
                            playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
                            strengthMultiplierOverride                  = value8
                        }
                    );
                }
                nPC5.position        = vector5;
                nPC5.velocity        = velocity5;
                nPC5.target          = num156;
                nPC5.direction       = (bitsByte11[0] ? 1 : (-1));
                nPC5.directionY      = (bitsByte11[1] ? 1 : (-1));
                nPC5.spriteDirection = (bitsByte11[6] ? 1 : (-1));
                if (bitsByte11[7])
                {
                    num159 = (nPC5.life = nPC5.lifeMax);
                }
                else
                {
                    nPC5.life = num159;
                }
                if (num159 <= 0)
                {
                    nPC5.active = false;
                }
                nPC5.SpawnedFromStatue = bitsByte12[1];
                if (nPC5.SpawnedFromStatue)
                {
                    nPC5.value = 0f;
                }
                for (int num161 = 0; num161 < NPC.maxAI; num161++)
                {
                    nPC5.ai[num161] = array2[num161];
                }
                if (num160 > -1 && num160 != nPC5.type)
                {
                    nPC5.TransformVisuals(num160, nPC5.type);
                }
                if (num158 == 262)
                {
                    NPC.plantBoss = num155;
                }
                if (num158 == 245)
                {
                    NPC.golemBoss = num155;
                }
                if (num158 == 668)
                {
                    NPC.deerclopsBoss = num155;
                }
                if (nPC5.type >= 0 && Main.npcCatchable[nPC5.type])
                {
                    nPC5.releaseOwner = self.reader.ReadByte();
                }
                // NPCLoader.ReceiveExtraAI(nPC5, NPCLoader.ReadExtraAI(self.reader));
                break;
            }

            case 27:
            {
                int      num34     = self.reader.ReadInt16();
                Vector2  position  = self.reader.ReadVector2();
                Vector2  velocity2 = self.reader.ReadVector2();
                int      num35     = self.reader.ReadByte();
                int      num36     = self.reader.ReadInt16();
                BitsByte bitsByte2 = self.reader.ReadByte();
                BitsByte bitsByte3 = (byte)(bitsByte2[2] ? self.reader.ReadByte() : 0);
                float[]  array     = self.ReUseTemporaryProjectileAI();
                array[0] = (bitsByte2[0] ? self.reader.ReadSingle() : 0f);
                array[1] = (bitsByte2[1] ? self.reader.ReadSingle() : 0f);
                int   bannerIdToRespondTo = (bitsByte2[3] ? self.reader.ReadUInt16() : 0);
                int   damage2             = (bitsByte2[4] ? self.reader.ReadInt16() : 0);
                float knockBack2          = (bitsByte2[5] ? self.reader.ReadSingle() : 0f);
                int   originalDamage      = (bitsByte2[6] ? self.reader.ReadInt16() : 0);
                int   num37               = (bitsByte2[7] ? self.reader.ReadInt16() : (-1));
                if (num37 >= 1000)
                {
                    num37 = -1;
                }
                array[2] = (bitsByte3[0] ? self.reader.ReadSingle() : 0f);
                // byte[] extraAI = (bitsByte3[1] ? ProjectileLoader.ReadExtraAI(self.reader) : null);
                if (Main.netMode == 2)
                {
                    if (num36 == 949)
                    {
                        num35 = 255;
                    }
                    else
                    {
                        num35 = self.whoAmI;
                        if (Main.projHostile[num36])
                        {
                            break;
                        }
                    }
                }
                int num38 = 1000;
                for (int n = 0; n < 1000; n++)
                {
                    if (Main.projectile[n].owner == num35 && Main.projectile[n].identity == num34 && Main.projectile[n].active)
                    {
                        num38 = n;
                        break;
                    }
                }
                if (num38 == 1000)
                {
                    for (int num39 = 0; num39 < 1000; num39++)
                    {
                        if (!Main.projectile[num39].active)
                        {
                            num38 = num39;
                            break;
                        }
                    }
                }
                if (num38 == 1000)
                {
                    num38 = Projectile.FindOldestProjectile();
                }
                Projectile projectile = Main.projectile[num38];
                if (!projectile.active || projectile.type != num36)
                {
                    projectile.SetDefaults(num36);
                    if (Main.netMode == 2)
                    {
                        Netplay.Clients[self.whoAmI].SpamProjectile += 1f;
                    }
                }
                projectile.identity            = num34;
                projectile.position            = position;
                projectile.velocity            = velocity2;
                projectile.type                = num36;
                projectile.damage              = damage2;
                projectile.bannerIdToRespondTo = bannerIdToRespondTo;
                projectile.originalDamage      = originalDamage;
                projectile.knockBack           = knockBack2;
                projectile.owner               = num35;
                for (int num40 = 0; num40 < Projectile.maxAI; num40++)
                {
                    projectile.ai[num40] = array[num40];
                }
                if (num37 >= 0)
                {
                    projectile.projUUID                   = num37;
                    Main.projectileIdentity[num35, num37] = num38;
                }
                // if (extraAI != null)
                // {
                //     ProjectileLoader.ReceiveExtraAI(projectile, extraAI);
                // }
                projectile.ProjectileFixDesperation();
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(27, -1, self.whoAmI, null, num38);
                }
                break;
            }

            case 28:
            {
                int num190 = self.reader.ReadInt16();
                // int         num191 = self.reader.Read7BitEncodedInt();
                // NPC.HitInfo hit    = default(NPC.HitInfo);
                // if (num191 >= 0)
                // {
                //     NPC.HitInfo hitInfo = new NPC.HitInfo();
                //     hitInfo.Damage       = num191;
                //     hitInfo.SourceDamage = self.reader.Read7BitEncodedInt();
                //     hitInfo.DamageType   = DamageClassLoader.DamageClasses[self.reader.Read7BitEncodedInt()];
                //     hitInfo.HitDirection = self.reader.ReadSByte();
                //     hitInfo.Knockback    = self.reader.ReadSingle();
                //     hit                  = hitInfo;
                //     BitsByte flags = self.reader.ReadByte();
                //     hit.Crit           = flags[0];
                //     hit.InstantKill    = flags[1];
                //     hit.HideCombatText = flags[2];
                // }
                int   num191 = self.reader.ReadInt16();
                float num192 = self.reader.ReadSingle();
                int   num193 = self.reader.ReadByte() - 1;
                byte  b10    = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    if (num191 < 0)
                    {
                        num191 = 0;
                    }
                    Main.npc[num190].PlayerInteraction(self.whoAmI);
                }
                if (num191 >= 0)
                {
                    // Main.npc[num190].StrikeNPC(hit, fromNet: true);
                    Main.npc[num190].StrikeNPC(num191, num192, num193, b10 == 1, /*noEffect: false,*/ fromNet: true);
                }
                else
                {
                    Main.npc[num190].life = 0;
                    Main.npc[num190].HitEffect();
                    Main.npc[num190].active = false;
                }
                if (Main.netMode != 2)
                {
                    break;
                }
                // NetMessage.SendStrikeNPC(Main.npc[num190], in hit, self.whoAmI);
                NetMessage.TrySendData(28, -1, self.whoAmI, null, num190, num191, num192, num193, b10);
                if (Main.npc[num190].life <= 0)
                {
                    NetMessage.TrySendData(23, -1, -1, null, num190);
                }
                else
                {
                    Main.npc[num190].netUpdate = true;
                }
                if (Main.npc[num190].realLife >= 0)
                {
                    if (Main.npc[Main.npc[num190].realLife].life <= 0)
                    {
                        NetMessage.TrySendData(23, -1, -1, null, Main.npc[num190].realLife);
                    }
                    else
                    {
                        Main.npc[Main.npc[num190].realLife].netUpdate = true;
                    }
                }
                break;
            }

            case 29:
            {
                int num98 = self.reader.ReadInt16();
                int num99 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num99 = self.whoAmI;
                }
                for (int num100 = 0; num100 < 1000; num100++)
                {
                    if (Main.projectile[num100].owner == num99 && Main.projectile[num100].identity == num98 && Main.projectile[num100].active)
                    {
                        Main.projectile[num100].Kill();
                        break;
                    }
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(29, -1, self.whoAmI, null, num98, num99);
                }
                break;
            }

            case 30:
            {
                int num137 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num137 = self.whoAmI;
                }
                bool flag8 = self.reader.ReadBoolean();
                Main.player[num137].hostile = flag8;
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(30, -1, self.whoAmI, null, num137);
                    LocalizedText obj2 = (flag8 ? Lang.mp[11] : Lang.mp[12]);
                    ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num137].team], text: NetworkText.FromKey(obj2.Key, Main.player[num137].name));
                }
                break;
            }

            case 31:
            {
                if (Main.netMode != 2)
                {
                    break;
                }
                int num219 = self.reader.ReadInt16();
                int num220 = self.reader.ReadInt16();
                int num221 = Chest.FindChest(num219, num220);
                if (num221 > -1 && Chest.UsingChest(num221) == -1)
                {
                    for (int num222 = 0; num222 < 40; num222++)
                    {
                        NetMessage.TrySendData(32, self.whoAmI, -1, null, num221, num222);
                    }
                    NetMessage.TrySendData(33, self.whoAmI, -1, null, num221);
                    Main.player[self.whoAmI].chest = num221;
                    if (Main.myPlayer == self.whoAmI)
                    {
                        Main.recBigList = false;
                    }
                    NetMessage.TrySendData(80, -1, self.whoAmI, null, self.whoAmI, num221);
                    if (Main.netMode == 2 && WorldGen.IsChestRigged(num219, num220))
                    {
                        Wiring.SetCurrentUser(self.whoAmI);
                        Wiring.HitSwitch(num219, num220);
                        Wiring.SetCurrentUser();
                        NetMessage.TrySendData(59, -1, self.whoAmI, null, num219, num220);
                    }
                }
                break;
            }

            case 32:
            {
                int num164 = self.reader.ReadInt16();
                int num165 = self.reader.ReadByte();
                // int stack5        = -1;
                // int prefixWeWant3 = -1;
                // int type8         = -1;
                int stack5        = self.reader.ReadInt16();
                int prefixWeWant3 = self.reader.ReadByte();
                int type8         = self.reader.ReadInt16();
                if (num164 >= 0 && num164 < 8000)
                {
                    if (Main.chest[num164] == null)
                    {
                        Main.chest[num164] = new Chest();
                    }
                    if (Main.chest[num164].item[num165] == null)
                    {
                        Main.chest[num164].item[num165] = new Item();
                    }
                    // ItemIO.Receive(Main.chest[num164].item[num165], self.reader, readStack: true);
                    Main.chest[num164].item[num165].netDefaults(type8);
                    Main.chest[num164].item[num165].Prefix(prefixWeWant3);
                    Main.chest[num164].item[num165].stack = stack5;
                    Recipe.FindRecipes(canDelayCheck: true);
                }
                break;
            }

            case 33:
            {
                int    num2 = self.reader.ReadInt16();
                int    num3 = self.reader.ReadInt16();
                int    num4 = self.reader.ReadInt16();
                int    num5 = self.reader.ReadByte();
                string name = string.Empty;
                if (num5 != 0)
                {
                    if (num5 <= 20 /*63*/)
                    {
                        name = self.reader.ReadString();
                    }
                    else if (num5 != 255)
                    {
                        num5 = 0;
                    }
                }
                if (Main.netMode == 1)
                {
                    Player player = Main.player[Main.myPlayer];
                    if (player.chest == -1)
                    {
                        Main.playerInventory = true;
                        SoundEngine.PlaySound(10);
                    }
                    else if (player.chest != num2 && num2 != -1)
                    {
                        Main.playerInventory = true;
                        SoundEngine.PlaySound(12);
                        Main.recBigList = false;
                    }
                    else if (player.chest != -1 && num2 == -1)
                    {
                        SoundEngine.PlaySound(11);
                        Main.recBigList = false;
                    }
                    player.chest  = num2;
                    player.chestX = num3;
                    player.chestY = num4;
                    Recipe.FindRecipes(canDelayCheck: true);
                    if (Main.tile[num3, num4].frameX >= 36 && Main.tile[num3, num4].frameX < 72)
                    {
                        AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
                    }
                }
                else
                {
                    if (num5 != 0)
                    {
                        int   chest  = Main.player[self.whoAmI].chest;
                        Chest chest2 = Main.chest[chest];
                        chest2.name = name;
                        NetMessage.TrySendData(69, -1, self.whoAmI, null, chest, chest2.x, chest2.y);
                    }
                    Main.player[self.whoAmI].chest = num2;
                    Recipe.FindRecipes(canDelayCheck: true);
                    NetMessage.TrySendData(80, -1, self.whoAmI, null, self.whoAmI, num2);
                }
                break;
            }

            case 34:
            {
                byte b12    = self.reader.ReadByte();
                int  num203 = self.reader.ReadInt16();
                int  num204 = self.reader.ReadInt16();
                int  num205 = self.reader.ReadInt16();
                int  num206 = self.reader.ReadInt16();
                if (Main.netMode == 2)
                {
                    num206 = 0;
                }
                // ushort modType = 0;
                // if (b12 >= 100)
                // {
                //     modType = self.reader.ReadUInt16();
                // }
                if (Main.netMode == 2)
                {
                    // if (b12 % 100 == 0)
                    // {
                    //     if (modType == 0)
                    //     {
                    //         modType = 21;
                    //     }
                    //     int num209 = WorldGen.PlaceChest(num203, num204, modType, notNearOtherChests: false, num205);
                    //     if (num209 == -1)
                    //     {
                    //         NetMessage.TrySendData(34, self.whoAmI, -1, null, b12, num203, num204, num205, num209, modType);
                    //         int itemSpawn = ((b12 < 100) ? Chest.chestItemSpawn[num205] : TileLoader.GetItemDropFromTypeAndStyle(modType, num205));
                    //         if (itemSpawn > 0)
                    //         {
                    //             Item.NewItem(new EntitySource_TileBreak(num203, num204), num203 * 16, num204 * 16, 32, 32, itemSpawn, 1, noBroadcast: true);
                    //         }
                    //     }
                    //     else
                    //     {
                    //         NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, num205, num209, modType);
                    //     }
                    //     break;
                    // }
                    // if (b12 % 100 == 1 && (Main.tile[num203, num204].type == 21 || (b12 == 101 && TileID.Sets.BasicChest[Main.tile[num203, num204].type])))
                    // {
                    //     Tile tile = Main.tile[num203, num204];
                    //     if (tile.frameX % 36 != 0)
                    //     {
                    //         num203--;
                    //     }
                    //     if (tile.frameY % 36 != 0)
                    //     {
                    //         num204--;
                    //     }
                    //     int number = Chest.FindChest(num203, num204);
                    //     WorldGen.KillTile(num203, num204);
                    //     if (!tile.active())
                    //     {
                    //         NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, 0f, number);
                    //     }
                    //     break;
                    // }
                    // if (b12 % 100 == 2)
                    // {
                    //     if (modType == 0)
                    //     {
                    //         modType = 88;
                    //     }
                    //     int num207 = WorldGen.PlaceChest(num203, num204, modType, notNearOtherChests: false, num205);
                    //     if (num207 == -1)
                    //     {
                    //         NetMessage.TrySendData(34, self.whoAmI, -1, null, b12, num203, num204, num205, num207, modType);
                    //         int itemSpawn = ((b12 < 100) ? Chest.dresserItemSpawn[num205] : TileLoader.GetItemDropFromTypeAndStyle(modType, num205));
                    //         if (itemSpawn > 0)
                    //         {
                    //             Item.NewItem(new EntitySource_TileBreak(num203, num204), num203 * 16, num204 * 16, 32, 32, itemSpawn, 1, noBroadcast: true);
                    //         }
                    //     }
                    //     else
                    //     {
                    //         NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, num205, num207, modType);
                    //     }
                    //     break;
                    // }
                    // if (b12 % 100 == 3 && (Main.tile[num203, num204].type == 88 || (b12 == 103 && TileID.Sets.BasicDresser[Main.tile[num203, num204].type])))
                    // {
                    //     Tile tile2 = Main.tile[num203, num204];
                    //     num203 -= tile2.frameX % 54 / 18;
                    //     if (tile2.frameY % 36 != 0)
                    //     {
                    //         num204--;
                    //     }
                    //     int number2 = Chest.FindChest(num203, num204);
                    //     WorldGen.KillTile(num203, num204);
                    //     if (!tile2.active())
                    //     {
                    //         NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, 0f, number2);
                    //     }
                    //     break;
                    // }
                    // switch (b12)
                    // {
                    //     case 4:
                    //     {
                    //         int num208 = WorldGen.PlaceChest(num203, num204, 467, notNearOtherChests: false, num205);
                    //         if (num208 == -1)
                    //         {
                    //             NetMessage.TrySendData(34, self.whoAmI, -1, null, b12, num203, num204, num205, num208);
                    //             Item.NewItem(new EntitySource_TileBreak(num203, num204), num203 * 16, num204 * 16, 32, 32, Chest.chestItemSpawn2[num205], 1, noBroadcast: true);
                    //         }
                    //         else
                    //         {
                    //             NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, num205, num208);
                    //         }
                    //         break;
                    //     }
                    // 
                    //     case 5:
                    //         if (Main.tile[num203, num204].type == 467)
                    //         {
                    //             Tile tile3 = Main.tile[num203, num204];
                    //             if (tile3.frameX % 36 != 0)
                    //             {
                    //                 num203--;
                    //             }
                    //             if (tile3.frameY % 36 != 0)
                    //             {
                    //                 num204--;
                    //             }
                    //             int number3 = Chest.FindChest(num203, num204);
                    //             WorldGen.KillTile(num203, num204);
                    //             if (!tile3.active())
                    //             {
                    //                 NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, 0f, number3);
                    //             }
                    //         }
                    //         break;
                    // }
                    // break;

                    switch (b12)
                    {
                        case 0:
                        {
                            int num209 = WorldGen.PlaceChest(num203, num204, 21, notNearOtherChests: false, num205);
                            if (num209 == -1)
                            {
                                NetMessage.TrySendData(34, self.whoAmI, -1, null, b12, num203, num204, num205, num209);
                                Item.NewItem(new EntitySource_TileBreak(num203, num204), num203 * 16, num204 * 16, 32, 32, Chest.chestItemSpawn[num205], 1, noBroadcast: true);
                            }
                            else
                            {
                                NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, num205, num209);
                            }
                            break;
                        }

                        case 1:
                            if (Main.tile[num203, num204].type == 21)
                            {
                                Tile tile = Main.tile[num203, num204];
                                if (tile.frameX % 36 != 0)
                                {
                                    num203--;
                                }
                                if (tile.frameY % 36 != 0)
                                {
                                    num204--;
                                }
                                int number = Chest.FindChest(num203, num204);
                                WorldGen.KillTile(num203, num204);
                                if (!tile.active())
                                {
                                    NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, 0f, number);
                                }
                                break;
                            }
                            goto default;

                        default:
                            switch (b12)
                            {
                                case 2:
                                {
                                    int num207 = WorldGen.PlaceChest(num203, num204, 88, notNearOtherChests: false, num205);
                                    if (num207 == -1)
                                    {
                                        NetMessage.TrySendData(34, self.whoAmI, -1, null, b12, num203, num204, num205, num207);
                                        Item.NewItem(new EntitySource_TileBreak(num203, num204), num203 * 16, num204 * 16, 32, 32, Chest.dresserItemSpawn[num205], 1, noBroadcast: true);
                                    }
                                    else
                                    {
                                        NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, num205, num207);
                                    }
                                    break;
                                }

                                case 3:
                                    if (Main.tile[num203, num204].type == 88)
                                    {
                                        Tile tile2 = Main.tile[num203, num204];
                                        num203 -= tile2.frameX % 54 / 18;
                                        if (tile2.frameY % 36 != 0)
                                        {
                                            num204--;
                                        }
                                        int number2 = Chest.FindChest(num203, num204);
                                        WorldGen.KillTile(num203, num204);
                                        if (!tile2.active())
                                        {
                                            NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, 0f, number2);
                                        }
                                        break;
                                    }
                                    goto default;

                                default:
                                    switch (b12)
                                    {
                                        case 4:
                                        {
                                            int num208 = WorldGen.PlaceChest(num203, num204, 467, notNearOtherChests: false, num205);
                                            if (num208 == -1)
                                            {
                                                NetMessage.TrySendData(34, self.whoAmI, -1, null, b12, num203, num204, num205, num208);
                                                Item.NewItem(new EntitySource_TileBreak(num203, num204), num203 * 16, num204 * 16, 32, 32, Chest.chestItemSpawn2[num205], 1, noBroadcast: true);
                                            }
                                            else
                                            {
                                                NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, num205, num208);
                                            }
                                            break;
                                        }

                                        case 5:
                                            if (Main.tile[num203, num204].type == 467)
                                            {
                                                Tile tile3 = Main.tile[num203, num204];
                                                if (tile3.frameX % 36 != 0)
                                                {
                                                    num203--;
                                                }
                                                if (tile3.frameY % 36 != 0)
                                                {
                                                    num204--;
                                                }
                                                int number3 = Chest.FindChest(num203, num204);
                                                WorldGen.KillTile(num203, num204);
                                                if (!tile3.active())
                                                {
                                                    NetMessage.TrySendData(34, -1, -1, null, b12, num203, num204, 0f, number3);
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                }
                // byte b17 = b12;
                // if (b12 % 100 == 0)
                // {
                //     if (num206 == -1)
                //     {
                //         WorldGen.KillTile(num203, num204);
                //         break;
                //     }
                //     SoundEngine.PlaySound(0, num203 * 16, num204 * 16);
                //     if (modType == 0)
                //     {
                //         modType = 21;
                //     }
                //     WorldGen.PlaceChestDirect(num203, num204, modType, num205, num206);
                // }
                // else if (b12 % 100 != 2)
                // {
                //     if (b17 == 4)
                //     {
                //         if (num206 == -1)
                //         {
                //             WorldGen.KillTile(num203, num204);
                //             break;
                //         }
                //         SoundEngine.PlaySound(0, num203 * 16, num204 * 16);
                //         WorldGen.PlaceChestDirect(num203, num204, 467, num205, num206);
                //     }
                //     else
                //     {
                //         Chest.DestroyChestDirect(num203, num204, num206);
                //         WorldGen.KillTile(num203, num204);
                //     }
                // }
                // else if (num206 == -1)
                // {
                //     WorldGen.KillTile(num203, num204);
                // }
                // else
                // {
                //     SoundEngine.PlaySound(0, num203 * 16, num204 * 16);
                //     if (modType == 0)
                //     {
                //         modType = 88;
                //     }
                //     WorldGen.PlaceDresserDirect(num203, num204, modType, num205, num206);
                // }
                // break;

                switch (b12)
                {
                    case 0:
                        if (num206 == -1)
                        {
                            WorldGen.KillTile(num203, num204);
                            break;
                        }
                        SoundEngine.PlaySound(0, num203 * 16, num204 * 16);
                        WorldGen.PlaceChestDirect(num203, num204, 21, num205, num206);
                        break;

                    case 2:
                        if (num206 == -1)
                        {
                            WorldGen.KillTile(num203, num204);
                            break;
                        }
                        SoundEngine.PlaySound(0, num203 * 16, num204 * 16);
                        WorldGen.PlaceDresserDirect(num203, num204, 88, num205, num206);
                        break;

                    case 4:
                        if (num206 == -1)
                        {
                            WorldGen.KillTile(num203, num204);
                            break;
                        }
                        SoundEngine.PlaySound(0, num203 * 16, num204 * 16);
                        WorldGen.PlaceChestDirect(num203, num204, 467, num205, num206);
                        break;

                    default:
                        Chest.DestroyChestDirect(num203, num204, num206);
                        WorldGen.KillTile(num203, num204);
                        break;
                }
                break;
            }

            case 35:
            {
                int num146 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num146 = self.whoAmI;
                }
                int num147 = self.reader.ReadInt16();
                if (num146 != Main.myPlayer || Main.ServerSideCharacter)
                {
                    Main.player[num146].HealEffect(num147);
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(35, -1, self.whoAmI, null, num146, num147);
                }
                break;
            }

            case 36:
            {
                int num101 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num101 = self.whoAmI;
                }
                Player player7 = Main.player[num101];
                bool   flag5   = player7.zone5[0];
                player7.zone1 = self.reader.ReadByte();
                player7.zone2 = self.reader.ReadByte();
                player7.zone3 = self.reader.ReadByte();
                player7.zone4 = self.reader.ReadByte();
                player7.zone5 = self.reader.ReadByte();
                // BiomeLoader.ReceiveCustomBiomes(player7, self.reader);
                // player7.ZonePurity = player7.InZonePurity();
                if (Main.netMode == 2)
                {
                    if (!flag5 && player7.zone5[0])
                    {
                        NPC.SpawnFaelings(num101);
                    }
                    NetMessage.TrySendData(36, -1, self.whoAmI, null, num101);
                }
                break;
            }

            case 37:
                if (Main.netMode == 1)
                {
                    if (Main.autoPass)
                    {
                        NetMessage.TrySendData(38);
                        Main.autoPass = false;
                    }
                    else
                    {
                        Netplay.ServerPassword = "";
                        Main.menuMode          = 31;
                    }
                }
                break;

            case 38:
                if (Main.netMode != 2)
                {
                    break;
                }
                if (self.reader.ReadString() == Netplay.ServerPassword)
                {
                    Netplay.Clients[self.whoAmI].State = 1;
                    if (false /*ModNet.isModdedClient[self.whoAmI]*/)
                    {
                        ModNet.SyncMods(self.whoAmI);
                    }
                    else
                    {
                        NetMessage.TrySendData(3, self.whoAmI);
                    }
                }
                else
                {
                    NetMessage.TrySendData(2, self.whoAmI, -1, Lang.mp[1].ToNetworkText());
                }
                break;

            case 39:
                if (Main.netMode == 1)
                {
                    int num23 = self.reader.ReadInt16();
                    Main.item[num23].playerIndexTheItemIsReservedFor = 255;
                    NetMessage.TrySendData(22, -1, -1, null, num23);
                }
                break;

            case 40:
            {
                int num255 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num255 = self.whoAmI;
                }
                int npcIndex = self.reader.ReadInt16();
                Main.player[num255].SetTalkNPC(npcIndex, fromNet: true);
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(40, -1, self.whoAmI, null, num255);
                }
                break;
            }

            case 41:
            {
                int num226 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num226 = self.whoAmI;
                }
                Player player15     = Main.player[num226];
                float  itemRotation = self.reader.ReadSingle();
                self.reader.ReadInt16();
                player15.itemRotation = itemRotation;
                player15.channel      = player15.inventory[player15.selectedItem].channel;
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(41, -1, self.whoAmI, null, num226);
                }
                break;
            }

            case 42:
            {
                int num202 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num202 = self.whoAmI;
                }
                else if (Main.myPlayer == num202 && !Main.ServerSideCharacter)
                {
                    break;
                }
                int statMana    = self.reader.ReadInt16();
                int statManaMax = self.reader.ReadInt16();
                Main.player[num202].statMana    = statMana;
                Main.player[num202].statManaMax = statManaMax;
                SyncStatManaMax(Main.player[num202]);
                break;
            }

            case 43:
            {
                int num151 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num151 = self.whoAmI;
                }
                int num152 = self.reader.ReadInt16();
                if (num151 != Main.myPlayer)
                {
                    Main.player[num151].ManaEffect(num152);
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(43, -1, self.whoAmI, null, num151, num152);
                }
                break;
            }

            case 45:
            {
                int num117 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num117 = self.whoAmI;
                }
                int    num118  = self.reader.ReadByte();
                Player player8 = Main.player[num117];
                int    team    = player8.team;
                player8.team = num118;
                Color color = Main.teamColor[num118];
                if (Main.netMode != 2)
                {
                    break;
                }
                NetMessage.TrySendData(45, -1, self.whoAmI, null, num117);
                LocalizedText localizedText = Lang.mp[13 + num118];
                if (num118 == 5)
                {
                    localizedText = Lang.mp[22];
                }
                for (int num119 = 0; num119 < 255; num119++)
                {
                    if (num119 == self.whoAmI || (team > 0 && Main.player[num119].team == team) || (num118 > 0 && Main.player[num119].team == num118))
                    {
                        ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player8.name), color, num119);
                    }
                }
                break;
            }

            case 46:
                if (Main.netMode == 2)
                {
                    short i3     = self.reader.ReadInt16();
                    int   j3     = self.reader.ReadInt16();
                    int   num111 = Sign.ReadSign(i3, j3);
                    if (num111 >= 0)
                    {
                        NetMessage.TrySendData(47, self.whoAmI, -1, null, num111, self.whoAmI);
                    }
                }
                break;

            case 47:
            {
                int      num8     = self.reader.ReadInt16();
                int      x        = self.reader.ReadInt16();
                int      y        = self.reader.ReadInt16();
                string   text     = self.reader.ReadString();
                int      num9     = self.reader.ReadByte();
                BitsByte bitsByte = self.reader.ReadByte();
                if (num8 >= 0 && num8 < 1000)
                {
                    string text2 = null;
                    if (Main.sign[num8] != null)
                    {
                        text2 = Main.sign[num8].text;
                    }
                    Main.sign[num8]   = new Sign();
                    Main.sign[num8].x = x;
                    Main.sign[num8].y = y;
                    Sign.TextSign(num8, text);
                    if (Main.netMode == 2 && text2 != text)
                    {
                        num9 = self.whoAmI;
                        NetMessage.TrySendData(47, -1, self.whoAmI, null, num8, num9);
                    }
                    if (Main.netMode == 1 && num9 == Main.myPlayer && Main.sign[num8] != null && !bitsByte[0])
                    {
                        Main.playerInventory = false;
                        Main.player[Main.myPlayer].SetTalkNPC(-1, fromNet: true);
                        Main.npcChatCornerItem = 0;
                        Main.editSign          = false;
                        SoundEngine.PlaySound(10);
                        Main.player[Main.myPlayer].sign = num8;
                        Main.npcChatText                = Main.sign[num8].text;
                    }
                }
                break;
            }

            case 48:
            {
                int  num237     = self.reader.ReadInt16();
                int  num238     = self.reader.ReadInt16();
                byte b15        = self.reader.ReadByte();
                byte liquidType = self.reader.ReadByte();
                if (Main.netMode == 2 && Netplay.SpamCheck)
                {
                    int num239 = self.whoAmI;
                    int num240 = (int)(Main.player[num239].position.X + (float)(Main.player[num239].width  / 2));
                    int num262 = (int)(Main.player[num239].position.Y + (float)(Main.player[num239].height / 2));
                    int num242 = 10;
                    int num243 = num240 - num242;
                    int num244 = num240 + num242;
                    int num245 = num262 - num242;
                    int num246 = num262 + num242;
                    if (num237 < num243 || num237 > num244 || num238 < num245 || num238 > num246)
                    {
                        Netplay.Clients[self.whoAmI].SpamWater += 1f;
                    }
                }
                if (Main.tile[num237, num238] == null)
                {
                    Main.tile[num237, num238] = default(Tile);
                }
                Main.tile[num237, num238].liquid = b15;
                Main.tile[num237, num238].liquidType(liquidType);
                if (Main.netMode == 2)
                {
                    WorldGen.SquareTileFrame(num237, num238);
                    if (b15 == 0)
                    {
                        NetMessage.SendData(48, -1, self.whoAmI, null, num237, num238);
                    }
                }
                break;
            }

            case 49:
                if (Netplay.Connection.State == 6)
                {
                    Netplay.Connection.State = 10;
                    Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
                }
                break;

            case 50:
            {
                int num180 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num180 = self.whoAmI;
                }
                else if (num180 == Main.myPlayer && !Main.ServerSideCharacter)
                {
                    break;
                }
                Player player13 = Main.player[num180];
                for (int num181 = 0; num181 < Player.maxBuffs; num181++)
                {
                    player13.buffType[num181] = self.reader.ReadUInt16();
                    if (player13.buffType[num181] > 0)
                    {
                        player13.buffTime[num181] = 60;
                    }
                    else
                    {
                        player13.buffTime[num181] = 0;
                    }
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(50, -1, self.whoAmI, null, num180);
                }
                break;
            }

            case 51:
            {
                byte b8 = self.reader.ReadByte();
                byte b9 = self.reader.ReadByte();
                switch (b9)
                {
                    case 1:
                        NPC.SpawnSkeletron(b8);
                        break;

                    case 2:
                        if (Main.netMode == 2)
                        {
                            NetMessage.TrySendData(51, -1, self.whoAmI, null, b8, (int)b9);
                        }
                        else
                        {
                            SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[b8].position.X, (int)Main.player[b8].position.Y);
                        }
                        break;

                    case 3:
                        if (Main.netMode == 2)
                        {
                            Main.Sundialing();
                        }
                        break;

                    case 4:
                        Main.npc[b8].BigMimicSpawnSmoke();
                        break;

                    case 5:
                        if (Main.netMode == 2)
                        {
                            NPC nPC6 = new NPC();
                            nPC6.SetDefaults(664);
                            Main.BestiaryTracker.Kills.RegisterKill(nPC6);
                        }
                        break;

                    case 6:
                        if (Main.netMode == 2)
                        {
                            Main.Moondialing();
                        }
                        break;
                }
                break;
            }

            case 52:
            {
                int num148 = self.reader.ReadByte();
                int num149 = self.reader.ReadInt16();
                int num150 = self.reader.ReadInt16();
                if (num148 == 1)
                {
                    Chest.Unlock(num149, num150);
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(52, -1, self.whoAmI, null, 0, num148, num149, num150);
                        NetMessage.SendTileSquare(-1, num149, num150, 2);
                    }
                }
                if (num148 == 2)
                {
                    WorldGen.UnlockDoor(num149, num150);
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(52, -1, self.whoAmI, null, 0, num148, num149, num150);
                        NetMessage.SendTileSquare(-1, num149, num150, 2);
                    }
                }
                if (num148 == 3)
                {
                    Chest.Lock(num149, num150);
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(52, -1, self.whoAmI, null, 0, num148, num149, num150);
                        NetMessage.SendTileSquare(-1, num149, num150, 2);
                    }
                }
                break;
            }

            case 53:
            {
                int num122 = self.reader.ReadInt16();
                int type6  = self.reader.ReadUInt16();
                int time2  = self.reader.ReadInt16();
                Main.npc[num122].AddBuff(type6, time2, quiet: true);
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(54, -1, -1, null, num122);
                }
                break;
            }

            case 54:
                if (Main.netMode == 1)
                {
                    int num109 = self.reader.ReadInt16();
                    NPC nPC4   = Main.npc[num109];
                    for (int num110 = 0; num110 < NPC.maxBuffs; num110++)
                    {
                        nPC4.buffType[num110] = self.reader.ReadUInt16();
                        nPC4.buffTime[num110] = self.reader.ReadInt16();
                    }
                }
                break;

            case 55:
            {
                int num48 = self.reader.ReadByte();
                int num49 = self.reader.ReadUInt16();
                int num50 = self.reader.ReadInt32();
                if (Main.netMode != 2 || num48 == self.whoAmI || Main.pvpBuff[num49])
                {
                    if (Main.netMode == 1 && num48 == Main.myPlayer)
                    {
                        Main.player[num48].AddBuff(num49, num50);
                    }
                    else if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(55, -1, -1, null, num48, num49, num50);
                    }
                }
                break;
            }

            case 56:
            {
                int num27 = self.reader.ReadInt16();
                if (num27 >= 0 && num27 < 200)
                {
                    if (Main.netMode == 1)
                    {
                        string givenName = self.reader.ReadString();
                        Main.npc[num27].GivenName = givenName;
                        int townNpcVariationIndex = self.reader.ReadInt32();
                        Main.npc[num27].townNpcVariationIndex = townNpcVariationIndex;
                    }
                    else if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(56, self.whoAmI, -1, null, num27);
                    }
                }
                break;
            }

            case 57:
                if (Main.netMode == 1)
                {
                    WorldGen.tGood  = self.reader.ReadByte();
                    WorldGen.tEvil  = self.reader.ReadByte();
                    WorldGen.tBlood = self.reader.ReadByte();
                }
                break;

            case 58:
            {
                int num256 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num256 = self.whoAmI;
                }
                float num257 = self.reader.ReadSingle();
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(58, -1, self.whoAmI, null, self.whoAmI, num257);
                    break;
                }
                Player player16 = Main.player[num256];
                int    type14   = player16.inventory[player16.selectedItem].type;
                switch (type14)
                {
                    case 4057:
                    case 4372:
                    case 4715:
                        player16.PlayGuitarChord(num257);
                        break;

                    case 4673:
                        player16.PlayDrums(num257);
                        break;

                    default:
                    {
                        Main.musicPitch = num257;
                        SoundStyle type15 = SoundID.Item26;
                        if (type14 == 507)
                        {
                            type15 = SoundID.Item35;
                        }
                        if (type14 == 1305)
                        {
                            type15 = SoundID.Item47;
                        }
                        SoundEngine.PlaySound(in type15, player16.position);
                        break;
                    }
                }
                break;
            }

            case 59:
            {
                int num10 = self.reader.ReadInt16();
                int num11 = self.reader.ReadInt16();
                Wiring.SetCurrentUser(self.whoAmI);
                Wiring.HitSwitch(num10, num11);
                Wiring.SetCurrentUser();
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(59, -1, self.whoAmI, null, num10, num11);
                }
                break;
            }

            case 60:
            {
                int  num250 = self.reader.ReadInt16();
                int  num251 = self.reader.ReadInt16();
                int  num252 = self.reader.ReadInt16();
                byte b16    = self.reader.ReadByte();
                if (num250 >= 200)
                {
                    NetMessage.BootPlayer(self.whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
                    break;
                }
                NPC  nPC7           = Main.npc[num250];
                bool isLikeATownNPC = nPC7.isLikeATownNPC;
                if (Main.netMode == 1)
                {
                    nPC7.homeless  = b16 == 1;
                    nPC7.homeTileX = num251;
                    nPC7.homeTileY = num252;
                }
                if (!isLikeATownNPC)
                {
                    break;
                }
                if (Main.netMode == 1)
                {
                    switch (b16)
                    {
                        case 1:
                            WorldGen.TownManager.KickOut(nPC7.type);
                            break;

                        case 2:
                            WorldGen.TownManager.SetRoom(nPC7.type, num251, num252);
                            break;
                    }
                }
                else if (b16 == 1)
                {
                    WorldGen.kickOut(num250);
                }
                else
                {
                    WorldGen.moveRoom(num251, num252, num250);
                }
                break;
            }

            case 61:
            {
                int num215 = self.reader.ReadInt16();
                int num216 = self.reader.ReadInt16();
                if (Main.netMode != 2)
                {
                    break;
                }
                if (num216 >= 0 && NPCID.Sets.MPAllowedEnemies[num216])
                {
                    if (!NPC.AnyNPCs(num216))
                    {
                        NPC.SpawnOnPlayer(num215, num216);
                    }
                }
                else if (num216 == -4)
                {
                    if (!Main.dayTime && !DD2Event.Ongoing)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, 255, 130));
                        Main.startPumpkinMoon();
                        NetMessage.TrySendData(7);
                        NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f);
                    }
                }
                else if (num216 == -5)
                {
                    if (!Main.dayTime && !DD2Event.Ongoing)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, 255, 130));
                        Main.startSnowMoon();
                        NetMessage.TrySendData(7);
                        NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f);
                    }
                }
                else if (num216 == -6)
                {
                    if (Main.dayTime && !Main.eclipse)
                    {
                        if (Main.remixWorld)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[106].Key), new Color(50, 255, 130));
                        }
                        else
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), new Color(50, 255, 130));
                        }
                        Main.eclipse = true;
                        NetMessage.TrySendData(7);
                    }
                }
                else if (num216 == -7)
                {
                    Main.invasionDelay = 0;
                    Main.StartInvasion(4);
                    NetMessage.TrySendData(7);
                    NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
                }
                else if (num216 == -8)
                {
                    if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
                    {
                        WorldGen.StartImpendingDoom(720);
                        NetMessage.TrySendData(7);
                    }
                }
                else if (num216 == -10)
                {
                    if (!Main.dayTime && !Main.bloodMoon)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), new Color(50, 255, 130));
                        Main.bloodMoon = true;
                        if (Main.GetMoonPhase() == MoonPhase.Empty)
                        {
                            Main.moonPhase = 5;
                        }
                        AchievementsHelper.NotifyProgressionEvent(4);
                        NetMessage.TrySendData(7);
                    }
                }
                else if (num216 == -11)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, 255, 130));
                    NPC.combatBookWasUsed = true;
                    NetMessage.TrySendData(7);
                }
                else if (num216 == -12)
                {
                    NPC.UnlockOrExchangePet(ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", num216);
                }
                else if (num216 == -13)
                {
                    NPC.UnlockOrExchangePet(ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", num216);
                }
                else if (num216 == -14)
                {
                    NPC.UnlockOrExchangePet(ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", num216);
                }
                else if (num216 == -15)
                {
                    NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, 670, "Misc.LicenseSlimeUsed", num216);
                }
                else if (num216 == -16)
                {
                    NPC.SpawnMechQueen(num215);
                }
                else if (num216 == -17)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed"), new Color(50, 255, 130));
                    NPC.combatBookVolumeTwoWasUsed = true;
                    NetMessage.TrySendData(7);
                }
                else if (num216 == -18)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed"), new Color(50, 255, 130));
                    NPC.peddlersSatchelWasUsed = true;
                    NetMessage.TrySendData(7);
                }
                else if (num216 < 0)
                {
                    int num217 = 1;
                    if (num216 > -InvasionID.Count)
                    {
                        num217 = -num216;
                    }
                    if (num217 > 0 && Main.invasionType == 0)
                    {
                        Main.invasionDelay = 0;
                        Main.StartInvasion(num217);
                    }
                    NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
                }
                break;
            }

            case 62:
            {
                int num166 = self.reader.ReadByte();
                int num167 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num166 = self.whoAmI;
                }
                if (num167 == 1)
                {
                    Main.player[num166].NinjaDodge();
                }
                if (num167 == 2)
                {
                    Main.player[num166].ShadowDodge();
                }
                if (num167 == 4)
                {
                    Main.player[num166].BrainOfConfusionDodge();
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(62, -1, self.whoAmI, null, num166, num167);
                }
                break;
            }

            case 63:
            {
                int  num144 = self.reader.ReadInt16();
                int  num145 = self.reader.ReadInt16();
                byte b5     = self.reader.ReadByte();
                byte b6     = self.reader.ReadByte();
                if (b6 == 0)
                {
                    WorldGen.paintTile(num144, num145, b5);
                }
                else
                {
                    WorldGen.paintCoatTile(num144, num145, b5);
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(63, -1, self.whoAmI, null, num144, num145, (int)b5, (int)b6);
                }
                break;
            }

            case 64:
            {
                int  num131 = self.reader.ReadInt16();
                int  num132 = self.reader.ReadInt16();
                byte b3     = self.reader.ReadByte();
                byte b4     = self.reader.ReadByte();
                if (b4 == 0)
                {
                    WorldGen.paintWall(num131, num132, b3);
                }
                else
                {
                    WorldGen.paintCoatWall(num131, num132, b3);
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(64, -1, self.whoAmI, null, num131, num132, (int)b3, (int)b4);
                }
                break;
            }

            case 65:
            {
                BitsByte bitsByte6 = self.reader.ReadByte();
                int      num51     = self.reader.ReadInt16();
                if (Main.netMode == 2)
                {
                    num51 = self.whoAmI;
                }
                Vector2 vector = self.reader.ReadVector2();
                int     num52  = 0;
                num52 = self.reader.ReadByte();
                int num53 = 0;
                if (bitsByte6[0])
                {
                    num53++;
                }
                if (bitsByte6[1])
                {
                    num53 += 2;
                }
                bool flag2 = false;
                if (bitsByte6[2])
                {
                    flag2 = true;
                }
                int num54 = 0;
                if (bitsByte6[3])
                {
                    num54 = self.reader.ReadInt32();
                }
                if (flag2)
                {
                    vector = Main.player[num51].position;
                }
                switch (num53)
                {
                    case 0:
                        Main.player[num51].Teleport(vector, num52, num54);
                        break;

                    case 1:
                        Main.npc[num51].Teleport(vector, num52, num54);
                        break;

                    case 2:
                    {
                        Main.player[num51].Teleport(vector, num52, num54);
                        if (Main.netMode != 2)
                        {
                            break;
                        }
                        RemoteClient.CheckSection(self.whoAmI, vector);
                        NetMessage.TrySendData(65, -1, -1, null, 0, num51, vector.X, vector.Y, num52, flag2.ToInt(), num54);
                        int   num55 = -1;
                        float num56 = 9999f;
                        for (int num57 = 0; num57 < 255; num57++)
                        {
                            if (Main.player[num57].active && num57 != self.whoAmI)
                            {
                                Vector2 vector2 = Main.player[num57].position - Main.player[self.whoAmI].position;
                                if (vector2.Length() < num56)
                                {
                                    num56 = vector2.Length();
                                    num55 = num57;
                                }
                            }
                        }
                        if (num55 >= 0)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[self.whoAmI].name, Main.player[num55].name), new Color(250, 250, 0));
                        }
                        break;
                    }
                }
                if (Main.netMode == 2 && num53 == 0)
                {
                    NetMessage.TrySendData(65, -1, self.whoAmI, null, num53, num51, vector.X, vector.Y, num52, flag2.ToInt(), num54);
                }
                break;
            }

            case 66:
            {
                int num30 = self.reader.ReadByte();
                int num31 = self.reader.ReadInt16();
                if (num31 > 0)
                {
                    Player player3 = Main.player[num30];
                    player3.statLife += num31;
                    if (player3.statLife > player3.statLifeMax2)
                    {
                        player3.statLife = player3.statLifeMax2;
                    }
                    player3.HealEffect(num31, broadcast: false);
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(66, -1, self.whoAmI, null, num30, num31);
                    }
                }
                break;
            }

            case 68:
                self.reader.ReadString();
                break;

            case 69:
            {
                int num247 = self.reader.ReadInt16();
                int num248 = self.reader.ReadInt16();
                int num249 = self.reader.ReadInt16();
                if (Main.netMode == 1)
                {
                    if (num247 >= 0 && num247 < 8000)
                    {
                        Chest chest3 = Main.chest[num247];
                        if (chest3 == null)
                        {
                            chest3             = new Chest();
                            chest3.x           = num248;
                            chest3.y           = num249;
                            Main.chest[num247] = chest3;
                        }
                        else if (chest3.x != num248 || chest3.y != num249)
                        {
                            break;
                        }
                        chest3.name = self.reader.ReadString();
                    }
                }
                else
                {
                    if (num247 < -1 || num247 >= 8000)
                    {
                        break;
                    }
                    if (num247 == -1)
                    {
                        num247 = Chest.FindChest(num248, num249);
                        if (num247 == -1)
                        {
                            break;
                        }
                    }
                    Chest chest4 = Main.chest[num247];
                    if (chest4.x == num248 && chest4.y == num249)
                    {
                        NetMessage.TrySendData(69, self.whoAmI, -1, null, num247, num248, num249);
                    }
                }
                break;
            }

            case 70:
                if (Main.netMode == 2)
                {
                    int num227 = self.reader.ReadInt16();
                    int who    = self.reader.ReadByte();
                    if (Main.netMode == 2)
                    {
                        who = self.whoAmI;
                    }
                    if (num227 < 200 && num227 >= 0)
                    {
                        NPC.CatchNPC(num227, who);
                    }
                }
                break;

            case 71:
                if (Main.netMode == 2)
                {
                    int  x15    = self.reader.ReadInt32();
                    int  y13    = self.reader.ReadInt32();
                    int  type11 = self.reader.ReadInt16();
                    byte style3 = self.reader.ReadByte();
                    NPC.ReleaseNPC(x15, y13, type11, style3, self.whoAmI);
                }
                break;

            case 72:
                if (Main.netMode == 1)
                {
                    for (int num210 = 0; num210 < 40; num210++)
                    {
                        Main.travelShop[num210] = self.reader.ReadInt16();
                    }
                }
                break;

            case 73:
                switch (self.reader.ReadByte())
                {
                    case 0:
                        Main.player[self.whoAmI].TeleportationPotion();
                        break;

                    case 1:
                        Main.player[self.whoAmI].MagicConch();
                        break;

                    case 2:
                        Main.player[self.whoAmI].DemonConch();
                        break;

                    case 3:
                        Main.player[self.whoAmI].Shellphone_Spawn();
                        break;
                }
                break;

            case 74:
                if (Main.netMode == 1)
                {
                    Main.anglerQuest         = self.reader.ReadByte();
                    Main.anglerQuestFinished = self.reader.ReadBoolean();
                }
                break;

            case 75:
                if (Main.netMode == 2)
                {
                    string name2 = Main.player[self.whoAmI].name;
                    if (!Main.anglerWhoFinishedToday.Contains(name2))
                    {
                        Main.anglerWhoFinishedToday.Add(name2);
                    }
                }
                break;

            case 76:
            {
                int num171 = self.reader.ReadByte();
                if (num171 != Main.myPlayer || Main.ServerSideCharacter)
                {
                    if (Main.netMode == 2)
                    {
                        num171 = self.whoAmI;
                    }
                    Player obj7 = Main.player[num171];
                    obj7.anglerQuestsFinished   = self.reader.ReadInt32();
                    obj7.golferScoreAccumulated = self.reader.ReadInt32();
                    if (Main.netMode == 2)
                    {
                        NetMessage.TrySendData(76, -1, self.whoAmI, null, num171);
                    }
                }
                break;
            }

            case 77:
            {
                short  type18   = self.reader.ReadInt16();
                ushort tileType = self.reader.ReadUInt16();
                short  x11      = self.reader.ReadInt16();
                short  y11      = self.reader.ReadInt16();
                Animation.NewTemporaryAnimation(type18, tileType, x11, y11);
                break;
            }

            case 78:
                if (Main.netMode == 1)
                {
                    Main.ReportInvasionProgress(self.reader.ReadInt32(), self.reader.ReadInt32(), self.reader.ReadSByte(), self.reader.ReadSByte());
                }
                break;

            case 79:
            {
                int   x9        = self.reader.ReadInt16();
                int   y9        = self.reader.ReadInt16();
                short type7     = self.reader.ReadInt16();
                int   style2    = self.reader.ReadInt16();
                int   num140    = self.reader.ReadByte();
                int   random    = self.reader.ReadSByte();
                int   direction = (self.reader.ReadBoolean() ? 1 : (-1));
                if (Main.netMode == 2)
                {
                    Netplay.Clients[self.whoAmI].SpamAddBlock += 1f;
                    if (!WorldGen.InWorld(x9, y9, 10) || !Netplay.Clients[self.whoAmI].TileSections[Netplay.GetSectionX(x9), Netplay.GetSectionY(y9)])
                    {
                        break;
                    }
                }
                WorldGen.PlaceObject(x9, y9, type7, mute: false, style2, num140, random, direction);
                if (Main.netMode == 2)
                {
                    NetMessage.SendObjectPlacement(self.whoAmI, x9, y9, type7, style2, num140, random, direction);
                }
                break;
            }

            case 80:
                if (Main.netMode == 1)
                {
                    int num127 = self.reader.ReadByte();
                    int num128 = self.reader.ReadInt16();
                    if (num128 >= -3 && num128 < 8000)
                    {
                        Main.player[num127].chest = num128;
                        Recipe.FindRecipes(canDelayCheck: true);
                    }
                }
                break;

            case 81:
                if (Main.netMode == 1)
                {
                    int x19 = (int)self.reader.ReadSingle();
                    int y7  = (int)self.reader.ReadSingle();
                    CombatText.NewText(color: self.reader.ReadRGB(), amount: self.reader.ReadInt32(), location: new Rectangle(x19, y7, 0, 0));
                }
                break;

            case 119:
                if (Main.netMode == 1)
                {
                    int x18 = (int)self.reader.ReadSingle();
                    int y8  = (int)self.reader.ReadSingle();
                    CombatText.NewText(color: self.reader.ReadRGB(), text: NetworkText.Deserialize(self.reader).ToString(), location: new Rectangle(x18, y8, 0, 0));
                }
                break;

            case 82:
                NetManager.Instance.Read(self.reader, self.whoAmI, length);
                break;

            case 83:
                if (Main.netMode == 1)
                {
                    int num103 = self.reader.ReadInt16();
                    int num104 = self.reader.ReadInt32();
                    if (num103 >= 0)
                    {
                        NPC.killCount[num103] = num104;
                    }
                }
                break;

            case 84:
            {
                int num102 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num102 = self.whoAmI;
                }
                float stealth = self.reader.ReadSingle();
                Main.player[num102].stealth = stealth;
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(84, -1, self.whoAmI, null, num102);
                }
                break;
            }

            case 85:
            {
                int num97 = self.whoAmI;
                int slot  = self.reader.ReadInt16();
                if (Main.netMode == 2 && num97 < 255)
                {
                    Chest.ServerPlaceItem(self.whoAmI, slot);
                }
                break;
            }

            case 86:
            {
                if (Main.netMode != 1)
                {
                    break;
                }
                int num67 = self.reader.ReadInt32();
                if (!self.reader.ReadBoolean())
                {
                    if (TileEntity.ByID.TryGetValue(num67, out var value3))
                    {
                        TileEntity.ByID.Remove(num67);
                        TileEntity.ByPosition.Remove(value3.Position);
                    }
                }
                else
                {
                    TileEntity tileEntity = TileEntity.Read(self.reader, networkSend: true /*, lightSend: true*/);
                    tileEntity.ID                              = num67;
                    TileEntity.ByID[tileEntity.ID]             = tileEntity;
                    TileEntity.ByPosition[tileEntity.Position] = tileEntity;
                }
                break;
            }

            case 87:
                if (Main.netMode == 2)
                {
                    int x6    = self.reader.ReadInt16();
                    int y6    = self.reader.ReadInt16();
                    int type3 = self.reader.ReadByte();
                    if (WorldGen.InWorld(x6, y6) && !TileEntity.ByPosition.ContainsKey(new Point16(x6, y6)))
                    {
                        TileEntity.PlaceEntityNet(x6, y6, type3);
                    }
                }
                break;

            case 88:
            {
                if (Main.netMode != 1)
                {
                    break;
                }
                int num218 = self.reader.ReadInt16();
                if (num218 < 0 || num218 > 400)
                {
                    break;
                }
                Item     item5      = Main.item[num218];
                BitsByte bitsByte27 = self.reader.ReadByte();
                if (bitsByte27[0])
                {
                    item5.color.PackedValue = self.reader.ReadUInt32();
                }
                if (bitsByte27[1])
                {
                    item5.damage = self.reader.ReadUInt16();
                }
                if (bitsByte27[2])
                {
                    item5.knockBack = self.reader.ReadSingle();
                }
                if (bitsByte27[3])
                {
                    item5.useAnimation = self.reader.ReadUInt16();
                }
                if (bitsByte27[4])
                {
                    item5.useTime = self.reader.ReadUInt16();
                }
                if (bitsByte27[5])
                {
                    item5.shoot = self.reader.ReadInt16();
                }
                if (bitsByte27[6])
                {
                    item5.shootSpeed = self.reader.ReadSingle();
                }
                if (bitsByte27[7])
                {
                    bitsByte27 = self.reader.ReadByte();
                    if (bitsByte27[0])
                    {
                        item5.width = self.reader.ReadInt16();
                    }
                    if (bitsByte27[1])
                    {
                        item5.height = self.reader.ReadInt16();
                    }
                    if (bitsByte27[2])
                    {
                        item5.scale = self.reader.ReadSingle();
                    }
                    if (bitsByte27[3])
                    {
                        item5.ammo = self.reader.ReadInt16();
                    }
                    if (bitsByte27[4])
                    {
                        item5.useAmmo = self.reader.ReadInt16();
                    }
                    if (bitsByte27[5])
                    {
                        item5.notAmmo = self.reader.ReadBoolean();
                    }
                }
                break;
            }

            case 89:
                if (Main.netMode == 2)
                {
                    short x21 = self.reader.ReadInt16();
                    int   y12 = self.reader.ReadInt16();
                    // Item  item = ItemIO.Receive(self.reader);
                    // item.stack = self.reader.Read7BitEncodedInt();
                    int netid3  = self.reader.ReadInt16();
                    int prefix3 = self.reader.ReadByte();
                    int stack6  = self.reader.ReadInt16();
                    // TEItemFrame.TryPlacing(x21, y12, item, 1);
                    var item = new Item(netid3, stack6, prefix3);
                    TEItemFrame.TryPlacing(x21, y12, item, 1);
                }
                break;

            case 91:
            {
                if (Main.netMode != 1)
                {
                    break;
                }
                int num185 = self.reader.ReadInt32();
                int num186 = self.reader.ReadByte();
                // if (num186 == 2)
                // {
                //     int owner = self.reader.ReadByte();
                //     num186 |= owner << 8;
                // }
                if (num186 == 255)
                {
                    if (EmoteBubble.byID.ContainsKey(num185))
                    {
                        EmoteBubble.byID.Remove(num185);
                    }
                    break;
                }
                int num187   = self.reader.ReadUInt16();
                int num188   = self.reader.ReadUInt16();
                int num189   = self.reader.ReadByte();
                int metadata = 0;
                if (num189 < 0)
                {
                    metadata = self.reader.ReadInt16();
                }
                WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num186, num187);
                if (num186 == 1)
                {
                    Main.player[num187].emoteTime = 360;
                }
                lock (EmoteBubble.byID)
                {
                    if (!EmoteBubble.byID.ContainsKey(num185))
                    {
                        EmoteBubble.byID[num185] = new EmoteBubble(num189, worldUIAnchor, num188);
                    }
                    else
                    {
                        EmoteBubble.byID[num185].lifeTime      = num188;
                        EmoteBubble.byID[num185].lifeTimeStart = num188;
                        EmoteBubble.byID[num185].emote         = num189;
                        EmoteBubble.byID[num185].anchor        = worldUIAnchor;
                    }
                    EmoteBubble.byID[num185].ID       = num185;
                    EmoteBubble.byID[num185].metadata = metadata;
                    EmoteBubble.OnBubbleChange(num185);
                    // EmoteBubbleLoader.OnSpawn(EmoteBubble.byID[num185]);
                    break;
                }
            }

            case 92:
            {
                int   num172 = self.reader.ReadInt16();
                int   num173 = self.reader.ReadInt32();
                float num174 = self.reader.ReadSingle();
                float num175 = self.reader.ReadSingle();
                if (num172 >= 0 && num172 <= 200)
                {
                    if (Main.netMode == 1)
                    {
                        Main.npc[num172].moneyPing(new Vector2(num174, num175));
                        Main.npc[num172].extraValue = num173;
                    }
                    else
                    {
                        Main.npc[num172].extraValue += num173;
                        NetMessage.TrySendData(92, -1, -1, null, num172, Main.npc[num172].extraValue, num174, num175);
                    }
                }
                break;
            }

            case 95:
            {
                ushort num168 = self.reader.ReadUInt16();
                int    num169 = self.reader.ReadByte();
                if (Main.netMode != 2)
                {
                    break;
                }
                for (int num170 = 0; num170 < 1000; num170++)
                {
                    if (Main.projectile[num170].owner == num168 && Main.projectile[num170].active && Main.projectile[num170].type == 602 && Main.projectile[num170].ai[1] == (float)num169)
                    {
                        Main.projectile[num170].Kill();
                        NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num170].identity, (int)num168);
                        break;
                    }
                }
                break;
            }

            case 96:
            {
                int     num162                = self.reader.ReadByte();
                Player  obj5                  = Main.player[num162];
                int     num163                = self.reader.ReadInt16();
                Vector2 newPos2               = self.reader.ReadVector2();
                Vector2 velocity6             = self.reader.ReadVector2();
                int     lastPortalColorIndex2 = num163 + ((num163 % 2 == 0) ? 1 : (-1));
                obj5.lastPortalColorIndex = lastPortalColorIndex2;
                obj5.Teleport(newPos2, 4, num163);
                obj5.velocity = velocity6;
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(96, -1, -1, null, num162, newPos2.X, newPos2.Y, num163);
                }
                break;
            }

            case 97:
                if (Main.netMode == 1)
                {
                    AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], self.reader.ReadInt16());
                }
                break;

            case 98:
                if (Main.netMode == 1)
                {
                    AchievementsHelper.NotifyProgressionEvent(self.reader.ReadInt16());
                }
                break;

            case 99:
            {
                int num141 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num141 = self.whoAmI;
                }
                Main.player[num141].MinionRestTargetPoint = self.reader.ReadVector2();
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(99, -1, self.whoAmI, null, num141);
                }
                break;
            }

            case 115:
            {
                int num136 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num136 = self.whoAmI;
                }
                Main.player[num136].MinionAttackTargetNPC = self.reader.ReadInt16();
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(115, -1, self.whoAmI, null, num136);
                }
                break;
            }

            case 100:
            {
                int     num129               = self.reader.ReadUInt16();
                NPC     obj4                 = Main.npc[num129];
                int     num130               = self.reader.ReadInt16();
                Vector2 newPos               = self.reader.ReadVector2();
                Vector2 velocity4            = self.reader.ReadVector2();
                int     lastPortalColorIndex = num130 + ((num130 % 2 == 0) ? 1 : (-1));
                obj4.lastPortalColorIndex = lastPortalColorIndex;
                obj4.Teleport(newPos, 4, num130);
                obj4.velocity  =  velocity4;
                obj4.netOffset *= 0f;
                break;
            }

            case 101:
                if (Main.netMode != 2)
                {
                    NPC.ShieldStrengthTowerSolar    = self.reader.ReadUInt16();
                    NPC.ShieldStrengthTowerVortex   = self.reader.ReadUInt16();
                    NPC.ShieldStrengthTowerNebula   = self.reader.ReadUInt16();
                    NPC.ShieldStrengthTowerStardust = self.reader.ReadUInt16();
                    if (NPC.ShieldStrengthTowerSolar < 0)
                    {
                        NPC.ShieldStrengthTowerSolar = 0;
                    }
                    if (NPC.ShieldStrengthTowerVortex < 0)
                    {
                        NPC.ShieldStrengthTowerVortex = 0;
                    }
                    if (NPC.ShieldStrengthTowerNebula < 0)
                    {
                        NPC.ShieldStrengthTowerNebula = 0;
                    }
                    if (NPC.ShieldStrengthTowerStardust < 0)
                    {
                        NPC.ShieldStrengthTowerStardust = 0;
                    }
                    if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerMax)
                    {
                        NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerMax;
                    }
                    if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerMax)
                    {
                        NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerMax;
                    }
                    if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerMax)
                    {
                        NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerMax;
                    }
                    if (NPC.ShieldStrengthTowerStardust > NPC.LunarShieldPowerMax)
                    {
                        NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerMax;
                    }
                }
                break;

            case 102:
            {
                int     num58 = self.reader.ReadByte();
                ushort  num59 = self.reader.ReadUInt16();
                Vector2 other = self.reader.ReadVector2();
                if (Main.netMode == 2)
                {
                    num58 = self.whoAmI;
                    NetMessage.TrySendData(102, -1, -1, null, num58, (int)num59, other.X, other.Y);
                    break;
                }
                Player player4 = Main.player[num58];
                for (int num60 = 0; num60 < 255; num60++)
                {
                    Player player5 = Main.player[num60];
                    if (!player5.active || player5.dead || (player4.team != 0 && player4.team != player5.team) || !(player5.Distance(other) < 700f))
                    {
                        continue;
                    }
                    Vector2 value2  = player4.Center - player5.Center;
                    Vector2 vector3 = Vector2.Normalize(value2);
                    if (!vector3.HasNaNs())
                    {
                        int     type4         = 90;
                        float   num61         = 0f;
                        float   num62         = (float)Math.PI / 15f;
                        Vector2 spinningpoint = new Vector2(0f, -8f);
                        Vector2 vector4       = new Vector2(-3f);
                        float   num63         = 0f;
                        float   num64         = 0.005f;
                        switch (num59)
                        {
                            case 179:
                                type4 = 86;
                                break;

                            case 173:
                                type4 = 90;
                                break;

                            case 176:
                                type4 = 88;
                                break;
                        }
                        for (int num65 = 0; (float)num65 < value2.Length() / 6f; num65++)
                        {
                            Vector2 position4 = player5.Center + 6f * (float)num65 * vector3 + spinningpoint.RotatedBy(num61) + vector4;
                            num61 += num62;
                            int num66 = Dust.NewDust(position4, 6, 6, type4, 0f, 0f, 100, default(Color), 1.5f);
                            Main.dust[num66].noGravity =  true;
                            Main.dust[num66].velocity  =  Vector2.Zero;
                            num63                      =  (Main.dust[num66].fadeIn = num63 + num64);
                            Main.dust[num66].velocity  += vector3 * 1.5f;
                        }
                    }
                    player5.NebulaLevelup(num59);
                }
                break;
            }

            case 103:
                if (Main.netMode == 1)
                {
                    NPC.MaxMoonLordCountdown = self.reader.ReadInt32();
                    NPC.MoonLordCountdown    = self.reader.ReadInt32();
                }
                break;

            case 104:
                if (Main.netMode == 1 && Main.npcShop > 0)
                {
                    Item[]   item         = Main.instance.shop[Main.npcShop].item;
                    int      num47        = self.reader.ReadByte();
                    int      type2        = self.reader.ReadInt16();
                    int      stack2       = self.reader.ReadInt16();
                    int      prefixWeWant = self.reader.ReadByte();
                    int      value        = self.reader.ReadInt32();
                    BitsByte bitsByte5    = self.reader.ReadByte();
                    if (num47 < item.Length)
                    {
                        item[num47] = new Item();
                        item[num47].netDefaults(type2);
                        item[num47].stack = stack2;
                        item[num47].Prefix(prefixWeWant);
                        item[num47].value   = value;
                        item[num47].buyOnce = bitsByte5[0];
                    }
                }
                break;

            case 105:
                if (Main.netMode != 1)
                {
                    short i2 = self.reader.ReadInt16();
                    int   j2 = self.reader.ReadInt16();
                    bool  on = self.reader.ReadBoolean();
                    WorldGen.ToggleGemLock(i2, j2, on);
                }
                break;

            case 106:
                if (Main.netMode == 1)
                {
                    HalfVector2 halfVector = default(HalfVector2);
                    halfVector.PackedValue = self.reader.ReadUInt32();
                    Utils.PoofOfSmoke(halfVector.ToVector2());
                }
                break;

            case 107:
                if (Main.netMode == 1)
                {
                    Color  c          = self.reader.ReadRGB();
                    string text3      = NetworkText.Deserialize(self.reader).ToString();
                    int    widthLimit = self.reader.ReadInt16();
                    Main.NewTextMultiline(text3, force: false, c, widthLimit);
                }
                break;

            case 108:
                if (Main.netMode == 1)
                {
                    int   damage    = self.reader.ReadInt16();
                    float knockBack = self.reader.ReadSingle();
                    int   x4        = self.reader.ReadInt16();
                    int   y4        = self.reader.ReadInt16();
                    int   angle     = self.reader.ReadInt16();
                    int   ammo      = self.reader.ReadInt16();
                    int   num32     = self.reader.ReadByte();
                    if (num32 == Main.myPlayer)
                    {
                        WorldGen.ShootFromCannon(x4, y4, angle, ammo, damage, knockBack, num32, fromWire: true);
                    }
                }
                break;

            case 109:
                if (Main.netMode == 2)
                {
                    short                          x16       = self.reader.ReadInt16();
                    int                            y2        = self.reader.ReadInt16();
                    int                            x3        = self.reader.ReadInt16();
                    int                            y3        = self.reader.ReadInt16();
                    byte                           toolMode3 = self.reader.ReadByte();
                    int                            num29     = self.whoAmI;
                    WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
                    WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode)toolMode3;
                    Wiring.MassWireOperation(new Point(x16, y2), new Point(x3, y3), Main.player[num29]);
                    WiresUI.Settings.ToolMode = toolMode2;
                }
                break;

            case 110:
            {
                if (Main.netMode != 1)
                {
                    break;
                }
                int type  = self.reader.ReadInt16();
                int num19 = self.reader.ReadInt16();
                int num20 = self.reader.ReadByte();
                if (num20 == Main.myPlayer)
                {
                    Player player2 = Main.player[num20];
                    for (int k = 0; k < num19; k++)
                    {
                        player2.ConsumeItem(type);
                    }
                    player2.wireOperationsCooldown = 0;
                }
                break;
            }

            case 111:
                if (Main.netMode == 2)
                {
                    BirthdayParty.ToggleManualParty();
                }
                break;

            case 112:
            {
                int num13 = self.reader.ReadByte();
                int num14 = self.reader.ReadInt32();
                int num15 = self.reader.ReadInt32();
                int num16 = self.reader.ReadByte();
                int num17 = self.reader.ReadInt16();
                switch (num13)
                {
                    case 1:
                        if (Main.netMode == 1)
                        {
                            WorldGen.TreeGrowFX(num14, num15, num16, num17);
                        }
                        if (Main.netMode == 2)
                        {
                            NetMessage.TrySendData(b, -1, -1, null, num13, num14, num15, num16, num17);
                        }
                        break;

                    case 2:
                        NPC.FairyEffects(new Vector2(num14, num15), num17);
                        break;
                }
                break;
            }

            case 113:
            {
                int x14 = self.reader.ReadInt16();
                int y14 = self.reader.ReadInt16();
                if (Main.netMode == 2 && !Main.snowMoon && !Main.pumpkinMoon)
                {
                    if (DD2Event.WouldFailSpawningHere(x14, y14))
                    {
                        DD2Event.FailureMessage(self.whoAmI);
                    }
                    DD2Event.SummonCrystal(x14, y14, self.whoAmI);
                }
                break;
            }

            case 114:
                if (Main.netMode == 1)
                {
                    DD2Event.WipeEntities();
                }
                break;

            case 116:
                if (Main.netMode == 1)
                {
                    DD2Event.TimeLeftBetweenWaves = self.reader.ReadInt32();
                }
                break;

            case 117:
            {
                int num234 = self.reader.ReadByte();
                // bool directHurt = false;
                // if (num234 == 255)
                // {
                //     num234     = self.reader.ReadByte();
                //     directHurt = true;
                // }
                if (Main.netMode == 2 && self.whoAmI != num234 && (!Main.player[num234].hostile || !Main.player[self.whoAmI].hostile))
                {
                    break;
                }
                if (true /*!directHurt*/)
                {
                    PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(self.reader);
                    int               damage3            = self.reader.ReadInt16();
                    int               num235             = self.reader.ReadByte() - 1;
                    BitsByte          bitsByte31         = self.reader.ReadByte();
                    bool              flag15             = bitsByte31[0];
                    bool              pvp2               = bitsByte31[1];
                    int               num236             = self.reader.ReadSByte();
                    Main.player[num234].Hurt(playerDeathReason2, damage3, num235, pvp2, quiet: true, flag15, num236);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendPlayerHurt(num234, playerDeathReason2, damage3, num235, flag15, pvp2, num236, -1, self.whoAmI);
                    }
                    break;
                }
                BitsByte        pack     = self.reader.ReadByte();
                Player.HurtInfo hurtInfo = new Player.HurtInfo();
                hurtInfo.DamageSource    = PlayerDeathReason.FromReader(self.reader);
                hurtInfo.PvP             = pack[0];
                hurtInfo.CooldownCounter = self.reader.ReadSByte();
                hurtInfo.Dodgeable       = pack[1];
                hurtInfo.SourceDamage    = self.reader.Read7BitEncodedInt();
                hurtInfo.Damage          = self.reader.Read7BitEncodedInt();
                hurtInfo.HitDirection    = self.reader.ReadSByte();
                hurtInfo.Knockback       = self.reader.ReadSingle();
                hurtInfo.DustDisabled    = pack[2];
                hurtInfo.SoundDisabled   = pack[3];
                Player.HurtInfo args = hurtInfo;
                Main.player[num234].Hurt(args, quiet: true);
                if (Main.netMode == 2)
                {
                    NetMessage.SendPlayerHurt(num234, args, self.whoAmI);
                }
                break;
            }

            case 118:
            {
                int num223 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num223 = self.whoAmI;
                }
                PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(self.reader);
                int               num224            = self.reader.ReadInt16();
                int               num225            = self.reader.ReadByte() - 1;
                bool              pvp               = ((BitsByte)self.reader.ReadByte())[0];
                Main.player[num223].KillMe(playerDeathReason, num224, num225, pvp);
                if (Main.netMode == 2)
                {
                    NetMessage.SendPlayerDeath(num223, playerDeathReason, num224, num225, pvp, -1, self.whoAmI);
                }
                break;
            }

            case 120:
            {
                int num213 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num213 = self.whoAmI;
                }
                int num214 = self.reader.ReadByte();
                if (num214 >= 0 && num214 < EmoteBubbleLoader.EmoteBubbleCount && Main.netMode == 2)
                {
                    EmoteBubble.NewBubble(num214, new WorldUIAnchor(Main.player[num213]), 360);
                    EmoteBubble.CheckForNPCsToReactToEmoteBubble(num214, Main.player[num213]);
                }
                break;
            }

            case 121:
            {
                int num182 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num182 = self.whoAmI;
                }
                int  num183 = self.reader.ReadInt32();
                int  num184 = self.reader.ReadByte();
                bool flag11 = false;
                if (num184 >= 8)
                {
                    flag11 =  true;
                    num184 -= 8;
                }
                if (!TileEntity.ByID.TryGetValue(num183, out var value9))
                {
                    self.reader.ReadInt32();
                    self.reader.ReadByte();
                    break;
                }
                if (num184 >= 8)
                {
                    value9 = null;
                }
                if (value9 is TEDisplayDoll tEDisplayDoll)
                {
                    tEDisplayDoll.ReadItem(num184, self.reader, flag11);
                }
                else
                {
                    self.reader.ReadInt32();
                    self.reader.ReadByte();
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(b, -1, num182, null, num182, num183, num184, flag11.ToInt());
                }
                break;
            }

            case 122:
            {
                int num153 = self.reader.ReadInt32();
                int num154 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num154 = self.whoAmI;
                }
                if (Main.netMode == 2)
                {
                    if (num153 == -1)
                    {
                        Main.player[num154].tileEntityAnchor.Clear();
                        NetMessage.TrySendData(b, -1, -1, null, num153, num154);
                        break;
                    }
                    if (!TileEntity.IsOccupied(num153, out interactingPlayer) && TileEntity.ByID.TryGetValue(num153, out var value6))
                    {
                        Main.player[num154].tileEntityAnchor.Set(num153, value6.Position.X, value6.Position.Y);
                        NetMessage.TrySendData(b, -1, -1, null, num153, num154);
                    }
                }
                if (Main.netMode == 1)
                {
                    TileEntity value7;
                    if (num153 == -1)
                    {
                        Main.player[num154].tileEntityAnchor.Clear();
                    }
                    else if (TileEntity.ByID.TryGetValue(num153, out value7))
                    {
                        TileEntity.SetInteractionAnchor(Main.player[num154], value7.Position.X, value7.Position.Y, num153);
                    }
                }
                break;
            }

            case 123:
                if (Main.netMode == 2)
                {
                    short x20 = self.reader.ReadInt16();
                    int   y10 = self.reader.ReadInt16();
                    // Item  item = ItemIO.Receive(self.reader, readStack: true);
                    int netid2  = self.reader.ReadInt16();
                    int prefix2 = self.reader.ReadByte();
                    int stack4  = self.reader.ReadInt16();
                    // TEWeaponsRack.TryPlacing(x20, y10, item, item.stack);
                    var item = new Item(netid2, stack4, prefix2);
                    TEWeaponsRack.TryPlacing(x20, y10, item, item.stack);
                }
                break;

            case 124:
            {
                int num133 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num133 = self.whoAmI;
                }
                int  num134 = self.reader.ReadInt32();
                int  num135 = self.reader.ReadByte();
                bool flag7  = false;
                if (num135 >= 2)
                {
                    flag7  =  true;
                    num135 -= 2;
                }
                if (!TileEntity.ByID.TryGetValue(num134, out var value4))
                {
                    self.reader.ReadInt32();
                    self.reader.ReadByte();
                    break;
                }
                if (num135 >= 2)
                {
                    value4 = null;
                }
                if (value4 is TEHatRack tEHatRack)
                {
                    tEHatRack.ReadItem(num135, self.reader, flag7);
                }
                else
                {
                    self.reader.ReadInt32();
                    self.reader.ReadByte();
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(b, -1, num133, null, num133, num134, num135, flag7.ToInt());
                }
                break;
            }

            case 125:
            {
                int num123 = self.reader.ReadByte();
                int num124 = self.reader.ReadInt16();
                int num125 = self.reader.ReadInt16();
                int num126 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num123 = self.whoAmI;
                }
                if (Main.netMode == 1)
                {
                    Main.player[Main.myPlayer].GetOtherPlayersPickTile(num124, num125, num126);
                }
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(125, -1, num123, null, num123, num124, num125, num126);
                }
                break;
            }

            case 126:
                if (Main.netMode == 1)
                {
                    NPC.RevengeManager.AddMarkerFromReader(self.reader);
                }
                break;

            case 127:
            {
                int markerUniqueID = self.reader.ReadInt32();
                if (Main.netMode == 1)
                {
                    NPC.RevengeManager.DestroyMarker(markerUniqueID);
                }
                break;
            }

            case 128:
            {
                int num112 = self.reader.ReadByte();
                int num113 = self.reader.ReadUInt16();
                int num114 = self.reader.ReadUInt16();
                int num115 = self.reader.ReadUInt16();
                int num116 = self.reader.ReadUInt16();
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(128, -1, num112, null, num112, num115, num116, 0f, num113, num114);
                }
                else
                {
                    GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num113, num114), num112, num115, num116);
                }
                break;
            }

            case 129:
                if (Main.netMode == 1)
                {
                    Main.FixUIScale();
                    Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
                }
                break;

            case 130:
            {
                if (Main.netMode != 2)
                {
                    break;
                }
                int num93 = self.reader.ReadUInt16();
                int num94 = self.reader.ReadUInt16();
                int num95 = self.reader.ReadInt16();
                if (num95 == 682)
                {
                    if (NPC.unlockedSlimeRedSpawn)
                    {
                        break;
                    }
                    NPC.unlockedSlimeRedSpawn = true;
                    NetMessage.TrySendData(7);
                }
                num93 *= 16;
                num94 *= 16;
                NPC nPC8 = new NPC();
                nPC8.SetDefaults(num95);
                int type5 = nPC8.type;
                int netID = nPC8.netID;
                int num96 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[self.whoAmI]), num93, num94, num95);
                if (netID != type5)
                {
                    Main.npc[num96].SetDefaults(netID);
                    NetMessage.TrySendData(23, -1, -1, null, num96);
                }
                if (num95 == 682)
                {
                    WorldGen.CheckAchievement_RealEstateAndTownSlimes();
                }
                break;
            }

            case 131:
                if (Main.netMode == 1)
                {
                    int num68 = self.reader.ReadUInt16();
                    NPC nPC   = null;
                    nPC = ((num68 >= 200) ? new NPC() : Main.npc[num68]);
                    if (self.reader.ReadByte() == 1)
                    {
                        int time    = self.reader.ReadInt32();
                        int fromWho = self.reader.ReadInt16();
                        nPC.GetImmuneTime(fromWho, time);
                    }
                }
                break;

            case 132:
                if (Main.netMode == 1)
                {
                    Point      point            = self.reader.ReadVector2().ToPoint();
                    ushort     key              = self.reader.ReadUInt16();
                    SoundStyle legacySoundStyle = SoundID.SoundByIndex[key];
                    BitsByte   bitsByte4        = self.reader.ReadByte();
                    if (bitsByte4[0])
                    {
                        legacySoundStyle.Variants = new int[1] { self.reader.ReadInt32() };
                    }
                    if (bitsByte4[1])
                    {
                        legacySoundStyle.Volume = MathHelper.Clamp(self.reader.ReadSingle(), 0f, 1f);
                    }
                    if (bitsByte4[2])
                    {
                        legacySoundStyle.Pitch = MathHelper.Clamp(self.reader.ReadSingle(), 0f, 1f);
                    }
                    SoundEngine.PlaySound(in legacySoundStyle, point.ToVector2());
                }
                break;

            case 133:
                if (Main.netMode == 2)
                {
                    short x17 = self.reader.ReadInt16();
                    int   y5  = self.reader.ReadInt16();
                    // Item  item = ItemIO.Receive(self.reader, readStack: true);
                    int netid  = self.reader.ReadInt16();
                    int prefix = self.reader.ReadByte();
                    int stack  = self.reader.ReadInt16();
                    var item   = new Item(netid, stack, prefix);
                    TEFoodPlatter.TryPlacing(x17, y5, item, item.stack);
                }
                break;

            case 134:
            {
                int num41 = self.reader.ReadByte();
                // double ladyBugLuckTimeLeft     = self.reader.ReadDouble();
                int   ladyBugLuckTimeLeft     = self.reader.ReadInt32();
                float torchLuck               = self.reader.ReadSingle();
                byte  luckPotion              = self.reader.ReadByte();
                bool  hasGardenGnomeNearby    = self.reader.ReadBoolean();
                float equipmentBasedLuckBonus = self.reader.ReadSingle();
                float coinLuck                = self.reader.ReadSingle();
                if (Main.netMode == 2)
                {
                    num41 = self.whoAmI;
                }
                Player obj3 = Main.player[num41];
                obj3.ladyBugLuckTimeLeft     = ladyBugLuckTimeLeft;
                obj3.torchLuck               = torchLuck;
                obj3.luckPotion              = luckPotion;
                obj3.HasGardenGnomeNearby    = hasGardenGnomeNearby;
                obj3.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
                obj3.coinLuck                = coinLuck;
                obj3.RecalculateLuck();
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(134, -1, num41, null, num41);
                }
                break;
            }

            case 135:
            {
                int num33 = self.reader.ReadByte();
                if (Main.netMode == 1)
                {
                    Main.player[num33].immuneAlpha = 255;
                }
                break;
            }

            case 136:
            {
                for (int l = 0; l < 2; l++)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        NPC.cavernMonsterType[l, m] = self.reader.ReadUInt16();
                    }
                }
                break;
            }

            case 137:
                if (Main.netMode == 2)
                {
                    int num28            = self.reader.ReadInt16();
                    int buffTypeToRemove = self.reader.ReadUInt16();
                    if (num28 >= 0 && num28 < 200)
                    {
                        Main.npc[num28].RequestBuffRemoval(buffTypeToRemove);
                    }
                }
                break;

            case 139:
                if (Main.netMode != 2)
                {
                    int  num26 = self.reader.ReadByte();
                    bool flag  = self.reader.ReadBoolean();
                    Main.countsAsHostForGameplay[num26] = flag;
                }
                break;

            case 140:
            {
                int num24 = self.reader.ReadByte();
                int num25 = self.reader.ReadInt32();
                switch (num24)
                {
                    case 0:
                        if (Main.netMode == 1)
                        {
                            CreditsRollEvent.SetRemainingTimeDirect(num25);
                        }
                        break;

                    case 1:
                        if (Main.netMode == 2)
                        {
                            NPC.TransformCopperSlime(num25);
                        }
                        break;

                    case 2:
                        if (Main.netMode == 2)
                        {
                            NPC.TransformElderSlime(num25);
                        }
                        break;
                }
                break;
            }

            case 141:
            {
                LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)self.reader.ReadByte();
                byte                         b2            = self.reader.ReadByte();
                Vector2                      velocity      = self.reader.ReadVector2();
                int                          num21         = self.reader.ReadInt32();
                int                          num22         = self.reader.ReadInt32();
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(141, -1, self.whoAmI, null, (int)messageSource, (int)b2, velocity.X, velocity.Y, num21, num22);
                }
                else
                {
                    LucyAxeMessage.CreateFromNet(messageSource, b2, new Vector2(num21, num22), velocity);
                }
                break;
            }

            case 142:
            {
                int num18 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num18 = self.whoAmI;
                }
                Player obj = Main.player[num18];
                obj.piggyBankProjTracker.TryReading(self.reader);
                obj.voidLensChest.TryReading(self.reader);
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(142, -1, self.whoAmI, null, num18);
                }
                break;
            }

            case 143:
                if (Main.netMode == 2)
                {
                    DD2Event.AttemptToSkipWaitTime();
                }
                break;

            case 144:
                if (Main.netMode == 2)
                {
                    NPC.HaveDryadDoStardewAnimation();
                }
                break;

            case 146:
                switch (self.reader.ReadByte())
                {
                    case 0:
                        Item.ShimmerEffect(self.reader.ReadVector2());
                        break;

                    case 1:
                    {
                        Vector2 coinPosition = self.reader.ReadVector2();
                        int     coinAmount   = self.reader.ReadInt32();
                        Main.player[Main.myPlayer].AddCoinLuck(coinPosition, coinAmount);
                        break;
                    }

                    case 2:
                    {
                        int num12 = self.reader.ReadInt32();
                        Main.npc[num12].SetNetShimmerEffect();
                        break;
                    }
                }
                break;

            case 147:
            {
                int num6 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num6 = self.whoAmI;
                }
                int num7 = self.reader.ReadByte();
                Main.player[num6].TrySwitchingLoadout(num7);
                MessageBuffer.ReadAccessoryVisibility(self.reader, Main.player[num6].hideVisibleAccessory);
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(b, -1, num6, null, num6, num7);
                }
                break;
            }

            case 249:
                ConfigManager.HandleInGameChangeConfigPacket(self.reader, self.whoAmI);
                break;

            case 250:
                ModNet.HandleModPacket(self.reader, self.whoAmI, length);
                break;

            case 251:
                if (Main.netMode == 1)
                {
                    ModNet.SyncClientMods(self.reader);
                    break;
                }
                ModNet.SendNetIDs(self.whoAmI);
                NetMessage.SendData(3, self.whoAmI);
                break;

            case 252:
                if (Main.netMode == 1)
                {
                    ModNet.ReceiveMod(self.reader);
                }
                else
                {
                    ModNet.SendMod(self.reader.ReadString(), self.whoAmI);
                }
                break;

            case 253:
            {
                string keepAlive = "Keep Alive During Mod Reload";
                ModNet.Log(self.whoAmI, keepAlive);
                RemoteClient client = Netplay.Clients[self.whoAmI];
                Console.WriteLine($"[{self.whoAmI}][{client.Socket?.GetRemoteAddress()?.GetFriendlyName()} ({client.Name})]: {keepAlive}");
                break;
            }

            default:
                if (Netplay.Clients[self.whoAmI].State == 0)
                {
                    NetMessage.BootPlayer(self.whoAmI, Lang.mp[2].ToNetworkText());
                }
                break;

            case 15:
            case 25:
            case 26:
            case 44:
            case 67:
            case 93:
                break;

            case 24:
                int num120 = self.reader.ReadInt16();
                int num121 = self.reader.ReadByte();
                if (Main.netMode == 2)
                {
                    num121 = self.whoAmI;
                }
                Player player9 = Main.player[num121];
                Main.npc[num120].StrikeNPC(player9.inventory[player9.selectedItem].damage, player9.inventory[player9.selectedItem].knockBack, player9.direction);
                if (Main.netMode == 2)
                {
                    NetMessage.TrySendData(24, -1, self.whoAmI, null, num120, num121);
                    NetMessage.TrySendData(23, -1, -1,          null, num120);
                }
                break;
        }
    }

    private static PlayerDeathReason PlayerDeathReason_FromReader(
        On_PlayerDeathReason.orig_FromReader orig,
        BinaryReader                         reader
    )
    {
        var hasItem    = false;
        var itemType   = 0;
        var itemPrefix = 0;

        PlayerDeathReason playerDeathReason = new PlayerDeathReason();
        BitsByte          bitsByte          = reader.ReadByte();
        if (bitsByte[0])
        {
            playerDeathReason._sourcePlayerIndex = reader.ReadInt16();
        }
        if (bitsByte[1])
        {
            playerDeathReason._sourceNPCIndex = reader.ReadInt16();
        }
        if (bitsByte[2])
        {
            playerDeathReason._sourceProjectileLocalIndex = reader.ReadInt16();
        }
        if (bitsByte[3])
        {
            playerDeathReason._sourceOtherIndex = reader.ReadByte();
        }
        if (bitsByte[4])
        {
            playerDeathReason._sourceProjectileType = reader.ReadInt16();
        }
        if (bitsByte[5])
        {
            hasItem  = true;
            itemType = reader.ReadInt16();
        }
        if (bitsByte[6])
        {
            itemPrefix = reader.ReadByte();
        }
        if (bitsByte[7])
        {
            playerDeathReason._sourceCustomReason = reader.ReadString();
        }

        if (hasItem)
        {
            playerDeathReason._sourceItem = new Item(itemType, 1, itemPrefix);
        }

        return playerDeathReason;
    }

    private static void SyncStatLifeMax(Player player)
    {
        var lifeCrystals = player.statLifeMax switch
        {
            <= 100 => 0,
            >= 400 => 15,
            _      => (player.statLifeMax - 100) / 20,
        };

        var lifeFruit = player.statLifeMax switch
        {
            <= 400 => 0,
            >= 500 => 20,
            _      => player.statLifeMax - 400 / 5,
        };

        player.ConsumedLifeCrystals = lifeCrystals;
        player.ConsumedLifeFruit    = lifeFruit;
        
        Debug.Assert(player.statLifeMax is >= 100 and <= 500);
        Debug.Assert(player.statLifeMax <= 400 ? player.statLifeMax % 20 == 0 : player.statLifeMax % 5 == 0);
    }

    private static void SyncStatManaMax(Player player)
    {
        var manaCrystals = player.statManaMax switch
        {
            <= 20  => 0,
            >= 200 => 9,
            _      => (player.statManaMax - 20) / 20,
        };

        player.ConsumedManaCrystals = manaCrystals;

        Debug.Assert(player.statManaMax is >= 20 and <= 200);
        Debug.Assert(player.statManaMax % 20 == 0);
    }
}