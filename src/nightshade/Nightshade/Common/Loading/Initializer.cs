using JetBrains.Annotations;

using Terraria.ModLoader;

namespace Nightshade.Common.Loading;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
internal interface IInitializer : ILoadable
{
    void ILoadable.Load(Mod mod)
    {
        (this as ILoad)?.Load();
    }

    void ILoadable.Unload()
    {
        (this as IUnload)?.Unload();
    }
}

internal interface ILoad : IInitializer
{
    void Load();
}

internal interface IUnload : IInitializer
{
    new void Unload();
}