using Microsoft.Xna.Framework.Graphics;

using Daybreak.Common.Features.Hooks;

using Terraria;
using Terraria.Graphics.Renderers;

namespace Nightshade.Common.Features;

/// <summary>
/// Contains particle systems.
/// </summary>
public static class ParticleEngine
{
	[OnLoad]
	public static void Load()
	{
		On_Main.UpdateParticleSystems += UpdateParticles;
		On_Main.DrawDust += DrawParticlesPreDust;
	}

	/// <summary>
	/// Renders behind dust.
	/// </summary>
	public static ParticleRenderer Particles = new ParticleRenderer();

	private static void UpdateParticles(On_Main.orig_UpdateParticleSystems orig, Main self)
	{
		orig(self);
		Particles.Update();
	}

	private static void DrawParticlesPreDust(On_Main.orig_DrawDust orig, Main self)
	{
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
		Particles.Settings.AnchorPosition = -Main.screenPosition;
		Particles.Draw(Main.spriteBatch);
		Main.spriteBatch.End();

		orig(self);
	}
}
