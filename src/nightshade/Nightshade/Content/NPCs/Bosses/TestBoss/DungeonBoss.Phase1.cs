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

    public override bool Update(params ConsumerData[] parameters)
    {
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
        return true;
    }
}



