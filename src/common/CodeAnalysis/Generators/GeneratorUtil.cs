using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tomat.Terraria.TML.SourceGenerator.Generators;

internal static class GeneratorUtil
{
    public static string GetAssemblyName(Compilation compilation)
    {
        return compilation.AssemblyName ?? compilation.Assembly.Name;
    }

    public static string? GetRootNamespaceOrRaiseDiagnostic(SourceProductionContext ctx, AnalyzerConfigOptions options)
    {
        if (options.TryGetValue("build_property.rootnamespace", out var rootNamespace))
        {
            return rootNamespace;
        }
        
        ctx.ReportDiagnostic(
            Diagnostic.Create(
                new DiagnosticDescriptor(
                    "SG0001",
                    "Failed to get root namespace",
                    "Property 'build_property.rootnamespace' wasn't found",
                    "CodeAnalysis",
                    DiagnosticSeverity.Error,
                    true
                ),
                null
            )
        );
        return null;

    }
}