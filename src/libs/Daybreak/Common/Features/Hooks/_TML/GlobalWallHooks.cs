namespace Daybreak.Common.Features.Hooks;

// Hooks to generate for 'Terraria.ModLoader.GlobalWall':
//     System.Boolean Terraria.ModLoader.GlobalWall::Drop(System.Int32,System.Int32,System.Int32,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalWall::KillWall(System.Int32,System.Int32,System.Int32,System.Boolean&)
//     System.Boolean Terraria.ModLoader.GlobalWall::WallFrame(System.Int32,System.Int32,System.Int32,System.Boolean,System.Int32&,System.Int32&)
public static partial class GlobalWallHooks
{
    public static partial class Drop
    {
        public delegate bool Definition(
            int i,
            int j,
            int type,
            ref int dropType
        );
    }

    public static partial class KillWall
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            ref bool fail
        );
    }

    public static partial class WallFrame
    {
        public delegate bool Definition(
            int i,
            int j,
            int type,
            bool randomizeFrame,
            ref int style,
            ref int frameNumber
        );
    }

}
