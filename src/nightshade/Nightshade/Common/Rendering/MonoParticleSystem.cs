using Nightshade.Content.NPCs.Bosses.RaA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Renderers;

namespace Nightshade.Common.Rendering;

public abstract class MonoParticleSystem<T> where T : struct
{
	protected MonoParticleSystem(int poolSize)
	{
		Particles = new T[poolSize];
	}

	public T[] Particles;

	public virtual void Draw(ParticleRendererSettings settings)
	{

	}
}
