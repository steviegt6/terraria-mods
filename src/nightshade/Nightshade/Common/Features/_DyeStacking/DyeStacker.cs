using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Nightshade.Common.Features._DyeStacking;

internal sealed class DyeStacker : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_PlayerDrawHelper.SetShaderForData += TestCancelShaders;
    }

    private static void TestCancelShaders(On_PlayerDrawHelper.orig_SetShaderForData orig, Player player, int cHead, ref DrawData cdd) { }
}