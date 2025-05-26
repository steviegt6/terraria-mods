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
        { AttackType.Propel, 0.25f },
        { AttackType.Flurry, 0.25f },
        { AttackType.BookOfSkulls, 0.25f },
        { AttackType.MaggotBall, 0.25f }
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

    Vector2 Jump(NPC npc, Vector2 targetPosition, float speed, float acceleration, float time, float gravity)
    {
        // ugh, something is wrong here

        Vector2 direction = targetPosition - npc.Center;
        float distance = direction.Length();
        direction.Normalize();

        float angleRadians = MathHelper.ToRadians(45); // Example angle of 45 degrees
        float gravityEffect = gravity;
        float dragCoefficient = 0.001f; // Example drag coefficient

        float initialVelocityMagnitude = (float)Math.Sqrt((distance * gravityEffect) / Math.Sin(2 * angleRadians));
        Vector2 initialVelocity = direction * initialVelocityMagnitude;

        float timeToTarget = distance / initialVelocityMagnitude;
        initialVelocity.Y -= gravityEffect * timeToTarget / 2;

        float dragFactor = (float)Math.Exp(-dragCoefficient * timeToTarget);
        initialVelocity *= dragFactor;

        return initialVelocity;
    }

    public override bool Update(params ConsumerData[] parameters)
    {
        if (!activated)
        {
            activated = true;
            Main.NewText("Water rocket!");
            stateData.ThisNPC.velocity += Jump(stateData.ThisNPC, Main.player[stateData.ThisNPC.target].Center, 1f, 1f, 0.5f, 0.1f);
            stateData.ThisNPC.noGravity = true;
        }

        if (timer++ < maxTime)
        {
            // do nothing

        }
        else
        {
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
            SoundEngine.PlaySound(SoundID.NPCDeath13, stateData.ThisNPC.Center);
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

                periodBetweenShotsAdjusted = Math.Max(1, periodBetweenShots / numShot);
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
            Main.NewText("Book of Skulls!");
        }

        stateData.ModNPC.SyncState();
        PopSelf();

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