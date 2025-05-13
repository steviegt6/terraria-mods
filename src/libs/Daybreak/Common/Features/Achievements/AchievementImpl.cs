using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Daybreak.Core.Hooks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Daybreak.Common.Features.Achievements;

/// <summary>
///     Implementation for achievements.
/// </summary>
internal sealed class AchievementImpl : ModSystem
{
    private sealed class CompatibleAchievementSnippet : TextSnippet
    {
        private Achievement achievement;

        public CompatibleAchievementSnippet(Achievement achievement) : base(achievement.DisplayName.Value, Color.LightBlue)
        {
            CheckForHover = true;
            this.achievement = achievement;
        }

        public override void OnClick()
        {
            base.OnClick();

            IngameOptions.Close();
            // TODO: IngameFancyUI.OpenAchievementsAndGoto(achievement);
        }
    }

    // TODO
    private sealed class CompatibleAchievementUnlockedPopup : IInGameNotification
    {
        public bool ShouldBeRemoved { get; }

        private readonly Achievement achievement;

        public CompatibleAchievementUnlockedPopup(Achievement achievement)
        {
            this.achievement = achievement;
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
        {
            throw new System.NotImplementedException();
        }

        public void PushAnchor(ref Vector2 positionAnchorBottom)
        {
            throw new System.NotImplementedException();
        }
    }

    private sealed class CustomAchievementAdvisor : ILoad
    {
        private static readonly Asset<Texture2D> achievements_border_texture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders");
        private static readonly Asset<Texture2D> achievements_border_mouse_hover_fat_texture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHover");
        private static readonly Asset<Texture2D> achievements_border_mouse_hover_thin_texture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHoverThin");

        internal static Achievement[] Cards = [];

        private static Achievement? hoveredCard;

        void ILoad.Load()
        {
            On_AchievementAdvisor.DrawOneAchievement += (_, _, batch, position, large) =>
            {
                DrawOneAchievement(batch, position, large);
            };

            On_AchievementAdvisor.Update += (_, _) =>
            {
                Update();
            };

            On_AchievementAdvisor.DrawOptionsPanel += (_, _, batch, position, rightPosition) =>
            {
                DrawOptionsPanel(batch, position, rightPosition);
            };

            On_AchievementAdvisor.DrawMouseHover += (_, _) =>
            {
                DrawMouseHover();
            };
        }

        private void DrawOneAchievement(SpriteBatch spriteBatch, Vector2 position, bool large)
        {
            var bestCard = GetBestCards(cardsAmount: 1).FirstOrDefault();
            if (bestCard is null)
            {
                return;
            }

            hoveredCard = null;

            var scale = large ? 0.75f : 0.35f;
            DrawCard(bestCard, spriteBatch, position + new Vector2(8f) * scale, scale, out var hovered);

            if (!hovered)
            {
                return;
            }

            hoveredCard = bestCard;

            if (PlayerInput.IgnoreMouseInterface)
            {
                return;
            }

            Main.LocalPlayer.mouseInterface = true;

            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                Main.ingameOptionsWindow = false;
                // TODO: IngameFancyUI.OpenAchievementsAndGoto(hoveredCard);
            }
        }

        private void Update()
        {
            hoveredCard = null;
        }

        private void DrawOptionsPanel(SpriteBatch spriteBatch, Vector2 leftPosition, Vector2 rightPosition)
        {
            var bestCards = GetBestCards().ToArray();
            hoveredCard = null;

            var numCardsToConsider = bestCards.Length;
            if (numCardsToConsider > 5)
            {
                numCardsToConsider = 5;
            }

            bool hovered;
            for (var i = 0; i < numCardsToConsider; i++)
            {
                DrawCard(bestCards[i], spriteBatch, leftPosition + new Vector2(42 * i, 0f), 0.5f, out hovered);
                if (hovered)
                {
                    hoveredCard = bestCards[i];
                }
            }

            for (var i = 5; i < bestCards.Length; i++)
            {
                DrawCard(bestCards[i], spriteBatch, rightPosition + new Vector2(42 * i, 0f), 0.5f, out hovered);
                if (hovered)
                {
                    hoveredCard = bestCards[i];
                }
            }

            if (hoveredCard is null)
            {
                return;
            }

            if (hoveredCard.IsCompleted)
            {
                hoveredCard = null;
            }
            else if (!PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (!Main.mouseLeft || !Main.mouseLeftRelease)
                {
                    return;
                }

                Main.ingameOptionsWindow = false;
                // TODO: IngameFancyUI.OpenAchievementsAndGoto(hoveredCard);
            }
        }

