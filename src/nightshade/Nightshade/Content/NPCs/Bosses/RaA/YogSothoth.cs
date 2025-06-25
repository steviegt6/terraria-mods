using Daybreak.Common.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

public sealed partial class YogSothoth : ModNPC
{
	public override string Texture => Assets.Images.NPCs.Bosses.RaA.YogSothoth.KEY;

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
		NPC.hide = false;
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		base.SetBestiary(database, bestiaryEntry);
	}

	public ref float Time => ref NPC.ai[0];

	public override void AI()
	{
		base.AI();

		NPC.velocity *= 0.9f;

		NPC.direction = NPC.velocity.X > 0 ? 1 : -1;

		NPC.velocity += NPC.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero) * NPC.Distance(Main.MouseWorld) * 0.005f;
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
		return false;
		//scale *= 1.25f;
		//position.Y += 50;
		//return base.DrawHealthBar(hbPosition, ref scale, ref position);
	}

	private int facingTimer;
	private int facingDirection;

	public override void DrawBehind(int index)
	{
		if (!NPC.IsABestiaryIconDummy)
		{
			DarkeningMistSystem.GasCenter = NPC.Center;
			DarkeningMistSystem.Draws.Add(DrawToMist);
		}
	}

	private void DrawToMist(SpriteBatch spriteBatch)
	{
		Texture2D glowTexture = Assets.Images.Extras.HardGlow.Asset.Value;
		float spin = Main.GlobalTimeWrappedHourly * MathHelper.Pi * NPC.direction;
		spriteBatch.Draw(glowTexture, NPC.Center + new Vector2(0, -86) - Main.screenPosition, glowTexture.Frame(), Color.White with { A = 112 }, NPC.rotation + spin, glowTexture.Size() / 2, NPC.scale / 2f, 0, 0);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D glow = Assets.Images.Extras.GlowBig.Asset.Value;

		Texture2D brainTexture = TextureAssets.Npc[Type].Value;
		Rectangle brainFrame = brainTexture.Frame();//brainTexture.Frame(3, 1, facingDirection + 1);
		Vector2 brainOrigin = new Vector2(brainFrame.Width / 2, brainFrame.Height / 2 + 86);

		RestartSpriteBatch(spriteBatch, SpriteSortMode.Deferred, DarkeningMistSystem.MultiplyBlendState, NPC.IsABestiaryIconDummy);

		spriteBatch.Draw(glow, NPC.Center - screenPos + new Vector2(0, -50), glow.Frame(), Color.White * 0.5f, NPC.rotation, glow.Size() / 2, NPC.scale * 1.2f, 0, 0);

		RestartSpriteBatch(spriteBatch, SpriteSortMode.Deferred, BlendState.AlphaBlend, NPC.IsABestiaryIconDummy);

		//Vector2 mouthPosition = NPC.Center + new Vector2(0, 100).RotatedBy(NPC.rotation) * NPC.scale;
		//spriteBatch.Draw(mouthTexture, mouthPosition - screenPos, mouthFrame, Color.White, NPC.rotation, mouthFrame.Size() / 2, NPC.scale, 0, 0);

		spriteBatch.Draw(brainTexture, NPC.Center - screenPos, brainFrame, Color.White, NPC.rotation, brainOrigin, NPC.scale, 0, 0);


		return false;
	}

	public void RestartSpriteBatch(SpriteBatch spriteBatch, SpriteSortMode sortMode, BlendState blendState, bool forUI)
	{
		if (forUI)
		{
			RasterizerState priorRasterizer = spriteBatch.GraphicsDevice.RasterizerState;
			Rectangle priorScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			spriteBatch.End();
			spriteBatch.GraphicsDevice.RasterizerState = priorRasterizer;
			spriteBatch.GraphicsDevice.ScissorRectangle = priorScissorRectangle;
			spriteBatch.Begin(sortMode, blendState, Main.DefaultSamplerState, DepthStencilState.None, priorRasterizer, null, Main.UIScaleMatrix);
		}
		else
		{
			spriteBatch.End();
			spriteBatch.Begin(sortMode, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
		}
	}
}