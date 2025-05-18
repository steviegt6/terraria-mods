namespace Daybreak.Common.Features.Hooks;

// Hooks to generate for 'Terraria.ModLoader.GlobalInfoDisplay':
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalInfoDisplay::Active(Terraria.ModLoader.InfoDisplay)
//     System.Void Terraria.ModLoader.GlobalInfoDisplay::ModifyDisplayName(Terraria.ModLoader.InfoDisplay,System.String&)
//     System.Void Terraria.ModLoader.GlobalInfoDisplay::ModifyDisplayValue(Terraria.ModLoader.InfoDisplay,System.String&)
//     System.Void Terraria.ModLoader.GlobalInfoDisplay::ModifyDisplayColor(Terraria.ModLoader.InfoDisplay,Microsoft.Xna.Framework.Color&,Microsoft.Xna.Framework.Color&)
//     System.Void Terraria.ModLoader.GlobalInfoDisplay::ModifyDisplayParameters(Terraria.ModLoader.InfoDisplay,System.String&,System.String&,Microsoft.Xna.Framework.Color&,Microsoft.Xna.Framework.Color&)
public static partial class GlobalInfoDisplayHooks
{
    public static partial class Active
    {
        public delegate bool? Definition(
            Terraria.ModLoader.InfoDisplay currentDisplay
        );
    }

    public static partial class ModifyDisplayName
    {
        public delegate void Definition(
            Terraria.ModLoader.InfoDisplay currentDisplay,
            ref string displayName
        );
    }

    public static partial class ModifyDisplayValue
    {
        public delegate void Definition(
            Terraria.ModLoader.InfoDisplay currentDisplay,
            ref string displayValue
        );
    }

    public static partial class ModifyDisplayColor
    {
        public delegate void Definition(
            Terraria.ModLoader.InfoDisplay currentDisplay,
            ref Microsoft.Xna.Framework.Color displayColor,
            ref Microsoft.Xna.Framework.Color displayShadowColor
        );
    }

    public static partial class ModifyDisplayParameters
    {
        public delegate void Definition(
            Terraria.ModLoader.InfoDisplay currentDisplay,
            ref string displayValue,
            ref string displayName,
            ref Microsoft.Xna.Framework.Color displayColor,
            ref Microsoft.Xna.Framework.Color displayShadowColor
        );
    }

}
