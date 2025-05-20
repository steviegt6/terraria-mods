using Daybreak.Common.CIL;
using Daybreak.Common.Features.Hooks;
using Daybreak.Core.Hooks;

using MonoMod.Cil;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Nightshade.Common.Hooks._ProjectileReflecting;

public interface INpcReflectable
{
    public static readonly GlobalHookList<GlobalProjectile> HOOK = ProjectileLoader.AddModHook(
        GlobalHookList<GlobalProjectile>.Create(x => ((INpcReflectable)x).CanBeReflected)
    );

    bool? CanBeReflected();

    public static bool? Invoke(Projectile projectile)
    {
        var globalResult = default(bool?);

        foreach (var g in HOOK.Enumerate(projectile))
        {
            if (g is not INpcReflectable hook)
            {
                continue;
            }

            var result = hook.CanBeReflected();
            if (result.HasValue && globalResult != false)
            {
                globalResult = result.Value;
            }
        }

        var modResult = (projectile.ModProjectile as INpcReflectable)?.CanBeReflected();
        return globalResult ?? modResult;
    }

    [OnLoad]
    private static void Load()
    {
        IL_Projectile.CanBeReflected += il =>
        {
            var valueOverride = il.AddVariable<bool?>();

            var c = new ILCursor(il);

            var label = il.DefineLabel();

            c.GotoNext(MoveType.After, x => x.MatchLdcI4(0));
            c.Index++;

            c.EmitLdarg0();
            c.EmitDelegate(
                static (Projectile self) => Invoke(self)
            );
            c.EmitStloc(valueOverride);

            c.EmitLdloca(valueOverride);
            c.EmitCall(typeof(bool?).GetMethod("get_HasValue")!);
            c.EmitBrfalse(label);

            c.EmitLdloca(valueOverride);
            c.EmitCall(typeof(bool?).GetMethod("get_Value")!);
            c.EmitRet();

            c.MarkLabel(label);
        };
    }
}