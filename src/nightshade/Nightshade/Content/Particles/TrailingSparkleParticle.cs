using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Features;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;

namespace Nightshade.Content.Particles;

public sealed class TrailingSparkleParticle : BaseParticle
{
	public static ParticlePool<TrailingSparkleParticle> pool = new ParticlePool<TrailingSparkleParticle>(200, GetNewParticle<TrailingSparkleParticle>);

	public Vector2 Position;
	public Vector2 Velocity;
	public float Scale;
	public int TimeLeft;
	private int MaxTime;
	public Color ColorTint;

	public void Prepare(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale)
	{
		Position = position;
		Velocity = velocity;
		ColorTint = color;
		TimeLeft = 0;
		MaxTime = lifeTime;
		Scale = scale;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		if (TimeLeft < 10)
			Velocity *= 1f + TimeLeft / 50f;
		else
			Velocity *= 0.95f;

		Position += Velocity;

		float trailScale = MathF.Sqrt(Utils.GetLerpValue(MaxTime, MaxTime - 18, TimeLeft, true));
		Dust trail = Dust.NewDustPerfect(Position, DustID.AncientLight, -Velocity * 0.1f, 0, ColorTint * 0.5f, trailScale);
		trail.noGravity = true;

		Scale *= 0.999f;

		if (++TimeLeft > MaxTime)
			ShouldBeRemovedFromRenderer = true;
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		Texture2D texture = TextureAssets.Extra[98].Value;

		float scale = Scale * (1f + MathF.Sin(TimeLeft * 0.1f) * 0.1f) * MathF.Sqrt(Utils.GetLerpValue(MaxTime, MaxTime - 18, TimeLeft, true));

		Color glowColor = ColorTint * 0.9f;
		glowColor.A /= 2;

		spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), glowColor, 0f, texture.Size() / 2, scale * new Vector2(0.3f, 1f), 0, 0);
		spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), glowColor, MathHelper.PiOver2, texture.Size() / 2, scale * new Vector2(0.3f, 0.7f), 0, 0);

		spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), ColorTint, 0f, texture.Size() / 2, scale * new Vector2(0.1f, 0.7f), 0, 0);
		spritebatch.Draw(texture, Position + settings.AnchorPosition, texture.Frame(), ColorTint, MathHelper.PiOver2, texture.Size() / 2, scale * new Vector2(0.1f, 0.5f), 0, 0);
	}
}
