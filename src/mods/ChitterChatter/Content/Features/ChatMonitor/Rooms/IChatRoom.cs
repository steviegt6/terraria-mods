using Microsoft.Xna.Framework;

namespace ChitterChatter.Content.Features.ChatMonitor.Rooms;

/// <summary>
///     A chat room within a <see cref="CustomChatMonitor" />.
/// </summary>
public interface IChatRoom
{
    /// <summary>
    ///     Adds a message to this chat room.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="textColor">The base text color.</param>
    void AddMessage(string text, Color textColor);

    /// <summary>
    ///     Adds a multi-line message to this chat room.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="textColor">The base text color.</param>
    /// <param name="maxWidthInPixels">
    ///     The maximum width of a given line in pixels.
    /// </param>
    void AddMultilineMessage(string text, Color textColor, int maxWidthInPixels);

    /// <summary>
    ///     Renders this chat room.
    /// </summary>
    /// <param name="extendedChatWindow">
    ///     Whether to show an extended window including messages whose regular
    ///     display time may have expired.
    /// </param>
    void RenderChat(bool extendedChatWindow);

    /// <summary>
    ///     Clears the messages of this chat room.
    /// </summary>
    void ClearChat();

    /// <summary>
    ///     Updates the chat room.
    /// </summary>
    void UpdateChat();

    /// <summary>
    ///     Sets the offset for the current chat, allowing for scrolling.
    /// </summary>
    /// <param name="linesOffset">The line offset.</param>
    void OffsetCurrentChat(int linesOffset);

    /// <summary>
    ///     Marks the chat as dirty, forcing a recalculation.
    /// </summary>
    void MarkChatDirty();
}
