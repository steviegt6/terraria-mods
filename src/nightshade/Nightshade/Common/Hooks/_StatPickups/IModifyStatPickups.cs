using MonoMod.Cil;

using Nightshade.Common.Loading;
using Nightshade.Common.Utilities;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

using Hook = Nightshade.Common.Hooks._StatPickups.IModifyStatPickups;

namespace Nightshade.Common.Hooks._StatPickups;

public interface IModifyStatPickups
{
    private sealed class ModifyStatPickupsImplementation : ILoad
    {
        void ILoad.Load()
        {
            IL_Player.PickupItem += il =>
            {
                // Give us control over how much mana is restored when picking
                // up the mana stars.

                // We will store the amount so we only have to calculate it
                // once.  The amount is used both to increment the relevant stat
                // and to display the in-game pop-up text.
                var amountVariable = il.AddVariable<int>();

                var c = new ILCursor(il);

                // Heart
                c.GotoNext(MoveType.After, x => x.MatchLdcI4(20));
                c.EmitLdarg0(); // this
                c.EmitDelegate(
                    static (int amount, Player self) =>
                    {
                        Invoke(self, StatPickupKind.Heart, ref amount);
                        return amount;
                    }
                );

                // Star
                c.GotoNext(MoveType.After, x => x.MatchLdcI4(100));
                c.EmitLdarg0(); // this
                c.EmitDelegate(
                    static (int amount, Player self) =>
                    {
                        Invoke(self, StatPickupKind.Star, ref amount);
                        return amount;
                    }
                );
                c.EmitDup();
                c.EmitStloc(amountVariable);

                c.GotoNext(MoveType.After, x => x.MatchLdcI4(100));
                c.EmitPop();
                c.EmitLdloc(amountVariable);

                // Mana Cloak Star
                c.GotoNext(MoveType.After, x => x.MatchLdcI4(50));
                c.EmitLdarg0(); // this
                c.EmitDelegate(
                    static (int amount, Player self) =>
                    {
                        Invoke(self, StatPickupKind.ManaCloakStar, ref amount);
                        return amount;
                    }
                );
                c.EmitDup();
                c.EmitStloc(amountVariable);

                c.GotoNext(MoveType.After, x => x.MatchLdcI4(50));
                c.EmitPop();
                c.EmitLdloc(amountVariable);

                return;
            };
        }
    }

    public static readonly HookList<ModPlayer> HOOK = PlayerLoader.AddModHook(
        HookList<ModPlayer>.Create(x => ((Hook)x).ModifyStatPickup)
    );

    void ModifyStatPickup(StatPickupKind kind, ref int amount);

    public static void Invoke(Player player, StatPickupKind kind, ref int amount)
    {
        foreach (var g in HOOK.Enumerate(player.ModPlayers))
        {
            if (g is not Hook hook)
            {
                continue;
            }

            hook.ModifyStatPickup(kind, ref amount);
        }
    }
}