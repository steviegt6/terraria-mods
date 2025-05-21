using Daybreak.Common.Features.Hooks;

using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

partial class SlimeBoss
{
    private readonly record struct NameProfile(int NameCount, int TitleCount, int SubtitleCount)
    {
        public int NameIndex => Main.rand.Next(NameCount);

        public int TitleIndex => Main.rand.Next(TitleCount);

        public int SubtitleIndex => Main.rand.Next(SubtitleCount);

        public string GetFullBossName()
        {
            return Mods.Nightshade.NPCs.SlimeBoss.FullBossName.GetTextValue(
                GetBossName(),
                GetBossSubtitle()
            );
        }

        public string GetBossName()
        {
            return Mods.Nightshade.NPCs.SlimeBoss.BossName.GetTextValue(
                GetName(NameIndex)
            );
        }

        public string GetBossSubtitle()
        {
            return Mods.Nightshade.NPCs.SlimeBoss.BossSubtitle.GetTextValue(
                GetSubtitle(SubtitleIndex)
            );
        }

        private static LocalizedText GetName(int i)
        {
            return Mods.Nightshade.NPCs.SlimeBoss.BossNames.GetChildText(i.ToString());
        }

        private static LocalizedText GetTitle(int i)
        {
            return Mods.Nightshade.NPCs.SlimeBoss.BossTitles.GetChildText(i.ToString());
        }

        private static LocalizedText GetSubtitle(int i)
        {
            return Mods.Nightshade.NPCs.SlimeBoss.BossSubtitles.GetChildText(i.ToString());
        }
    }

    private static NameProfile nameProfile;

    [OnLoad]
    private static void InitializeNameInjections()
    {
        InitNameProfile();

        LanguageManager.Instance.OnLanguageChanged += InitNameProfile_OnLanguageChanged;

        On_Lang.GetNPCName += (orig, id) =>
        {
            if (id != ModContent.NPCType<SlimeBoss>())
            {
                return orig(id);
            }

            var text = Language.GetText("Mods.Nightshade.NPCs.SlimeBoss.DummyEntryForNpcName");
            {
                text.SetValue(nameProfile.GetFullBossName());
            }
            return text;
        };
    }

    private static void InitNameProfile_OnLanguageChanged(LanguageManager languageManager)
    {
        InitNameProfile();
    }

    private static void InitNameProfile()
    {
        nameProfile = new NameProfile(
            NameCount: GetCount(Mods.Nightshade.NPCs.SlimeBoss.BossNames.KEY + '.'),
            TitleCount: GetCount(Mods.Nightshade.NPCs.SlimeBoss.BossTitles.KEY + '.'),
            SubtitleCount: GetCount(Mods.Nightshade.NPCs.SlimeBoss.BossSubtitles.KEY + '.')
        );

        return;

        static int GetCount(string filter)
        {
            return Language.FindAll(Lang.CreateDialogFilter(filter)).Length;
        }
    }
}