using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tomat.Terraria.TML.SourceGenerator.Generators;

[Generator]
public sealed class GlobalUsingsGenerator : IIncrementalGenerator
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
                    "GlobalUsings.g.cs",
                    SourceText.From(GenerateGlobalUsings(rootNamespace), Encoding.UTF8)
                );
            }
        );
    }

    private static string GenerateGlobalUsings(string rootNamespace)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"global using static {rootNamespace}.Core.AssetReferences;");
        sb.AppendLine($"global using static {rootNamespace}.Core.LocalizationReferences;");

        return sb.ToString();
    }
}