using Terraria.ModLoader.Config;

namespace Tomat.TML.Mod.PrefixGrammar;

public sealed class GrammarConfig : ModConfig
{
    public enum PrefixOptions
    {
        Default,
        Replace,
        After,
    }

    public enum DoubleOptions
    {
        Default,
        Doubly,
    }
    
    public override ConfigScope Mode => ConfigScope.ClientSide;
    
    public PrefixOptions PrefixFormatting { get; set; } = PrefixOptions.Default;
    
    public DoubleOptions DoubleFormatting { get; set; } = DoubleOptions.Default;

    public bool InsertComma { get; set; } = false;
}