using Daybreak.Common.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Features;
using Nightshade.Common.Rendering;
using Nightshade.Core;
using Nightshade.Core.Attributes;
using Steamworks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Nightshade.Content.NPCs.Bosses.Thaw;
public class ThawBoss : ModNPC
{
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.BasicPixelizationShader.Parameters>? pixelizationShader;
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.FlameShader.Parameters>? flameShader;
    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.FourPointLightingShader.Parameters>? lightingShader;
    [InitializedInLoad]
    private static ManagedRenderTarget? managedRenderTarget;
    public override void Load()
    {
        Main.QueueMainThreadAction(() =>
        {
            lightingShader = Assets.Shaders.Misc.FourPointLightingShader.CreateStripShader();

            flameShader = Assets.Shaders.Misc.FlameShader.CreateStripShader();
            flameShader.Parameters.uIntensity = 10f;

            pixelizationShader = Assets.Shaders.Misc.BasicPixelizationShader.CreateStripShader();
            pixelizationShader.Parameters.uSize = new Vector2(Main.screenWidth, Main.screenHeight);
            pixelizationShader.Parameters.uPixel = 2;
        });

        managedRenderTarget = new ManagedRenderTarget(reinitOnResolutionChange: true);
        Main.QueueMainThreadAction(() => managedRenderTarget.Initialize(Main.screenWidth, Main.screenHeight));
    }
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 3;
        NPCID.Sets.TrailCacheLength[Type] = 3;
        NPCID.Sets.TrailingMode[Type] = 3;
    }
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
    float flameProgress;
    public void DrawFlame(SpriteBatch spriteBatch, Vector2 screenPos) // hell torture
    {
        if (Main.GameUpdateCount % 5 == 0 && !Main.gameInactive) flameProgress += 0.05f;
        Texture2D flame = Assets.Images.NPCs.ThawBoss_FlameMask.Asset.Value;
        Texture2D flameNoise = Assets.Images.Noise.SmearNoise.Asset.Value;
        Texture2D flameBase = Assets.Images.Particles.Glow.Asset.Value;

        Debug.Assert(flameShader is not null);
        Debug.Assert(pixelizationShader is not null);
        Debug.Assert(managedRenderTarget is not null);
        Debug.Assert(managedRenderTarget.Value is not null);

        var ss = new SpriteBatchSnapshot(spriteBatch);
        spriteBatch.End();

        var renderTargets = Main.instance.GraphicsDevice.GetRenderTargets();
        RtContentPreserver.ApplyToBindings(renderTargets);

        Main.instance.GraphicsDevice.SetRenderTarget(managedRenderTarget.Value);
        Main.instance.GraphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, ss.DepthStencilState, ss.RasterizerState, flameShader.Shader, ss.TransformMatrix);
        flameShader.Parameters.uSecondaryTexture = flameNoise;
        flameShader.Parameters.uProgress = flameProgress;
        flameShader.Parameters.uColorRes = 10f;
        flameShader.Parameters.uColor = new Color(255, 147, 3).ToVector4() * 0.4f * (BreathMult() + 0.8f) * 0.4f;
        for (int i = -1; i < 2; i++)
        {
            UnifiedRandom flameRand = new UnifiedRandom(i + 10);
            flameShader.Parameters.uScale = flameRand.NextFloat(1, 3);
            flameShader.Parameters.uWaviness = flameRand.NextFloat(1, 2f) * (i % 2 == 0 ? 1 : -1);
            flameShader.Apply();
            spriteBatch.Draw(flame, NPC.Center - screenPos, null, Color.White, 0, flame.Size() / 2 + new Vector2(i, 56 - MathF.Abs(i) * 12), 1, SpriteEffects.None, 0);
        }
        flameShader.Parameters.uColor = new Color(247, 209, 81).ToVector4() * (BreathMult() + 0.8f);
        for (int i = -1; i < 2; i++)
        {
            UnifiedRandom flameRand = new UnifiedRandom(i + 20);
            flameShader.Parameters.uScale = flameRand.NextFloat(1, 2);
            flameShader.Parameters.uWaviness = flameRand.NextFloat(1, 2f) * (i % 2 == 0 ? 1 : -1);
            flameShader.Apply();
            spriteBatch.Draw(flame, NPC.Center - screenPos, null, Color.White, 0, flame.Size() / 2 + new Vector2(i * 6, 56 - MathF.Abs(i) * 10), 0.7f, SpriteEffects.None, 0);
        }
        flameShader.Parameters.uScale = 2;
        flameShader.Parameters.uWaviness = 0;
        flameShader.Parameters.uColorRes = 32f;
        flameShader.Apply();
        for (int i = 0; i < 2; i++)
            spriteBatch.Draw(flameBase, NPC.Center - screenPos, null, Color.White, 0, flameBase.Size() / 2 + new Vector2(0, 50 * i), .18f, SpriteEffects.None, 0);

        spriteBatch.End();
        Main.instance.GraphicsDevice.SetRenderTargets(renderTargets);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);

        pixelizationShader.Parameters.uPixel = 2f * Main.GameViewMatrix.Zoom.X;
        pixelizationShader.Parameters.uSize = new Vector2(managedRenderTarget.Value.Width, managedRenderTarget.Value.Height);
        pixelizationShader.Apply();
        spriteBatch.Draw(managedRenderTarget.Value, new Rectangle(0, 0, managedRenderTarget.Value.Width, managedRenderTarget.Value.Height), Color.White);

        spriteBatch.Restart(in ss);
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Debug.Assert(lightingShader is not null);
        Texture2D body = TextureAssets.Npc[Type].Value;
        Texture2D head = Assets.Images.NPCs.ThawBoss_Head.Asset.Value;
        Texture2D orb = Assets.Images.NPCs.ThawBoss_Orb.Asset.Value;
        Texture2D cloth = Assets.Images.NPCs.ThawBoss_Cloth.Asset.Value;

        if (!NPC.IsABestiaryIconDummy)
            DrawFlame(spriteBatch, screenPos);

        Main.EntitySpriteDraw(orb, NPC.Center - screenPos, OrbFrame, Color.White, 0, new Vector2(42) / 2, 1, OrbDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

        // cloth stuff here

        var ss = new SpriteBatchSnapshot(spriteBatch);
        float bodyRotation = NPC.rotation + MathHelper.Clamp(NPC.velocity.X * 0.01f, -2.5f, .8f); // ice side is heavier, cool details!!
        spriteBatch.End();
        spriteBatch.Begin(ss.SortMode, ss.BlendState, ss.SamplerState, ss.DepthStencilState, ss.RasterizerState, lightingShader.Shader, ss.TransformMatrix);
        lightingShader.Parameters.colorTL = new Vector4(Lighting.GetSubLight(NPC.Center - body.Size() / 2), 1);
        lightingShader.Parameters.colorTR = new Vector4(Lighting.GetSubLight(NPC.Center + new Vector2(body.Width, -body.Height) / 2), 1);
        lightingShader.Parameters.colorBL = new Vector4(Lighting.GetSubLight(NPC.Center + new Vector2(-body.Width, body.Height) / 2), 1);
        lightingShader.Parameters.colorBR = new Vector4(Lighting.GetSubLight(NPC.Center + body.Size() / 2), 1);
        lightingShader.Apply();
        Main.EntitySpriteDraw(body, NPC.Center - screenPos, null, Color.White, bodyRotation, body.Size() / 2 + new Vector2(24, -24), NPC.scale, SpriteEffects.None);
        spriteBatch.Restart(in ss);

        Main.EntitySpriteDraw(head, NPC.Center + new Vector2(0, BreathMult() * -4) - screenPos, NPC.frame, drawColor, 0, new Vector2(160 / 2, 126), NPC.scale, SpriteEffects.None);
        return false;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D boneArm = Assets.Images.NPCs.ThawBoss_Bone.Asset.Value;
        Texture2D bladeArm = Assets.Images.NPCs.ThawBoss_Blade.Asset.Value;

        Vector2 boneArmPos = NPC.Center + new Vector2(66, 48);
        for (int i = NPC.oldPos.Length - 1; i > 0; i--)
        {
            if (NPC.oldPos[i] != Vector2.Zero)
            {
                boneArmPos = NPC.oldPos[i] + NPC.Size / 2 + new Vector2(66, 48);
                break;
            }
        }

        visualArmTarget = Vector2.Lerp(visualArmTarget, armTarget, 0.1f);
        float limit = boneArm.Height * 2;
        float length = Vector2.Distance(boneArmPos, visualArmTarget);
        float boneAngle = MathF.Acos(MathHelper.Clamp(MathF.Pow(length, 2) / (2 * length * limit), -1, 1));
        float boneArmRotation = boneArmPos.DirectionTo(visualArmTarget).ToRotation() - boneAngle;

        Vector2 bladeArmPos = boneArmPos + boneArmRotation.ToRotationVector2() * boneArm.Height * 0.9f;
        Texture2D orb = Assets.Images.NPCs.ThawBoss_Orb.Asset.Value;

        Main.EntitySpriteDraw(orb, visualArmTarget - screenPos, OrbFrame with { Y = 44 * 5 }, Color.White, 0, new Vector2(42) / 2, 1, OrbDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

        Main.EntitySpriteDraw(boneArm, boneArmPos - screenPos, null, new(Lighting.GetSubLight(boneArmPos + boneArmRotation.ToRotationVector2() * boneArm.Height * 0.5f)), boneArmRotation - MathHelper.PiOver2, new Vector2(boneArm.Width / 2, 0), NPC.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(bladeArm, bladeArmPos - screenPos, null, new(Lighting.GetSubLight(bladeArmPos + bladeArmPos.DirectionTo(visualArmTarget) * bladeArm.Height * 0.5f)), bladeArmPos.DirectionTo(visualArmTarget).ToRotation() - MathHelper.PiOver2 + MathHelper.ToRadians(5), new Vector2(bladeArm.Width / 2 - 14, 0), NPC.scale, SpriteEffects.None);
    }
    float BreathMult() => MathF.Pow(MathHelper.Clamp(MathF.Sin(Main.GlobalTimeWrappedHourly * 0.8f), 0, 1), 2);
    public override void FindFrame(int frameHeight)
    {
        NPC.frame.Width = 160;
        NPC.frame.Height = 128;
    }
    public int OrbDirection = 1;
    public Rectangle OrbFrame = new Rectangle(0, 0, 42, 44);
    public Vector2 armTarget, visualArmTarget;
    public float State { get => NPC.ai[0]; set => NPC.ai[0] = value; }
    public float Timer { get => NPC.ai[1]; set => NPC.ai[1] = value; }
    public override void AI()
    {
        Player player = Main.player[NPC.target];
        if (!NPC.HasValidTarget)
            NPC.TargetClosest(false);
        AmbientVFX();
        armTarget = NPC.Center + new Vector2(66, 450).RotatedBy(NPC.velocity.X * 0.1f - BreathMult() * 0.05f); //temp here
        //BasicIdleMovement();
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
    public void AmbientVFX()
    {
        Lighting.AddLight(NPC.Center, 0.4f, 0.3f, 0);
    }
    public void BasicIdleMovement()
    {
        Player player = Main.player[NPC.target];
        float wobble = MathF.Sin(new UnifiedRandom((int)State).Next(2000) + Timer * 0.04f);
        NPC.velocity = Vector2.Lerp(NPC.velocity, (player.Center - new Vector2(wobble * 40, 200 - MathHelper.SmoothStep(0, 30, MathF.Abs(wobble))) - NPC.Center) * 0.05f, 0.025f);

        armTarget = NPC.Center + new Vector2(66, 450).RotatedBy(NPC.velocity.X * 0.1f);
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
        return SetState(0, 1, 300, -300);
    }
    public float FlamingHail()
    {
        return SetState(1, 2, 200, -2000);
    }
    public float SetState(float curState, float nextState, float timeLimit, float resetTime = -150)
    {
        return curState;

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
