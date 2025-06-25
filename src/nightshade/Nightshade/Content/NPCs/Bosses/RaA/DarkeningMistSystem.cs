using Daybreak.Common.Rendering;
using Daybreak.Core.Hooks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Rendering;
using Nightshade.Core.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

public class DarkeningMistSystem : ILoadable
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

	public class MistParticle
	{
		public Vector2 Position;
		public float Progress;
		public float Scale;
		public float Rotation;
	}

	public List<MistParticle> particles = new List<MistParticle>(200);

	public static List<Action<SpriteBatch>> Draws = new List<Action<SpriteBatch>>();
	public static Vector2 GasCenter;

	[InitializedInLoad]
	public ManagedRenderTarget target;

	[InitializedInLoad]
	private ManagedRenderTarget swapTarget;

	public void Load(Mod mod)
	{
		Main.QueueMainThreadAction(() =>
		{
			target = new ManagedRenderTarget(true);
			target.Initialize(Main.screenWidth, Main.screenHeight);
			swapTarget = new ManagedRenderTarget(true);
			swapTarget.Initialize(Main.screenWidth, Main.screenHeight);
		});

		On_Main.DrawWoF += DrawMist;
	}

	private void DrawMist(On_Main.orig_DrawWoF orig, Main self)
	{
		orig(self);

		var bindings = RtContentPreserver.GetAndPreserveMainRTs();

		Main.spriteBatch.End(out SpriteBatchSnapshot ss);
		Main.instance.GraphicsDevice.SetRenderTarget(swapTarget.Value);
		Main.instance.GraphicsDevice.Clear(Color.Transparent);
		Main.spriteBatch.Begin(ss with { SortMode = SpriteSortMode.Immediate });

		Effect effect = Assets.Shaders.Misc.DarkeningMistShader.Asset.Value;
		if (effect is not null)
		{
			effect.Parameters["uScreenPosition"]?.SetValue(Main.screenPosition);
			effect.Parameters["uScreenSize"]?.SetValue(target.Value.Size());
			effect.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly / 3f);
			effect.Parameters["uTexture0"]?.SetValue(Assets.Images.Extras.MistNoise.Asset.Value);
			effect.Parameters["uGasCenter"]?.SetValue((GasCenter - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight));
			effect.Parameters["uLoss"]?.SetValue(0.95f);
			effect.Parameters["uPropDistance"]?.SetValue(1.5f);
			effect.CurrentTechnique.Passes[0].Apply();
		}

		Main.spriteBatch.Draw(target.Value, Vector2.Zero, Color.White);

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

		for (int i = 0; i < Draws.Count; i++)
		{
			Draws[i].Invoke(Main.spriteBatch);
		}

		Main.spriteBatch.End(out ss);
		Main.instance.GraphicsDevice.SetRenderTarget(target.Value);
		Main.instance.GraphicsDevice.Clear(Color.Transparent);
		Main.spriteBatch.Begin(ss);

		Main.spriteBatch.Draw(swapTarget.Value, Vector2.Zero, Color.White);

		Main.spriteBatch.End(out ss);
		Main.instance.GraphicsDevice.SetRenderTargets(bindings);
		Main.spriteBatch.Begin(ss with { BlendState = MultiplyBlendState });

		Main.spriteBatch.Draw(target.Value, Vector2.Zero, Color.White);

		Main.spriteBatch.End(out ss);
		Main.spriteBatch.Begin(ss with { BlendState = BlendState.AlphaBlend });

		Draws.Clear();
	}

	public void Unload()
	{
	}
}
