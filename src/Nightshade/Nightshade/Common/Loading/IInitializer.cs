using JetBrains.Annotations;

using Terraria.ModLoader;

namespace Nightshade.Common.Loading;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
public interface IInitializer : ILoadable
{
    void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
    {
        Load();
    }

    void ILoadable.Unload() { }

    void Load();
}