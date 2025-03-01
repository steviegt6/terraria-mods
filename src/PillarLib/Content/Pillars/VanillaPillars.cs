using Terraria;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ID;

using Tomat.TML.Mod.PillarLib.Common;

namespace Tomat.TML.Mod.PillarLib.Content.Pillars;

internal abstract class VanillaPillar : ICelestialPillar
{
    public abstract int CelestialPillarType { get; }

    public abstract LunarPillarBigProgessBar ProgressBar { get; }

    public abstract bool IsActive { get; set; }

    public abstract int ShieldStrength { get; set; }
}

internal sealed class SolarPillar : VanillaPillar
{
    public override int CelestialPillarType => NPCID.LunarTowerSolar;

    public override LunarPillarBigProgessBar ProgressBar => BigProgressBarSystem._solarPillarBar;

    public override bool IsActive
    {
        get => NPC.TowerActiveSolar;
        set => NPC.TowerActiveSolar = value;
    }

    public override int ShieldStrength
    {
        get => NPC.ShieldStrengthTowerSolar;
        set => NPC.ShieldStrengthTowerSolar = value;
    }
}

internal sealed class VortexPillar : VanillaPillar
{
    public override int CelestialPillarType => NPCID.LunarTowerVortex;

    public override LunarPillarBigProgessBar ProgressBar => BigProgressBarSystem._vortexPillarBar;

    public override bool IsActive
    {
        get => NPC.TowerActiveVortex;
        set => NPC.TowerActiveVortex = value;
    }

    public override int ShieldStrength
    {
        get => NPC.ShieldStrengthTowerVortex;
        set => NPC.ShieldStrengthTowerVortex = value;
    }
}

internal sealed class NebulaPillar : VanillaPillar
{
    public override int CelestialPillarType => NPCID.LunarTowerNebula;

    public override LunarPillarBigProgessBar ProgressBar => BigProgressBarSystem._nebulaPillarBar;

    public override bool IsActive
    {
        get => NPC.TowerActiveNebula;
        set => NPC.TowerActiveNebula = value;
    }

    public override int ShieldStrength
    {
        get => NPC.ShieldStrengthTowerNebula;
        set => NPC.ShieldStrengthTowerNebula = value;
    }
}

internal sealed class StardustPillar : VanillaPillar
{
    public override int CelestialPillarType => NPCID.LunarTowerStardust;

    public override LunarPillarBigProgessBar ProgressBar => BigProgressBarSystem._stardustPillarBar;

    public override bool IsActive
    {
        get => NPC.TowerActiveStardust;
        set => NPC.TowerActiveStardust = value;
    }

    public override int ShieldStrength
    {
        get => NPC.ShieldStrengthTowerStardust;
        set => NPC.ShieldStrengthTowerStardust = value;
    }
}