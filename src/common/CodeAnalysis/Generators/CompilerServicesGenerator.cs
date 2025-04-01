using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Tomat.Terraria.TML.SourceGenerator.Generators;

[Generator]
public sealed class IncludedFromAttributeGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.CompilationProvider,
            static (x, _) => x.AddSource(
                "CompilerServices.g.cs",
                SourceText.From(Generate(), Encoding.UTF8)
            )
        );
    }

    private static string Generate()
    {
        /*lang=c#*/
        return """
               using System;

               namespace System.Runtime.CompilerServices
               {
               }

               namespace Tomat.Runtime.CompilerServices
               {
                   [AttributeUsage(AttributeTargets.All)]
                   internal sealed class IncludedFromAttribute(string includeName, string includeVersion) : Attribute
                   {
                       public string IncludeName { get; } = includeName;
               
                       public string IncludeVersion { get; } = includeVersion;
                   }
               }
               """.Trim();
    }
}