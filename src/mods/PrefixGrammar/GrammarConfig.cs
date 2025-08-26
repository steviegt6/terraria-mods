using System.ComponentModel;

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
    
    [DrawTicks]
    [DefaultValue(PrefixOptions.Default)]
    public PrefixOptions PrefixFormatting { get; set; } = PrefixOptions.Default;
    
    [DrawTicks]
    [DefaultValue(DoubleOptions.Doubly)]
    public DoubleOptions DoubleFormatting { get; set; } = DoubleOptions.Doubly;

    public bool InsertComma { get; set; } = false;
}