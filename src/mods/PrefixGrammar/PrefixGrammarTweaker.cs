using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PrefixGrammar;

public sealed class PrefixGrammarTweaker : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_Item.AffixName += AffixName_ModifyPrefixComposition;
    }

    private static string AffixName_ModifyPrefixComposition(On_Item.orig_AffixName orig, Item self)
    {
        var text = orig(self);

        // Simple early return case here that replicates the source.  It's
        // possible a mod might do something to circumvent regular vanilla
        // procedures that requires us to remove this check, so we can do so
        // later.
        if (self.prefix < 0 || self.prefix >= Lang.prefix.Length)
        {
            return text;
        }

        var config = ModContent.GetInstance<GrammarConfig>();
        var prefixOptions = config.PrefixFormatting;
        var doubleOptions = config.DoubleFormatting;
        var insertComma = config.InsertComma;
        
        return LanguageRules.GetRulesForLanguage(Language.ActiveCulture).FormatString(
            text,
            prefixOptions,
            doubleOptions,
            insertComma
        );
    }
}