namespace Daybreak.Common.Features.Hooks;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalBossBar':
//     System.Boolean Terraria.ModLoader.GlobalBossBar::PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.NPC,Terraria.DataStructures.BossBarDrawParams&)
//     System.Void Terraria.ModLoader.GlobalBossBar::PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.NPC,Terraria.DataStructures.BossBarDrawParams)
public static partial class GlobalBossBarHooks
{
    public static partial class PreDraw
    {
        public delegate bool Definition(
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.NPC npc,
            ref Terraria.DataStructures.BossBarDrawParams drawParams
        );
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.NPC npc,
            Terraria.DataStructures.BossBarDrawParams drawParams
        );
    }

}
