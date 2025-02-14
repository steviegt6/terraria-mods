using JetBrains.Annotations;

using Terraria.ModLoader;

namespace Tomat.Terraria.TML.LiquidLib.Common.Liquid;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class LiquidSystem : ModSystem
{
    /// <summary>
    ///     The maximum amount of liquids that can be loaded (including vanilla
    ///     liquids).  Currently, it is 64 (only 6 bytes are allocated to the
    ///     liquid type).
    /// </summary>
    public static int MaxLiquidCount => 2 << 6;
}