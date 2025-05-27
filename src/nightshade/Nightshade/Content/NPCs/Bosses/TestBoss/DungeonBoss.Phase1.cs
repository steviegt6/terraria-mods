using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using Nightshade.Common.Utilities;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.Audio;

namespace Nightshade.Content.NPCs.Bosses;

internal class Consumer_PhaseOneBasicTransitionState : BossState
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

    enum AttackType
    {
        Propel,
        Flurry,
        BookOfSkulls,
        MaggotBall
    }

    private Dictionary<AttackType, float> attackList = new()
    {
        { AttackType.Propel, 0.25f }, // todo: visuals, slam attack maybe?
        { AttackType.Flurry, 0.25f }, // todo: rain
        { AttackType.BookOfSkulls, 0.25f }, // todo: book functionality
        { AttackType.MaggotBall, 0.25f } // todo: do
    };

    public override bool Update(params ConsumerData[] parameters)
    {
        if (!activated)
        {
            activated = true;
            Main.NewText("The Consumer is transitioning to Phase One!");
            PopSelf(); // make it so we are fated to die! 
        }

        WeightedRandom<AttackType> random = new WeightedRandom<AttackType>();

        foreach (var attack in attackList)
        {
            random.Add(attack.Key, attack.Value);
        }
        int numAttacks = 4;

        List<AttackType> selectedAttacks = new List<AttackType>();

        for (int i = 0; i < numAttacks; i++)
        {
            AttackType attack = random.Get();
            while (selectedAttacks.Contains(attack))
            {
                attack = random.Get();
            }
            selectedAttacks.Add(attack);
        }

        selectedAttacks.Reverse();
        Main.NewText($"The Consumer will use the following attacks: {string.Join(", ", selectedAttacks)}");

        stateData.ModNPC.AddState(new Consumer_PhaseOneDoneState());

        foreach (var attack in selectedAttacks)
        {
            switch (attack)
            {
                case AttackType.Propel:
                    stateData.ModNPC.AddState(new Consumer_PhaseOnePropelAttackState());
                    break;
                case AttackType.Flurry:
                    stateData.ModNPC.AddState(new Consumer_PhaseOneFlurryAttackState());
                    break;
                case AttackType.BookOfSkulls:
                    stateData.ModNPC.AddState(new Consumer_PhaseOneBookOfSkullsAttackState());
                    break;
                case AttackType.MaggotBall:
                    stateData.ModNPC.AddState(new Consumer_PhaseOneMaggotBallAttackState());
                    break;
            }
        }

        stateData.stateDone = true;
        stateData.ModNPC.SyncState(this);

        return true;
    }
}

