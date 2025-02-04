using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using MonoMod.RuntimeDetour;

using Terraria.ModLoader;

namespace Tomat.Terraria.TML.VanillaNetworking;

internal sealed class NetworkOverrideSystem : ModSystem
{
#pragma warning disable CS0618 // Type or member is obsolete
    public static bool AllowVanillaClients
    {
        get => ModNet.AllowVanillaClients;
        set => allow_vanilla_clients_property.SetValue(null, value);
    }

    private static readonly PropertyInfo allow_vanilla_clients_property = typeof(ModNet).GetProperty(nameof(ModNet.AllowVanillaClients), BindingFlags.Public | BindingFlags.Static)!;
#pragma warning restore CS0618 // Type or member is obsolete

    private static Hook? mod_content_load_hook;

#pragma warning disable CA2255
    [ModuleInitializer]
    public static void SetAllowVanillaClientsToTrue()
    {
        // We are supposedly the only ones modifying this.
        Debug.Assert(!AllowVanillaClients);

        AllowVanillaClients = true;
    }

    [ModuleInitializer]
    public static void PerformEarlyHooks()
    {
        mod_content_load_hook = new Hook(
            typeof(ModContent).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Static)!,
            ModContent_Load
        );
    }
#pragma warning restore CA2255

    public override void Unload()
    {
        base.Unload();

        AllowVanillaClients = false;

        mod_content_load_hook?.Dispose();
        mod_content_load_hook = null;
    }

    // ReSharper disable once InconsistentNaming
    private static void ModContent_Load(Action<CancellationToken> orig, CancellationToken token)
    {
        var illegalMods = ModLoader.Mods.Where(x => x.Side is not ModSide.Client and not ModSide.NoSync).ToArray();
        if (illegalMods.Length > 0)
        {
            throw new Exception(
                "Failed to load the following mods because their Mod Side is not Client or NoSync:"
              + "\n" + string.Join("\n", illegalMods.Select(x => $"{x.DisplayName} ({x.Name}) v{x.Version} -> ModSide: {x.Side}"))
              + "\n\nTHIS ERROR IS THROWN BY Tomat's Vanilla Server Compat; IT IS NOT NECESSARILY AN ISSUE WITH ANY OF THE ABOVE MODS."
              + "\nPlease join the Discord linked in my mod's homepage and talk to me about whether this is an issue with an above mod that should be fixed on their behalf."
              + "\nEither disable this mod and lose vanilla server compatibility and disable the listed mods to maintain vanilla server compatibility.\n\n"
            );
        }

        orig(token);
    }
}