using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Renderers;

namespace Nightshade.Common.Features;

public abstract class BaseParticle : IPooledParticle
{
	protected static T GetNewParticle<T>() where T : BaseParticle, new() => new T();

	public bool IsRestingInPool { get; private set; }

	public bool ShouldBeRemovedFromRenderer { get; protected set; }

	public virtual void FetchFromPool()
	{
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
	}

	public virtual void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
	}

	public virtual void Update(ref ParticleRendererSettings settings)
	{
	}
}