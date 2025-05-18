namespace Daybreak.Common.Features.Hooks;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalBuff':
//     System.Void Terraria.ModLoader.GlobalBuff::Update(System.Int32,Terraria.Player,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalBuff::Update(System.Int32,Terraria.NPC,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalBuff::ReApply(System.Int32,Terraria.Player,System.Int32,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalBuff::ReApply(System.Int32,Terraria.NPC,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalBuff::ModifyBuffText(System.Int32,System.String&,System.String&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalBuff::CustomBuffTipSize(System.String,System.Collections.Generic.List`1<Microsoft.Xna.Framework.Vector2>)
//     System.Void Terraria.ModLoader.GlobalBuff::DrawCustomBuffTip(System.String,Microsoft.Xna.Framework.Graphics.SpriteBatch,System.Int32,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalBuff::PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch,System.Int32,System.Int32,Terraria.DataStructures.BuffDrawParams&)
//     System.Void Terraria.ModLoader.GlobalBuff::PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch,System.Int32,System.Int32,Terraria.DataStructures.BuffDrawParams)
//     System.Boolean Terraria.ModLoader.GlobalBuff::RightClick(System.Int32,System.Int32)
public static partial class GlobalBuffHooks
{
    public static partial class Update
    {
        public delegate void Definition(
            int type,
            Terraria.Player player,
            ref int buffIndex
        );

        public static event Definition Event;
    }

    public static partial class Update
    {
        public delegate void Definition(
            int type,
            Terraria.NPC npc,
            ref int buffIndex
        );

        public static event Definition Event;
    }

    public static partial class ReApply
    {
        public delegate bool Definition(
            int type,
            Terraria.Player player,
            int time,
            int buffIndex
        );

        public static event Definition Event;
    }

    public static partial class ReApply
    {
        public delegate bool Definition(
            int type,
            Terraria.NPC npc,
            int time,
            int buffIndex
        );

        public static event Definition Event;
    }

    public static partial class ModifyBuffText
    {
        public delegate void Definition(
            int type,
            ref string buffName,
            ref string tip,
            ref int rare
        );

        public static event Definition Event;
    }

    public static partial class CustomBuffTipSize
    {
        public delegate void Definition(
            string buffTip,
            System.Collections.Generic.List<Microsoft.Xna.Framework.Vector2> sizes
        );

        public static event Definition Event;
    }

    public static partial class DrawCustomBuffTip
    {
        public delegate void Definition(
            string buffTip,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            int originX,
            int originY
        );

        public static event Definition Event;
    }

    public static partial class PreDraw
    {
        public delegate bool Definition(
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            int type,
            int buffIndex,
            ref Terraria.DataStructures.BuffDrawParams drawParams
        );

        public static event Definition Event;
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            int type,
            int buffIndex,
            Terraria.DataStructures.BuffDrawParams drawParams
        );

        public static event Definition Event;
    }

    public static partial class RightClick
    {
        public delegate bool Definition(
            int type,
            int buffIndex
        );

        public static event Definition Event;
    }

}
