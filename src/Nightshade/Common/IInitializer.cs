using JetBrains.Annotations;

using Terraria.ModLoader;

namespace Tomat.TML.Mod.Nightshade.Common;

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