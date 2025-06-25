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
	public static Vector2 GasVelocity;

	[InitializedInLoad]
	private ManagedRenderTarget target;

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
		Main.spriteBatch.Begin(ss with { SortMode = SpriteSortMode.Immediate, TransformMatrix = Matrix.identity });

		var effect = Assets.Shaders.Misc.DarkeningMistShader.CreateStripShader();
		effect.Parameters.uScreenPosition = Main.screenPosition;
		effect.Parameters.uScreenSize = target.Value.Size();
		effect.Parameters.uTime = Main.GlobalTimeWrappedHourly / 3f;
		effect.Parameters.uTexture0 = Assets.Images.Extras.MistNoise.Asset.Value;
		effect.Parameters.uGasCenter = (GasCenter - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight);
		effect.Parameters.uGasVelocity = GasVelocity;
		effect.Parameters.uLoss = 0.95f;
		effect.Parameters.uPropDistance = 1f;
		effect.Apply();

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

		var pixel = Assets.Shaders.Misc.BasicPixelizationShader.CreateStripShader();
		pixel.Parameters.uSize = target.Value.Size();
		pixel.Parameters.uPixel = 2f;
		Main.spriteBatch.Begin(ss with { SortMode = SpriteSortMode.Immediate, BlendState = MultiplyBlendState, TransformMatrix = Main.Transform });
		pixel.Apply();

		Main.spriteBatch.Draw(target.Value, Vector2.Zero, Color.White);

		Main.spriteBatch.End(out ss);
		Main.spriteBatch.Begin(ss with { BlendState = BlendState.AlphaBlend });

		Draws.Clear();
	}

	public void Unload()
	{
	}
}
