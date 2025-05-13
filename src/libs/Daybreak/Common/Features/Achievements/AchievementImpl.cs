using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.UI.Chat;
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

    public override void Load()
    {
        base.Load();

        On_AchievementTagHandler.Terraria_UI_Chat_ITagHandler_Parse += UseCompatibleTextSnippetForAchievementTag;
        On_InGameNotificationsTracker.AddCompleted += AddModdedAchievementsAsCompletedInPlaceOfVanilla;
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