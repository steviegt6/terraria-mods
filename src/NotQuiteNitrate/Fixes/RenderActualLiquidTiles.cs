using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

using Tomat.TML.Mod.NotQuiteNitrate.Utilities;

namespace Tomat.TML.Mod.NotQuiteNitrate.Fixes;

/// <summary>
///     Fixes liquid rendering to render actual liquid behind tiles in corners
///     instead of drawing stupid triangles.
/// </summary>
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class RenderActualLiquidTiles : ModSystem
{
    public override void Load()
    {
        base.Load();

        // Discard drawing of partial triangles.
        On_TileDrawing.DrawPartialLiquid += (
            On_TileDrawing.orig_DrawPartialLiquid _,
            TileDrawing                           self,
            bool                                  behindBlocks,
            Tile                                  tileCache,
            ref Vector2                           position,
            ref Rectangle                         liquidSize,
            int                                   liquidType,
            ref VertexColors                      colors
        ) =>
        {
            var renderer = LiquidRenderer.Instance;
            var frameY   = renderer._animationFrame * 80;

            var drawBehindBlock = behindBlocks && !TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.TileType];

            var (x, y) = TilemapHelper.GetTilePosition(tileCache, Main.tile);
            var tileAbove = Framing.GetTileSafely(x, y - 1);

            var slope     = drawBehindBlock ? SlopeType.Solid : tileCache.Slope;
            var smthAbove = tileAbove.HasTile || tileAbove.LiquidAmount > 0;
            var slopeRect = slope switch
            {
                SlopeType.Solid          => new Rectangle(16, smthAbove ? 48 : 0, 16, 16),
                SlopeType.SlopeDownLeft  => new Rectangle(16, 48,                 16, 16),
                SlopeType.SlopeDownRight => new Rectangle(16, 48,                 16, 16),
                SlopeType.SlopeUpLeft    => new Rectangle(16, 48,                 16, 16),
                SlopeType.SlopeUpRight   => new Rectangle(16, 48,                 16, 16),
                _                        => throw new ArgumentOutOfRangeException(nameof(slope), slope, null),
            };
            {
                slopeRect.Y += frameY;
            }

            var opacity = behindBlocks ? 1f : LiquidRenderer.DEFAULT_OPACITY[tileCache.LiquidType];
            {
                colors.BottomLeftColor  *= opacity;
                colors.BottomRightColor *= opacity;
                colors.TopLeftColor     *= opacity;
                colors.TopRightColor    *= opacity;
            }

            Main.tileBatch.Draw(renderer._liquidTextures[liquidType].Value, position, slopeRect, colors, Vector2.Zero, 1f, SpriteEffects.None);
        };

        /*On_LiquidRenderer.DrawNormalLiquids += (
            _,
            self,
            _,
            drawOffset,
            waterStyle,
            globalAlpha,
            isBackgroundDraw
        ) =>
        {
            Main.tileBatch.Begin();
            {
                var drawArea = self._drawArea;

                var idx = 0;
                for (var x = drawArea.X; x < drawArea.X + drawArea.Width; x++)
                for (var y = drawArea.Y; y < drawArea.Y + drawArea.Height; y++)
                {
                    var liquid = self._drawCache[idx++];

                    if (!liquid.IsVisible)
                    {
                        //DrawPartialLiquid(isBackgroundDraw, Main.tile[x, y], new Vector2(x << 4, y << 4) + drawOffset, liquidSize, type);
                        continue;
                    }

                    var sourceRectangle = liquid.SourceRectangle;
                    if (liquid.IsSurfaceLiquid)
                    {
                        sourceRectangle.Y = 1280;
                    }
                    else
                    {
                        sourceRectangle.Y += self._animationFrame * 80;
                    }

                    var type    = (int)liquid.Type;
                    var opacity = liquid.Opacity * (isBackgroundDraw ? 1f : LiquidRenderer.DEFAULT_OPACITY[type]);
                    switch (type)
                    {
                        case LiquidID.Water:
                            type = waterStyle;

                            opacity *= globalAlpha;
                            break;

                        case LiquidID.Honey:
                            type = 11;
                            break;
                    }

                    opacity = Math.Min(1f, opacity);

                    Lighting.GetCornerColors(x, y, out var vertices);
                    {
                        vertices.BottomLeftColor  *= opacity;
                        vertices.BottomRightColor *= opacity;
                        vertices.TopLeftColor     *= opacity;
                        vertices.TopRightColor    *= opacity;
                    }

                    Main.DrawTileInWater(drawOffset, x, y);
                    Main.tileBatch.Draw(self._liquidTextures[type].Value, new Vector2(x << 4, y << 4) + drawOffset + liquid.LiquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, SpriteEffects.None);
                }
            }
            Main.tileBatch.End();
        };*/

        On_Main.DrawLiquid += (
            _,
            self,
            bg,
            waterStyle,
            alpha,
            drawSinglePassLiquids
        ) =>
        {
            if (!Lighting.NotRetro)
            {
                self.oldDrawWater(bg, waterStyle, alpha);
                return;
            }

            var stopwatch = new Stopwatch();
            {
                stopwatch.Start();
            }

            var drawOffset = (
                Main.drawToScreen
                    ? Vector2.Zero
                    : new Vector2(Main.offScreenRange, Main.offScreenRange)
            ) - Main.screenPosition;

            if (bg)
            {
                self.TilesRenderer.DrawLiquidBehindTiles(waterStyle);
            }
            else
            {
                var pos = Main.Camera.UnscaledPosition;
                var offScreen = Main.drawToScreen
                    ? Vector2.Zero
                    : new Vector2(Main.offScreenRange, Main.offScreenRange);

                self.TilesRenderer.GetScreenDrawArea(
                    pos,
                    offScreen + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition),
                    out var firstTileX,
                    out var lastTileX,
                    out var firstTileY,
                    out var lastTileY
                );
                {
                    for (var x = firstTileY; x < lastTileY + 4; x++)
                    for (var y = firstTileX - 2; y < lastTileX + 2; y++)
                    {
                        var tile = Main.tile[y, x];
                        self.TilesRenderer.DrawTile_LiquidBehindTile(solidLayer: true, inFrontOfPlayers: false, waterStyle, pos, offScreen, y, x, tile);
                    }
                }
            }

            LiquidRenderer.Instance.DrawNormalLiquids(Main.spriteBatch, drawOffset, waterStyle, alpha, bg);

            if (drawSinglePassLiquids)
            {
                LiquidRenderer.Instance.DrawShimmer(Main.spriteBatch, drawOffset, bg);
            }

            if (!bg)
            {
                TimeLogger.DrawTime(4, stopwatch.Elapsed.TotalMilliseconds);
            }

            stopwatch.Stop();
        };
    }
}