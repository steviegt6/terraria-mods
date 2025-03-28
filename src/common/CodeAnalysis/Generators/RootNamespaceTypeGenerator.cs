using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tomat.Terraria.TML.SourceGenerator.Generators;

/// <summary>
///     Creates a type in a namespace matching the assembly name to work around
///     an arbitrary requirement by tModLoader.
/// </summary>
[Generator]
public sealed class RootNamespaceTypeGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.CompilationProvider,
            static (x, compilation) => x.AddSource(
                "InternalTypeInRootNamespace.g.cs",
                SourceText.From(MakeInternalType(GeneratorUtil.GetAssemblyName(compilation)), Encoding.UTF8)
            )
        );
    }

    private static string MakeInternalType(string assemblyName)
    {
        var sb = new StringBuilder();
        {
            sb.AppendLine($"namespace {assemblyName};");
            sb.AppendLine();
            sb.AppendLine("[global::System.Runtime.CompilerServices.CompilerGenerated]");
            sb.AppendLine("internal static class Exists;");
        }
        return sb.ToString();
    }
}