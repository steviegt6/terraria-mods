using Daybreak.Core.Hooks;

using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Nightshade.Common.Features;

internal interface ILoadableTagHandler<TSelf> : ITagHandler, ILoadable
    where TSelf : ILoadableTagHandler<TSelf>, new()
{
    string[] TagNames { get; }

    void ILoadable.Load(Mod mod)
    {
        ChatManager.Register<TSelf>(TagNames);
    }

    void ILoadable.Unload() { }
}