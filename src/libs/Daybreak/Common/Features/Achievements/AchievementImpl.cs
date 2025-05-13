using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader;
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

    public override void Load()
    {
        base.Load();

        On_AchievementTagHandler.Terraria_UI_Chat_ITagHandler_Parse += UseCompatibleTextSnippetForAchievementTag;
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

    public static void Register(Achievement achievement) { }

    public static void RegisterCategory(AchievementCategory category) { }
}