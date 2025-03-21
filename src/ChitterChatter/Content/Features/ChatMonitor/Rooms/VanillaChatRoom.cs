using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace Tomat.TML.Mod.ChitterChatter.Content.Features.ChatMonitor.Rooms;

internal sealed class VanillaChatRoom : IChatRoom
{
    private int messagesToShow = 10;
    private int startMessageIdx;

    private bool isDirty;

    private readonly List<ChatMessageContainer> messages = [];

    public void AddMessage(string text, Color textColor)
    {
        AddMultilineMessage(text, textColor, -1);
    }

    public void AddMultilineMessage(string text, Color textColor, int maxWidthInPixels)
    {
        if (Main.dedServ)
        {
            return;
        }

        var message = new ChatMessageContainer();
        {
            message.SetContents(text, textColor, maxWidthInPixels);
        }

        messages.Insert(0, message);
    }

    public void RenderChat(bool extendedChatWindow)
    {
        var remainingLines = startMessageIdx;
        var messageIndex   = 0;
        var lineOffset     = 0;

        while (remainingLines > 0 && messageIndex < messages.Count)
        {
            var linesToProcess = Math.Min(remainingLines, messages[messageIndex].LineCount);
            {
                remainingLines -= linesToProcess;
                lineOffset     =  linesToProcess;
            }

            if (lineOffset != messages[messageIndex].LineCount)
            {
                continue;
            }

            lineOffset = 0;
            messageIndex++;
        }

        var displayedLines      = 0;
        var hoveredMessageIndex = default(int?);
        var snippetIndex        = -1;
        var hoveredSnippetIndex = default(int?);

        while (displayedLines < messagesToShow && messageIndex < messages.Count)
        {
            var message = messages[messageIndex];

            if (!message.Prepared || !(extendedChatWindow || message.CanBeShownWhenChatIsClosed))
            {
                break;
            }

            var snippets = message.GetSnippetWithInversedIndex(lineOffset);

            ChatManager.DrawColorCodedStringWithShadow(
                Main.spriteBatch,
                FontAssets.MouseText.Value,
                snippets,
                new Vector2(88f, Main.screenHeight - 30 - 28 - displayedLines * 21),
                0f,
                Vector2.Zero,
                Vector2.One,
                out var hoveredSnippet
            );

            if (hoveredSnippet >= 0)
            {
                hoveredSnippetIndex = hoveredSnippet;
                hoveredMessageIndex = messageIndex;
                snippetIndex        = lineOffset;
            }

            displayedLines++;
            lineOffset++;

            if (lineOffset < message.LineCount)
            {
                continue;
            }

            lineOffset = 0;
            messageIndex++;
        }

        if (!hoveredMessageIndex.HasValue || !hoveredSnippetIndex.HasValue)
        {
            return;
        }

        {
            var snippets = messages[hoveredMessageIndex.Value].GetSnippetWithInversedIndex(snippetIndex);
            var snippet  = snippets[hoveredSnippetIndex.Value];
            {
                snippet.OnHover();
            }

            if (Main.mouseLeft && Main.mouseLeftRelease)
            {
                snippet.OnClick();
            }
        }
    }

    public void ClearChat()
    {
        // TODO: Find a way to preserve history?
        messages.Clear();
    }

    public void UpdateChat()
    {
        if (isDirty)
        {
            isDirty = false;

            foreach (var message in messages)
            {
                message.MarkToNeedRefresh();
            }
        }

        foreach (var message in messages)
        {
            message.Update();
        }
    }

    public void OffsetCurrentChat(int linesOffset)
    {
        startMessageIdx += linesOffset;
        ClampMessageIndex();

        return;

        void ClampMessageIndex()
        {
            var totalProcessedLines = 0;
            var messageIndex        = 0;
            var lineOffset          = 0;
            var targetLineCount     = startMessageIdx + messagesToShow;

            while (totalProcessedLines < targetLineCount && messageIndex < messages.Count)
            {
                var linesToProcess = Math.Min(targetLineCount - totalProcessedLines, messages[messageIndex].LineCount);
                totalProcessedLines += linesToProcess;

                if (totalProcessedLines < targetLineCount)
                {
                    messageIndex++;
                    lineOffset = 0;
                }
                else
                {
                    lineOffset = linesToProcess;
                }
            }

            var remainingLines = messagesToShow;
            while (remainingLines > 0 && totalProcessedLines > 0)
            {
                lineOffset--;
                remainingLines--;
                totalProcessedLines--;

                if (lineOffset >= 0)
                {
                    continue;
                }

                messageIndex--;

                if (messageIndex == -1)
                {
                    break;
                }

                lineOffset = messages[messageIndex].LineCount - 1;
            }

            startMessageIdx = totalProcessedLines;
        }
    }

    public void MarkChatDirty()
    {
        isDirty = true;
    }
}