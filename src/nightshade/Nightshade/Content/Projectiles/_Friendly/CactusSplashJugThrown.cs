using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Features;
using Nightshade.Content.Dusts;
using Nightshade.Content.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Projectiles._Friendly;

public class CactusSplashJugThrown : ModProjectile
{
	public override string Texture => Assets.Images.Items.Misc.CactusSplashJug.KEY;

	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 10;
		Projectile.hostile = false;
		Projectile.tileCollide = true;
	}

	public ref float BurstState => ref Projectile.ai[0];
	public ref float Time => ref Projectile.ai[1];

	public override void AI()
	{
		if (BurstState == 1 && Time > 0)
		{
			SoundEngine.PlaySound(SoundID.SplashWeak with { Pitch = -1f, PitchVariance = 0.3f, MaxInstances = 0 }, Projectile.Center);
			Projectile.velocity.Y *= -1;
			Projectile.Kill();
		}

		if (Time > 10)
		{
			foreach (Player player in Main.ActivePlayers)
			{
				if (Projectile.Colliding(Projectile.Hitbox.Modified(-10, -10, 20, 20), player.Hitbox))
				{
					Projectile.Kill();
					break;
				}
			}

			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y += 0.6f;
			if (Projectile.velocity.Y > 24f)
				Projectile.velocity.Y = 24f;
		}

		Projectile.rotation += Projectile.velocity.X * 0.1f / (Time / 10f + 1f);

		if (Time % 15 == 0 || Main.rand.NextBool(8))
			Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -10).RotatedBy(Projectile.rotation), ModContent.DustType<DotDropletDust>(), Main.rand.NextVector2Circular(2, 1) + Projectile.velocity * Main.rand.NextFloat(0.4f, 1f), 0, LiquidColor(), Main.rand.NextFloat(0.5f, 1f));

		Time++;

		if (Projectile.velocity.X != 0)
			Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
	}

	public const int HEAL_AMOUNT = 30;
	public const int HEAL_RADIUS = 150;

	public override void OnKill(int timeLeft)
	{
		// TODO: Sync health gains ? dunno if it works

		HashSet<int> playersToHeal = new HashSet<int>();
		foreach (Player player in Main.ActivePlayers)
		{
			Vector2 checkPoint = Projectile.Center + Projectile.DirectionTo(player.Center).SafeNormalize(Vector2.Zero) * Math.Min(Projectile.Distance(player.Center), HEAL_RADIUS);
			
			if (!player.dead && player.Hitbox.Contains(checkPoint.ToPoint()))
				playersToHeal.Add(player.whoAmI);
		}

		HashSet<int> npcsToHeal = new HashSet<int>();
		foreach (NPC npc in Main.ActiveNPCs)
		{
			Vector2 checkPoint = Projectile.Center + Projectile.DirectionTo(npc.Center) * Math.Min(Projectile.Distance(npc.Center), HEAL_RADIUS);
			if (npc.Hitbox.Contains(checkPoint.ToPoint()) && npc.friendly)
				npcsToHeal.Add(npc.whoAmI);
		}

		int divisor = (int)Math.Ceiling(MathF.Sqrt(playersToHeal.Count + npcsToHeal.Count));
		int amtToHeal = HEAL_AMOUNT / Math.Max(1, divisor);

		if (Main.netMode != NetmodeID.MultiplayerClient)
		{
			foreach (int index in npcsToHeal)
				HealNPC(Main.npc[index], amtToHeal + Main.rand.Next(-8, 8));
		}

		foreach (int index in playersToHeal)
			Main.player[index].Heal(amtToHeal + Main.rand.Next(-8, 8));

		for (int i = 0; i < Main.rand.Next(15, 25); i++)
		{
			Vector2 dropletVel = new Vector2(Projectile.velocity.X * 0.33f, -Projectile.velocity.Y - 2f) * Main.rand.NextFloat(0.5f, 1f) + Main.rand.NextVector2Circular(6, 6);
			Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8, 8), ModContent.DustType<DotDropletDust>(), dropletVel, 0, LiquidColor(), Main.rand.NextFloat(0.6f, 2f));
		}

		GlowSplashParticle glowSplash = GlowSplashParticle.pool.RequestParticle();
		glowSplash.Prepare(Projectile.Center, 0f, (LiquidColor() * 0.4f) with { A = 0 }, 0.75f, 15);
		glowSplash.Lighted = false;
		ParticleEngine.Particles.Add(glowSplash);

		for (int i = 0; i < Main.rand.Next(10, 15); i++)
		{
			Vector2 dropletVel = new Vector2(Projectile.velocity.X * 0.33f, -Projectile.velocity.Y - 2f) * Main.rand.NextFloat() + Main.rand.NextVector2Circular(12, 12);
			LiquidSplashParticle splash = LiquidSplashParticle.pool.RequestParticle();
			splash.Prepare(Projectile.Center, dropletVel, LiquidColor() * 1.2f, Main.rand.NextFloat(1f, 2.5f), Main.rand.Next(18, 30));
			splash.Lighted = true;
			ParticleEngine.Particles.Add(splash);
		}

		// TODO: Sound design
		//		 Gourd gore/particles

		SoundEngine.PlaySound(SoundID.Dig with { Volume = 0.7f, Pitch = 1, PitchVariance = 0.2f, MaxInstances = 0 }, Projectile.Center);
		SoundEngine.PlaySound(SoundID.Item167 with { Volume = 0.7f, Pitch = 1f, PitchVariance = 0.2f, MaxInstances = 0 }, Projectile.Center);
		SoundEngine.PlaySound(SoundID.Splash with { Volume = 0.8f, Pitch = 0.8f, PitchVariance = 0.3f, MaxInstances = 0 }, Projectile.Center);
		SoundEngine.PlaySound(SoundID.Item107 with { Volume = 0.3f, Pitch = 0.6f, PitchVariance = 0.2f, MaxInstances = 0 }, Projectile.Center);
	}

	private void HealNPC(NPC npc, int amount)
	{
		npc.life = Math.Min(npc.life + amount, npc.lifeMax);
		npc.HealEffect(amount);
		npc.netUpdate = true;
	}

	private Color LiquidColor() => Color.Lerp(new Color(192, 255, 186, 180), new Color(91, 226, 190, 150) * 0.7f, Main.rand.NextFloat());

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		SpriteEffects spriteEffect = Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : 0;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffect, 0);

		return false;
	}
}
