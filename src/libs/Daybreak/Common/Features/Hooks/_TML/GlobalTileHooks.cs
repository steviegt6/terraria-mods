namespace Daybreak.Common.Features.Hooks;

// Hooks to generate for 'Terraria.ModLoader.GlobalTile':
//     System.Void Terraria.ModLoader.GlobalTile::DropCritterChance(System.Int32,System.Int32,System.Int32,System.Int32&,System.Int32&,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalTile::CanDrop(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::Drop(System.Int32,System.Int32,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalTile::CanKillTile(System.Int32,System.Int32,System.Int32,System.Boolean&)
//     System.Void Terraria.ModLoader.GlobalTile::KillTile(System.Int32,System.Int32,System.Int32,System.Boolean&,System.Boolean&,System.Boolean&)
//     System.Void Terraria.ModLoader.GlobalTile::NearbyEffects(System.Int32,System.Int32,System.Int32,System.Boolean)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalTile::IsTileDangerous(System.Int32,System.Int32,System.Int32,Terraria.Player)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalTile::IsTileBiomeSightable(System.Int32,System.Int32,System.Int32,Microsoft.Xna.Framework.Color&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalTile::IsTileSpelunkable(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::SetSpriteEffects(System.Int32,System.Int32,System.Int32,Microsoft.Xna.Framework.Graphics.SpriteEffects&)
//     System.Void Terraria.ModLoader.GlobalTile::AnimateTile()
//     System.Void Terraria.ModLoader.GlobalTile::DrawEffects(System.Int32,System.Int32,System.Int32,Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.DataStructures.TileDrawInfo&)
//     System.Void Terraria.ModLoader.GlobalTile::EmitParticles(System.Int32,System.Int32,Terraria.Tile,System.UInt16,System.Int16,System.Int16,Microsoft.Xna.Framework.Color,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalTile::SpecialDraw(System.Int32,System.Int32,System.Int32,Microsoft.Xna.Framework.Graphics.SpriteBatch)
//     System.Boolean Terraria.ModLoader.GlobalTile::TileFrame(System.Int32,System.Int32,System.Int32,System.Boolean&,System.Boolean&)
//     System.Int32[] Terraria.ModLoader.GlobalTile::AdjTiles(System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::RightClick(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::MouseOver(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::MouseOverFar(System.Int32,System.Int32,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalTile::AutoSelect(System.Int32,System.Int32,System.Int32,Terraria.Item)
//     System.Boolean Terraria.ModLoader.GlobalTile::PreHitWire(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::HitWire(System.Int32,System.Int32,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalTile::Slope(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::FloorVisuals(System.Int32,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalTile::ChangeWaterfallStyle(System.Int32,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalTile::CanReplace(System.Int32,System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalTile::PostSetupTileMerge()
//     System.Void Terraria.ModLoader.GlobalTile::PreShakeTree(System.Int32,System.Int32,Terraria.Enums.TreeTypes)
//     System.Boolean Terraria.ModLoader.GlobalTile::ShakeTree(System.Int32,System.Int32,Terraria.Enums.TreeTypes)
public static partial class GlobalTileHooks
{
    public static partial class DropCritterChance
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            ref int wormChance,
            ref int grassHopperChance,
            ref int jungleGrubChance
        );
    }

    public static partial class CanDrop
    {
        public delegate bool Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class Drop
    {
        public delegate void Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class CanKillTile
    {
        public delegate bool Definition(
            int i,
            int j,
            int type,
            ref bool blockDamaged
        );
    }

    public static partial class KillTile
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            ref bool fail,
            ref bool effectOnly,
            ref bool noItem
        );
    }

    public static partial class NearbyEffects
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            bool closer
        );
    }

    public static partial class IsTileDangerous
    {
        public delegate bool? Definition(
            int i,
            int j,
            int type,
            Terraria.Player player
        );
    }

    public static partial class IsTileBiomeSightable
    {
        public delegate bool? Definition(
            int i,
            int j,
            int type,
            ref Microsoft.Xna.Framework.Color sightColor
        );
    }

    public static partial class IsTileSpelunkable
    {
        public delegate bool? Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class SetSpriteEffects
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            ref Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );
    }

    public static partial class AnimateTile
    {
        public delegate void Definition(
        );
    }

    public static partial class DrawEffects
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            ref Terraria.DataStructures.TileDrawInfo drawData
        );
    }

    public static partial class EmitParticles
    {
        public delegate void Definition(
            int i,
            int j,
            Terraria.Tile tileCache,
            ushort typeCache,
            short tileFrameX,
            short tileFrameY,
            Microsoft.Xna.Framework.Color tileLight,
            bool visible
        );
    }

    public static partial class SpecialDraw
    {
        public delegate void Definition(
            int i,
            int j,
            int type,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch
        );
    }

    public static partial class TileFrame
    {
        public delegate bool Definition(
            int i,
            int j,
            int type,
            ref bool resetFrame,
            ref bool noBreak
        );
    }

    public static partial class AdjTiles
    {
        public delegate System.Int32[] Definition(
            int type
        );
    }

    public static partial class RightClick
    {
        public delegate void Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class MouseOver
    {
        public delegate void Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class MouseOverFar
    {
        public delegate void Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class AutoSelect
    {
        public delegate bool Definition(
            int i,
            int j,
            int type,
            Terraria.Item item
        );
    }

    public static partial class PreHitWire
    {
        public delegate bool Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class HitWire
    {
        public delegate void Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class Slope
    {
        public delegate bool Definition(
            int i,
            int j,
            int type
        );
    }

    public static partial class FloorVisuals
    {
        public delegate void Definition(
            int type,
            Terraria.Player player
        );
    }

    public static partial class ChangeWaterfallStyle
    {
        public delegate void Definition(
            int type,
            ref int style
        );
    }

    public static partial class CanReplace
    {
        public delegate bool Definition(
            int i,
            int j,
            int type,
            int tileTypeBeingPlaced
        );
    }

    public static partial class PostSetupTileMerge
    {
        public delegate void Definition(
        );
    }

    public static partial class PreShakeTree
    {
        public delegate void Definition(
            int x,
            int y,
            Terraria.Enums.TreeTypes treeType
        );
    }

    public static partial class ShakeTree
    {
        public delegate bool Definition(
            int x,
            int y,
            Terraria.Enums.TreeTypes treeType
        );
    }

}