// The Consumer propels herself forward toward a target using a water attack
internal class Consumer_PhaseOnePropelAttackState : BossState
{
    private const int minTime = 60;
    public override bool Enter(params ConsumerData[] parameters)
    {
        maxTime = minTime + (Main.rand.Next() % 15);
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

    Vector2 Jump(NPC npc, Vector2 targetPosition, float gravity, float angle, float? specifiedTimeToTarget = null)
    {
        Vector2 direction = targetPosition - npc.Center;
        float distance = direction.Length();
        direction.Normalize();

        float angleRadians = angle;

        float timeToTarget = specifiedTimeToTarget ?? (float)Math.Sqrt((2 * distance) / gravity);
        float initialVelocityMagnitude = distance / timeToTarget;
        Vector2 initialVelocity = direction * initialVelocityMagnitude;

        initialVelocity.Y -= gravity * timeToTarget / 2;

        Vector2 position = npc.Center;
        Vector2 velocity = initialVelocity;
        float timeStep = 1f / 60f;

        for (float t = 0; t < timeToTarget; t += timeStep)
        {
            position += velocity * timeStep;
            velocity.Y += gravity * timeStep;

            if (Collision.SolidCollision(position, 1, 1))
            {
                return Vector2.Zero; // stop fucking hiding!!!!!!!!!!!!!!!!!!!!!!!
            }
        }

        return initialVelocity;
    }

    float FindLaunchAngle(NPC npc, Vector2 targetPosition, float gravity)
    {
        Vector2 direction = targetPosition - npc.Center;
        float distance = direction.Length();
        float heightDifference = npc.Center.Y - targetPosition.Y;

        float velocitySquared = (distance * gravity) / MathF.Sin(MathHelper.PiOver4);
        if (velocitySquared <= 0) return MathHelper.PiOver4;

        float velocity = MathF.Sqrt(velocitySquared);

        float discriminant = (velocity * velocity * velocity * velocity) - gravity * (gravity * distance * distance - 2 * heightDifference * velocity * velocity);
        if (discriminant < 0) return MathHelper.PiOver4; // no solution

        float angle1 = MathF.Atan((velocity * velocity + MathF.Sqrt(discriminant)) / (gravity * distance));
        float angle2 = MathF.Atan((velocity * velocity - MathF.Sqrt(discriminant)) / (gravity * distance));

        if (float.IsNaN(angle1) || float.IsNaN(angle2))
        {
            return MathHelper.PiOver4; // what are we doing here?
        }

        return MathF.Abs(MathF.Max(angle1, angle2));
    }

    public override bool Update(params ConsumerData[] parameters)
    {
        if (!activated)
        {

            activated = true;
            stateData.ThisNPC.TargetClosest();
            ConsumerOfSouls.gravity = 0.2f;
            float gravity = ConsumerOfSouls.gravity;
            Main.NewText("Water rocket!");
            float angle = FindLaunchAngle(stateData.ThisNPC, Main.player[stateData.ThisNPC.target].Center, gravity);
            Main.NewText($"Calculated launch angle: {MathHelper.ToDegrees(angle)} degrees", Color.Green);


            if ((stateData.ThisNPC.velocity = Jump(stateData.ThisNPC, Main.player[stateData.ThisNPC.target].Center, gravity, angle, 120f)) != Vector2.Zero)
            {
                Main.NewText("Jump is possible", Color.Green);

            }
            else
            {
                Main.NewText("Jump is impossible", Color.Red);

                // todo: teleport
                stateData.ThisNPC.position = Main.player[stateData.ThisNPC.target].Center + new Vector2(0, -stateData.ThisNPC.height);
            }

            SoundEngine.PlaySound(SoundID.AbigailCry, stateData.ThisNPC.Center);
        }

        if (timer++ < maxTime)
        {
            if (timer % (maxTime / 4) == 0)
            {
                SoundEngine.PlaySound(SoundID.ShimmerWeak1, stateData.ThisNPC.Center);
            }
            else if (timer % (maxTime / 8) == 0)
            {
                // todo: replace with actual visual
                Dust.NewDustPerfect(stateData.ThisNPC.Center, DustID.DungeonWater);
            }

            stateData.ThisNPC.rotation = 0f;
        }
        else
        {
            if (stateData.ThisNPC.velocity.Y != 0f && !stateData.ThisNPC.collideY)
            {
                return true;
            }
            SoundEngine.PlaySound(SoundID.DD2_SkeletonDeath, stateData.ThisNPC.Center);
            Main.NewText("Water rocket finished", Color.Red);
            stateData.ModNPC.SyncState();
            PopSelf();
        }

        return true;
    }
}

// The consumer summons a flurry of water bolt attacks to assail the enemy
internal class Consumer_PhaseOneFlurryAttackState : BossState
{
    private const int minTime = 60;
    private const float maxProjectileSpeed = 10.0f;

    public override bool Enter(params ConsumerData[] parameters)
    {
        maxTime = minTime + (Main.rand.Next() % 15);
        numProjectiles = 5 + (Main.rand.Next() % 5);

        periodBetweenShots = maxTime / numProjectiles;

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

    int numProjectiles;
    int periodBetweenShots;
    int periodBetweenShotsAdjusted;
    int numShot;
    Vector2 savedPosition;
    Player target;

    float angle = MathHelper.PiOver4;
    public override bool Update(params ConsumerData[] parameters)
    {
        if (!activated)
        {
            activated = true;

            stateData.ThisNPC.TargetClosest();
            target = Main.player[stateData.ThisNPC.target];
            savedPosition = target.Center;

            periodBetweenShotsAdjusted = periodBetweenShots;

            Main.NewText("Water Bolt!");
            SoundEngine.PlaySound(SoundID.AbigailUpgrade, stateData.ThisNPC.Center);
        }

        if (timer++ < maxTime)
        {
            if (timer % periodBetweenShotsAdjusted == 0 && ++numShot <= numProjectiles)
            {
                Vector2 targetPosition = savedPosition; // todo: account for edge cases here

                Vector2 direction = targetPosition - stateData.ThisNPC.Center;
                direction.Normalize();

                float quotient = Math.Clamp(1 - (numShot / (float)numProjectiles), 0.2f, 1.0f);
                float speed = 10.0f * quotient;
                direction *= speed;

                angle *= quotient;
                Vector2 projectileDirection = direction.RotatedBy(angle * -stateData.ThisNPC.direction);

                var proj = Projectile.NewProjectileDirect(stateData.ThisNPC.GetSource_FromThis(), stateData.ThisNPC.Center, projectileDirection, ProjectileID.WaterBolt, 20, 1f, Main.myPlayer);
                proj.friendly = false;
                proj.hostile = true;
                proj.timeLeft = 60 * 10;
                var gProj = proj.GetGlobalProjectile<ModifyProjectiles>();
                gProj.consumerMark = stateData.ThisNPC.whoAmI;
                gProj.shouldDieOnHitInstantly = true;

                stateData.ThisNPC.velocity += projectileDirection * -0.1f;

                var id = SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, stateData.ThisNPC.Center);
                var sound = SoundEngine.GetActiveSound(id) ?? throw new Exception("Failed to get sound from SoundEngine after playing it.");
                sound.Pitch = Main.rand.NextFloat();
                sound.Volume += .5f;

                sound.Sound.dspSettings = new FAudio.F3DAUDIO_DSP_SETTINGS();
                sound.Sound.dspSettings.DopplerFactor = 1.0f;

                // todo: move this elsewhere
                var style = sound.Style;
                style.MaxInstances = style.MaxInstances == 0 ? 1 : style.MaxInstances;
                sound.Style = style;

                periodBetweenShotsAdjusted = Math.Max(12, periodBetweenShots / numShot);
            }
        }
        else
        {
            Main.NewText("Flurry finished", Color.Red);
            stateData.ModNPC.SyncState();
            PopSelf();
        }

        return true;
    }
}

// The consumer summons book of skulls projectiles around her to home in on the player
internal class Consumer_PhaseOneBookOfSkullsAttackState : BossState
{
    private const int minTime = 60;
    private const float maxProjectileSpeed = 10.0f;

