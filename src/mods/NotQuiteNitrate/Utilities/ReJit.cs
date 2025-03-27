using System.Reflection;

using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Utilities;

internal static class ReJit
{
    public static void Force(MethodInfo method)
    {
        MonoModHooks.Modify(method, _ => { });
    }
}