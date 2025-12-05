using JetBrains.Annotations;

using Terraria.ModLoader;

namespace ChitterChatter.Common.Loading;

/// <summary>
///     Provides a default implementation for loading and unloading, exposing
///     instead a simplified <see cref="Load"/> entrypoint.
/// </summary>
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