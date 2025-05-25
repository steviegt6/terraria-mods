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
        int numAttacks = 1 + Main.rand.Next(3);

        List<AttackType> selectedAttacks = new List<AttackType>();

        for (int i = 0; i < numAttacks; i++)
        {
            AttackType attack = random.Get();
            selectedAttacks.Add(attack);
        }
        Main.NewText($"The Consumer will use the following attacks: {string.Join(", ", selectedAttacks)}");

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

        stateData.ModNPC.AddState(new Consumer_PhaseOneDoneState());

        stateData.stateDone = true;
        stateData.ModNPC.SyncState(this);

        return true;
    }
}

// The Consumer propels herself forward toward a target using a water attack
internal class Consumer_PhaseOnePropelAttackState : BossState
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
            Main.NewText("Water rocket!");
        }

        stateData.ModNPC.SyncState();
        PopSelf();

        return true;
    }
}

// The consumer summons a flurry of water bolt attacks to assail the enemy
internal class Consumer_PhaseOneFlurryAttackState : BossState
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
            Main.NewText("Water Bolt!");
        }

        stateData.ModNPC.SyncState();
        PopSelf();

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