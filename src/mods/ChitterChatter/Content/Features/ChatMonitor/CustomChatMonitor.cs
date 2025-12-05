using System;
using ChitterChatter.Common.Loading;
using ChitterChatter.Content.Features.ChatMonitor.Rooms;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader;

namespace ChitterChatter.Content.Features.ChatMonitor;

internal sealed class CustomChatMonitor : IChatMonitor
{
    private sealed class InstallCustomChatMonitor : IInitializer
    {
        private IChatMonitor? oldMonitor;

        void IInitializer.Load()
        {
            oldMonitor = Main.chatMonitor;
            Main.chatMonitor = new CustomChatMonitor();
        }

        void ILoadable.Unload()
        {
            Main.chatMonitor = oldMonitor ?? new RemadeChatMonitor();
        }
    }

    private readonly IChatRoom[] chatRooms;
    private readonly IChatRoom vanillaChatRoom;

    private int selectedChatRoom;

    public CustomChatMonitor()
    {
        chatRooms =
        [
            vanillaChatRoom = new VanillaChatRoom(),
        ];
    }

    public void DrawChat(bool drawingPlayerChat)
    {
        // TODO: Render our extra UI features here.

        GetSelectedChatRoom().RenderChat(drawingPlayerChat);
    }

    public void Clear()
    {
        GetSelectedChatRoom().ClearChat();
    }

    public void Update()
    {
        GetSelectedChatRoom().UpdateChat();
    }

    public void Offset(int linesOffset)
    {
        GetSelectedChatRoom().OffsetCurrentChat(linesOffset);
    }

    public void ResetOffset()
    {
        GetSelectedChatRoom().OffsetCurrentChat(0);
    }

    public void OnResolutionChange()
    {
        GetSelectedChatRoom().MarkChatDirty();
    }

    public IChatRoom GetSelectedChatRoom()
    {
        selectedChatRoom = Math.Clamp(selectedChatRoom, 0, chatRooms.Length - 1);
        return chatRooms[selectedChatRoom];
    }

    public void SetChatRoom(int index)
    {
        selectedChatRoom = index;
    }

    public void CycleChatRoom(int direction)
    {
        selectedChatRoom = (selectedChatRoom + direction + chatRooms.Length) % chatRooms.Length;
    }

#region NewText
    void IChatMonitor.NewText(string newText, byte r, byte g, byte b)
    {
        // TODO: Is it fine to send all of these to the vanilla chat room?
        vanillaChatRoom.AddMessage(newText, new Color(r, g, b));
    }

    void IChatMonitor.NewTextMultiline(string text, bool force, Color c, int widthLimit)
    {
        // TODO: Is it fine to send all of these to the vanilla chat room?
        vanillaChatRoom.AddMultilineMessage(text, c, widthLimit);
    }
#endregion
}
