using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Features;
using Nightshade.Content.Dusts;
using Nightshade.Content.Particles;

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
		Projectile.width = 12;
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
			Projectile.Kill();
		}

		if (Time > 10)
		{
			foreach (var player in Main.ActivePlayers)
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
			{
				Projectile.velocity.Y = 24f;
			}
		}

		Projectile.rotation += Projectile.velocity.X * 0.1f / (Time / 10f + 1f);

		if (Time % 15 == 0 || Main.rand.NextBool(8))
		{
			Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -10).RotatedBy(Projectile.rotation), ModContent.DustType<DotDropletDust>(), Main.rand.NextVector2Circular(2, 1) + Projectile.velocity * Main.rand.NextFloat(0.4f, 1f), 0, JuiceColor(), Main.rand.NextFloat(0.5f, 1f));
		}

		Time++;

		if (Projectile.velocity.X != 0)
		{
			Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
		}
	}

	public const int HEAL_AMOUNT = 30;
	public const int HEAL_RADIUS = 150;

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (BurstState == 1)
		{
			Projectile.velocity = oldVelocity;
			return true;
		}

		if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > 0)
			Projectile.velocity.X = -oldVelocity.X;
		if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > 0)
			Projectile.velocity.Y = -oldVelocity.Y;

		return true;
	}

	public override void OnKill(int timeLeft)
	{
		// TODO: Sync health gains ? dunno if it works

		var playersToHeal = new HashSet<int>();
		foreach (var player in Main.ActivePlayers)
		{
			var checkPoint = Projectile.Center + Projectile.DirectionTo(player.Center).SafeNormalize(Vector2.Zero) * Math.Min(Projectile.Distance(player.Center), HEAL_RADIUS);

			if (!player.dead && player.Hitbox.Contains(checkPoint.ToPoint()))
			{
				playersToHeal.Add(player.whoAmI);
			}
		}

		var npcsToHeal = new HashSet<int>();
		foreach (var npc in Main.ActiveNPCs)
		{
			var checkPoint = Projectile.Center + Projectile.DirectionTo(npc.Center) * Math.Min(Projectile.Distance(npc.Center), HEAL_RADIUS);
			if (npc.Hitbox.Contains(checkPoint.ToPoint()) && npc.friendly)
			{
				npcsToHeal.Add(npc.whoAmI);
			}
		}

		var divisor = (int)Math.Ceiling(MathF.Sqrt(playersToHeal.Count + npcsToHeal.Count));
		var amtToHeal = HEAL_AMOUNT / Math.Max(1, divisor);

		if (Main.netMode != NetmodeID.MultiplayerClient)
		{
			foreach (var index in npcsToHeal)
			{
				HealNPC(Main.npc[index], amtToHeal + Main.rand.Next(-8, 8));
			}
		}

		foreach (var index in playersToHeal)
		{
			Main.player[index].Heal(amtToHeal + Main.rand.Next(-8, 8));
		}

		for (var i = 0; i < Main.rand.Next(15, 25); i++)
		{
			var dropletVel = Projectile.velocity * Main.rand.NextFloat() + Main.rand.NextVector2Circular(5, 5);
			Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8, 8), ModContent.DustType<DotDropletDust>(), dropletVel, 0, JuiceColor(), Main.rand.NextFloat(0.6f, 2f));
		}

		var glowSplash = GlowSplashParticle.pool.RequestParticle();
		glowSplash.Prepare(Projectile.Center, 0, (JuiceColor() * 0.4f) with { A = 0 }, 0.5f, 10);
		glowSplash.Lighted = false;
		ParticleEngine.Particles.Add(glowSplash);

		for (var i = 0; i < Main.rand.Next(18, 25); i++)
		{
			var dropletVel = Projectile.velocity * Main.rand.NextFloat() + Main.rand.NextVector2Circular(12, 12);
			var splash = LiquidSplashParticle.pool.RequestParticle();
			splash.Prepare(Projectile.Center, dropletVel, JuiceColor() * 1.1f, Main.rand.NextFloat(1f, 2f), Main.rand.Next(15, 35));
			splash.Lighted = true;
			ParticleEngine.Particles.Add(splash);
		}

		// TODO: Sound, gore

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

	private Color JuiceColor() => Color.Lerp(Color.DeepSkyBlue * 0.8f, Color.LightSkyBlue, Main.rand.NextFloat()) with { A = 170 };

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = TextureAssets.Projectile[Type].Value;

		var spriteEffect = Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : 0;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), lightColor, Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2, Projectile.scale, spriteEffect);

		return false;
	}
}
