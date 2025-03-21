using Terraria.UI.Chat;

namespace Tomat.TML.Mod.ChitterChatter.Common.Loading;

public interface ILoadableTagHandler<TSelf> : ITagHandler, IInitializer
    where TSelf : ILoadableTagHandler<TSelf>, new()
{
    string[] TagNames { get; }

    void IInitializer.Load()
    {
        ChatManager.Register<TSelf>(TagNames);
    }
}