using Microsoft.Xna.Framework;

using Terraria.ModLoader;

namespace Nightshade.Common.Compat;

public class CalamityFablesCompat
{
    // ModCall APIs taken from WotG
    public record class CardProfile(
        string BossName,
        string BossTitle,
        int AnimationDuration,
        bool Flip,
        Color BorderColor,
        Color BossTitleColor,
        Color BossNameChromaA,
        Color BossNameChromaB
    )
    {
        /// <summary>
        ///     The optional name of the music track title.
        /// </summary>
        public string? MusicTitle { get; private set; }


        /// <summary>
        ///     The optional name of the composer that created the music track.
        /// </summary>
        public string? MusicComposerName { get; private set; }


        public CardProfile WithMusicAttributes(string musicTitle, string composerName)
        {
            MusicTitle = musicTitle;
            MusicComposerName = composerName;
            return this;
        }
    }

    private static readonly Mod? mod = ModLoader.TryGetMod("CalamityFables", out var theMod) ? theMod : null;

    /// <summary>
    ///     Displays a custom boss intro card based on the variant used for Fables.
    /// </summary>
    public static void DisplayBossCard(CardProfile cardProfile)
    {
        if (mod is null)
        {
            return;
        }

        const string mod_call = "vfx.displayBossIntroCard";

        var hasMusicAttributes = cardProfile.MusicTitle is not null && cardProfile.MusicComposerName is not null;

        if (hasMusicAttributes)
        {
            mod.Call(
                mod_call,
                cardProfile.BossName,
                cardProfile.BossTitle,
                cardProfile.AnimationDuration,
                cardProfile.Flip,
                cardProfile.BorderColor,
                cardProfile.BossTitleColor,
                cardProfile.BossNameChromaA,
                cardProfile.BossNameChromaB,
                cardProfile.MusicTitle,
                cardProfile.MusicComposerName
            );
        }
        else
        {
            mod.Call(
                mod_call,
                cardProfile.BossName,
                cardProfile.BossTitle,
                cardProfile.AnimationDuration,
                cardProfile.Flip,
                cardProfile.BorderColor,
                cardProfile.BossTitleColor,
                cardProfile.BossNameChromaA,
                cardProfile.BossNameChromaB
            );
        }
    }
}