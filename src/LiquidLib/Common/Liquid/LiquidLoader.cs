using Terraria.ModLoader;

namespace Tomat.Terraria.TML.LiquidLib.Common.Liquid;

/// <summary>
///     Responsible for loading modded liquids.
/// </summary>
public sealed class LiquidLoader : ModSystem
{
    public static ModLiquid GetLiquid(int type) => GetLiquid(type);

    public override void Unload()
    {
        base.Unload();
    }

    private static void ResizeArrays()
    {
        
    }
}