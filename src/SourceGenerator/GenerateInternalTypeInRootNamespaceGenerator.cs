using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tomat.Terraria.TML.SourceGenerator;

[Generator]
public sealed class GenerateInternalTypeInRootNamespaceGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.CompilationProvider,
            static (x, compilation) => x.AddSource(
                "InternalTypeInRootNamespace.g.cs",
                SourceText.From(MakeInternalType(compilation.AssemblyName ?? compilation.Assembly.Name), Encoding.UTF8)
            )
        );
    }

    private static string MakeInternalType(string rootNamespace)
    {
        var sb = new StringBuilder();
        {
            sb.AppendLine($"namespace {rootNamespace};");
            sb.AppendLine($"");
            sb.AppendLine($"[System.Runtime.CompilerServices.CompilerGenerated]");
            sb.AppendLine($"internal static class Exists;");
        }
        return sb.ToString();
    }
}