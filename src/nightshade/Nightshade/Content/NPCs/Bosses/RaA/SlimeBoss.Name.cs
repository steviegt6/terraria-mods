using System;
using System.Linq;

using Daybreak.Common.Features.Hooks;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

partial class SlimeBoss
{
    private readonly struct HoverNameOverride : IDisposable
    {
        private static int count;

        public static bool IsActive => count > 0;

        public HoverNameOverride()
        {
            count++;
        }

        public void Dispose()
        {
            count--;
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
        On_UnlockableNPCEntryIcon.GetHoverText += ModifyBestiaryIconToUseCustomName;

        On_Main.MouseText_string_string_int_byte_int_int_int_int_int_bool += ApplyHoverNameHack;
        On_Main.HoverOverNPCs += ApplyHoverNameHack_EnableHoveringNpcs;
    }

    private static string ModifyBestiaryIconToUseCustomName(On_UnlockableNPCEntryIcon.orig_GetHoverText orig, UnlockableNPCEntryIcon self, BestiaryUICollectionInfo providedInfo)
    {
        if (self._npcNetId == ModContent.NPCType<SlimeBoss>() && self.GetUnlockState(providedInfo))
        {
            return Starspeak.GetBossNameTag();
        }

        return orig(self, providedInfo);
    }

    private static void ApplyHoverNameHack_EnableHoveringNpcs(On_Main.orig_HoverOverNPCs orig, Main self, Rectangle mouseRectangle)
    {
        using (new HoverNameOverride())
        {
            orig(self, mouseRectangle);
        }
    }

    private static void ApplyHoverNameHack(
        On_Main.orig_MouseText_string_string_int_byte_int_int_int_int_int_bool orig,
        Main self,
        string cursorText,
        string buffTooltip,
        int rare,
        byte diff,
        int hackedMouseX,
        int hackedMouseY,
        int hackedScreenWidth,
        int hackedScreenHeight,
        int pushWidthX,
        bool noOverride
    )
    {
        if (HoverNameOverride.IsActive)
        {
            var nameParts = cursorText.Split(':');
            if (nameParts[0] == Mods.Nightshade.NPCs.SlimeBoss.DisplayName.GetTextValue())
            {
                nameParts[0] = Starspeak.GetBossNameTag() + ' ';
            }

            cursorText = string.Join(':', nameParts);
        }

        orig(self, cursorText, buffTooltip, rare, diff, hackedMouseX, hackedMouseY, hackedScreenWidth, hackedScreenHeight, pushWidthX, noOverride);
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