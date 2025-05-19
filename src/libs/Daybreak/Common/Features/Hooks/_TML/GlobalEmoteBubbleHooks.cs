namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeDefaultValueWhenTypeNotEvident
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalEmoteBubble':
//     System.Void Terraria.ModLoader.GlobalEmoteBubble::OnSpawn(Terraria.GameContent.UI.EmoteBubble)
//     System.Boolean Terraria.ModLoader.GlobalEmoteBubble::UpdateFrame(Terraria.GameContent.UI.EmoteBubble)
//     System.Boolean Terraria.ModLoader.GlobalEmoteBubble::UpdateFrameInEmoteMenu(System.Int32,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalEmoteBubble::PreDraw(Terraria.GameContent.UI.EmoteBubble,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Graphics.SpriteEffects)
//     System.Void Terraria.ModLoader.GlobalEmoteBubble::PostDraw(Terraria.GameContent.UI.EmoteBubble,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Graphics.SpriteEffects)
//     System.Boolean Terraria.ModLoader.GlobalEmoteBubble::PreDrawInEmoteMenu(System.Int32,Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.GameContent.UI.Elements.EmoteButton,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Vector2)
//     System.Void Terraria.ModLoader.GlobalEmoteBubble::PostDrawInEmoteMenu(System.Int32,Microsoft.Xna.Framework.Graphics.SpriteBatch,Terraria.GameContent.UI.Elements.EmoteButton,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Vector2)
//     System.Nullable`1<Microsoft.Xna.Framework.Rectangle> Terraria.ModLoader.GlobalEmoteBubble::GetFrame(Terraria.GameContent.UI.EmoteBubble)
//     System.Nullable`1<Microsoft.Xna.Framework.Rectangle> Terraria.ModLoader.GlobalEmoteBubble::GetFrameInEmoteMenu(System.Int32,System.Int32,System.Int32)
public static partial class GlobalEmoteBubbleHooks
{
    public static partial class OnSpawn
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        )
        {
            Event?.Invoke(self, emoteBubble);
        }
    }

    public static partial class UpdateFrame
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        )
        {
            var result = true;
            if (Event == null)
            {
                return result;
            }

            foreach (var handler in GetInvocationList())
            {
                result &= handler.Invoke(self, emoteBubble);
            }

            return result;
        }
    }

    public static partial class UpdateFrameInEmoteMenu
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            ref int frameCounter
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            ref int frameCounter
        )
        {
            var result = true;
            if (Event == null)
            {
                return result;
            }

            foreach (var handler in GetInvocationList())
            {
                result &= handler.Invoke(self, emoteType, ref frameCounter);
            }

            return result;
        }
    }

    public static partial class PreDraw
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Graphics.Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Graphics.Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        )
        {
            var result = true;
            if (Event == null)
            {
                return result;
            }

            foreach (var handler in GetInvocationList())
            {
                result &= handler.Invoke(self, emoteBubble, spriteBatch, texture, position, frame, origin, spriteEffects);
            }

            return result;
        }
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Graphics.Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Graphics.Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        )
        {
            Event?.Invoke(self, emoteBubble, spriteBatch, texture, position, frame, origin, spriteEffects);
        }
    }

    public static partial class PreDrawInEmoteMenu
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.GameContent.UI.Elements.EmoteButton uiEmoteButton,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.GameContent.UI.Elements.EmoteButton uiEmoteButton,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin
        )
        {
            var result = true;
            if (Event == null)
            {
                return result;
            }

            foreach (var handler in GetInvocationList())
            {
                result &= handler.Invoke(self, emoteType, spriteBatch, uiEmoteButton, position, frame, origin);
            }

            return result;
        }
    }

    public static partial class PostDrawInEmoteMenu
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.GameContent.UI.Elements.EmoteButton uiEmoteButton,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.GameContent.UI.Elements.EmoteButton uiEmoteButton,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin
        )
        {
            Event?.Invoke(self, emoteType, spriteBatch, uiEmoteButton, position, frame, origin);
        }
    }

    public static partial class GetFrame
    {
        public delegate Microsoft.Xna.Framework.Rectangle? Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static Microsoft.Xna.Framework.Rectangle? Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        )
        {
            var result = default(Microsoft.Xna.Framework.Rectangle?);
            if (Event == null)
            {
                return result;
            }

            foreach (var handler in GetInvocationList())
            {
                var newValue = handler.Invoke(self, emoteBubble);
                if (newValue != null)
                {
                    result = newValue;
                }
            }

            return result;
        }
    }

    public static partial class GetFrameInEmoteMenu
    {
        public delegate Microsoft.Xna.Framework.Rectangle? Definition(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            int frame,
            int frameCounter
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static Microsoft.Xna.Framework.Rectangle? Invoke(
            Terraria.ModLoader.GlobalEmoteBubble self,
            int emoteType,
            int frame,
            int frameCounter
        )
        {
            var result = default(Microsoft.Xna.Framework.Rectangle?);
            if (Event == null)
            {
                return result;
            }

            foreach (var handler in GetInvocationList())
            {
                var newValue = handler.Invoke(self, emoteType, frame, frameCounter);
                if (newValue != null)
                {
                    result = newValue;
                }
            }

            return result;
        }
    }
}
