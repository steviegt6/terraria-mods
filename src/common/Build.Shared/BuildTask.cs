using JetBrains.Annotations;

namespace Build.Shared;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithInheritors)]
public abstract class BuildTask
{
    public abstract void Run(ProjectContext ctx);
}