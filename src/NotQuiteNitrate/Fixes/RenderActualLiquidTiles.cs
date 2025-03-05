using System;
using System.Collections.Generic;
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
    private sealed class LiquidSlopeGenerator : ILoadable
    {
        // private readonly Dictionary<>

        void ILoadable.Load(global::Terraria.ModLoader.Mod mod) { }

        void ILoadable.Unload() { }
    }

    /*private readonly record struct LiquidSlopeKey(
    )*/

    public override void Load()
    {
        base.Load();

        On_TileDrawing.DrawPartialLiquid += DrawPartialLiquid;

        On_Main.DrawLiquid += DrawLiquid;
    }

    // Rewrite "partial liquid rendering" to use our generated liquid slope
    // textures derived from the actual liquid textures instead of separate,
    // ugly frames.
    private static void DrawPartialLiquid(
        On_TileDrawing.orig_DrawPartialLiquid orig,
        TileDrawing                           self,
        bool                                  behindBlocks,
        Tile                                  tile,
        ref Vector2                           position,
        ref Rectangle                         liquidSize,
        int                                   liquidType,
        ref VertexColors                      colors
    )
    {
        var renderer = LiquidRenderer.Instance;
        var frameY   = renderer._animationFrame * 80;

        var drawBehindBlock = behindBlocks && !TileID.Sets.BlocksWaterDrawingBehindSelf[tile.TileType];

        var (x, y) = TilemapHelper.GetTilePosition(tile, Main.tile);
        var tileAbove = Framing.GetTileSafely(x, y - 1);

        var slope     = drawBehindBlock ? SlopeType.Solid : tile.Slope;
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

        var opacity = behindBlocks ? 1f : LiquidRenderer.DEFAULT_OPACITY[tile.LiquidType];
        {
            colors.BottomLeftColor  *= opacity;
            colors.BottomRightColor *= opacity;
            colors.TopLeftColor     *= opacity;
            colors.TopRightColor    *= opacity;
        }

        Main.tileBatch.Draw(renderer._liquidTextures[liquidType].Value, position, slopeRect, colors, Vector2.Zero, 1f, SpriteEffects.None);

        if (!behindBlocks)
        {
            Main.instance.TilesRenderer.DrawSingleTile(Main.instance.TilesRenderer._currentTileDrawInfo.Value, true, liquidType, Main.Camera.UnscaledPosition, Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange), x, y);
        }
    }

    // Modify liquid rendering to add an extra case for rendering liquids on top
    // of tiles on the edges.
    private static void DrawLiquid(
        On_Main.orig_DrawLiquid orig,
        Main                    self,
        bool                    bg,
        int                     waterStyle,
        float                   alpha,
        bool                    drawSinglePassLiquids
    )
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
            // Our extra rendering part, derived from DrawLiquidBehindTiles.
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
                    {
                        // Specifically set solidLayer to true (only change).
                        self.TilesRenderer.DrawTile_LiquidBehindTile(solidLayer: true, inFrontOfPlayers: false, waterStyle, pos, offScreen, y, x, tile);
                    }
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
    }
}