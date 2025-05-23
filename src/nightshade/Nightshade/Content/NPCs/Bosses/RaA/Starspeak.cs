using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Features;

using ReLogic.Graphics;

using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Terraria.Utilities;

namespace Nightshade.Content.NPCs.Bosses.RaA;

/// <summary>
///     'Starspeak' is the name given to the abstract constellation rendering
///     obfuscating text.
/// </summary>
internal static class Starspeak
{
    private sealed class Tag : ILoadableTagHandler<Tag>
    {
        private sealed class Snippet(string text, Color color) : TextSnippet(text, color)
        {
            private const int horizontal_margin = 4;

            private static DynamicSpriteFont Font => FontAssets.MouseText.Value;

            public override bool UniqueDraw(
                bool justCheckingString,
                out Vector2 size,
                SpriteBatch spriteBatch,
                Vector2 position = new(),
                Color color = new(),
                float scale = 1f
            )
            {
                size = GetSize(Text, scale, out var margin);
                if (justCheckingString || (color.R == 0 && color.G == 0 && color.B == 0))
                {
                    return true;
                }

                DrawText(
                    spriteBatch,
                    Font,
                    Text,
                    position,
                    color,
                    scale,
                    margin,
                    Main.LocalPlayer
                );
                return true;
            }

            private static Vector2 GetSize(string text, float scale, out float margin)
            {
                // margin applied to left and right sides
                margin = scale * horizontal_margin;

                var size = Font.MeasureString(text) * scale;
                {
                    size.X += margin * 2f; // both sides
                }

                return size;
            }
        }

        public const string NAME = "starspeak";

        public string[] TagNames { get; } = [NAME];

        public TextSnippet Parse(string text, Color baseColor, string options)
        {
            return new Snippet(text, baseColor);
        }
    }

    /// <summary>
    ///     A 'starspeak' sentence, knowing its size and the normalized points
    ///     in which stars may be drawn.
    /// </summary>
    /// <param name="Size">The width and height of the sentence.</param>
    /// <param name="NormalPoints">The normalized star points.</param>
    private readonly record struct Sentence(Vector2 Size, Vector2[] NormalPoints);

    // Maps fonts to a hashmap of sentences denoted by a string hash.
    private static readonly ConditionalWeakTable<DynamicSpriteFont, Dictionary<string, Sentence>> font_sentence_cache = [];

    public static string GetBossNameTag()
    {
        return $"[{Tag.NAME}/{Mods.Nightshade.NPCs.SlimeBoss.DisplayName.GetTextValue()}]";
    }

    public static void DrawText(
        SpriteBatch sb,
        DynamicSpriteFont font,
        string text,
        Vector2 position,
        Color color,
        float scale,
        float margin,
        Player player
    )
    {
        // temporary color fix maybe
        // color.A = 255;

        var thickness = 6f;

        var sentence = GetSentence(font, text);
        var size = font.MeasureString(text) * scale;

        var drawArea = new Rectangle(
            (int)position.X,
            (int)position.Y,
            (int)(size.X + margin * 2f),
            (int)size.Y
        );

        // Draw constellation.
        {
            DrawConstellation(sb, sentence, drawArea, thickness, new Color(0, 0, 0, color.A), scale, text.GetHashCode(), bold: true);
            DrawConstellation(sb, sentence, drawArea, thickness, color, scale, text.GetHashCode());
        }

        // Draw text.
        if (PlayerKnowsStarspeak(player)) { }
    }

