using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Daybreak.Core.Hooks;
using MonoMod.Cil;
using System.Text.RegularExpressions;
using Mono.Cecil.Cil;
using System;

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
    public override void Load()
    {
        IL_WorldGen.DungeonEnt += IL_DungeonEnt;
    }

    // TODO: Place Consumer's NPC here 
    private static void IL_DungeonEnt(ILContext il)
    {
        var c = new ILCursor(il);

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
        c.EmitLdcR8(60.0);
        c.EmitSub();

        // modify height, int num5 = (int)(vector2D.Y - dyStrength * 0.6 - (double)genRand.Next(2, 5));
        c.GotoNext(MoveType.After, x => x.MatchLdloc3(),
        x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")),
        x => x.MatchLdloc2());

        c.GotoPrev(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")));
        c.EmitLdcR8(60.0);
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
        c.EmitLdcR8(60.0);
        c.EmitSub();

        // wall y size
        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")));
        c.EmitLdcR8(60.0);
        c.EmitSub();
    }
}
