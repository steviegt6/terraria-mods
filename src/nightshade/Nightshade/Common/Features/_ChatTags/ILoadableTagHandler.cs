using JetBrains.Annotations;

using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Nightshade.Common.Features;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
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