    private static void DrawConstellation(
        SpriteBatch sb,
        Sentence sentence,
        Rectangle drawArea,
        float thickness,
        Color color,
        float scale,
        int seed,
        bool bold = false
    )
    {
        if (bold)
        {
            thickness *= 2f;
        }

        var rand = new FastRandom(seed);
        var points = sentence.NormalPoints.Select(
            p =>
            {
                var x = drawArea.X + p.X * drawArea.Width;
                var y = drawArea.Y + p.Y * drawArea.Height;

                var reverse = rand.NextFloat() > 0.5f;
                x += MathF.Sin(Main.GlobalTimeWrappedHourly * 2f + NextFloat(rand, -MathHelper.Pi, MathHelper.Pi)) * 0.5f * (reverse ? -1f : 1f);
                y += MathF.Cos(Main.GlobalTimeWrappedHourly * 2f + NextFloat(rand, -MathHelper.Pi, MathHelper.Pi)) * 0.5f * (reverse ? -1f : 1f);

                return new Point((int)x, (int)y);
            }
        ).ToArray();

        var starCount = points.Length;
        for (var i = 0; i < starCount; i++)
        {
            var point = points[i];
            var star = new Rectangle(point.X, point.Y, (int)(thickness * scale), (int)(thickness * scale));

            // Draw star.
            sb.Draw(
                Assets.Images.UI.StarspeakStar.Asset.Value,
                star,
                null,
                color,
                0f,
                Assets.Images.UI.StarspeakStar.Asset.Size() / 2f,
                SpriteEffects.None,
                0
            );
        }

        // Draw lines.
        for (var i = 0; i < starCount - 1; i++)
        {
            var point1 = points[i];
            var point2 = points[i + 1];

            // center the points

            var x1 = point1.X;
            var y1 = point1.Y;
            var x2 = point2.X;
            var y2 = point2.Y;

            var line = new Rectangle(
                x1,
                y1,
                (int)Math.Sqrt(Math.Pow(x2 - x1 /*+ (thickness * scale / 3f)*/, 2) + Math.Pow(y2 - y1, 2)),
                (int)(thickness * scale)
            );

            if (bold)
            {
                line.Height = (int)(line.Height * 2f);
            }

            var color2 = color;
            /*if (i == 0)
            {
                color2 = Color.Red;
            }
            else if (i == starCount - 2)
            {
                color2 = Color.Blue;
            }*/
            sb.Draw(
                Assets.Images.UI.StarspeakLine.Asset.Value,
                line,
                null,
                color2,
                (float)Math.Atan2(y2 - y1, x2 - x1),
                new Vector2(0f, Assets.Images.UI.StarspeakLine.Asset.Height() / 2f),
                SpriteEffects.None,
                0
            );
        }

        return;

        static float NextFloat(FastRandom rand, float minValue, float maxValue)
        {
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }
    }

    private static Sentence GetSentence(DynamicSpriteFont font, string text)
    {
        if (font_sentence_cache.TryGetValue(font, out var sentences))
        {
            if (sentences.TryGetValue(text, out var sentence))
            {
                return sentence;
            }
        }
        else
        {
            sentences = new Dictionary<string, Sentence>();
            font_sentence_cache.Add(font, sentences);
        }

        var size = font.MeasureString(text);
        return sentences[text] = new Sentence(size, GetStarPoints(size.X, text.GetHashCode()));
    }

    private static Vector2[] GetStarPoints(float width, int seed)
    {
        // Guaranteed to produce at least 2 stars.  Points are at first
        // uniformly distributed (aside from the first and last points), before
        // being offset randomly horizontally and vertically.

        var rand = new FastRandom(seed);

        var starCount = (int)(width / 15f);
        starCount = Math.Max(starCount, 2);

        var points = new Vector2[starCount];
        var step = width / (starCount - 1);
        for (var i = 0; i < starCount; i++)
        {
            var x = step * i;
            var y = rand.NextFloat() * 0.6f + 0.05f;
            points[i] = new Vector2(x, y);
        }

        // Clamp beginning and end stars so they reach the end of the sentence.
        // points[0].X = 0f;
        // points[^1].X = width;

        // Distribute randomly horizontally.
        /*if (starCount > 2)
        {
            for (var i = 1; i < starCount - 1; i++)
            {
                points[i].X += NextFloat(rand, -step, step) / 2f;
            }
        }*/

        // Normalize horizontal values to [0, 1]
        for (var i = 0; i < starCount; i++)
        {
            points[i].X /= width;
        }

        return points;
    }

    private static bool PlayerKnowsStarspeak(Player player)
    {
        // TODO
        return true;
    }
}