using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tomat.Terraria.TML.SourceGenerator;

[Generator]
public sealed class GenerateModType : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.AnalyzerConfigOptionsProvider,
            static (x, options) =>
            {
                if (!options.GlobalOptions.TryGetValue("build_property.rootnamespace", out var rootNamespace))
                {
                    x.ReportDiagnostic(
                        Diagnostic.Create(
                            new DiagnosticDescriptor(
                                "SG0001",
                                "Failed to get root namespace",
                                "Property 'build_property.rootnamspace' wasn't found",
                                "SourceGenerator",
                                DiagnosticSeverity.Error,
                                true
                            ),
                            null
                        )
                    );
                    return;
                }

                x.AddSource(
                    "Mod.g.cs",
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
            sb.AppendLine("public sealed partial class Mod : global::Terraria.ModLoader.Mod;");
        }
        return sb.ToString();
    }
}