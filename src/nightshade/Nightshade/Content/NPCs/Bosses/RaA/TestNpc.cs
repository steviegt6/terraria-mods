using System;

using CalamityFables;
using CalamityFables.Core;

using Daybreak.Common.Features.Hooks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Compat;

using ReLogic.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

[JITWhenModsEnabled("CalamityFables")]
internal static class FablesCompatTest
{
    public sealed class TestCard() : BossIntroCard(
        "Reductio ad Astra",
        "Sleeping Star from the Darkest Expanse", // "Dormant Star across the Darkest Expanse",
        60 * 3,
        false,
        Color.Red,
        Color.Blue,
        Color.Green,
        Color.White
    )
    {
        public override void DrawCard()
        {
            var bottomLeftCorner = new Vector2(0f, Main.screenHeight * slant);
            var bottomRightCorner = new Vector2(Main.screenWidth, Main.screenHeight * shiftDown);
            var tangent = bottomLeftCorner.DirectionTo(bottomRightCorner);
            var perpendicular = tangent.RotatedBy(-1.5707963705062866);
            bottomRightCorner += tangent * Main.screenHeight * shiftDown;
            var topLeftCorner = bottomLeftCorner + perpendicular * Main.screenHeight * 0.5f;
            var topRightCorner = bottomRightCorner + perpendicular * Main.screenHeight * 0.5f;
            var corners = new[] { topLeftCorner, topRightCorner, bottomLeftCorner, bottomRightCorner };
            if (flipped)
            {
                FlipVerticesHorizontally(ref corners);
                tangent.Y *= -1f;
                perpendicular.X *= -1f;
            }
            kinoBars.Vertices = corners;
            var fadeInPercent = MathF.Pow(Utils.GetLerpValue(25f, 0f, time, clamped: true), 3f);
            var fadeOutPercent = MathF.Pow(Utils.GetLerpValue(maxTime - 35, maxTime, time, clamped: true), 4f);
            var totalFade = 1f - fadeInPercent - fadeOutPercent;
            var defaultTextScrollPercent = 0.3f + Utils.GetLerpValue(1900f, 1000f, Main.screenWidth, clamped: true) * 0.05f;
            var textScrollPercent = defaultTextScrollPercent;
            textScrollPercent -= 0.5f * MathF.Pow(Utils.GetLerpValue(10f, 0f, time, clamped: true), 3f);
            textScrollPercent += 2.5f * MathF.Pow(Utils.GetLerpValue(maxTime - 15, maxTime, time, clamped: true), 3f);
            var effect = InitializeEffect(fadeInPercent, fadeOutPercent);
            DrawKinoBarWithEdge(effect, 0.1f);
            if (music.HasValue)
            {
                var scrollOffset = (flipped ? 0.035f : 0.055f);
                var musicTextCenter = GetTextCenter(corners, textScrollPercent - scrollOffset, musicText: true);
                DrawOSTDetails(musicTextCenter, tangent, perpendicular, totalFade, (textScrollPercent == defaultTextScrollPercent) ? musicTextCenter : GetTextCenter(corners, defaultTextScrollPercent - scrollOffset, musicText: true));
            }
            RotateVertices180(ref corners);
            DrawKinoBarWithEdge(effect, -0.1f);
            var textCenter = GetTextCenter(corners, textScrollPercent, musicText: false);
            DrawTitleAndName_WithSubtitle(textCenter, tangent, perpendicular, totalFade, (textScrollPercent == defaultTextScrollPercent) ? textCenter : GetTextCenter(corners, defaultTextScrollPercent, musicText: false));
        }

        public void DrawTitleAndName_WithSubtitle(Vector2 textCenter, Vector2 tangent, Vector2 perpendicular, float fade, Vector2 defaultTextOrigin)
        {
            var nameScaleMult = 6f * fontScaling;
            var titleScaleMult = 2f * fontScaling;
            if (Main.screenHeight > 1080)
            {
                nameScaleMult *= Main.screenHeight / 1080f;
                titleScaleMult *= Main.screenHeight / 1080f;
            }
            var padding = 25f - Utils.GetLerpValue(1900f, 1000f, Main.screenWidth, clamped: true) * 15f;
            textCenter.Y -= Utils.GetLerpValue(1900f, 1000f, Main.screenWidth, clamped: true) * 15f;
            var rotation = tangent.ToRotation();
            var namePosition = textCenter;
            var titlePosition = textCenter + tangent * 23f;
            var rawTitleSize = font.MeasureString(bossTitle);
            var titleSize = rawTitleSize * titleScaleMult;
            var rawNameSize = font.MeasureString(bossName);
            var nameSize = rawNameSize * nameScaleMult;
            var titleOrigin = new Vector2(0f, rawTitleSize.Y * 0.85f);
            var nameOrigin = new Vector2(0f, 0f);
            if (flipped)
            {
                nameOrigin.X += rawNameSize.X;
            }
            var nameBottomLeftCorner = defaultTextOrigin - perpendicular * nameSize.Y;
            var titleBottomLeftCorner = defaultTextOrigin - perpendicular * titleSize * 0.15f + tangent * 23f;
            var tangentSlope = tangent.Y / tangent.X;
            if (!flipped)
            {
                var tangentNameYintercept = nameBottomLeftCorner.Y - tangentSlope * nameBottomLeftCorner.X;
                var screenRightNameIntersectionY = tangentSlope * Main.screenWidth + tangentNameYintercept;
                var tangentTitleYintercept = titleBottomLeftCorner.Y - tangentSlope * nameBottomLeftCorner.X;
                var screenRightTitleIntersectionY = tangentSlope * Main.screenWidth + tangentTitleYintercept;
                var maxNameWidth = nameBottomLeftCorner.Distance(new Vector2(Main.screenWidth, screenRightNameIntersectionY)) - padding;
                if (nameSize.X > maxNameWidth)
                {
                    nameScaleMult *= maxNameWidth / nameSize.X;
                }
                var maxTitleWidth = titleBottomLeftCorner.Distance(new Vector2(Main.screenWidth, screenRightTitleIntersectionY)) - padding;
                if (titleSize.X > maxTitleWidth)
                {
                    titleScaleMult *= maxTitleWidth / titleSize.X;
                }
            }
            else
            {
                var screennLeftNameIntersectionY = nameBottomLeftCorner.Y - tangentSlope * nameBottomLeftCorner.X;
                var maxNameWidth2 = nameBottomLeftCorner.Distance(new Vector2(0f, screennLeftNameIntersectionY)) - padding;
                if (nameSize.X > maxNameWidth2)
                {
                    nameScaleMult *= maxNameWidth2 / nameSize.X;
                }
                titlePosition = textCenter - tangent * (rawNameSize.X * nameScaleMult - 23f);
                titleBottomLeftCorner = defaultTextOrigin - perpendicular * titleSize * 0.15f + tangent * (rawNameSize.X * nameScaleMult - 23f);
                var screenLeftTitleIntersectionY = titleBottomLeftCorner.Y - tangentSlope * titleBottomLeftCorner.X;
                var maxTitleWidth2 = nameBottomLeftCorner.Distance(new Vector2(0f, screenLeftTitleIntersectionY)) - padding;
                if (titleSize.X > maxTitleWidth2)
                {
                    titleScaleMult *= maxTitleWidth2 / titleSize.X;
                }
            }
            
            // Title
            {
                for (var i = 0; i < 8; i++)
                {
                    Main.spriteBatch.DrawString(font, bossTitle, titlePosition + Vector2.UnitY.RotatedBy(rotation + i / 8f * ((float)Math.PI * 2f)) * 4f, Color.Lerp(titleColor, Color.Black, 0.95f) * 0.3f * fade, rotation, titleOrigin, titleScaleMult, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.DrawString(font, bossTitle, titlePosition, titleColor * fade, rotation, titleOrigin, titleScaleMult, SpriteEffects.None, 0f);
            }
            
            // Subtitle
            /*{
                var subtitlePosition = titlePosition;
                subtitlePosition.Y += rawNameSize.Y * 2f;
                
                for (var i = 0; i < 8; i++)
                {
                    Main.spriteBatch.DrawString(font, bossTitle, subtitlePosition + Vector2.UnitY.RotatedBy(rotation + i / 8f * ((float)Math.PI * 2f)) * 4f, Color.Lerp(titleColor, Color.Black, 0.95f) * 0.3f * fade, rotation, titleOrigin, titleScaleMult, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.DrawString(font, bossTitle, subtitlePosition, titleColor * fade, rotation, titleOrigin, titleScaleMult, SpriteEffects.None, 0f);
            }*/

            DrawTextWithBlur(bossName, namePosition, Color.White * fade, nameColorChroma1 * fade, nameColorChroma2 * fade, tangent, nameOrigin, nameScaleMult, 6, 40f);
        }
    }

    public static void ShowCard(BossIntroCard card)
    {
        if (!FablesConfig.Instance.BossIntroCardsActivated)
        {
            return;
        }

        if (BossIntroScreens.currentCard is not null)
        {
            return;
        }

        BossIntroScreens.currentCard = card;
    }

    [OnLoad]
    private static void Load()
    {
        GlobalNPCHooks.OnSpawn.Event += ShowCard;
    }

    private static void ShowCard(GlobalNPC self, NPC npc, IEntitySource source)
    {
        if (npc.type != ModContent.NPCType<TestNpc>())
        {
            return;
        }

        ShowCard(new TestCard());
    }
}

public class TestNpc : ModNPC
{
    public override string Texture => "ModLoader/UnloadedItem";

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 16;
        NPC.height = 16;

        NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
    }
}