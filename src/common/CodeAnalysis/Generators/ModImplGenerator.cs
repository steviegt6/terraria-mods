using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tomat.Terraria.TML.SourceGenerator.Generators;

/// <summary>
///     Generates a default implementation of <c>Terraria.ModLoader.Mod</c>
///     named <c>ModImpl</c> which is defined as partial for additional
///     extensions.  This exists for consistencies with extending the mod
///     implementation by other source generators and because using APIs
///     directly exposed by the <c>Mod</c> type should be discouraged in favor
///     of alternatives such as <c>ModSystem</c>.
/// </summary>
[Generator]
public sealed class ModImplGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.AnalyzerConfigOptionsProvider,
            static (x, options) =>
            {
                if (GeneratorUtil.GetRootNamespaceOrRaiseDiagnostic(x, options.GlobalOptions) is not { } rootNamespace)
                {
                    return;
                }

                x.AddSource(
                    "ModImpl.g.cs",
                    SourceText.From(MakeModType(rootNamespace), Encoding.UTF8)
                );
            }
        );
    }

    private static string MakeModType(string rootNamespace)
    {
        var sb = new StringBuilder();
        {
            sb.AppendLine($"namespace {rootNamespace};");
            sb.AppendLine();
            sb.AppendLine("[global::JetBrains.Annotations.UsedImplicitly(global::JetBrains.Annotations.ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]");
            sb.AppendLine("[global::System.Runtime.CompilerServices.CompilerGenerated]");
            sb.AppendLine("public sealed partial class ModImpl : global::Terraria.ModLoader.Mod;");
        }
        return sb.ToString();
    }
}