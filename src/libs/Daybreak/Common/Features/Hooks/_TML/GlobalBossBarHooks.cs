namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalBossBar':
//     System.Boolean Terraria.ModLoader.GlobalBossBar::PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.NPC,Terraria.DataStructures.BossBarDrawParams&)
//     System.Void Terraria.ModLoader.GlobalBossBar::PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.NPC,Terraria.DataStructures.BossBarDrawParams)
public static partial class GlobalBossBarHooks
{
    public static partial class PreDraw
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalBossBar self,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.NPC npc,
            ref Terraria.DataStructures.BossBarDrawParams drawParams
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static partial bool Invoke(
            Terraria.ModLoader.GlobalBossBar self,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.NPC npc,
            ref Terraria.DataStructures.BossBarDrawParams drawParams
        );
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalBossBar self,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.NPC npc,
            Terraria.DataStructures.BossBarDrawParams drawParams
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalBossBar self,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.NPC npc,
            Terraria.DataStructures.BossBarDrawParams drawParams
        )
        {
            Event?.Invoke(self, spriteBatch, npc, drawParams);
        }
    }
}
