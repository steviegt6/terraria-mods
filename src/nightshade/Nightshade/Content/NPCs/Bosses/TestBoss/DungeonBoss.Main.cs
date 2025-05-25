using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using Nightshade.Common.Utilities;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nightshade.Content.NPCs.Bosses;

internal struct ConsumerData
{
    public int bossTimer;
    public int phase;
    public ConsumerOfSouls ModNPC { get; set; }
    public NPC ThisNPC => ModNPC.NPC;
    public int npcID => ThisNPC.whoAmI;
    public Vector2 curPosition => ThisNPC.position;
    public bool stateDone;
    public bool shouldBeDeadNow;

}

internal abstract class BossState : State<ConsumerData>
{
    protected bool activated;
    protected int timer;
    internal static Action<SpriteBatch, Vector2>? StatelessDrawActions;
    public abstract void Broadcast(BinaryWriter writer);
    public abstract void Listen(BinaryReader reader);
}

public class ConsumerOfSouls : ModNPC
{
    private ConsumerData _data = new();
    private StateController<ConsumerData> StateController { get; set; } = new();
    private BossState? currentState => StateController.CurrentState as BossState;

    public override string Texture => Assets.Images.NPCs.Consumer_Boss_Sheet.KEY;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 4;
        base.SetDefaults();
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 34;
        NPC.height = 44;
        NPC.damage = 50;
        NPC.defense = 10;
        NPC.lifeMax = 5000;
        NPC.aiStyle = 0;
        NPC.value = Item.buyPrice(0, 10, 0, 0);
        NPC.knockBackResist = 0.5f;
        NPC.boss = true;
        NPC.noGravity = false;
        NPC.npcSlots = 10f;
        NPC.lavaImmune = true;


        if (!Main.dedServ)
        {
            Music = MusicLoader.GetMusicSlot(Mod, "Nightshade/Assets/Music/Bosses/Self-Annihilation_REPLACE"); // todo: replace with sourcegenned version later
        }
    }

    public override void AI()
    {
        base.AI();

        if (_data.bossTimer++ == 0)
        {
            _data.ModNPC = this;
            StateController.PushState(new Consumer_InitTransitionState());
        }

        if (!StateController.Update(_data))
        {
            StateController.PopCurState();
        }
    }

    internal void AddState(BossState state)
    {
        if (state is not null)
        {
            state.stateData = _data;

            if (!StateController.PushState(state))
            {
                StateController.PopCurState();
            }
        }
    }

    internal void SyncState()
    {
        if (currentState is not null)
        {
            _data = currentState.stateData;
        }
    }

    internal void SyncState(BossState state)
    {
        _data = state.stateData;
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        base.SendExtraAI(writer);

        currentState?.Broadcast(writer);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        base.ReceiveExtraAI(reader);

        currentState?.Listen(reader);
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (currentState is not null)
        {
            SpriteEffects effects = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 pos = NPC.Center + new Vector2(0, 68);

            int frameY = 44 * _data.phase;
            int frameX = 0;

            Rectangle frame = new Rectangle(frameX, frameY, 34, 44);

            Texture2D texture = ModContent.Request<Texture2D>(Assets.Images.NPCs.Consumer_Boss_Sheet.KEY).Value;

            spriteBatch.Draw(texture, pos - screenPos, frame, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale, effects, 0f);
            return false;
        }

        BossState.StatelessDrawActions?.Invoke(spriteBatch, screenPos);
        return false;
    }
    public override bool PreKill()
    {
        return _data.shouldBeDeadNow;
    }
    public override void OnKill()
    {

        base.OnKill();
    }
}

internal class Consumer_InitTransitionState : BossState
{
    public override bool Enter(params ConsumerData[] parameters)
    {
        return true;
    }

    public override bool Exit(params ConsumerData[] parameters)
    {
        return true;
    }

    public override void Broadcast(BinaryWriter writer)
    {

    }
    public override void Listen(BinaryReader reader)
    {

    }

    public override bool Update(params ConsumerData[] parameters)
    {
        stateData = parameters[0];

        stateData.shouldBeDeadNow = false;
        stateData.stateDone = true;

        PopSelf();
        stateData.ModNPC.AddState(new Consumer_PhaseTransitionState());
        stateData.ModNPC.SyncState(this);

        return true;
    }
}

