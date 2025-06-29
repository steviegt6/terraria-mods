﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Features;

using Terraria;
using Terraria.Graphics.Renderers;

namespace Nightshade.Content.Particles;

public class LiquidSplashParticle : BaseParticle
{
	public static ParticlePool<LiquidSplashParticle> pool = new ParticlePool<LiquidSplashParticle>(100, GetNewParticle<LiquidSplashParticle>);

	public Vector2 Position;
	public Vector2 Velocity;
	public float Rotation;
	public Color ColorTint;
	public float Scale;

	public int LifeTime;
	private int MaxLifeTime;
	private int Variant;

	public bool Lighted;

	public void Prepare(Vector2 position, Vector2 velocity, Color color, float scale, int lifeTime = 20)
	{
		Position = position;
		Velocity = velocity;
		ColorTint = color;
		Scale = scale;

		MaxLifeTime = lifeTime;
		Variant = Main.rand.Next(10);
		Rotation = (int)Math.Round(velocity.ToRotation() / MathHelper.PiOver2) * MathHelper.PiOver2 + MathHelper.PiOver2;
	}

	public override void FetchFromPool()
	{
		base.FetchFromPool();
		LifeTime = 0;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		Position += Velocity;
		Velocity.X *= 0.95f;
		Velocity.Y *= 0.93f;
		Velocity.Y += (float)LifeTime / MaxLifeTime;

		if (Collision.WetCollision(Position - new Vector2(6), 12, 12))
		{
			Velocity.Y *= 0.9f;
			Velocity.Y -= 0.4f;
		}

		if (Collision.SolidTiles(Position - new Vector2(6), 12, 12))
		{
			LifeTime++;
			Velocity *= 0.9f;
		}

		if (++LifeTime >= MaxLifeTime)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		var progress = (float)LifeTime / MaxLifeTime;

		var texture = Assets.Images.Particles.LiquidSplashParticle.Asset.Value;
		var frame = texture.Frame(7, 5, (int)MathF.Floor(MathF.Sin(progress * MathHelper.PiOver2 * 0.95f) * 7f), Variant % 5);
		SpriteEffects spriteEffects = 0;
		if (Variant > 1)
		{
			spriteEffects = SpriteEffects.FlipHorizontally;
		}

		var drawScale = Scale * MathF.Cbrt(progress);
		var drawColor = (Lighted ? Lighting.GetColor((int)(Position.X / 16), (int)(Position.Y / 16)).MultiplyRGBA(ColorTint) : ColorTint) * Utils.GetLerpValue(MaxLifeTime, MaxLifeTime / 2, LifeTime, true);

		spritebatch.Draw(texture, Position + settings.AnchorPosition, frame, drawColor, Rotation, frame.Size() / 2, drawScale, spriteEffects, 0);
	}
}
