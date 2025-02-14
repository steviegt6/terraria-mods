using JetBrains.Annotations;

using Terraria.ModLoader;

using Tomat.Terraria.TML.LiquidLib.Content.Liquid.Default;

namespace Tomat.Terraria.TML.LiquidLib.Common.Liquid;

/// <summary>
///     The basis for a modded liquid implementation.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
public abstract class ModLiquid : ModBlockType
{
    /// <summary>
    ///     Dummy definition for Water.
    /// </summary>
    public static readonly ModLiquid WATER = new WaterLiquid();

    /// <summary>
    ///     Dummy definition for Lava.
    /// </summary>
    public static readonly ModLiquid LAVA = new LavaLiquid();

    /// <summary>
    ///     Dummy definition for Honey.
    /// </summary>
    public static readonly ModLiquid HONEY = new HoneyLiquid();

    /// <summary>
    ///     Dummy definition for Shimmer.
    /// </summary>
    public static readonly ModLiquid SHIMMER = new ShimmerLiquid();

    /// <summary>
    ///     Definition for unloaded liquids.
    /// </summary>
    public static readonly ModLiquid UNLOADED = new UnloadedLiquid();
}