internal class Consumer_DeathTransitionState : BossState
{
    public override bool Enter(params ConsumerData[] parameters)
    {
        Main.NewText("You have defeated the Consumer of Souls!", Color.Red);
        return true;
    }

    public override bool Exit(params ConsumerData[] parameters)
    {
        return true;
    }

    public override void Broadcast(BinaryWriter writer)
    {

    }
    public override void Listen(BinaryReader reader)
    {

    }

    public override bool Update(params ConsumerData[] parameters)
    {
        stateData.shouldBeDeadNow = true;
        return true;
    }
}

internal class Consumer_PhaseTransitionState : BossState
{
    public override bool Enter(params ConsumerData[] parameters)
    {
        return true;
    }

    public override bool Exit(params ConsumerData[] parameters)
    {
        return true;
    }

    public override void Broadcast(BinaryWriter writer)
    {

    }
    public override void Listen(BinaryReader reader)
    {

    }

    public override bool Update(params ConsumerData[] parameters)
    {
        if (!activated)
        {
            activated = true;
            Main.NewText("The Consumer is in a transition state.", Color.Red);
            PopSelf();
        }

        stateData.phase = (int)(4 - stateData.ThisNPC.life / (stateData.ThisNPC.lifeMax / 4f));



        switch (stateData.phase)
        {
            case 0:
                stateData.ModNPC.AddState(new Consumer_PhaseOneBasicTransitionState());
                break;
            default:
                stateData.ModNPC.AddState(new Consumer_DeathTransitionState());
                break;
        }

        stateData.stateDone = true;
        stateData.ModNPC.SyncState(this);


        return true;
    }
}

public class ConsumerOfSoulsNPC : ModNPC
{
    public override string Texture => Assets.Images.NPCs.ConsumerSheet.KEY;
    public override string BossHeadTexture => Assets.Images.NPCs.ConsumerHead.KEY;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 24;

        NPCID.Sets.ExtraFramesCount[NPC.type] = 5; // revisit later
        NPCID.Sets.AttackFrameCount[NPC.type] = 4;
        NPCID.Sets.DangerDetectRange[NPC.type] = 16 * 10; // length in tiles
        NPCID.Sets.AttackType[NPC.type] = 0;
        NPCID.Sets.AttackTime[NPC.type] = 100;
        NPCID.Sets.AttackAverageChance[NPC.type] = 50;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.friendlyRegen = 10;

        NPC.width = 30;
        NPC.height = 44;

        NPC.aiStyle = 7; // passive ai
        AnimationType = NPCID.Dryad;

        NPC.defense = 300;
        NPC.damage = 300;

        NPC.lifeMax = 300;
        NPC.knockBackResist = 0.1f;
    }
    static List<string> chat = new List<string>
        {
            "My thrall was recently freed from his curse... are you the one who did it?",
            "I can feel that one of my guardians has left this place. I'll have that old man's head.",
            "Where did it go!? Where did it go... where did it go..."
        };

    static List<(Func<bool> predicate, string message)> SpecialInteractions = new()
        {
            new(() => NPC.AnyNPCs(NPCID.Clothier), "Ugh... I can still feel my connection to the host. Where is he?"),
            new(() => NPC.AnyNPCs(NPCID.Mechanic), "Did you free our new convert? Suspicious..."),
            new(() => NPC.AnyNPCs(NPCID.Truffle), "The Dungeons could benefit from a faster way of cleaning a skeleton. Speaking of which..."),
        };

    public override string GetChat()
    {
        base.GetChat();

        int randomNumber = Main.rand.Next();
        var randomTuple = SpecialInteractions[randomNumber % SpecialInteractions.Count];
        string chatText = randomTuple.predicate() ? randomTuple.message : chat[randomNumber % chat.Count];
        return chatText;
    }

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = "Confess";
        button2 = "Ask about the Dungeon";

        base.SetChatButtons(ref button, ref button2);
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
    {
        if (!firstButton)
        {
            Main.npcChatText = "Why would I tell you?";
        }
        else
        {
            NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<ConsumerOfSouls>());
        }
        base.OnChatButtonClicked(firstButton, ref shopName);
    }
}

