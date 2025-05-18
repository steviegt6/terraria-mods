namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalPylon':
//     System.Boolean Terraria.ModLoader.GlobalPylon::PreDrawMapIcon(Terraria.Map.MapOverlayDrawContext&,System.String&,Terraria.GameContent.TeleportPylonInfo&,System.Boolean&,Microsoft.Xna.Framework.Color&,System.Single&,System.Single&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalPylon::PreCanPlacePylon(System.Int32,System.Int32,System.Int32,Terraria.GameContent.TeleportPylonType)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalPylon::ValidTeleportCheck_PreNPCCount(Terraria.GameContent.TeleportPylonInfo,System.Int32&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalPylon::ValidTeleportCheck_PreAnyDanger(Terraria.GameContent.TeleportPylonInfo)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalPylon::ValidTeleportCheck_PreBiomeRequirements(Terraria.GameContent.TeleportPylonInfo,Terraria.SceneMetrics)
//     System.Void Terraria.ModLoader.GlobalPylon::PostValidTeleportCheck(Terraria.GameContent.TeleportPylonInfo,Terraria.GameContent.TeleportPylonInfo,System.Boolean&,System.Boolean&,System.String&)
public static partial class GlobalPylonHooks
{
    public static partial class PreDrawMapIcon
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalPylon self,
            ref Terraria.Map.MapOverlayDrawContext context,
            ref string mouseOverText,
            ref Terraria.GameContent.TeleportPylonInfo pylonInfo,
            ref bool isNearPylon,
            ref Microsoft.Xna.Framework.Color drawColor,
            ref float deselectedScale,
            ref float selectedScale
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool Invoke(
            Terraria.ModLoader.GlobalPylon self,
            ref Terraria.Map.MapOverlayDrawContext context,
            ref string mouseOverText,
            ref Terraria.GameContent.TeleportPylonInfo pylonInfo,
            ref bool isNearPylon,
            ref Microsoft.Xna.Framework.Color drawColor,
            ref float deselectedScale,
            ref float selectedScale
        );
    }

    public static partial class PreCanPlacePylon
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalPylon self,
            int x,
            int y,
            int tileType,
            Terraria.GameContent.TeleportPylonType pylonType
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool? Invoke(
            Terraria.ModLoader.GlobalPylon self,
            int x,
            int y,
            int tileType,
            Terraria.GameContent.TeleportPylonType pylonType
        );
    }

    public static partial class ValidTeleportCheck_PreNPCCount
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo pylonInfo,
            ref int defaultNecessaryNPCCount
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool? Invoke(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo pylonInfo,
            ref int defaultNecessaryNPCCount
        );
    }

    public static partial class ValidTeleportCheck_PreAnyDanger
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo pylonInfo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool? Invoke(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo pylonInfo
        );
    }

    public static partial class ValidTeleportCheck_PreBiomeRequirements
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo pylonInfo,
            Terraria.SceneMetrics sceneData
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool? Invoke(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo pylonInfo,
            Terraria.SceneMetrics sceneData
        );
    }

    public static partial class PostValidTeleportCheck
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo destinationPylonInfo,
            Terraria.GameContent.TeleportPylonInfo nearbyPylonInfo,
            ref bool destinationPylonValid,
            ref bool validNearbyPylonFound,
            ref string errorKey
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalPylon self,
            Terraria.GameContent.TeleportPylonInfo destinationPylonInfo,
            Terraria.GameContent.TeleportPylonInfo nearbyPylonInfo,
            ref bool destinationPylonValid,
            ref bool validNearbyPylonFound,
            ref string errorKey
        )
        {
            Event?.Invoke(self, destinationPylonInfo, nearbyPylonInfo, ref destinationPylonValid, ref validNearbyPylonFound, ref errorKey);
        }
    }
}
