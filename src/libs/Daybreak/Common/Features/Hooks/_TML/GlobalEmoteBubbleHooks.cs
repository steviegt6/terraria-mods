namespace Daybreak.Common.Features.Hooks;

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
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        );
    }

    public static partial class UpdateFrame
    {
        public delegate bool Definition(
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        );
    }

    public static partial class UpdateFrameInEmoteMenu
    {
        public delegate bool Definition(
            int emoteType,
            ref int frameCounter
        );
    }

    public static partial class PreDraw
    {
        public delegate bool Definition(
            Terraria.GameContent.UI.EmoteBubble emoteBubble,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Graphics.Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Terraria.GameContent.UI.EmoteBubble emoteBubble,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Graphics.Texture2D texture,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin,
            Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );
    }

    public static partial class PreDrawInEmoteMenu
    {
        public delegate bool Definition(
            int emoteType,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.GameContent.UI.Elements.EmoteButton uiEmoteButton,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin
        );
    }

    public static partial class PostDrawInEmoteMenu
    {
        public delegate void Definition(
            int emoteType,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Terraria.GameContent.UI.Elements.EmoteButton uiEmoteButton,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Vector2 origin
        );
    }

    public static partial class GetFrame
    {
        public delegate Microsoft.Xna.Framework.Rectangle? Definition(
            Terraria.GameContent.UI.EmoteBubble emoteBubble
        );
    }

    public static partial class GetFrameInEmoteMenu
    {
        public delegate Microsoft.Xna.Framework.Rectangle? Definition(
            int emoteType,
            int frame,
            int frameCounter
        );
    }

}