public class Loader : ModSystem
{
    const double DungeonBlobExtensionX = 60.0;
    const double DungeonBlobExtensionY = 20.0;
    private static int side = 0;
    public override void Load()
    {
        IL_WorldGen.MakeDungeon += IL_DungeonGen;
        IL_WorldGen.DungeonEnt += IL_DungeonEnt;
        IL_WorldGen.MakeDungeon_Banners += IL_DungeonBanners;
    }

    // TODO: Place Consumer's NPC here 
    static bool chand = false;
    static int lastY = 0;
    static int dungeonBrick = 0;
    private static void IL_DungeonGen(ILContext il)
    {
        var c = new ILCursor(il);
        // IL_0042: stloc.2
        c.GotoNext(MoveType.Before, x => x.MatchStloc0());
        c.EmitDelegate<Func<int, int>>((dungeonBrickCol) =>
        {
            dungeonBrick = dungeonBrickCol;
            return dungeonBrickCol;
        });
    }

    private static void IL_DungeonBanners(ILContext il)
    {
        var c = new ILCursor(il);
        /*
        IL_00b9: ldloc.2
		IL_00ba: ldc.i4.1
		IL_00bb: add
		IL_00bc: stloc.2
        */
        c.GotoNext(MoveType.After, x => x.MatchLdloc2(),
        x => x.MatchLdcI4(1),
        x => x.MatchAdd());
        c.EmitDelegate<Func<int, int>>((y) =>
        {
            lastY = y;
            return y;
        });
        /*
        // PlaceTile(num, num2, 91, mute: true, forced: false, -1, num3);
		IL_025d: ldloc.1
		IL_025e: ldloc.2
		IL_025f: ldc.i4.s 91 // banner
		IL_0261: ldc.i4.1
		IL_0262: ldc.i4.0
		IL_0263: ldc.i4.m1
		IL_0264: ldloc.s 7 // style
        */

        c.GotoNext(MoveType.Before, x => x.MatchLdloc1(),
        x => x.MatchLdloc2(),
        x => x.MatchLdcI4(91),
        x => x.Match(OpCodes.Ldc_I4_1),
        x => x.Match(OpCodes.Ldc_I4_0),
        x => x.Match(OpCodes.Ldc_I4_M1),
        x => x.MatchLdloc(7));

        c.GotoPrev(MoveType.Before, x => x.MatchStloc(7));
        c.EmitDelegate<Func<int, int>>((style) =>
        {
            if (chand = WorldGen.genRand.NextBool(1, 3) && lastY < Main.worldSurface)
            {
                // bone chandelier place style,
                // placeStyle = 18 + type - 2141;
                // type 2144

                return 27 + dungeonBrick;
            }
            else
            {
                chand = false;
            }

            return style;
        });


        c.GotoNext(MoveType.Before, x => x.MatchLdcI4(91));
        c.Remove();
        c.EmitDelegate<Func<int>>(() =>
        {
            if (chand)
            {
                return TileID.Chandeliers;
            }

            chand = false;
            return TileID.Banners;
        });
    }
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
            return (int)(side == -1 ? DungeonBlobExtensionX : 0);
        });
        c.EmitSub();

        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")));
        c.EmitDelegate<Func<double>>(() =>
        {
            return (int)(side == 1 ? DungeonBlobExtensionX : 0);
        });
        c.EmitAdd();

        // modify height, int num5 = (int)(vector2D.Y - dyStrength * 0.6 - (double)genRand.Next(2, 5));
        c.GotoNext(MoveType.After, x => x.MatchLdloc3(),
        x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")),
        x => x.MatchLdloc2());

        c.GotoPrev(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")));
        c.EmitLdcR8(DungeonBlobExtensionY);
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
            return (int)(side == -1 ? DungeonBlobExtensionX : 0);
        });
        c.EmitSub();

        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("X")));
        c.EmitDelegate<Func<double>>(() =>
        {
            return (int)(side == 1 ? DungeonBlobExtensionX : 0);
        });
        c.EmitAdd();

        // wall y size
        c.GotoNext(MoveType.After, x => x.MatchLdfld(typeof(ReLogic.Utilities.Vector2D).GetField("Y")));
        c.EmitLdcR8(DungeonBlobExtensionY);
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
