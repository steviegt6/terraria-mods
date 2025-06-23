using Daybreak.Common.Features.Hooks;

using MonoMod.Cil;

using Terraria;

namespace Nightshade.Content;

internal static class MakeStrangePlantsSpawnAlways
{
    [OnLoad]
    public static void OnLoad()
    {
        IL_WorldGen.UpdateWorld_OvergroundTile += ReplaceHardmodeWithTrue;
        IL_WorldGen.UpdateWorld_UndergroundTile += ReplaceHardmodeWithTrue;
    }

    private static void ReplaceHardmodeWithTrue(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchLdsfld<Main>(nameof(Main.hardMode)));
        c.EmitPop();
        c.EmitLdcI4(1);
    }
}