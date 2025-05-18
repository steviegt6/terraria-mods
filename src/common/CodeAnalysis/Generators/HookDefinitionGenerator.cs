using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tomat.Terraria.TML.SourceGenerator.Generators;

// https://github.com/dotnet/runtime/blob/e2ad5fcc1702105d9cb9c32802181976df1b97ba/src/libraries/System.Runtime.InteropServices/gen/LibraryImportGenerator/LibraryImportGenerator.cs

[Generator]
public sealed class HookDefinitionGenerator : IIncrementalGenerator
{
    // private static readonly NameSyntax hook_definition_attribute = 

    private const string hook_definition_attribute_full_name = "Daybreak.Common.Features.Hooks.HookDefinitionAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var attributedTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
            hook_definition_attribute_full_name,
            static (node, _) => node is ClassDeclarationSyntax,
            static (context, _) => context.TargetSymbol is ITypeSymbol typeSymbol
                ? new { Syntax = (ClassDeclarationSyntax)context.TargetNode, Symbol = typeSymbol }
                : null
        ).Where(static modelData => modelData is not null);
        
        context.Register
    }
}