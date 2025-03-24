using Terraria.UI.Chat;

namespace Tomat.TML.Mod.Nightshade.Common.Features.ChatTags;

public interface ILoadableTagHandler<TSelf> : ITagHandler, IInitializer
    where TSelf : ILoadableTagHandler<TSelf>, new()
{
    string[] TagNames { get; }

    void IInitializer.Load()
    {
        ChatManager.Register<TSelf>(TagNames);
    }
}