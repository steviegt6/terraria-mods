using Nightshade.Common.Loading;

using Terraria.UI.Chat;

namespace Nightshade.Common.Features;

internal interface ILoadableTagHandler<TSelf> : ITagHandler, ILoad
    where TSelf : ILoadableTagHandler<TSelf>, new()
{
    string[] TagNames { get; }

    void ILoad.Load()
    {
        ChatManager.Register<TSelf>(TagNames);
    }
}