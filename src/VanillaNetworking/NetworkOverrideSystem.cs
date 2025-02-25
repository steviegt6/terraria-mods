using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using MonoMod.RuntimeDetour;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.VanillaNetworking;

internal sealed partial class NetworkOverrideSystem : ModSystem
{
#pragma warning disable CS0618 // Type or member is obsolete
    public static bool AllowVanillaClients
    {
        get => ModNet.AllowVanillaClients;
        set => allow_vanilla_clients_property.SetValue(null, value);
    }

    private static readonly PropertyInfo allow_vanilla_clients_property = typeof(ModNet).GetProperty(nameof(ModNet.AllowVanillaClients), BindingFlags.Public | BindingFlags.Static)!;
#pragma warning restore CS0618 // Type or member is obsolete

    private static          Hook?      modContentLoadHook;
    private static readonly List<Hook> hooks = [];

#pragma warning disable CA2255
    [ModuleInitializer]
    public static void SetAllowVanillaClientsToTrue()
    {
        // We are supposedly the only ones modifying this.
        // Debug.Assert(!AllowVanillaClients);

        AllowVanillaClients = true;
    }

    [ModuleInitializer]
    public static void PerformEarlyHooks()
    {
        modContentLoadHook = new Hook(
            typeof(ModContent).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Static)!,
            ModContent_Load
        );
    }
#pragma warning restore CA2255

    public override void Load()
    {
        base.Load();

        /*IL_NetMessage.SendData += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(MoveType.After, x => x.MatchCall("Terraria.ModLoader.ModNet", "get_NetVersionString"));
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldstr, "Terraria" + 279);

            c.GotoNext(x => x.MatchLdfld<Player>("hairDye"));
            c.GotoNext(MoveType.Before, x => x.MatchCallvirt(out _));
            c.Remove();
            c.EmitDelegate((BinaryWriter writer, int hairDye) => writer.Write(hairDye));

            c.GotoNext(MoveType.Before, x => x.MatchCall("Terraria.ModLoader.IO.ItemIO", "Send"));
            c.Remove();
            c.Emit(OpCodes.Ldarg, 6);
            c.EmitDelegate(
                (Item item, BinaryWriter writer, bool _, bool _, float number3) =>
                {
                    var stack = item.stack;
                    var netId = item.netID;

                    if (stack < 0)
                    {
                        stack = 0;
                    }

                    writer.Write((short)stack);
                    writer.Write((byte)number3);
                    writer.Write((short)netId);
                }
            );
        };*/

        On_NetMessage.SendData                  += NetMessage_SendData;
        On_NetMessage.DecompressTileBlock_Inner += NetMessage_DecompressTileBlock_Inner;
        On_NetMessage.SyncOnePlayer             += NetMessage_SyncOnePlayer;
        On_MessageBuffer.GetData                += MessageBuffer_GetData;
        On_PlayerDeathReason.FromReader         += PlayerDeathReason_FromReader;
        On_PlayerDeathReason.WriteSelfTo        += PlayerDeathReason_WriteSelfTo;

        var playerLoaderType = typeof(PlayerLoader);
        var playerLoaderMethods = new (MethodInfo, Delegate)[]
        {
            (playerLoaderType.GetMethod("SyncPlayer",        BindingFlags.Public | BindingFlags.Static)!, (Player player, int    toWho, int fromWho, bool newPlayer) => { }),
            (playerLoaderType.GetMethod("SendClientChanges", BindingFlags.Public | BindingFlags.Static)!, (Player player, Player clientPlayer) => { }),
            (playerLoaderType.GetMethod("CopyClientState",   BindingFlags.Public | BindingFlags.Static)!, (Player player, Player targetCopy) => { }),
        };

        foreach (var meth in playerLoaderMethods)
        {
            hooks.Add(new Hook(meth.Item1, meth.Item2));
        }
    }

    public override void Unload()
    {
        base.Unload();

        AllowVanillaClients = false;

        modContentLoadHook?.Dispose();
        modContentLoadHook = null;

        foreach (var hook in hooks)
        {
            hook.Dispose();
        }

        hooks.Clear();
    }

    // ReSharper disable once InconsistentNaming
    private static void ModContent_Load(Action<CancellationToken> orig, CancellationToken token)
    {
        var illegalMods = ModLoader.Mods.Where(x => x.Side is not ModSide.Client and not ModSide.NoSync).ToArray();
        if (illegalMods.Length > 0)
        {
            throw new Exception(
                "Failed to load the following mods because their Mod Side is not Client or NoSync:"
              + "\n" + string.Join("\n", illegalMods.Select(x => $"    {x.DisplayName} ({x.Name}) v{x.Version} -> ModSide: {x.Side}"))
              + "\n\nTHIS ERROR IS THROWN BY Tomat's Vanilla Server Compat; IT IS NOT NECESSARILY AN ISSUE WITH ANY OF THE ABOVE MODS."
              + "\nPlease join the Discord linked in my mod's homepage and talk to me about whether this is an issue with an above mod that should be fixed on their behalf."
              + "\nEither disable this mod and lose vanilla server compatibility and disable the listed mods to maintain vanilla server compatibility.\n\n"
            );
        }

        orig(token);
    }
}