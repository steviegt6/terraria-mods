using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Nightshade.Content.NPCs.Bosses.Thaw;
public class ThawBoss : ModNPC
{
    public override string Texture => "Nightshade/Assets/Images/Empty";
    public override void SetDefaults()
    {
        NPC.Size = new Vector2(20, 20);
        NPC.lifeMax = 4500;
        NPC.defense = 5;
        NPC.damage = 20;
        NPC.aiStyle = -1;
        NPC.knockBackResist = 0f;
        NPC.noTileCollide = NPC.noGravity = NPC.boss = true;
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D orb = Assets.Images.NPCs.ThawBoss_Orb.Asset.Value;
        Main.EntitySpriteDraw(orb, NPC.Center - screenPos, OrbFrame, Color.White, 0 /*orb rotation?*/, new Vector2(42) / 2, 1, OrbDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        return true;
    }
    public int OrbDirection = 1;
    public Rectangle OrbFrame = new Rectangle(0, 0, 42, 44);
    public float State { get => NPC.ai[0]; set => NPC.ai[0] = value; }
    public float Timer { get => NPC.ai[1]; set => NPC.ai[1] = value; }
    public override void AI()
    {
        Player player = Main.player[NPC.target];
        if (!NPC.HasValidTarget)
            NPC.TargetClosest(false);
        Main.NewText(Timer);
        Timer++;
        if (Timer < 0)
            Idle();
        else
        {
            State = State switch
            {
                -1 => DeathAnim(),
                0 => SpawnAnim(),
                1 => FlamingHail(),
                _ => State,
            };
        }
    }
    public void BasicIdleMovement()
    {
        Player player = Main.player[NPC.target];
        float wobble = MathF.Sin(new UnifiedRandom((int)State).Next(2000) + Timer * 0.04f);
        NPC.velocity = Vector2.Lerp(NPC.velocity, (player.Center - new Vector2(wobble * 40, 200 - MathHelper.SmoothStep(0, 30, MathF.Abs(wobble))) - NPC.Center) * 0.05f, 0.025f);
    }
    public void Idle()
    {
        if (Timer < -30)
            BasicIdleMovement();
        else
        {
            NPC.velocity *= 0.97f;
            if (Timer == -1)
                NPC.velocity = Vector2.Zero;
        }
    }
    public float DeathAnim()
    {
        return -1;
    }
    public float SpawnAnim()
    {
        if (Timer % 20 == 0)
            CombatText.NewText(NPC.getRect(), Color.OrangeRed, "Cool spawn animation");
        return SetState(0, 1, 300, -300);
    }
    public float FlamingHail()
    {
        if (Timer % 20 == 0)
            CombatText.NewText(NPC.getRect(), Color.Green, "Cool flame hail");
        return SetState(1, 2, 200, -2000);
    }
    public float SetState(float curState, float nextState, float timeLimit, float resetTime = -150)
    {
        if (Timer > timeLimit)
        {
            Timer = resetTime;
            NPC.velocity = Vector2.Zero;
            NPC.damage = 0;
            NPC.rotation = 0;
            return nextState;
        }
        return curState;
    }
}
