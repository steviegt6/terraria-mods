using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Threading;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

/// <summary>
///     Rewrites tile drawing partially to be multithreaded when drawing single
///     tiles.  Incompatible with Nitrate.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class FasterTileRendering : ModSystem
{
    private readonly record struct TbCall(
        Texture2D     Texture,
        Vector4       Destination,
        Rectangle?    SourceRectangle,
        VertexColors  Color,
        float         Rotation,
        Vector2       Origin,
        SpriteEffects Effects,
        float         Depth
    );

    private readonly record struct SbCall(
        Texture2D Texture,
        float     SourceX,
        float     SourceY,
        float     SourceW,
        float     SourceH,
        float     DestinationX,
        float     DestinationY,
        float     DestinationW,
        float     DestinationH,
        Color     Color,
        float     OriginX,
        float     OriginY,
        float     RotationSin,
        float     RotationCos,
        float     Depth,
        byte      Effects
    );

    private static bool Enabled => !ModLoader.HasMod("Nitrate");

    private static          bool                  captureDrawCalls;
    private static readonly ConcurrentBag<TbCall> tb_calls = [];
    private static readonly ConcurrentBag<SbCall> sb_calls = [];

    public override void Load()
    {
        base.Load();

        On_TileDrawing.Draw += Draw;

        On_TilePaintSystemV2.TryGetTileAndRequestIfNotReady += TryGetTileAndRequestIfNotReady;

        MonoModHooks.Add(
            typeof(TileBatch).GetMethod(nameof(TileBatch.InternalDraw), BindingFlags.NonPublic | BindingFlags.Instance)!,
            InternalDraw
        );

        MonoModHooks.Add(
            typeof(SpriteBatch).GetMethod(nameof(SpriteBatch.PushSprite), BindingFlags.NonPublic | BindingFlags.Instance)!,
            PushSprite
        );
    }

    private delegate void TbDelegate(
        TileBatch     self,
        Texture2D     texture,
        Vector4       destination,
        Rectangle?    sourceRectangle,
        VertexColors  color,
        float         rotation,
        Vector2       origin,
        SpriteEffects effects,
        float         depth
    );

    private static void InternalDraw(
        TbDelegate    orig,
        TileBatch     self,
        Texture2D     texture,
        Vector4       destination,
        Rectangle?    sourceRectangle,
        VertexColors  color,
        float         rotation,
        Vector2       origin,
        SpriteEffects effects,
        float         depth
    )
    {
        return;
        if (captureDrawCalls)
        {
            tb_calls.Add(
                new TbCall(
                    texture,
                    destination,
                    sourceRectangle,
                    color,
                    rotation,
                    origin,
                    effects,
                    depth
                )
            );
        }
        else
        {
            orig(
                self,
                texture,
                destination,
                sourceRectangle,
                color,
                rotation,
                origin,
                effects,
                depth
            );
        }
    }

    private delegate void SbDelegate(
        SpriteBatch self,
        Texture2D   texture,
        float       sourceX,
        float       sourceY,
        float       sourceW,
        float       sourceH,
        float       destinationX,
        float       destinationY,
        float       destinationW,
        float       destinationH,
        Color       color,
        float       originX,
        float       originY,
        float       rotationSin,
        float       rotationCos,
        float       depth,
        byte        effects
    );

    private static void PushSprite(
        SbDelegate  orig,
        SpriteBatch self,
        Texture2D   texture,
        float       sourceX,
        float       sourceY,
        float       sourceW,
        float       sourceH,
        float       destinationX,
        float       destinationY,
        float       destinationW,
        float       destinationH,
        Color       color,
        float       originX,
        float       originY,
        float       rotationSin,
        float       rotationCos,
        float       depth,
        byte        effects
    )
    {
        return;
        if (captureDrawCalls)
        {
            sb_calls.Add(
                new SbCall(
                    texture,
                    sourceX,
                    sourceY,
                    sourceW,
                    sourceH,
                    destinationX,
                    destinationY,
                    destinationW,
                    destinationH,
                    color,
                    originX,
                    originY,
                    rotationSin,
                    rotationCos,
                    depth,
                    effects
                )
            );
        }
        else
        {
            orig(
                self,
                texture,
                sourceX,
                sourceY,
                sourceW,
                sourceH,
                destinationX,
                destinationY,
                destinationW,
                destinationH,
                color,
                originX,
                originY,
                rotationSin,
                rotationCos,
                depth,
                effects
            );
        }
    }

    private static Texture2D TryGetTileAndRequestIfNotReady(
        On_TilePaintSystemV2.orig_TryGetTileAndRequestIfNotReady orig,
        TilePaintSystemV2                                        self,
        int                                                      tileType,
        int                                                      tileStyle,
        int                                                      paintColor
    )
    {
        if (!Enabled)
        {
            return orig(self, tileType, tileStyle, paintColor);
        }

        lock (self._tilesRenders)
        {
            return orig(self, tileType, tileStyle, paintColor);
        }
    }

    private void Draw(
        On_TileDrawing.orig_Draw orig,
        TileDrawing              self,
        bool                     solidLayer,
        bool                     forRenderTargets,
        bool                     intoRenderTargets,
        int                      waterStyleOverride
    )
    {
        if (!Enabled)
        {
            orig(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);
        }

        var stopwatch = Stopwatch.StartNew();

        self._isActiveAndNotPaused = !Main.gamePaused && Main.instance.IsActive;
        self._localPlayer          = Main.LocalPlayer;

        var unscaledPosition = Main.Camera.UnscaledPosition;
        var screenOffset     = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

        if (!solidLayer)
        {
            Main.critterCage = false;
        }

        self.EnsureWindGridSize();
        self.ClearLegacyCachedDraws();

        var invalidateCache = intoRenderTargets || Main.LightingEveryFrame;
        if (invalidateCache)
        {
            self.ClearCachedTileDraws(solidLayer);
        }

        var highQualityColor = 255f * (1f - Main.gfxQuality) + 30f * Main.gfxQuality;
        {
            self._highQualityLightingRequirement.R = (byte)highQualityColor;
            self._highQualityLightingRequirement.G = (byte)(highQualityColor * 1.1f);
            self._highQualityLightingRequirement.B = (byte)(highQualityColor * 1.2f);
        }

        var mediumQualityColor = 50f * (1f - Main.gfxQuality) + 2f * Main.gfxQuality;
        {
            self._mediumQualityLightingRequirement.R = (byte)mediumQualityColor;
            self._mediumQualityLightingRequirement.G = (byte)(mediumQualityColor * 1.1f);
            self._mediumQualityLightingRequirement.B = (byte)(mediumQualityColor * 1.2f);
        }

        self.GetScreenDrawArea(
            unscaledPosition,
            screenOffset + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition),
            out var firstTileX,
            out var lastTileX,
            out var firstTileY,
            out var lastTileY
        );

        var martianGlow = (byte)(100f + 150f * Main.martianLight);
        {
            self._martianGlow = new Color(martianGlow, martianGlow, martianGlow, 0);
        }

        var drawInfo = self._currentTileDrawInfo.Value;

        var postponedActions = new ConcurrentBag<Action>();

        captureDrawCalls = true;
        FastParallel.For(
            firstTileY,
            lastTileY,
            (relativeFirstY, relativeLastY, _) =>
            {
                for (var y = relativeFirstY; y < relativeLastY + 4; y++)
                for (var x = firstTileX - 2; x < lastTileX + 2; x++)
                {
                    var tile = Main.tile[x, y];
                    if (!tile.HasTile || self.IsTileDrawLayerSolid(tile.TileType) != solidLayer)
                    {
                        continue;
                    }

                    if (solidLayer)
                    {
                        self.DrawTile_LiquidBehindTile(
                            true,
                            false,
                            waterStyleOverride,
                            unscaledPosition,
                            screenOffset,
                            x,
                            y,
                            tile
                        );
                    }

                    var type   = tile.TileType;
                    var frameX = tile.TileFrameX;
                    var frameY = tile.TileFrameY;

                    if (!TextureAssets.Tile[type].IsLoaded)
                    {
                        Main.instance.LoadTiles(type);
                    }

                    var i = y;
                    var j = x;

                    switch (type)
                    {
                        case 373:
                        case 374:
                        case 375:
                        case 461:
                            postponedActions.Add(() => self.EmitLiquidDrops(i, j, tile, type));
                            continue;
                    }

                    if (invalidateCache)
                    {
                        switch (type)
                        {
                            case 52:
                            case 62:
                            case 115:
                            case 205:
                            case 382:
                            case 528:
                            case 636:
                            case 638:
                                postponedActions.Add(() => self.CrawlToTopOfVineAndAddSpecialPoint(i, j));
                                continue;

                            case 549:
                                postponedActions.Add(() => self.CrawlToBottomOfReverseVineAndAddSpecialPoint(i, j));
                                continue;

                            case 34:
                                if (frameX % 54 == 0 && frameY % 54 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine));
                                }
                                continue;

                            case 454:
                                if (frameX % 72 == 0 && frameY % 54 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine));
                                }
                                continue;

                            case 42:
                            case 270:
                            case 271:
                            case 572:
                            case 581:
                            case 660:
                                if (frameX % 18 == 0 && frameY % 36 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine));
                                }
                                continue;

                            case 91:
                                if (frameX % 18 == 0 && frameY % 54 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine));
                                }
                                continue;

                            case 95:
                            case 126:
                            case 444:
                                if (frameX % 36 == 0 && frameY % 36 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine));
                                }
                                continue;

                            case 465:
                            case 591:
                            case 592:
                                if (frameX % 36 == 0 && frameY % 54 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine));
                                }
                                continue;

                            case 27:
                                if (frameX % 36 == 0 && frameY == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 236:
                            case 238:
                                if (frameX % 36 == 0 && frameY == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 233:
                                if (frameY == 0 && frameX % 54 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                if (frameY == 34 && frameX % 36 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 652:
                                if (frameX % 36 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 651:
                                if (frameX % 54 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 530:
                                if (frameX < 270 && frameX % 54 == 0 && frameY == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                break;

                            case 485:
                            case 489:
                            case 490:
                                if (frameY == 0 && frameX % 36 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 521:
                            case 522:
                            case 523:
                            case 524:
                            case 525:
                            case 526:
                            case 527:
                                if (frameY == 0 && frameX % 36 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 493:
                                if (frameY == 0 && frameX % 18 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 519:
                                if (frameX / 18 <= 4)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass));
                                }
                                continue;

                            case 491:
                                if (frameX == 18 && frameY == 18)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.VoidLens));
                                }
                                break;

                            case 597:
                                if (frameX % 54 == 0 && frameY == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.TeleportationPylon));
                                }
                                break;

                            case 617:
                                if (frameX % 54 == 0 && frameY % 72 == 0)
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MasterTrophy));
                                }
                                break;

                            case 184:
                                postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.AnyDirectionalGrass));
                                continue;

                            default:
                                if (self.ShouldSwayInWind(j, i, tile))
                                {
                                    postponedActions.Add(() => self.AddSpecialPoint(j, i, TileDrawing.TileCounterType.WindyGrass));
                                }
                                break;
                        }
                    }

                    self.DrawSingleTile(drawInfo, solidLayer, waterStyleOverride, unscaledPosition, screenOffset, j, i);
                }
            }
        );

        foreach (var action in postponedActions)
        {
            action();
        }

        captureDrawCalls = false;
        {
            {
                foreach (var drawCall in tb_calls)
                {
                    Main.tileBatch.InternalDraw(
                        drawCall.Texture,
                        drawCall.Destination,
                        drawCall.SourceRectangle,
                        drawCall.Color,
                        drawCall.Rotation,
                        drawCall.Origin,
                        drawCall.Effects,
                        drawCall.Depth
                    );
                }

                tb_calls.Clear();
            }

            {
                foreach (var drawCall in sb_calls)
                {
                    Main.spriteBatch.PushSprite(
                        drawCall.Texture,
                        drawCall.SourceX,
                        drawCall.SourceY,
                        drawCall.SourceW,
                        drawCall.SourceH,
                        drawCall.DestinationX,
                        drawCall.DestinationY,
                        drawCall.DestinationW,
                        drawCall.DestinationH,
                        drawCall.Color,
                        drawCall.OriginX,
                        drawCall.OriginY,
                        drawCall.RotationSin,
                        drawCall.RotationCos,
                        drawCall.Depth,
                        drawCall.Effects
                    );
                }

                sb_calls.Clear();
            }
        }

        if (solidLayer)
        {
            Main.instance.DrawTileCracks(1, Main.LocalPlayer.hitReplace);
            Main.instance.DrawTileCracks(1, Main.LocalPlayer.hitTile);
        }

        self.DrawSpecialTilesLegacy(unscaledPosition, screenOffset);

        if (TileObject.objectPreview.Active && self._localPlayer.cursorItemIconEnabled && Main.placementPreview && !CaptureManager.Instance.Active)
        {
            Main.instance.LoadTiles(TileObject.objectPreview.Type);
            TileObject.DrawPreview(Main.spriteBatch, TileObject.objectPreview, unscaledPosition - screenOffset);
        }

        TimeLogger.DrawTime(solidLayer ? 0 : 1, stopwatch.Elapsed.TotalMilliseconds);
    }
}