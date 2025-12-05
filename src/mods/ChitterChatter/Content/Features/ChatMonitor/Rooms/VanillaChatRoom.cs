using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

using ChitterChatter.Content.Features.TagHandlers;

namespace ChitterChatter.Content.Features.ChatMonitor.Rooms;

internal sealed class VanillaChatRoom : IChatRoom
{
    private sealed class ChatMessageContainer
    {
        public int LineCount => parsedText.Count;

        public bool CanBeShownWhenChatIsClosed => timeLeft > 0;

        public bool Prepared { get; private set; }

        public DateTime UtcTime { get; }

        private int timeLeft;

        private readonly List<TextSnippet[]> parsedText;
        private readonly string?             text;
        private readonly int                 widthLimitInPixels;
        private readonly Color               color;
        private readonly TextSnippet         modSourceSnippet;

        private static readonly TextSnippet space_snippet = new(" ");

        public ChatMessageContainer(
            string   text,
            Color    color,
            int      widthLimitInPixels,
            string?  modSource,
            DateTime utcTime
        )
        {
            this.text               = text;
            this.color              = color;
            this.widthLimitInPixels = widthLimitInPixels;
            parsedText              = [];
            timeLeft                = 600;
            modSourceSnippet        = ModIconTagHandler.CreateSnippet(modSource);
            this.UtcTime            = utcTime;

            MarkToNeedRefresh();
            Refresh();
        }

        public void MarkToNeedRefresh()
        {
            Prepared = false;
        }

        public void Update()
        {
            if (timeLeft > 0)
            {
                timeLeft--;
            }

            Refresh();
        }

        public void Refresh()
        {
            if (Prepared)
            {
                return;
            }

            Prepared = true;
            var width = widthLimitInPixels;
            if (width == -1)
            {
                width = Main.screenWidth - 320;
            }

            var font = FontAssets.MouseText.Value;

            width -= (int)modSourceSnippet.GetStringLength(font);
            width -= (int)space_snippet.GetStringLength(font);

            var lines = Utils.WordwrapStringSmart(text, color, font, width, 10);
            {
                parsedText.Clear();
            }

            foreach (var snippets in lines)
            {
                snippets.Insert(0, modSourceSnippet);
                snippets.Insert(1, space_snippet);
                parsedText.Add(snippets.ToArray());
            }
        }

        public TextSnippet[] GetSnippetWithInversedIndex(int snippetIndex)
        {
            return parsedText[parsedText.Count - 1 - snippetIndex];
        }
    }

    private static readonly Dictionary<Type, string?> mod_source_cache = [];

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

        messages.Insert(
            0,
            new ChatMessageContainer(
                text,
                textColor,
                maxWidthInPixels,
                GetModSource(),
                DateTime.UtcNow
            )
        );

        return;

        static string? GetModSource()
        {
            using (new Logging.QuietExceptionHandle())
            {
                try
                {
                    var stackFrames = new StackTrace().GetFrames();

                    var foundNewText    = false;
                    var postNewIndexIdx = -1;
                    foreach (var frame in stackFrames)
                    {
                        postNewIndexIdx++;

                        var methodName = frame.GetMethod()?.Name;
                        if (methodName is null)
                        {
                            continue;
                        }

                        // Main::NewText / IChatMonitor::NewText
                        // Main::NewTextMultiline / IChatMonitor::NewTextMultiline
                        if (methodName.Contains("NewText") || methodName.Contains("AddNewMessage"))
                        {
                            foundNewText = true;
                        }
                        else if (foundNewText)
                        {
                            if (postNewIndexIdx == stackFrames.Length)
                            {
                                return null;
                            }

                            break;
                        }
                    }

                    var declaringType = stackFrames[postNewIndexIdx].GetMethod()?.DeclaringType;
                    if (declaringType is null)
                    {
                        return "Terraria";
                    }

                    if (declaringType.Namespace is null)
                    {
                        return null;
                    }

                    if (declaringType.Namespace.StartsWith("Terraria"))
                    {
                        return "Terraria";
                    }

                    if (mod_source_cache.TryGetValue(declaringType, out var cached))
                    {
                        return cached;
                    }

                    return mod_source_cache[declaringType] = ModLoader.Mods.FirstOrDefault(x => x.Name != "ModLoader" && x.Code == declaringType.Assembly)?.Name ?? null;
                }
                catch
                {
                    return null;
                }
            }
        }
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

            int hoveredSnippet;
            {
                var yPos = Main.screenHeight - 30 - 28 - displayedLines * 21;
                var font = FontAssets.MouseText.Value;

                // Draw timestamp.
                {
                    // TODO: Format based on in-game language.
                    var timestamp = message.UtcTime.ToLocalTime().ToShortTimeString();

                    var tsWidth = font.MeasureString(timestamp).X;

                    ChatManager.DrawColorCodedStringWithShadow(
                        Main.spriteBatch,
                        font,
                        timestamp,
                        new Vector2(88f - tsWidth - 4f, yPos),
                        Color.DarkGray,
                        0f,
                        Vector2.Zero,
                        Vector2.One
                    );
                }

                // Draw main message.
                {
                    ChatManager.DrawColorCodedStringWithShadow(
                        Main.spriteBatch,
                        font,
                        snippets,
                        new Vector2(88f, yPos),
                        0f,
                        Vector2.Zero,
                        Vector2.One,
                        out hoveredSnippet
                    );
                }
            }

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