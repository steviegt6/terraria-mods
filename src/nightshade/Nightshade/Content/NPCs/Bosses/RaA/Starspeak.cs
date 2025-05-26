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
                if (justCheckingString || color is { R: 0, G: 0, B: 0 })
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

    private readonly record struct Sentence(Vector2[] Points, (int from, int to)[] Connections);

    // Maps fonts to a hashmap of sentences denoted by a string hash.
    private static readonly ConditionalWeakTable<DynamicSpriteFont, Dictionary<string, Sentence>> font_sentence_cache = [];

    public static string GetBossNameTag()
    {
        return $"[{Tag.NAME}:{Mods.Nightshade.NPCs.SlimeBoss.DisplayName.GetTextValue()}]";
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
        const float thickness = 6f;

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
        var points = sentence.Points.Select(
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
        foreach (var (fromIdx, toIdx) in sentence.Connections)
        {
            var point1 = points[fromIdx];
            var point2 = points[toIdx];

            var x1 = point1.X;
            var y1 = point1.Y;
            var x2 = point2.X;
            var y2 = point2.Y;

            var line = new Rectangle(
                x1,
                y1,
                (int)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)),
                (int)(thickness * scale)
            );

            if (bold)
            {
                line.Height = (int)(line.Height * 1.75f);
            }

            sb.Draw(
                Assets.Images.UI.StarspeakLine.Asset.Value,
                line,
                null,
                color,
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
        return sentences[text] = GetStarPoints(size.X, text.GetHashCode());
    }

    private static Sentence GetStarPoints(float width, int seed)
    {
        var rand = new FastRandom(seed);

        var baseCount = Math.Max((int)(width / 15f), 2);
        var points = new List<Vector2>();
        var connections = new List<(int from, int to)>();

        var step = width / (baseCount - 1);
        var nextIndex = 0;
        int? lastIndex = null;

        for (var i = 0; i < baseCount; i++)
        {
            var x = step * i;
            var y = rand.NextFloat() * 0.6f + 0.2f;

            var canFork = i > 0 && i < baseCount - 1;
            var shouldFork = canFork && rand.NextFloat() < 0.075f;

            if (shouldFork)
            {
                var forkBaseY = y;
                var minSeparation = 0.35f; // Minimum vertical distance between fork points.
                var variance = rand.NextFloat() * 0.1f;

                var upY = Math.Clamp(forkBaseY - (minSeparation / 2f + variance), 0.05f, 0.95f);
                var downY = Math.Clamp(forkBaseY + (minSeparation / 2f + variance), 0.05f, 0.95f);

                // Randomize which one is added first to avoid same bias.
                var forkPoints = rand.NextFloat() < 0.5f
                    ? new[] { new Vector2(x, upY), new Vector2(x, downY) }
                    : new[] { new Vector2(x, downY), new Vector2(x, upY) };

                var forkA = nextIndex++;
                var forkB = nextIndex++;
                points.Add(forkPoints[0]);
                points.Add(forkPoints[1]);

                if (lastIndex is { } from)
                {
                    if (rand.NextFloat() > 0.35f)
                    {
                        connections.Add((from, forkA));
                    }

                    if (rand.NextFloat() > 0.35f)
                    {
                        connections.Add((from, forkB));
                    }
                }

                lastIndex = null;

                // Add a single star after the fork and connect both fork points to it.
                // TODO: Continue into two more stars?
                if (i + 1 < baseCount)
                {
                    var nextX = step * (i + 1);
                    var nextY = rand.NextFloat() * 0.6f + 0.2f;
                    var next = new Vector2(nextX, nextY);
                    var nextStarIndex = nextIndex++;

                    points.Add(next);

                    if (rand.NextFloat() > 0.35f)
                    {
                        connections.Add((forkA, nextStarIndex));
                    }

                    if (rand.NextFloat() > 0.35f)
                    {
                        connections.Add((forkB, nextStarIndex));
                    }

                    lastIndex = nextStarIndex;
                    i++; // Skip the already-used next base point.
                }
            }
            else
            {
                var thisIndex = nextIndex++;
                points.Add(new Vector2(x, y));

                if (lastIndex is { } from && rand.NextFloat() > 0.2f)
                {
                    connections.Add((from, thisIndex));
                }

                lastIndex = thisIndex;
            }
        }

        for (var i = 0; i < points.Count; i++)
        {
            points[i] = new Vector2(
                points[i].X / width,
                Math.Clamp(points[i].Y * 0.8f + 0.1f, 0.1f, 0.90f)
            );
        }

        return new Sentence(points.ToArray(), connections.ToArray());
    }

    private static bool PlayerKnowsStarspeak(Player player)
    {
        // TODO
        return true;
    }
}