namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalWall':
//     System.Boolean Terraria.ModLoader.GlobalWall::Drop(System.Int32,System.Int32,System.Int32,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalWall::KillWall(System.Int32,System.Int32,System.Int32,System.Boolean&)
//     System.Boolean Terraria.ModLoader.GlobalWall::WallFrame(System.Int32,System.Int32,System.Int32,System.Boolean,System.Int32&,System.Int32&)
public static partial class GlobalWallHooks
{
    public static partial class Drop
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalWall self,
            int i,
            int j,
            int type,
            ref int dropType
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalWall self,
            int i,
            int j,
            int type,
            ref int dropType
        )
        {
            if (Event == null)
            {
                return true;
            }

            foreach (var handler in GetInvocationList())
            {
                if (!handler.Invoke(self, i, j, type, ref dropType))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public static partial class KillWall
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalWall self,
            int i,
            int j,
            int type,
            ref bool fail
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalWall self,
            int i,
            int j,
            int type,
            ref bool fail
        )
        {
            Event?.Invoke(self, i, j, type, ref fail);
        }
    }

    public static partial class WallFrame
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalWall self,
            int i,
            int j,
            int type,
            bool randomizeFrame,
            ref int style,
            ref int frameNumber
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalWall self,
            int i,
            int j,
            int type,
            bool randomizeFrame,
            ref int style,
            ref int frameNumber
        )
        {
            if (Event == null)
            {
                return true;
            }

            foreach (var handler in GetInvocationList())
            {
                if (!handler.Invoke(self, i, j, type, randomizeFrame, ref style, ref frameNumber))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
