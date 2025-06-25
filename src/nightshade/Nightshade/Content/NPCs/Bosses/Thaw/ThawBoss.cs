using Daybreak.Common.Rendering;
using Microsoft.Build.Construction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Rendering;
using Nightshade.Core;
using Nightshade.Core.Attributes;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
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
        NPC.buffImmune[BuffID.OnFire] = NPC.buffImmune[BuffID.OnFire3] =
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
        flameShader.Parameters.uColorRes = 16f;
        flameShader.Apply();
        for (int j = 0; j < 2; j++)
        {
            float rotation = NPC.velocity.X * -0.02f;
            if (possessionProgress < 1)
            {
                if (j == 1)
                    flameShader.Parameters.uColor = new Color(255, 147, 3).ToVector4() * 0.4f * (BreathMult() + 0.8f) * 0.4f * (1 - possessionProgress);
                else
                    flameShader.Parameters.uColor = new Color(255, 10, 3).ToVector4() * 0.3f * (BreathMult() + 0.8f) * 0.4f * (1 - possessionProgress);
                for (int i = -1; i < 2; i++)
                {
                    UnifiedRandom flameRand = new UnifiedRandom(i + 10);
                    flameShader.Parameters.uScale = flameRand.NextFloat(1, 3);
                    flameShader.Parameters.uWaviness = flameRand.NextFloat(1, 2f) * i;
                    flameShader.Parameters.Apply(flameShader.Shader.Parameters);
                    spriteBatch.Draw(flame, NPC.Center - screenPos, null, Color.White, i * 0.3f + rotation * (i == 0 ? 1 : 0), flame.Size() / 2 + new Vector2(i, 54 - MathF.Abs(i) * 12), new Vector2(1, 1 - MathF.Abs(i) * 0.1f), SpriteEffects.None, 0);
                }
            }
            if (possessionProgress > 0)
            {
                if (j == 1)
                    flameShader.Parameters.uColor = new Color(255, 147, 3).ToVector4() * 0.4f * (BreathMult() + 0.8f) * 0.4f * possessionProgress;
                else
                    flameShader.Parameters.uColor = new Color(255, 10, 3).ToVector4() * 0.3f * (BreathMult() + 0.8f) * 0.4f * possessionProgress;
                flameShader.Parameters.Apply(flameShader.Shader.Parameters);
                spriteBatch.Draw(flameBase, NPC.Center - screenPos, null, Color.White, rotation, new Vector2(flameBase.Width / 2, flameBase.Height / 1.5f), new Vector2(0.25f, 0.5f - j * 0.05f), SpriteEffects.None, 0);
                spriteBatch.Draw(flame, NPC.Center - screenPos, null, Color.White, rotation * 2, flame.Size() / 2 + new Vector2(0, 54), 0.5f, SpriteEffects.None, 0);
            }
        }

        flameShader.Parameters.uColor = new Color(247, 209, 81).ToVector4() * (BreathMult() + 0.8f);
        for (int i = -1; i < 2; i++)
        {
            UnifiedRandom flameRand = new UnifiedRandom(i + 20);
            flameShader.Parameters.uScale = flameRand.NextFloat(1, 2);
            flameShader.Parameters.uWaviness = flameRand.NextFloat(1, 2f) * (i % 2 == 0 ? 1 : -1);
            flameShader.Parameters.Apply(flameShader.Shader.Parameters);
            spriteBatch.Draw(flame, NPC.Center - screenPos, null, Color.White, i * 0.4f, flame.Size() / 2 + new Vector2(i * 6, 72 - MathF.Abs(i) * 10), new Vector2(0.6f, 0.5f - MathF.Abs(i) * 0.2f) * MathHelper.Lerp(1, 0.6f, possessionProgress), SpriteEffects.None, 0);
        }
        flameShader.Parameters.uScale = 2;
        flameShader.Parameters.uWaviness = 0;
        flameShader.Parameters.uColorRes = 32f;
        flameShader.Parameters.Apply(flameShader.Shader.Parameters);
        for (int i = 0; i < 2; i++)
            spriteBatch.Draw(flameBase, NPC.Center - screenPos, null, Color.White, 0, flameBase.Size() / 2 + new Vector2(0, 40 + 50 * i), .18f, SpriteEffects.None, 0);

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

        float bodyRotation = Utils.AngleLerp(NPC.rotation + MathHelper.Clamp(NPC.velocity.X * 0.01f, -2.5f, .8f), NPC.rotation + MathHelper.Clamp(bodyVelocity.X * 0.02f, -3f, 1f), possessionProgress); // ice side is heavier, cool details!!

        var ss = new SpriteBatchSnapshot(spriteBatch);
        spriteBatch.End();
        spriteBatch.Begin(ss.SortMode, ss.BlendState, ss.SamplerState, ss.DepthStencilState, ss.RasterizerState, lightingShader.Shader, ss.TransformMatrix);
        Vector4 lightingColor(Vector2 offset) => new Vector4(Lighting.GetSubLight(bodyCenter + new Vector2(0, 20) + offset.RotatedBy(bodyRotation)), 1);
        lightingShader.Parameters.colorTL = lightingColor(-body.Size() / 2);
        lightingShader.Parameters.colorTR = lightingColor(new Vector2(body.Width / 4, -body.Height / 4));
        lightingShader.Parameters.colorBL = lightingColor(new Vector2(-body.Width / 4, body.Height * 0.7f));
        lightingShader.Parameters.colorBR = lightingColor(new Vector2(body.Width / 4, body.Height * 0.7f));
        lightingShader.Apply();
        Main.EntitySpriteDraw(body, bodyCenter - screenPos, null, Color.White, bodyRotation, body.Size() / 2 + new Vector2(24, -24), NPC.scale, SpriteEffects.None);
        spriteBatch.Restart(in ss);

        Vector2 headOffset = new Vector2(0, BreathMult() * -4);
        if (bodyCenter.Length() > 0)
            headOffset = Vector2.SmoothStep(Vector2.Zero, Vector2.UnitY * 14, possessionProgress);
        Main.EntitySpriteDraw(head, bodyCenter + headOffset - screenPos, NPC.frame, new Color(Lighting.GetSubLight(bodyCenter + headOffset)), bodyRotation, new Vector2(160 / 2, 126), NPC.scale, SpriteEffects.None);
        return false;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D boneArm = Assets.Images.NPCs.ThawBoss_Bone.Asset.Value;
        Texture2D bladeArm = Assets.Images.NPCs.ThawBoss_Blade.Asset.Value;

        Vector2 boneArmPos = bodyCenter + new Vector2(66, 48);
        if (bodyCenter == NPC.Center)
        {
            for (int i = NPC.oldPos.Length - 1; i > 0; i--)
            {
                if (NPC.oldPos[i] != Vector2.Zero)
                {
                    boneArmPos = NPC.oldPos[i] + NPC.Size / 2 + new Vector2(66, 48);
                    break;
                }
            }
        }
        float limit = boneArm.Height * 2;
        float length = Vector2.Distance(boneArmPos, visualArmTarget);
        float boneAngle = MathF.Acos(MathHelper.Clamp(MathF.Pow(length, 2) / (2 * length * limit), -1, 1));
        float boneArmRotation = boneArmPos.DirectionTo(visualArmTarget).ToRotation() - boneAngle;

        bladeArmPos = boneArmPos + boneArmRotation.ToRotationVector2() * boneArm.Height * 0.9f;
        Main.EntitySpriteDraw(boneArm, boneArmPos - screenPos, null, new(Lighting.GetSubLight(boneArmPos + boneArmRotation.ToRotationVector2() * boneArm.Height * 0.5f)), boneArmRotation - MathHelper.PiOver2, new Vector2(boneArm.Width / 2, 0), NPC.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(bladeArm, bladeArmPos - screenPos, null, new(Lighting.GetSubLight(bladeArmPos + bladeArmPos.DirectionTo(visualArmTarget) * bladeArm.Height * 0.5f)), bladeArmPos.DirectionTo(visualArmTarget).ToRotation() - MathHelper.PiOver2 + MathHelper.ToRadians(5), new Vector2(bladeArm.Width / 2 - 14, 0), NPC.scale, SpriteEffects.None);
    }
    float BreathMult() => MathF.Pow(MathHelper.Clamp(MathF.Sin(Main.GlobalTimeWrappedHourly * 0.8f), 0, 1), 2);
    public Vector2 BladeArmTip() => bladeArmPos + bladeArmPos.DirectionTo(visualArmTarget).RotatedBy(-0.1f) * 160;
    public override void FindFrame(int frameHeight)
    {
        NPC.frame.Width = 160;
        NPC.frame.Height = frameHeight = 128;
        NPC.frameCounter++;
        switch (State)
        {
            case 0:
                NPC.frame.Y = 0;
                break;
            case 1:
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 2 * frameHeight && Timer < 70)
                        NPC.frame.Y += frameHeight;
                    else if (Timer > 70 && NPC.frame.Y > 0)
                        NPC.frame.Y -= frameHeight;
                }
                break;
            case 2:
                {
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < 2 * frameHeight && Timer < 80 && Timer > 50)
                            NPC.frame.Y += frameHeight;
                        else if (Timer > 330 && bodyVelocity.Length() <= 0 && NPC.frame.Y > 0)
                            NPC.frame.Y -= frameHeight;
                    }
                    break;
                }
        }
    }
    public int OrbDirection = 1;
    public Rectangle OrbFrame = new Rectangle(0, 0, 42, 44);
    public Vector2 armTarget, visualArmTarget, bladeArmPos, storedPlayerPosition, bodyCenter, bodyVelocity;
    public float visualArmTargetSpeed = 0.1f, possessionProgress;
    public float State { get => NPC.ai[0]; set => NPC.ai[0] = value; }
    public float Timer { get => NPC.ai[1]; set => NPC.ai[1] = value; }
    public float ExtraTimer { get => NPC.ai[2]; set => NPC.ai[2] = value; }
    public Player player { get => Main.player[NPC.target]; }
    public override void AI()
    {
        if (!NPC.HasValidTarget)
            NPC.TargetClosest(false);
        AmbientVFX();
        if (!Collision.SolidCollision(bodyCenter - new Vector2(40, 25), 80, 50))
            bodyCenter += bodyVelocity;
        else if (bodyVelocity.Length() > 0)
        {
            Main.instance.CameraModifiers.Add(new PunchCameraModifier(bodyCenter, -bodyVelocity.SafeNormalize(-Vector2.UnitY), 5, 8, 30));
            bodyVelocity = Vector2.Zero;
            Projectile.NewProjectile(NPC.GetSource_FromThis(), bodyCenter, Vector2.Zero, ProjectileID.DD2OgreSmash, 0, 0);
            SoundEngine.PlaySound(SoundID.Item70, bodyCenter);
            if (State == 2)
                Timer = 120;
        }
        if (possessionProgress <= 0.01f)
            bodyCenter = NPC.Center + NPC.velocity;

        if (Main.mouseRight)
        {
            Timer = -200;
            possessionProgress = 0;
            NPC.Center = Main.MouseWorld;
            ExtraTimer = 0;
            NPC.frame.Y = 0;
            NPC.frameCounter = 1;
            visualArmTargetSpeed = 0.1f;
            bodyCenter = NPC.Center;
            NPC.velocity = Vector2.Zero;
            bodyVelocity = Vector2.Zero;
        }

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
                2 => LungeAttack(),
                _ => State,
            };
        }
    }
    public void AmbientVFX()
    {
        if (Main.rand.NextBool(500))
            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - new Vector2(20 * Main.rand.NextBool().ToDirectionInt(), MathHelper.Lerp(80, 20, possessionProgress)), -Vector2.UnitY.RotatedByRandom(1) * Main.rand.NextFloat(5, 10), ProjectileID.GreekFire1, 20, 0);
        visualArmTarget = Vector2.Lerp(visualArmTarget, armTarget, visualArmTargetSpeed);
        Lighting.AddLight(NPC.Center, 0.4f, 0.3f, 0);
    }
    public void BasicIdleMovement(Vector2? target = null)
    {
        float wobble = MathF.Sin(new UnifiedRandom((int)State).Next(2000) + Timer * 0.04f);
        NPC.velocity = Vector2.Lerp(NPC.velocity, ((target ?? (player.Center - new Vector2(wobble * 40, 200 - MathHelper.SmoothStep(0, 30, MathF.Abs(wobble))))) - NPC.Center) * 0.05f, 0.025f);

        armTarget = NPC.Center + new Vector2(66, 450).RotatedBy(NPC.velocity.X * 0.1f - BreathMult() * 0.05f);
    }
    public void Idle()
    {
        ExtraTimer = 0;
        possessionProgress = MathHelper.Lerp(possessionProgress, 0, 0.1f);
        if (Timer < -30)
            BasicIdleMovement();
        else
        {
            possessionProgress = 0;
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
        return 2;
        return SetState(0, 1, 300, -300);
    }
    public float FlamingHail()
    {
        if (Timer < 70)
            NPC.velocity = Vector2.Lerp(NPC.velocity, -Vector2.UnitY, 0.02f);
        else if (Timer == 80)
            NPC.velocity = Vector2.UnitY * 5;
        else
            NPC.velocity *= 0.9f;

        if (Timer > 20 && Timer < 100)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 vel = -Vector2.UnitY.RotatedByRandom(1) * Main.rand.NextFloat(5);
                if (Timer < 76)
                    vel = Main.rand.NextVector2Unit();
                Dust.NewDustPerfect(BladeArmTip(), DustID.Torch, vel);
            }
        }
        if (Timer < 60)
            armTarget = NPC.Center - new Vector2(0, 100).RotatedByRandom(Timer * 0.02f);
        else if (Timer > 70 && Timer < 100)
        {
            visualArmTargetSpeed = 0.2f;
            float progress = MathHelper.SmoothStep(0, 0.8f, MathF.Pow((Timer - 70) / 30f, 2));
            float rotation = -0.6f - MathHelper.PiOver4 * 3 + MathHelper.Pi * 3 / 2 * progress;
            armTarget = NPC.Center + rotation.ToRotationVector2() * 250;

            if (Timer > 82 && Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(NPC.GetSource_FromThis(), BladeArmTip(), NPC.Center.DirectionTo(BladeArmTip()) * Main.rand.NextFloat(5, 10), ProjectileID.GreekFire1, 20, 0);
        }
        else if (Timer > 110)
        {
            visualArmTargetSpeed = 0.1f;
            armTarget = NPC.Center + new Vector2(66, 450).RotatedBy(NPC.velocity.X * 0.1f - BreathMult() * 0.05f);
        }
        return SetState(1, 2, 130, -2000);
    }
    public float LungeAttack()
    {
        if ((Timer > 60 && Timer < 120) || (bodyVelocity.Length() > 0 && ExtraTimer < 25))
        {
            Vector2 pos = BladeArmTip() + Main.rand.NextVector2Circular(50, 50);
            Vector2 vel = pos.DirectionTo(BladeArmTip()) * Main.rand.NextFloat(1, 5);
            Dust.NewDustPerfect(pos, DustID.IceTorch, vel, Scale: 2).noGravity = true;
        }
        if (Timer == 1)
            ExtraTimer = Main.rand.Next(-2, 0);
        else if (Timer < 50)
            BasicIdleMovement(player.Center + new Vector2(700 * (ExtraTimer == -1 ? -1 : 1), -400));
        else if (Timer < 80)
        {
            ExtraTimer = 0;
            NPC.velocity = Vector2.Zero;
            visualArmTargetSpeed = 0.1f;
            possessionProgress = MathHelper.SmoothStep(0, 1, (Timer - 50f) / 30);
            armTarget = NPC.Center + new Vector2(230 * MathF.Sign(player.Center.X - NPC.Center.X), -250);
            bodyCenter = Vector2.Lerp(bodyCenter, NPC.Center + Vector2.UnitY * 60, 0.1f);
        }
        else if (Timer < 110)
            bodyCenter += Vector2.UnitY * MathHelper.SmoothStep(0, 2, (Timer - 80) / 30f) - bodyCenter.DirectionTo(player.Center);
        else if (Timer < 120)
        {
            armTarget = NPC.Center + new Vector2(230 * MathF.Sign(bodyVelocity.X), -250);
            bodyVelocity = NPC.Center.DirectionTo(player.Center - new Vector2(0, 200)) * (Timer - 110) * 4f;
        }
        else
        {
            if (bodyVelocity.Length() > 0)
            {
                NPC.velocity = (bodyCenter - NPC.Center) * 0.01f;
                bodyVelocity = Vector2.Lerp(bodyVelocity, Vector2.UnitY * 30, 0.01f);
                if (bodyCenter.Distance(player.Center) < 700 && ExtraTimer <= 0)
                {
                    storedPlayerPosition = bodyCenter + NPC.DirectionTo(player.Center) * 10000;
                    ExtraTimer = MathF.Floor(MathHelper.Lerp(1, 7, MathHelper.Clamp(1 - bodyCenter.Distance(player.Center) / 1000f, 0, 1)));
                }
                if (ExtraTimer > 0 && ExtraTimer < 20)
                {
                    ExtraTimer++;
                    float progress = MathHelper.SmoothStep(0, 1, MathF.Pow(ExtraTimer / 20f, 2));
                    visualArmTargetSpeed = MathHelper.Clamp(progress * 3, 0.2f, 0.7f);
                    float rotation = bodyCenter.DirectionTo(storedPlayerPosition).ToRotation() - MathHelper.PiOver4 * 3 + MathHelper.Pi * 3 / 2 * progress;
                    if (bodyVelocity.X < 0)
                        rotation = bodyCenter.DirectionTo(storedPlayerPosition).ToRotation() + MathHelper.PiOver4 * 3 - MathHelper.Pi * 3 / 2 * progress;
                    armTarget = bodyCenter + rotation.ToRotationVector2() * 250;

                    if (ExtraTimer > 10)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 vel = Main.rand.NextVector2Circular(5, 5);
                            Dust.NewDustPerfect(BladeArmTip(), DustID.IceTorch, vel, Scale: 2).noGravity = true;
                        }
                    }
                }
            }
            else
            {
                if (Timer < 280)
                    NPC.velocity = Vector2.Lerp(NPC.velocity, (bodyCenter - NPC.Center - new Vector2(MathF.Sin(Timer * 0.05f) * 100, 200 - MathF.Abs(MathF.Sin(Timer * 0.05f)) * 100)) * 0.025f, 0.1f);
                else if (Timer < 330)
                    NPC.velocity = Vector2.Lerp(NPC.velocity, (bodyCenter - NPC.Center) * 0.05f, 0.1f);
                else if (Timer == 330)
                {
                    NPC.Center = bodyCenter;
                    NPC.velocity = Vector2.zeroVector;
                }
                else if (Timer < 370)
                    possessionProgress = MathHelper.Lerp(1, 0, (Timer - 330) / 40f);
            }
        }
        return SetState(2, 3, 300);
    }
    public float SetState(float curState, float nextState, float timeLimit, float resetTime = -150)
    {
        return curState;

        if (Timer > timeLimit)
        {
            Timer = resetTime;
            visualArmTargetSpeed = 0.1f;
            NPC.velocity = Vector2.Zero;
            NPC.damage = 0;
            NPC.rotation = 0;
            bodyVelocity = Vector2.Zero;
            NPC.netUpdate = true;
            return nextState;
        }
        return curState;
    }
}
