using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Features;

using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;

namespace Nightshade.Content.Particles;

public class GlowSplashParticle : BaseParticle
{
	public static ParticlePool<GlowSplashParticle> pool = new ParticlePool<GlowSplashParticle>(100, GetNewParticle<GlowSplashParticle>);

	public Vector2 Position;
	public float Rotation;
	public Color ColorTint;
	public float Scale;

	public int LifeTime;
	private int MaxLifeTime;

	public bool Lighted;

	public void Prepare(Vector2 position, float rotation, Color color, float scale, int lifeTime = 20)
	{
		Position = position;
		Rotation = rotation;
		ColorTint = color;
		Scale = scale;

		MaxLifeTime = lifeTime;
	}

	public override void FetchFromPool()
	{
		base.FetchFromPool();
		LifeTime = 0;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		if (++LifeTime >= MaxLifeTime)
		{
			ShouldBeRemovedFromRenderer = true;
		}

		if (!Lighted)
		{
			Lighting.AddLight(Position, ColorTint.ToVector3() * (1f - (float)LifeTime / MaxLifeTime));
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		var texture = Assets.Images.Particles.RingGlow.Asset.Value;
		var flareTexture = TextureAssets.Extra[98].Value;
		var progress = (float)LifeTime / MaxLifeTime;
		var curve = MathF.Sqrt(MathF.Sin(progress * MathHelper.PiOver2));
		var drawColor = Lighted ? Lighting.GetColor((int)(Position.X / 16), (int)(Position.Y / 16)).MultiplyRGBA(ColorTint) : ColorTint;
		spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), drawColor * (1f - progress) * 0.33f, Rotation, texture.Size() / 2, Scale * curve, 0, 0);

		var flareScale = new Vector2(0.4f, 14f * curve) * Scale;
		spritebatch.Draw(flareTexture, Position + settings.AnchorPosition, flareTexture.Frame(), drawColor * (1f - progress), Rotation - MathHelper.PiOver2, flareTexture.Size() / 2, flareScale, 0, 0);
	}
}
