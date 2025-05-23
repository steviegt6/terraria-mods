using System.Linq;

using Daybreak.Common.Features.Hooks;

using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Content.NPCs.Bosses.RaA;

partial class SlimeBoss
{
    private sealed class ChatTagNamePlateInfoElement(string display, string key) : IBestiaryInfoElement, IProvideSearchFilterString
    {
        public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
        {
            UIElement text = info.UnlockState != 0 ? new UIText(display) : new UIText("???");
            {
                text.HAlign = 0.5f;
                text.VAlign = 0.5f;
                text.Top = new StyleDimension(2f, 0f);
                text.IgnoresMouseInteraction = true;
            }
            
            var element = new UIElement
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(24f, 0f),
            };
            {
                element.Append(text);
            }
            
            return element;
        }

        public string? GetSearchString(ref BestiaryUICollectionInfo info)
        {
            if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
            {
                return null;
            }

            return Language.GetText(key).Value;
        }
    }
    
    // Lang.GetNPCName
    // Lang.GetNPCNameValue
    // NPC.GetTypeNetName
    // NPC.TypeName
    // NPC.GetFullnameByID
    // NPC.GetFullNetName
    
    // Condition.NpcIsPresent - x
    // BestiaryEntry.Enemy
    // BestiaryEntry.TownNPC - x
    // BestiaryEntry.Critter - x
    // Lang.GetInvasionWaveText
    // NPCDefinitionElement.GetPassedOptionElements - x
    // NPC.CountKillForBannersAndDropThem - x
    // NPC.SpawnSkeletron - x
    
    // UnlockableNPCEntryIcon.GetHoverText
    // Main.MouseText_DrawBuffTooltip
    // Main.DrawInfoAccs
    // NPCDefinition.DisplayName - x

    [OnLoad]
    private static void ApplyRenderingTweaksToRenderNameAsStarspeak()
    {
        On_BestiaryEntry.Enemy += ModifyInfoItemsToUseCustomName;
    }

    private static BestiaryEntry ModifyInfoItemsToUseCustomName(On_BestiaryEntry.orig_Enemy orig, int npcNetId)
    {
        var entry = orig(npcNetId);

        if (npcNetId != ModContent.NPCType<SlimeBoss>())
        {
            return entry;
        }

        if (entry.Info.FirstOrDefault(x => x is NamePlateInfoElement) is NamePlateInfoElement namePlateInfoElement)
        {
            // Not a key, technically, but it should work fine.
            namePlateInfoElement._key = Starspeak.GetBossNameTag();
        }
        
        return entry;
    }
}