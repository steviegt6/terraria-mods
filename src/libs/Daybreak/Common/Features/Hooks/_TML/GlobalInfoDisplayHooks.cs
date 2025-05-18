namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalInfoDisplay':
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalInfoDisplay::Active(Terraria.ModLoader.InfoDisplay)
//     System.Void Terraria.ModLoader.GlobalInfoDisplay::ModifyDisplayParameters(Terraria.ModLoader.InfoDisplay,System.String&,System.String&,Microsoft.Xna.Framework.Color&,Microsoft.Xna.Framework.Color&)
public static partial class GlobalInfoDisplayHooks
{
    public static partial class Active
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalInfoDisplay self,
            Terraria.ModLoader.InfoDisplay currentDisplay
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool? Invoke(
            Terraria.ModLoader.GlobalInfoDisplay self,
            Terraria.ModLoader.InfoDisplay currentDisplay
        );
    }

    public static partial class ModifyDisplayParameters
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalInfoDisplay self,
            Terraria.ModLoader.InfoDisplay currentDisplay,
            ref string displayValue,
            ref string displayName,
            ref Microsoft.Xna.Framework.Color displayColor,
            ref Microsoft.Xna.Framework.Color displayShadowColor
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalInfoDisplay self,
            Terraria.ModLoader.InfoDisplay currentDisplay,
            ref string displayValue,
            ref string displayName,
            ref Microsoft.Xna.Framework.Color displayColor,
            ref Microsoft.Xna.Framework.Color displayShadowColor
        )
        {
            Event?.Invoke(self, currentDisplay, ref displayValue, ref displayName, ref displayColor, ref displayShadowColor);
        }
    }
}