    public override bool Enter(params ConsumerData[] parameters)
    {
        maxTime = minTime + (Main.rand.Next() % 15);
        numProjectiles = 5 + (Main.rand.Next() % 5);

        periodBetweenShots = maxTime / numProjectiles;

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

    int numProjectiles;
    int periodBetweenShots;
    int periodBetweenShotsAdjusted;
    int numShot;
    Vector2 savedPosition;
    Player target;

    float angle = MathHelper.PiOver4;
    public override bool Update(params ConsumerData[] parameters)
    {
        if (!activated)
        {
            activated = true;

            stateData.ThisNPC.TargetClosest();
            target = Main.player[stateData.ThisNPC.target];
            savedPosition = target.Center;

            periodBetweenShotsAdjusted = periodBetweenShots;

            Main.NewText("Book of Skulls!");
            SoundEngine.PlaySound(SoundID.AbigailUpgrade, stateData.ThisNPC.Center);
        }

        if (timer++ < maxTime)
        {
            if (timer % periodBetweenShotsAdjusted == 0 && ++numShot <= numProjectiles)
            {
                Vector2 targetPosition = savedPosition; // todo: account for edge cases here

                Vector2 direction = targetPosition - stateData.ThisNPC.Center;
                direction.Normalize();

                float quotient = Math.Clamp(1 - (numShot / (float)numProjectiles), 0.2f, 1.0f);
                float speed = 2.0f;
                direction *= speed;

                Vector2 position = stateData.ThisNPC.Center + ((Vector2.One * stateData.ThisNPC.direction).RotatedBy((MathHelper.TwoPi / (numProjectiles)) * numShot) * 64f);
                angle *= quotient;
                Vector2 projectileDirection = direction;

                var proj = Projectile.NewProjectileDirect(stateData.ThisNPC.GetSource_FromThis(), position, projectileDirection, ProjectileID.BookOfSkullsSkull, 20, 1f, 0);
                proj.friendly = false;
                proj.hostile = true;
                proj.tileCollide = false;
                proj.timeLeft = 60 * 10;
                var gProj = proj.GetGlobalProjectile<ModifyProjectiles>();
                gProj.consumerMark = stateData.ThisNPC.whoAmI;
                gProj.shouldDieOnHitInstantly = true;
                gProj.target = target.whoAmI;

                var id = SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, stateData.ThisNPC.Center);
                var sound = SoundEngine.GetActiveSound(id) ?? throw new Exception("Failed to get sound from SoundEngine after playing it.");
                sound.Pitch = Main.rand.NextFloat();
                sound.Volume += .5f;

                sound.Sound.dspSettings = new FAudio.F3DAUDIO_DSP_SETTINGS();
                sound.Sound.dspSettings.DopplerFactor = 1.0f;

                // todo: move this elsewhere
                var style = sound.Style;
                style.MaxInstances = style.MaxInstances == 0 ? 1 : style.MaxInstances;
                sound.Style = style;

            }
        }
        else
        {
            Main.NewText("Book of Skulls finished", Color.Red);
            stateData.ModNPC.SyncState();
            PopSelf();
        }

        return true;
    }
}

// The consumer shoots a gross ball of maggots which leaves a trail of them the ground
internal class Consumer_PhaseOneMaggotBallAttackState : BossState
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
            Main.NewText("Maggot Ball!");
        }

        stateData.ModNPC.SyncState();
        PopSelf();

        return true;
    }
}

internal class Consumer_PhaseOneDoneState : BossState
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
            Main.NewText("Attacks finished", Color.Red);
        }

        stateData.ModNPC.SyncState();
        if (timer++ == 60)
        {
            PopSelf();

            Main.NewText("Go back to phase check", Color.Red);
            stateData.ModNPC.AddState(new Consumer_PhaseTransitionState());
        }

        return true;
    }
}