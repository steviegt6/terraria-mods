using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using ReLogic.Threading;

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

/// <summary>
///     Parallelizes RenderBlack.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class FasterRenderBlack : ModSystem
{
    private static readonly ConcurrentBag<(Vector2 position, Rectangle rectangle)> draw_calls = [];

    public override void Load()
    {
        base.Load();

        On_Main.DrawBlack += DrawBlack;
    }

    private static void DrawBlack(On_Main.orig_DrawBlack orig, Main self, bool force)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (Main.shimmerAlpha == 1f)
        {
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        var screenOffset     = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
        var averageTileColor = (Main.tileColor.R + Main.tileColor.G + Main.tileColor.B) / 3;

        var minBrightness = Lighting.Mode switch
        {
            LightMode.Retro  => Math.Max((Main.tileColor.R - 55) / 255f, 0f),
            LightMode.Trippy => Math.Max((averageTileColor - 55) / 255f, 0f),
            _                => (float)(averageTileColor         * 0.4) / 255f,
        };

        var screenOverdrawOffset = Main.GetScreenOverdrawOffset();
        var tileOffset = new Point(
            -Main.offScreenRange / 16 + screenOverdrawOffset.X,
            -Main.offScreenRange / 16 + screenOverdrawOffset.Y
        );

        var startX = Math.Max((int)((Main.screenPosition.X - screenOffset.X)                     / 16f - 1f) + tileOffset.X, tileOffset.X);
        var endX   = Math.Min((int)((Main.screenPosition.X + Main.screenWidth + screenOffset.X)  / 16f) + 2  - tileOffset.X, Main.maxTilesX - tileOffset.X);
        var startY = Math.Max((int)((Main.screenPosition.Y - screenOffset.Y)                     / 16f - 1f) + tileOffset.Y, tileOffset.Y);
        var endY   = Math.Min((int)((Main.screenPosition.Y + Main.screenHeight + screenOffset.Y) / 16f) + 5  - tileOffset.Y, Main.maxTilesY - tileOffset.Y);

        if (!force)
        {
            if (startY < Main.maxTilesY / 2)
            {
                endY   = Math.Min(endY,   (int)Main.worldSurface + 1);
                startY = Math.Min(startY, (int)Main.worldSurface + 1);
            }
            else
            {
                endY   = Math.Max(endY,   Main.UnderworldLayer);
                startY = Math.Max(startY, Main.UnderworldLayer);
            }
        }

        // TODO: Is this case possible?
        if (startX < 0 || startX > Main.maxTilesX || startY < 0 || startY > Main.maxTilesY)
        {
            return;
        }

        var showInvisibleWalls = Main.ShouldShowInvisibleWalls();

        /*if (startY >= Main.UnderworldLayer)
        {
            // Always underworld
            FastParallel.For(
                startY,
                endY,
                (relativeStartY, relativeEndY, _) =>
                {
                    for (var y = relativeStartY; y <= relativeEndY; y++)
                    {
                        for (var x = startX; x < endX; x++)
                        {
                            var segmentStart = x;

                            while (x < endX)
                            {
                                var tile         = Main.tile[x, y];
                                var brightness   = (float)Math.Floor(Lighting.Brightness(x, y) * 255f) / 255f;
                                var liquidAmount = tile.liquid;

                                var isDarkTile = brightness <= 0.2f &&
                                                 (WorldGen.SolidTile(tile) || (liquidAmount >= 200 && brightness == 0f));

                                var isBlockingLight = tile.active() && Main.tileBlockLight[tile.type] &&
                                                      (!tile.invisibleBlock() || showInvisibleWalls);

                                var hasWall = !WallID.Sets.Transparent[tile.wall] &&
                                              (!tile.invisibleWall() || showInvisibleWalls);

                                if (!isDarkTile || (!hasWall && !isBlockingLight) ||
                                    (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(x, y) && tile.wall == 0 &&
                                     !tile.halfBrick()  && y                                                       <= Main.worldSurface))
                                {
                                    break;
                                }
                                x++;
                            }

                            if (x > segmentStart)
                            {
                                draw_calls.Add(
                                    (
                                        new Vector2(segmentStart << 4, y << 4) - Main.screenPosition + screenOffset,
                                        new Rectangle(0, 0, (x - segmentStart) << 4, 16)
                                    )
                                );
                            }
                        }
                    }
                }
            );
        }
        else if (endY < Main.UnderworldLayer)
        {
            // Never underworld
            FastParallel.For(
                startY,
                endY,
                (relativeStartY, relativeEndY, _) =>
                {
                    for (var y = relativeStartY; y <= relativeEndY; y++)
                    {
                        for (var x = startX; x < endX; x++)
                        {
                            var segmentStart = x;

                            while (x < endX)
                            {
                                var tile         = Main.tile[x, y];
                                var brightness   = (float)Math.Floor(Lighting.Brightness(x, y) * 255f) / 255f;
                                var liquidAmount = tile.liquid;

                                var isDarkTile = brightness <= minBrightness &&
                                                 ((liquidAmount < 250) || WorldGen.SolidTile(tile) || (brightness == 0f));

                                var isBlockingLight = tile.active() && Main.tileBlockLight[tile.type] &&
                                                      (!tile.invisibleBlock() || showInvisibleWalls);

                                var hasWall = !WallID.Sets.Transparent[tile.wall] &&
                                              (!tile.invisibleWall() || showInvisibleWalls);

                                if (!isDarkTile || (!hasWall && !isBlockingLight) ||
                                    (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(x, y) && tile.wall == 0 &&
                                     !tile.halfBrick()  && y                                                       <= Main.worldSurface))
                                {
                                    break;
                                }
                                x++;
                            }

                            if (x > segmentStart)
                            {
                                draw_calls.Add(
                                    (
                                        new Vector2(segmentStart << 4, y << 4) - Main.screenPosition + screenOffset,
                                        new Rectangle(0, 0, (x - segmentStart) << 4, 16)
                                    )
                                );
                            }
                        }
                    }
                }
            );
        }
        else
        {
            // Possibly underworld
            FastParallel.For(
                startY,
                endY,
                (relativeStartY, relativeEndY, _) =>
                {
                    for (var y = relativeStartY; y <= relativeEndY; y++)
                    {
                        var isUnderworld        = y >= Main.UnderworldLayer;
                        var brightnessThreshold = isUnderworld ? 0.2f : minBrightness;

                        for (var x = startX; x < endX; x++)
                        {
                            var segmentStart = x;

                            while (x < endX)
                            {
                                var tile         = Main.tile[x, y];
                                var brightness   = (float)Math.Floor(Lighting.Brightness(x, y) * 255f) / 255f;
                                var liquidAmount = tile.liquid;

                                var isDarkTile = brightness <= brightnessThreshold &&
                                                 ((!isUnderworld && liquidAmount < 250) || WorldGen.SolidTile(tile) || (liquidAmount >= 200 && brightness == 0f));

                                var isBlockingLight = tile.active() && Main.tileBlockLight[tile.type] &&
                                                      (!tile.invisibleBlock() || showInvisibleWalls);

                                var hasWall = !WallID.Sets.Transparent[tile.wall] &&
                                              (!tile.invisibleWall() || showInvisibleWalls);

                                if (!isDarkTile || (!hasWall && !isBlockingLight) ||
                                    (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(x, y) && tile.wall == 0 &&
                                     !tile.halfBrick()  && y                                                       <= Main.worldSurface))
                                {
                                    break;
                                }
                                x++;
                            }

                            if (x > segmentStart)
                            {
                                draw_calls.Add(
                                    (
                                        new Vector2(segmentStart << 4, y << 4) - Main.screenPosition + screenOffset,
                                        new Rectangle(0, 0, (x - segmentStart) << 4, 16)
                                    )
                                );
                            }
                        }
                    }
                }
            );
        }*/

        FastParallel.For(
            startY,
            endY,
            (relativeStartY, relativeEndY, _) =>
            {
                for (var y = relativeStartY; y <= relativeEndY; y++)
                {
                    var isUnderworld        = y >= Main.UnderworldLayer;
                    var brightnessThreshold = isUnderworld ? 0.2f : minBrightness;

                    for (var x = startX; x < endX; x++)
                    {
                        var segmentStart = x;

                        while (x < endX)
                        {
                            var tile         = Main.tile[x, y];
                            var brightness   = (float)Math.Floor(Lighting.Brightness(x, y) * 255f) / 255f;
                            var liquidAmount = tile.liquid;

                            var isDarkTile = brightness <= brightnessThreshold &&
                                             ((!isUnderworld && liquidAmount < 250) || WorldGen.SolidTile(tile) || (liquidAmount >= 200 && brightness == 0f));

                            var isBlockingLight = tile.active() && Main.tileBlockLight[tile.type] &&
                                                  (!tile.invisibleBlock() || showInvisibleWalls);

                            var hasWall = !WallID.Sets.Transparent[tile.wall] &&
                                          (!tile.invisibleWall() || showInvisibleWalls);

                            if (!isDarkTile || (!hasWall && !isBlockingLight) ||
                                (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(x, y) && tile.wall == 0 &&
                                 !tile.halfBrick()  && y                                                       <= Main.worldSurface))
                            {
                                break;
                            }
                            x++;
                        }

                        if (x > segmentStart)
                        {
                            draw_calls.Add(
                                (
                                    new Vector2(segmentStart << 4, y << 4) - Main.screenPosition + screenOffset,
                                    new Rectangle(0, 0, (x - segmentStart) << 4, 16)
                                )
                            );
                        }
                    }
                }
            }
        );

        foreach (var (position, rectangle) in draw_calls)
        {
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, position, rectangle, Color.Black);
        }

        draw_calls.Clear();

        TimeLogger.DrawTime(5, stopwatch.Elapsed.TotalMilliseconds);
    }
}