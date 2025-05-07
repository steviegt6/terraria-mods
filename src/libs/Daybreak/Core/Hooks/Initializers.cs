using JetBrains.Annotations;

using Terraria.ModLoader;

namespace Daybreak.Core.Hooks;

/// <summary>
///     The basic implementation of the DAYBREAK initializer APIs.  Implements
///     <see cref="ILoadable"/> and facilitates basic execution of the
///     <see cref="ILoad"/> and <see cref="IUnload"/> hooks if present.
/// </summary>
[UsedImplicitly(
    ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature,
    ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers
)]
public interface IInitializer : ILoadable
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

public interface ILoad : IInitializer
{
    void Load();
}

public interface IUnload : IInitializer
{
    new void Unload();
}