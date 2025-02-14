using System;
using System.Reflection;

using JetBrains.Annotations;

using MonoMod.RuntimeDetour;

using Terraria;
using Terraria.ModLoader;

namespace Tomat.Terraria.TML.InlineJitTest;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class IjtMod : Mod
{
    private Hook? hook;

    public override void Load()
    {
        var type   = typeof(Tilemap);
        var method = type.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public, [typeof(int), typeof(int)])!;
        hook = new Hook(method, Hook_GetItem);
    }

    public override void Unload()
    {
        base.Unload();

        hook?.Dispose();
        hook = null;
    }

    private static Tile Hook_GetItem(
        ref Tilemap self,
        int         x,
        int         y
    )
    {
        if (!WorldGen.InWorld(x, y))
        {
            return new Tile();
        }

        return new Tile((uint)(y + x * self.Height));
    }
}