using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MonoMod.Cil;

using Terraria;
using Terraria.ModLoader;

using Tomat.TML.Mod.PillarLib.Content.Pillars;

namespace Tomat.TML.Mod.PillarLib.Common;

/// <summary>
///     Handles registering and handling custom Celestial Pillars as well as
///     integrating them with vanilla mechanics.
/// </summary>
public sealed class CelestialPillarSystem : ModSystem
{
    /// <summary>
    ///     The registered Celestial Pillars.
    /// </summary>
    /// <remarks>
    ///     Intentionally exposed as an enumerable set of pillars rather than a
    ///     concrete type to discourage anything other than iterating over the
    ///     pillars.
    /// </remarks>
    public Dictionary<int, ICelestialPillar> CelestialPillars { get; } = [];

    public CelestialPillarSystem()
    {
        RegisterCelestialPillar(new SolarPillar());
        RegisterCelestialPillar(new VortexPillar());
        RegisterCelestialPillar(new NebulaPillar());
        RegisterCelestialPillar(new VortexPillar());
    }

    public void RegisterCelestialPillar(ICelestialPillar celestialPillar)
    {
        if (!CelestialPillars.TryAdd(celestialPillar.CelestialPillarType, celestialPillar))
        {
            throw new InvalidOperationException($"Attempted to add duplicate Celestial Pillar \"{celestialPillar.GetType().Name}\": {celestialPillar.CelestialPillarType}");
        }
    }

    public override void Load()
    {
        base.Load();

        IL_WorldGen.TriggerLunarApocalypse += il =>
        {
            var c = new ILCursor(il);

            var listCandidates = il.Body.Variables.Where(x => x.VariableType.FullName.StartsWith("System.Collections.Generic.List")).ToArray();
            if (listCandidates.Length != 1)
            {
                throw new InvalidOperationException("Couldn't find pillar list");
            }

            var listIndex = listCandidates.Single().Index;
            {
                Debug.Assert(listIndex >= 0 && listIndex <= il.Body.Variables.Count);
            }

            if (!c.TryGotoNext(MoveType.After, x => x.MatchStloc(listIndex)))
            {
                throw new InvalidOperationException("Couldn't find where list is set");
            }

            // Manually set the list to something new.
            c.EmitDelegate(
                () =>
                {
                    return ModContent.GetInstance<CelestialPillarSystem>().CelestialPillars.Select(x => x.Key).ToList();
                }
            );
            c.EmitStloc(listIndex);

            // Replace all instances of the hard-coded constant `4` with our new
            // list count.  This also replaces the initial array size.
            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(4)))
            {
                c.EmitPop();
                EmitListCount(c, listIndex);
            }

            c.Index = 0;
            if (!c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(5)))
            {
                throw new InvalidOperationException("Couldn't find value to patch to distribute pillars evenly (5)");
            }

            c.EmitPop();
            EmitListCount(c, listIndex);
            c.EmitLdcI4(1);
            c.EmitAdd();

            return;

            // TODO: Is ldlen usable at all? lol
            static void EmitListCount(ILCursor c, int listIndex)
            {
                c.EmitLdloc(listIndex);
                c.EmitDelegate((List<int> list) => list.Count);
            }
        };

        On_WorldGen.UpdateLunarApocalypse += orig =>
        {
            if (!NPC.LunarApocalypseIsUp)
            {
                return;
            }

            var system    = ModContent.GetInstance<CelestialPillarSystem>();
            var coreAlive = false;
            var pillarsAlive = new bool[system.CelestialPillars.c]
        };
    }

    public override void Unload()
    {
        base.Unload();

        celestialPillars.Clear();
    }
}