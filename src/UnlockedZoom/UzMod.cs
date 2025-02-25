using System;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Light;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Tomat.Terraria.TML.UnlockedZoom;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class UzMod : Mod;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ZoomAndScreenParamOverride : ModSystem
{
    private int oldMaxScreenW;
    private int oldMaxScreenH;

    private int oldRenderTargetMaxSize;
    // private float oldMinimumZoomComparerX;
    // private float oldMinimumZoomComparerY;

    public override void Load()
    {
        base.Load();

        const int   res_i = 8192  * 2;
        const float res_f = 8192f * 2;

        // Override a lot of fields controlling screen and zoom limits.

        oldMaxScreenW = Main.maxScreenW;
        oldMaxScreenH = Main.maxScreenH;
        {
            Main.maxScreenW = res_i;
            Main.maxScreenH = res_i;
        }

        oldRenderTargetMaxSize = Main._renderTargetMaxSize;
        {
            Main._renderTargetMaxSize = res_i;
        }

        // Use tML settings.
        // oldMinimumZoomComparerX = Main.MinimumZoomComparerX;
        // oldMinimumZoomComparerY = Main.MinimumZoomComparerY;
        // {
        //     Main.MinimumZoomComparerX = res_f;
        //     Main.MinimumZoomComparerY = res_f;
        // }

        // Support display sizes based on zoom comparer instead of max screen
        // size.
        IL_Main.CacheSupportedDisplaySizes += il =>
        {
            var c = new ILCursor(il);

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdsfld<Main>("maxScreenW")))
            {
                c.EmitPop();
                c.EmitLdsfld(typeof(Main).GetField(nameof(Main.MinimumZoomComparerX), BindingFlags.Public | BindingFlags.Static)!);
            }
            c.Index = 0;

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdsfld<Main>("maxScreenH")))
            {
                c.EmitPop();
                c.EmitLdsfld(typeof(Main).GetField(nameof(Main.MinimumZoomComparerY), BindingFlags.Public | BindingFlags.Static)!);
            }
        };
    }

    public override void Unload()
    {
        base.Unload();

        Main.maxScreenW           = oldMaxScreenW;
        Main.maxScreenH           = oldMaxScreenH;
        Main._renderTargetMaxSize = oldRenderTargetMaxSize;
        // Main.MinimumZoomComparerX = oldMinimumZoomComparerX;
        // Main.MinimumZoomComparerY = oldMinimumZoomComparerY;
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class LightMapEdit : ModSystem
{
    public override void Load()
    {
        base.Load();

        IL_LightMap.ctor += il =>
        {
            var c = new ILCursor(il);

            const int width  = LightMap.DEFAULT_WIDTH;
            const int height = LightMap.DEFAULT_HEIGHT;
            const int size   = width * height;

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(width)))
            {
                c.EmitPop();
                c.EmitLdcI4(width * 2);
            }
            c.Index = 0;

            // Height is the same as width, but eh.
            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(height)))
            {
                c.EmitPop();
                c.EmitLdcI4(height * 2);
            }

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(size)))
            {
                c.EmitPop();
                c.EmitLdcI4(size);
            }
        };
    }

    public override void PostSetupContent()
    {
        base.PostSetupContent();

        Lighting.Initialize();
        Lighting.LegacyEngine._lightMap     = new LightMap();
        Lighting.NewEngine._activeLightMap  = new LightMap();
        Lighting.NewEngine._workingLightMap = new LightMap();
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ExpandClampedWorldRectangles : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_WorldUtils.ClampToWorld += (orig, rectangle) => orig(ClampRectangle(rectangle));

        return;

        static Rectangle ClampRectangle(Rectangle rect)
        {
            rect.Inflate(
                Math.Min(0, 1920 - rect.Width),
                Math.Min(0, 1080 - rect.Height)
            );
            return rect;
        }
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ExpandScreenDrawAreaBasedOnZoom : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_TileDrawing.GetScreenDrawArea += (
            On_TileDrawing.orig_GetScreenDrawArea _, // orig
            TileDrawing                           _, // self
            Vector2                               screenPosition,
            Vector2                               offSet,
            out int                               firstTileX,
            out int                               lastTileX,
            out int                               firstTileY,
            out int                               lastTileY
        ) =>
        {
            var realWidth  = (int)(Main.screenWidth  / Main.GameViewMatrix.Zoom.X);
            var realHeight = (int)(Main.screenHeight / Main.GameViewMatrix.Zoom.Y);

            firstTileX = (int)((screenPosition.X - offSet.X)              / 16f - 1f);
            lastTileX  = (int)((screenPosition.X + realWidth + offSet.X)  / 16f) + 2;
            firstTileY = (int)((screenPosition.Y - offSet.Y)              / 16f - 1f);
            lastTileY  = (int)((screenPosition.Y + realHeight + offSet.Y) / 16f) + 5;

            // Guards against drawing outside the world.
            {
                if (firstTileX < 4)
                {
                    firstTileX = 4;
                }

                if (lastTileX > Main.maxTilesX - 4)
                {
                    lastTileX = Main.maxTilesX - 4;
                }

                if (firstTileY < 4)
                {
                    firstTileY = 4;
                }

                if (lastTileY > Main.maxTilesY - 4)
                {
                    lastTileY = Main.maxTilesY - 4;
                }
            }

            if (Main.sectionManager.AnyUnfinishedSections)
            {
                TimeLogger.DetailedDrawReset();
                WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
                TimeLogger.DetailedDrawTime(5);
            }

            if (Main.sectionManager.AnyNeedRefresh)
            {
                WorldGen.RefreshSections(firstTileX, firstTileY, lastTileX, lastTileY);
            }
        };
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class RenderExtraTilesInMoreRenderTargets : ModSystem
{
    public sealed class SectionedRenderTargets : IDisposable
    {
        public int Columns { get; }

        public int Rows { get; }

        private readonly RenderTarget2D[] targets;

        public SectionedRenderTargets(int columns, int rows, int width, int height)
        {
            Columns = columns;
            Rows    = rows;

            targets = new RenderTarget2D[columns * rows];
            for (var i = 0; i < targets.Length; i++)
            {
                targets[i] = new RenderTarget2D(
                    Main.instance.GraphicsDevice,
                    width,
                    height,
                    mipMap: false,
                    Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat,
                    DepthFormat.None
                );
            }
        }

        public RenderTarget2D this[int x, int y]
        {
            get => targets[x + y * Columns];
            set => targets[x + y * Columns] = value;
        }

        public static SectionedRenderTargets Create()
        {
            var maxRtWidth  = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
            var maxRtHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;

            // These are the extremes we'll be dealing with.
            var maxZoomedWidth  = (int)(maxRtWidth  / 0.3);
            var maxZoomedHeight = (int)(maxRtHeight / 0.3);

            var cols = (int)Math.Ceiling((float)maxZoomedWidth  / maxRtWidth);
            var rows = (int)Math.Ceiling((float)maxZoomedHeight / maxRtHeight);

            return new SectionedRenderTargets(cols, rows, maxRtWidth, maxRtHeight);
        }

        public void Dispose()
        {
            foreach (var target in targets)
            {
                target.Dispose();
            }
        }
    }

    // Here's the actual interesting part of the mod.  When tiles aren't being
    // drawn directly to the screen, they're drawn to render targets.  They are
    // stored in Main::tileTarget and Main::tile2Target.  Tiles are rendered at
    // full resolution and then the RT is scaled when drawn for supporting zoom.
    // These fields are what are used to apply shaders, etc., so we can't just
    // not use them.  As such, we need to modify the way tiles are rendered to
    // draw to multiple Render Targets instead.  We cannot guarantee shader- and
    // other effect-compatibility, but we can at least maintain compatibility
    // with the main Render Target.  To do so, we'll draw the middle space with
    // the actual game Render Target and then chunk the other parts of the
    // screen to render the rest of the tiles.

    private SectionedRenderTargets? tileTargets;
    private SectionedRenderTargets? tile2Targets;

    public override void Load()
    {
        base.Load();

        On_Main.RenderTiles += (orig, self) =>
        {
            if (Main.drawToScreen || Main.GameViewMatrix.Zoom.X >= 1f || tileTargets is null)
            {
                orig(self);
                return;
            }

            self.RenderBlack();

            Main.spriteBatch.Begin();
            Main.tileBatch.Begin();

            try
            {
                for (var row = 0; row < tileTargets.Rows; row++)
                for (var col = 0; col < tileTargets.Columns; col++)
                {
                    var offsetX = col * Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
                    var offsetY = row * Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;

                    self.GraphicsDevice.SetRenderTarget(tileTargets[col, row]);
                    self.GraphicsDevice.Clear(Color.Transparent);

                    self.TilesRenderer.PreDrawTiles(solidLayer: true, forRenderTargets: false, intoRenderTargets: true);
                    self.DrawTiles(solidLayer: true, forRenderTargets: false, intoRenderTargets: true);
                }
            }
            catch (Exception e)
            {
                if (!Main.ignoreErrors)
                {
                    throw;
                }

                TimeLogger.DrawException(e);
            }

            TimeLogger.DetailedDrawReset();
            Main.tileBatch.End();
            Main.spriteBatch.End();

            for (var row = 0; row < tileTargets.Rows; row++)
            for (var col = 0; col < tileTargets.Columns; col++)
            {
                var offsetX = col * Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
                var offsetY = row * Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;

                self.GraphicsDevice.SetRenderTarget(tileTargets[col, row]);
                self.DrawTileEntities(solidLayer: true, overRenderTargets: false, intoRenderTargets: true);
            }

            TimeLogger.DetailedDrawTime(28);
            self.GraphicsDevice.SetRenderTarget(null);
        };

        On_Main.RenderTiles2 += (orig, self) =>
        {
            if (Main.drawToScreen || Main.GameViewMatrix.Zoom.X >= 1f || tile2Targets is null)
            {
                orig(self);
                return;
            }
        };

        On_Main.DoDraw_Tiles_Solid += (orig, self) =>
        {
            if (Main.drawToScreen || Main.GameViewMatrix.Zoom.X >= 1f || tileTargets is null)
            {
                orig(self);
                return;
            }

            self.TilesRenderer.PreDrawTiles(solidLayer: true, !Main.drawToScreen, intoRenderTargets: false);
            Main.tileBatch.Begin(Main.Rasterizer, Main.Transform);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            try
            {
                if (Main.drawToScreen)
                {
                    self.DrawTiles(solidLayer: true, !Main.drawToScreen, intoRenderTargets: false);
                }
                else
                {
                    // spriteBatch.Draw(tileTarget, sceneTilePos - screenPosition, Microsoft.Xna.Framework.Color.White);

                    for (var row = 0; row < tileTargets.Rows; row++)
                    for (var col = 0; col < tileTargets.Columns; col++)
                    {
                        var offsetX = col * Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
                        var offsetY = row * Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;

                        Main.spriteBatch.Draw(
                            tileTargets[col, row],
                            new Vector2(offsetX, offsetY),
                            Color.White
                        );
                    }
                    TimeLogger.DetailedDrawTime(17);
                }
            }
            catch (Exception e)
            {
                TimeLogger.DrawException(e);
            }

            Main.tileBatch.End();
            Main.spriteBatch.End();
            self.DrawTileEntities(solidLayer: true, !Main.drawToScreen, intoRenderTargets: false);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            try
            {
                Main.LocalPlayer.hitReplace.DrawFreshAnimations(Main.spriteBatch);
                Main.LocalPlayer.hitTile.DrawFreshAnimations(Main.spriteBatch);
            }
            catch (Exception e2)
            {
                TimeLogger.DrawException(e2);
            }

            Main.spriteBatch.End();
        };

        Main.OnResolutionChanged        += InitializeRenderTargetsByChangedResolution;
        Main.OnRenderTargetsInitialized += InitializeRenderTargets;
        Main.OnRenderTargetsReleased    += ReleaseRenderTargets;
    }

    public override void Unload()
    {
        base.Unload();

        Main.OnResolutionChanged        -= InitializeRenderTargetsByChangedResolution;
        Main.OnRenderTargetsInitialized -= InitializeRenderTargets;
        Main.OnRenderTargetsReleased    -= ReleaseRenderTargets;
    }

    public override void PostSetupContent()
    {
        base.PostSetupContent();

        Main.QueueMainThreadAction(
            () =>
            {
                ReleaseRenderTargets();

                tileTargets  = SectionedRenderTargets.Create();
                tile2Targets = SectionedRenderTargets.Create();
            }
        );
    }

    private void InitializeRenderTargetsByChangedResolution(Vector2 _)
    {
        ReleaseRenderTargets();

        tileTargets  = SectionedRenderTargets.Create();
        tile2Targets = SectionedRenderTargets.Create();
    }

    private void InitializeRenderTargets(int _, int __)
    {
        ReleaseRenderTargets();

        tileTargets  = SectionedRenderTargets.Create();
        tile2Targets = SectionedRenderTargets.Create();
    }

    private void ReleaseRenderTargets()
    {
        tileTargets?.Dispose();
        tile2Targets?.Dispose();
    }
}