using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

public sealed class VanityCursorSets : ModSystem
{
    public static bool[] IsVanityCursor { get; private set; } = [];

    public override void ResizeArrays()
    {
        base.ResizeArrays();

        IsVanityCursor = ItemID.Sets.Factory.CreateNamedSet("IsVanityCursor")
                               .RegisterBoolSet(ItemID.RainbowCursor);
    }
}