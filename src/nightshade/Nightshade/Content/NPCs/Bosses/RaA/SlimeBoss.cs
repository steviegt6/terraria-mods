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

        NPC.width = 340;
        NPC.height = 280;

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

		NPC.velocity *= 0.95f;

		NPC.direction = NPC.velocity.X > 0 ? 1 : -1;

		if (NPC.Distance(Main.MouseWorld) > 100)
			NPC.velocity += NPC.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero) * 0.5f;
	}

	public override void FindFrame(int frameHeight)
	{
		if (facingDirection != NPC.direction)
		{
			if (--facingTimer <= 0)
			{
				facingTimer = 8;
				facingDirection += Math.Sign(NPC.direction);
			}
		}
	}

	public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
	{
		scale *= 1.25f;
		position.Y += 50;
		return base.DrawHealthBar(hbPosition, ref scale, ref position);
	}

	private int facingTimer;
	private int facingDirection;

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D mouthTexture = TextureAssets.Npc[Type].Value;
		Rectangle mouthFrame = mouthTexture.Frame(3, 1, facingDirection + 1);

		Texture2D brainTexture = Assets.Images.NPCs.Bosses.RaA.SlimeBossBrain.Asset.Value;
		Rectangle brainFrame = brainTexture.Frame(3, 1, facingDirection + 1);

		Vector2 mouthPosition = NPC.Center + new Vector2(0, 100).RotatedBy(NPC.rotation) * NPC.scale;
		spriteBatch.Draw(mouthTexture, mouthPosition - screenPos, mouthFrame, Color.White, NPC.rotation, mouthFrame.Size() / 2, NPC.scale, 0, 0);

		Vector2 brainOrigin = new Vector2(brainFrame.Width / 2 + 26 * facingDirection, brainFrame.Height / 2 + 86);
		spriteBatch.Draw(brainTexture, NPC.Center - screenPos, brainFrame, Color.White, NPC.rotation, brainOrigin, NPC.scale, 0, 0);

		return false;
	}
}