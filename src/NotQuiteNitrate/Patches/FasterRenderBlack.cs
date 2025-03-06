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

        var screenOffset     = Main.drawToScreen ? 0f : Main.offScreenRange;
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

        var startX = Math.Max((int)((Main.screenPosition.X - screenOffset)                     / 16f - 1f) + tileOffset.X, tileOffset.X);
        var endX   = Math.Min((int)((Main.screenPosition.X + Main.screenWidth + screenOffset)  / 16f) + 2  - tileOffset.X, Main.maxTilesX - tileOffset.X);
        var startY = Math.Max((int)((Main.screenPosition.Y - screenOffset)                     / 16f - 1f) + tileOffset.Y, tileOffset.Y);
        var endY   = Math.Min((int)((Main.screenPosition.Y + Main.screenHeight + screenOffset) / 16f) + 5  - tileOffset.Y, Main.maxTilesY - tileOffset.Y);

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

        FastParallel.For(
            startY,
            endY,
            (relativeStartY, relativeEndY, _) =>
            {
                var underworldLayer = Main.UnderworldLayer;

                for (var y = relativeStartY; y <= relativeEndY; y++)
                {
                    var isUnderworld        = y >= underworldLayer;
                    var brightnessThreshold = isUnderworld ? 0.2f : minBrightness;

                    for (var x = startX; x < endX; x++)
                    {
                        var segmentStart = x;

                        while (x < endX)
                        {
                            var tile = Main.tile[x, y];

                            // var brightness = (float)Math.Floor(Lighting.Brightness(x, y) * 255f) / 255f;
                            var brightness = Lighting.Brightness(x, y);

                            var liquidAmount = tile.LiquidAmount;

                            var isDarkTile = brightness <= brightnessThreshold && (
                                (!isUnderworld && liquidAmount < 250)
                             || (liquidAmount                  >= 200 && brightness == 0f)
                             || SolidTile(tile)
                            );
                            if (!isDarkTile)
                            {
                                break;
                            }

                            var isBlockingLight = Main.tileBlockLight[tile.type] &&
                                                  tile.HasTile                   &&
                                                  (showInvisibleWalls || !tile.IsTileInvisible);

                            var hasWall = !WallID.Sets.Transparent[tile.wall] &&
                                          (showInvisibleWalls || !tile.IsWallInvisible);

                            if ((!hasWall           && !isBlockingLight)
                             || (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(x, y) && tile is { WallType: 0, IsHalfBlock: false } && y <= Main.worldSurface))
                            {
                                break;
                            }
                            x++;
                        }

                        if (x > segmentStart)
                        {
                            draw_calls.Add(
                                (
                                    new Vector2((segmentStart << 4) + screenOffset, (y << 4) + screenOffset),
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
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, position - Main.screenPosition, rectangle, Color.Black);
        }

        draw_calls.Clear();

        TimeLogger.DrawTime(5, stopwatch.Elapsed.TotalMilliseconds);

        return;

        static bool SolidTile(Tile tile)
        {
            return tile.HasTile && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && tile is { IsHalfBlock: false, Slope: SlopeType.Solid, IsActuated: false };
        }
    }
}