using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

public class DarkMistParticleSystem
{
	public static readonly BlendState MultiplyBlendState = new BlendState
	{
		ColorBlendFunction = BlendFunction.ReverseSubtract,
		ColorDestinationBlend = Blend.One,
		ColorSourceBlend = Blend.SourceAlpha,
		AlphaBlendFunction = BlendFunction.ReverseSubtract,
		AlphaDestinationBlend = Blend.One,
		AlphaSourceBlend = Blend.SourceAlpha
	};

	public struct MistParticle
	{
		public Vector2 Position;
		public float Progress;
		public float Scale;
		public float Rotation;
	}

	public MistParticle[] particles = new MistParticle[300];
}
