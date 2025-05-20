using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Daybreak.Core.Hooks;
using MonoMod.Cil;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using System;
using System.Reflection.Emit;

public class ConsumerOfSouls : ModNPC
{
    public override string Texture => Assets.Images.NPCs.TestBoss_Sheet.KEY;
    public override void SetStaticDefaults()
    {

    }

    public override void SetDefaults()
    {
        NPC.width = 50;
        NPC.height = 50;
        NPC.damage = 50;
        NPC.defense = 10;
        NPC.lifeMax = 5000;
        NPC.aiStyle = -1;
        NPC.value = Item.buyPrice(0, 10, 0, 0);
        NPC.knockBackResist = 0.5f;
        NPC.boss = true;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
    }
}

public class Loader : ModSystem
{
    const double DungeonBlobSize = 60.0;
    private static int side = 0;
    public override void Load()
    {
        IL_WorldGen.DungeonEnt += IL_DungeonEnt;
    }

    // TODO: Place Consumer's NPC here 
    private static void IL_DungeonEnt(ILContext il)
    {

        var c = new ILCursor(il);

        c.EmitLdsfld(typeof(Terraria.WorldBuilding.GenVars).GetField("dungeonSide"));
        c.EmitDelegate<Action<int>>((dside) => side = dside);
        /*
        num4 = (int)(val.X + dxStrength * 0.6);
	    IL_0646: ldloc.3
	    IL_0647: ldfld float64 [ReLogic]ReLogic.Utilities.Vector2D::X
	    IL_064c: ldloc.1
	    IL_064d: ldc.r8 0.6
	    IL_0656: mul
	    IL_0657: add
	    IL_0658: conv.i4
	    IL_0659: stloc.s 6
        */
        // dungeon blob size
        // modify width, int num4 = (int)(vector2D.X + dxStrength * 0.6 + (double)genRand.Next(2, 5));
        c.GotoNext(MoveType.After, x => x.MatchLdloc3(),
        x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")),
        x => x.MatchLdloc1());

        c.GotoPrev(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")));
        c.EmitDelegate<Func<double>>(() => 
        {
            return (int)(side == -1 ? DungeonBlobSize : 0);
        });
        c.EmitSub();

        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")));
        c.EmitDelegate<Func<double>>(() => 
        {
            return (int)(side == 1 ? DungeonBlobSize : 0);
        });
        c.EmitAdd();

        // modify height, int num5 = (int)(vector2D.Y - dyStrength * 0.6 - (double)genRand.Next(2, 5));
        c.GotoNext(MoveType.After, x => x.MatchLdloc3(),
        x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")),
        x => x.MatchLdloc2());

        c.GotoPrev(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")));
        c.EmitLdcR8(DungeonBlobSize);
        c.EmitSub();

        // modify inner walls, do a precise match to get to the right instruction 
        c.GotoNext(MoveType.After, x => x.MatchLdloc3(),
        x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")),
        x => x.MatchLdloc1(),
        x => x.MatchLdcR8(0.5),
        x => x.MatchMul(),
        x => x.MatchSub());

        // wall x size
        c.GotoPrev(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")));
        c.EmitDelegate<Func<double>>(() => 
        {
            return (int)(side == -1 ? DungeonBlobSize : 0);
        });
        c.EmitSub();

        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")));
        c.EmitDelegate<Func<double>>(() => 
        {
            return (int)(side == 1 ? DungeonBlobSize : 0);
        });
        c.EmitAdd();

        // wall y size
        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")));
        c.EmitLdcR8(DungeonBlobSize);
        c.EmitSub();

        /*
        		IL_175e: ldloc.s 7
		IL_1760: stloc.s 63
		// PlaceWall(num54, num55, wallType, mute: true);
		IL_1762: br.s IL_1775
		// loop start (head: IL_1775)
			IL_1764: ldloc.s 62
			IL_1766: ldloc.s 63
			IL_1768: ldarg.3
			IL_1769: ldc.i4.1
			IL_176a: call void Terraria.WorldGen::PlaceWall(int32, int32, int32, bool)
        */

        c.GotoNext(MoveType.After, x => x.MatchLdloc(7),
        x => x.MatchStloc(63));

        c.GotoNext(MoveType.After, x => x.MatchLdloc(62),
        x => x.MatchLdloc(63),
        x => x.MatchLdarg3(),
        x => x.Match(Mono.Cecil.Cil.OpCodes.Ldc_I4_1));

        c.GotoPrev(MoveType.Before, x => x.MatchLdarg3());
        c.Remove();
        c.EmitDelegate<Func<int>>(() =>
        {
            return WallID.AdamantiteBeam;
        });
    }
}
