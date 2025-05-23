using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

public sealed partial class SlimeBoss : ModNPC
{
    public override string Texture => Assets.Images.NPCs.Bosses.RaA.SlimeBoss.KEY;

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();

		NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
		NPCID.Sets.ImmuneToAllBuffs[Type] = true;
		NPCID.Sets.TeleportationImmune[Type] = true;
	}

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 180;
        NPC.height = 154;

		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.boss = true;

        // Stats
        NPC.lifeMax = 10000;
    }

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		base.SetBestiary(database, bestiaryEntry);
	}

	public override void AI()
	{
		base.AI();

		NPC.velocity *= 0.9f;

		NPC.direction = NPC.velocity.X > 0 ? 1 : -1;

		if (NPC.Distance(Main.MouseWorld) > 100)
			NPC.velocity += NPC.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero) * 0.5f;
	}

	public override void FindFrame(int frameHeight)
	{
		if (facing != NPC.direction)
		{
			if (++facingTimer > 5)
			{
				facingTimer = 0;
				facing += Math.Sign(NPC.direction);
			}
		}
	}

	private int facingTimer;
	private int facing;

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D mouthTexture = TextureAssets.Npc[Type].Value;
		Rectangle mouthFrame = mouthTexture.Frame(3, 1, facing + 1);

		spriteBatch.Draw(mouthTexture, NPC.Center - screenPos, mouthFrame, Color.White, NPC.rotation, mouthFrame.Size() / 2, NPC.scale, 0, 0);


		return false;
	}
}