using System;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

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
            TileDrawing                           _,
            bool                                  _,
            Tile                                  _,
            ref Vector2                           _,
            ref Rectangle                         _,
            int                                   _,
            ref VertexColors                      _
        ) => { };

        On_LiquidRenderer.DrawNormalLiquids += (
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
        };
    }
}