using Daybreak.Common.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Common.Features;
using Nightshade.Common.Rendering;
using Nightshade.Core;
using Nightshade.Core.Attributes;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using static Nightshade.Common.Features.VerletIntegratedBodySystem;
using static Nightshade.Core.AssetReferences;

namespace Nightshade.Content.VisualTweaks;

[Autoload(Side = ModSide.Client)]
internal class FlailChainTweak : GlobalProjectile
{
    private static Dictionary<short, ChainInitializationParameters> flailValues = new Dictionary<short, ChainInitializationParameters>()
    {
        { ProjectileID.BallOHurt,  new ChainInitializationParameters()
        {
            PointAmount = 10,
        } },
        { ProjectileID.BlueMoon, new ChainInitializationParameters()
        {
            PointAmount = 10,
        } },
    };

    public VerletIntegratedBodyHandle? Handler { get; set; } = null;

    [InitializedInLoad]
    private static WrapperShaderData<Assets.Shaders.Misc.BasicPixelizationShader.Parameters>? pixelizationShader;
    
    //[InitializedInLoad]
    //private static WrapperShaderData<Assets.Shaders.Misc.TexturedMeshShader.Parameters>? vertexShader;

    [InitializedInLoad]
    private static ManagedRenderTarget? managedRenderTarget;

    [InitializedInLoad]
    private static VertexStrip? vertexStrip;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.aiStyle == ProjAIStyleID.Flail;
    }

    public override bool InstancePerEntity => true;

    public override void Load()
    {
        base.Load();

        managedRenderTarget = new ManagedRenderTarget((width, height) => new RenderTarget2D(Main.instance.GraphicsDevice, width / 2, height / 2), true);
        Main.QueueMainThreadAction(
            () =>
            {
                managedRenderTarget.Initialize(Main.screenWidth, Main.screenHeight);
            }
        );

        Main.QueueMainThreadAction(() =>
        {
            pixelizationShader = Assets.Shaders.Misc.BasicPixelizationShader.CreateStripShader();
            {
                pixelizationShader.Parameters.uPixel = 2f;
            }
            //vertexShader = Assets.Shaders.Misc.text
        }
        );

        vertexStrip = new VertexStrip();

        On_Main.DrawProj_FlailChains += (orig, projectile, mountedCenter) =>
        {
            if (!Handler.HasValue)
            {
                return;
            }

            Debug.Assert(pixelizationShader is not null);
            Debug.Assert(managedRenderTarget is not null);
            Debug.Assert(managedRenderTarget.Value is not null);

            SpriteBatchSnapshot snapshot = new SpriteBatchSnapshot(Main.spriteBatch);
            Main.spriteBatch.End();

            var renderTargets = Main.instance.GraphicsDevice.GetRenderTargets();
            RtContentPreserver.ApplyToBindings(renderTargets);

            Main.instance.GraphicsDevice.SetRenderTarget(managedRenderTarget.Value);
            Main.instance.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin(in snapshot);

            Vector2[] positions = new Vector2[Handler.Value.PointAmount];
            float[] rotations = new float[Handler.Value.PointAmount];
            for (int i = 0; i < Handler.Value.PointAmount; i++)
            {
                VerletIntegratedBody.Point point = Handler.Value[i];
                positions[i] = point.Position;
                rotations[i] = (point.Position - point.PreviousPosition).ToRotation();
            }
            vertexStrip.PrepareStrip(
                positions, rotations,
                (float progress) => Color.White,
                (float progress) => 1f,
                -Main.screenPosition,
                Handler.Value.PointAmount
            );
            vertexStrip.DrawTrail();

            Main.spriteBatch.End();
            Main.instance.GraphicsDevice.SetRenderTargets(renderTargets);

            Main.spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.EffectMatrix
            );
            {
                pixelizationShader.Parameters.uPixel = 2f * Main.GameViewMatrix.Zoom.X;
                pixelizationShader.Parameters.uSize = new Vector2(managedRenderTarget.Value.Width, managedRenderTarget.Value.Height);
                pixelizationShader.Apply();

                Main.spriteBatch.Draw(managedRenderTarget.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            }
            Main.spriteBatch.Restart(in snapshot);
        };
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        base.OnSpawn(projectile, source);

        Handler = VerletIntegratedBodySystem.RequestHandler();
        if (Handler.HasValue)
        {
            Player player = Main.player[projectile.owner];
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(projectile);
            playerArmPosition -= Vector2.UnitY * player.gfxOffY;

            ChainInitializationParameters parameters = flailValues[(short)projectile.type];
            parameters.StartingPosition = playerArmPosition;
            parameters.EndingPosition = projectile.Center;

            Handler.Value.InitializeAsChain(parameters);
        }
    }

    /*
    public override void PostAI(Projectile projectile)
    {
        base.PostAI(projectile);

        if (!Handler.HasValue)
        {
            Handler = VerletIntegratedBodySystem.RequestHandler();
            if (Handler.HasValue)
            {
                Player player = Main.player[projectile.owner];
                Vector2 playerArmPosition = Main.GetPlayerArmPosition(projectile);
                playerArmPosition -= Vector2.UnitY * player.gfxOffY;

                ChainInitializationParameters parameters = flailValues[(short)projectile.type];
                parameters.StartingPosition = playerArmPosition;
                parameters.EndingPosition = projectile.Center;

                Handler.Value.InitializeAsChain(parameters);
            }
        }
    }
    */

    public override void OnKill(Projectile projectile, int timeLeft)
    {
        base.OnKill(projectile, timeLeft);

        Handler?.Deinitialize();
    }
}
