using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		if (Collision.SolidTiles(Position - new Vector2(4), 8, 8))
			LifeTime += 2;

		if (++LifeTime >= MaxLifeTime)
			ShouldBeRemovedFromRenderer = true;
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		float progress = (float)LifeTime / MaxLifeTime;

		Texture2D texture = Assets.Images.Particles.LiquidSplashParticle.Asset.Value;
		Rectangle frame = texture.Frame(7, 5, (int)MathF.Floor(MathF.Sin(progress * MathHelper.PiOver2 * 0.95f) * 7f), Variant % 5);
		SpriteEffects spriteEffects = 0;
		if (Variant > 1)
			spriteEffects = SpriteEffects.FlipHorizontally;

		float drawScale = Scale * MathF.Cbrt(progress);
		Color drawColor = (Lighted ? Lighting.GetColor((int)(Position.X / 16), (int)(Position.Y / 16)).MultiplyRGBA(ColorTint) : ColorTint) * Utils.GetLerpValue(MaxLifeTime, MaxLifeTime / 2, LifeTime, true);

		spritebatch.Draw(texture, Position + settings.AnchorPosition, frame, drawColor, Rotation, frame.Size() / 2, drawScale, spriteEffects, 0);
	}
}