        private void DrawMouseHover()
        {
            if (hoveredCard is null)
            {
                return;
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);

            PlayerInput.SetZoom_UI();

            var item = new Item();
            {
                item.SetDefaults(0, noMatCheck: true);
                item.SetNameOverride(hoveredCard.DisplayName.Value);
                item.ToolTip = ItemTooltip.FromLanguageKey(hoveredCard.Description.Key);
                item.type = 1;
                item.scale = 0f;
                item.rare = 10;
                item.value = -1;
            }

            Main.HoverItem = item;
            Main.instance.MouseText("");
            Main.mouseText = true;
        }

        private void DrawCard(Achievement card, SpriteBatch spriteBatch, Vector2 position, float scale, out bool hovered)
        {
            hovered = false;

            var texture = card.GetAdvisorIcon(out var cardFrame, out var hoveredOffset);
            if (texture is null)
            {
                // TODO: Throw?
                return;
            }

            if (Main.MouseScreen.Between(position, position + cardFrame.Size() * scale))
            {
                Main.LocalPlayer.mouseInterface = true;
                hovered = true;
            }

            var color = Color.White;
            if (!hovered)
            {
                color = new Color(220, 220, 220, 220);
            }

            var vector = new Vector2(-4f) * scale;
            var vector2 = new Vector2(-8f) * scale;
            var value = achievements_border_mouse_hover_fat_texture.Value;
            if (scale > 0.5f)
            {
                value = achievements_border_mouse_hover_thin_texture.Value;
                vector2 = new Vector2(-5f) * scale;
            }

            var frame = cardFrame;
            frame.X += hoveredOffset;
            spriteBatch.Draw(texture.Value, position, frame, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(achievements_border_texture.Value, position + vector, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if (hovered)
            {
                spriteBatch.Draw(value, position + vector2, null, Main.OurFavoriteColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        private IEnumerable<Achievement> GetBestCards(int cardsAmount = 10)
        {
            var added = 0;
            foreach (var card in Cards)
            {
                if (card.IsCompleted || !card.IsPresentlyAvailable())
                {
                    continue;
                }

                yield return card;
                added++;

                if (added >= cardsAmount)
                {
                    break;
                }
            }
        }
    }

    private static readonly List<Achievement> achievements = [];
    private static readonly List<AchievementCategory> categories = [];

    public override void Load()
    {
        base.Load();

        On_AchievementTagHandler.Terraria_UI_Chat_ITagHandler_Parse += UseCompatibleTextSnippetForAchievementTag;
        On_InGameNotificationsTracker.AddCompleted += AddModdedAchievementsAsCompletedInPlaceOfVanilla;
    }

    public override void PostSetupContent()
    {
        base.PostSetupContent();

        CustomAchievementAdvisor.Cards = achievements.Where(x => x.AdvisorOrder >= 0f).OrderBy(x => x.AdvisorOrder).ToArray();
    }

    private static TextSnippet UseCompatibleTextSnippetForAchievementTag(
        On_AchievementTagHandler.orig_Terraria_UI_Chat_ITagHandler_Parse orig,
        AchievementTagHandler self,
        string text,
        Color baseColor,
        string options
    )
    {
        if (!TrySplitName(text, out var modName, out var achName))
        {
            return VanillaAchievements.VANILLA_ACHIEVEMENTS_BY_NAME.TryGetValue(text, out var vanillaAch)
                ? new CompatibleAchievementSnippet(vanillaAch)
                : new TextSnippet(text);
        }

        if (!ModLoader.TryGetMod(modName, out var mod))
        {
            return new TextSnippet(text);
        }

        if (!mod.TryFind<Achievement>(achName, out var ach))
        {
            return new TextSnippet(text);
        }

        return new CompatibleAchievementSnippet(ach);

        static bool TrySplitName(
            string name,
            [NotNullWhen(returnValue: true)] out string? domain,
            [NotNullWhen(returnValue: true)] out string? subName
        )
        {
            try
            {
                ModContent.SplitName(name, out domain, out subName);
                return true;
            }
            catch
            {
                domain = null;
                subName = null;
                return false;
            }
        }
    }

    private static void AddModdedAchievementsAsCompletedInPlaceOfVanilla(
        On_InGameNotificationsTracker.orig_AddCompleted orig,
        Terraria.Achievements.Achievement achievement
    )
    {
        if (Main.netMode == NetmodeID.Server)
        {
            return;
        }

        if (!VanillaAchievements.VANILLA_ACHIEVEMENTS_BY_NAME.TryGetValue(achievement.Name, out var ach))
        {
            // TODO: Throw exception?
            return;
        }

        InGameNotificationsTracker.AddNotification(new CompatibleAchievementUnlockedPopup(ach));
    }

    public static void Register(Achievement achievement) { }

    public static void RegisterCategory(AchievementCategory category) { }
}