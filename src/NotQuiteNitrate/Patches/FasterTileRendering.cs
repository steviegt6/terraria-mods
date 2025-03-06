using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Threading;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
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
    private static bool Enabled => !ModLoader.HasMod("Nitrate");

    public override void Load()
    {
        base.Load();

        On_TileDrawing.Draw += Draw;

        On_TilePaintSystemV2.TryGetTileAndRequestIfNotReady += TryGetTileAndRequestIfNotReady;
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
                                CrawlToTopOfVineAndAddSpecialPoint(i, j, self);
                                continue;

                            case 549:
                                CrawlToBottomOfReverseVineAndAddSpecialPoint(i, j, self);
                                continue;

                            case 34:
                                if (frameX % 54 == 0 && frameY % 54 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine, self);
                                }
                                continue;

                            case 454:
                                if (frameX % 72 == 0 && frameY % 54 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine, self);
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
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine, self);
                                }
                                continue;

                            case 91:
                                if (frameX % 18 == 0 && frameY % 54 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine, self);
                                }
                                continue;

                            case 95:
                            case 126:
                            case 444:
                                if (frameX % 36 == 0 && frameY % 36 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine, self);
                                }
                                continue;

                            case 465:
                            case 591:
                            case 592:
                                if (frameX % 36 == 0 && frameY % 54 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine, self);
                                }
                                continue;

                            case 27:
                                if (frameX % 36 == 0 && frameY == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 236:
                            case 238:
                                if (frameX % 36 == 0 && frameY == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 233:
                                if (frameY == 0 && frameX % 54 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                if (frameY == 34 && frameX % 36 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 652:
                                if (frameX % 36 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 651:
                                if (frameX % 54 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 530:
                                if (frameX < 270 && frameX % 54 == 0 && frameY == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                break;

                            case 485:
                            case 489:
                            case 490:
                                if (frameY == 0 && frameX % 36 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
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
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 493:
                                if (frameY == 0 && frameX % 18 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 519:
                                if (frameX / 18 <= 4)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass, self);
                                }
                                continue;

                            case 491:
                                if (frameX == 18 && frameY == 18)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.VoidLens, self);
                                }
                                break;

                            case 597:
                                if (frameX % 54 == 0 && frameY == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.TeleportationPylon, self);
                                }
                                break;

                            case 617:
                                if (frameX % 54 == 0 && frameY % 72 == 0)
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.MasterTrophy, self);
                                }
                                break;

                            case 184:
                                AddSpecialPoint(j, i, TileDrawing.TileCounterType.AnyDirectionalGrass, self);
                                continue;

                            default:
                                if (self.ShouldSwayInWind(j, i, tile))
                                {
                                    AddSpecialPoint(j, i, TileDrawing.TileCounterType.WindyGrass, self);
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

        return;

        static void AddSpecialPoint(int x, int y, TileDrawing.TileCounterType type, TileDrawing instance)
        {
            instance._specialPositions[(int)type][Interlocked.Increment(ref instance._specialsCount[(int)type])] = new Point(x, y);
        }

        static void CrawlToTopOfVineAndAddSpecialPoint(int j, int i, TileDrawing instance)
        {
            var y = j;
            for (var num = j - 1; num > 0; num--)
            {
                var tile = Main.tile[i, num];
                if (!WorldGen.SolidTile(i, num) && tile.active())
                {
                    continue;
                }

                y = num + 1;
                break;
            }

            var item = new Point(i, y);
            if (instance._vineRootsPositions.Contains(item))
            {
                return;
            }

            instance._vineRootsPositions.Add(item);
            AddSpecialPoint(i, y, TileDrawing.TileCounterType.Vine, instance);
        }

        static void CrawlToBottomOfReverseVineAndAddSpecialPoint(int j, int i, TileDrawing instance)
        {
            var y = j;
            for (var k = j; k < Main.maxTilesY; k++)
            {
                var tile = Main.tile[i, k];
                if (!WorldGen.SolidTile(i, k) && tile.active())
                {
                    continue;
                }

                y = k - 1;
                break;
            }

            var item = new Point(i, y);
            if (instance._reverseVineRootsPositions.Contains(item))
            {
                return;
            }

            instance._reverseVineRootsPositions.Add(item);
            AddSpecialPoint(i, y, TileDrawing.TileCounterType.ReverseVine, instance);
        }
    }
}