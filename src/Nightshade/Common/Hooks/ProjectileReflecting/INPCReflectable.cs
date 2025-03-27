using MonoMod.Cil;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

using Tomat.TML.Mod.Nightshade.Common.Loading;
using Tomat.TML.Mod.Nightshade.Common.Utilities;

using Hook = Tomat.TML.Mod.Nightshade.Common.Hooks.ProjectileReflecting.INPCReflectable;

namespace Tomat.TML.Mod.Nightshade.Common.Hooks.ProjectileReflecting;

public interface INPCReflectable
{
    private sealed class INPCReflectableImplementation : IInitializer
    {
        void IInitializer.Load()
        {
            
            IL_Projectile.CanBeReflected += il =>
            {
                var label = il.DefineLabel();
                var c = new ILCursor(il);

                c.GotoNext(MoveType.After, x => x.MatchLdcI4(0));
                c.Index++;

                c.EmitLdarg0();
                c.EmitDelegate(
                    static (Projectile self) => Invoke
                );
                c.EmitStloc(0);

                c.EmitLdloca(0);
                c.EmitCall(typeof(bool?).GetMethod("get_HasValue"));
                c.EmitBrfalse(label);

                c.EmitLdloca(0);
                c.EmitCall(typeof(bool?).GetMethod("get_Value"));
                c.EmitRet();

                c.MarkLabel(label);

                return;
            };
        }
    }

    public static readonly GlobalHookList<GlobalProjectile> HOOK = ProjectileLoader.AddModHook(
    GlobalHookList<GlobalProjectile>.Create(x => ((Hook)x).CanBeReflected)
    );

    bool? CanBeReflected();

    public static bool? Invoke(Projectile projectile)
    {
        bool? globalResult = null;

        foreach (var g in HOOK.Enumerate(projectile))
        {
            if (g is not Hook hook)
            {
                continue;
            }

            bool? result = hook.CanBeReflected();
            if (result.HasValue && globalResult != false)
            {
                globalResult = result.Value;
            }
        }

        bool? modResult = (projectile.ModProjectile as Hook)?.CanBeReflected();

        return globalResult ?? modResult;
    }
}