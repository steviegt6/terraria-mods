using System.Reflection;

using MonoMod.Cil;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.VanityTreeTest;

internal sealed class VanityTreeWoodSystem : ILoadable
{
    private static readonly MethodInfo get_tree_drops_method = typeof(WorldGen).GetMethod("KillTile_GetTreeDrops", BindingFlags.Static | BindingFlags.NonPublic)!;

    void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
    {
        /*IL_WorldGen.KillTile_GetTreeDrops += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(x => x.MatchCall(typeof(TileLoader).FullName!, nameof(TileLoader.DropTreeWood)));
            c.GotoPrev(MoveType.Before, x => x.MatchLdsflda<Main>(nameof(Main.tile)));

            // var beforeDropTreeWood = c.Index;

            var xLoc = 0;
            var yLoc = 0;
            c.GotoNext(x => x.MatchLdloc(out xLoc));
            c.GotoNext(x => x.MatchLdloc(out yLoc));

            var dropItemArg = 0;
            c.GotoNext(x => x.MatchLdarg(out dropItemArg));

            // c.Index = beforeDropTreeWood;

            c.GotoNext(MoveType.After, x => x.MatchCall<Player>(nameof(Player.FindClosest)));

            c.EmitLdloc(xLoc);
            c.EmitLdloc(yLoc);
            c.EmitLdarg(dropItemArg);
            c.EmitDelegate(OverrideVanityWood);
        };*/

        IL_WorldGen.KillTile_GetItemDrops += il =>
        {
            var c = new ILCursor(il);

            c.GotoNext(MoveType.Before, x => x.MatchCall(get_tree_drops_method));
            c.Remove();
            c.EmitDelegate(HijackKillTile);
        };
    }


    private static void HijackKillTile(int i, int j, Tile tileCache, ref bool bonusWood, ref int dropItem, ref int secondaryItem)
    {
        var parameters = new object[] { i, j, tileCache, bonusWood, dropItem, secondaryItem };
        get_tree_drops_method.Invoke(null, parameters);

        bonusWood     = (bool)parameters[3];
        dropItem      = (int)parameters[4];
        secondaryItem = (int)parameters[5];

        // Change the wood type here.
        dropItem = tileCache.TileType switch
        {
            TileID.VanityTreeYellowWillow => ItemID.StoneBlock,
            TileID.VanityTreeSakura       => ItemID.DirtBlock,
            _                             => dropItem,
        };

        // You could make it drop the saplings instead of acorns by changing
        // secondaryItem accordingly, too.
    }

    void ILoadable.Unload() { }

    /*private static void OverrideVanityWood(int x, int y, ref int dropItem)
    {
        if (!WorldGen.InWorld(x, y - 1))
        {
            return;
        }

        var tile = Main.tile[x, y - 1];
        Main.NewText(tile.TileType);

        dropItem = tile.TileType switch
        {
            TileID.VanityTreeYellowWillow => ItemID.StoneBlock,
            TileID.VanityTreeSakura       => ItemID.DirtBlock,
            _                             => dropItem,
        };
    }*/
}