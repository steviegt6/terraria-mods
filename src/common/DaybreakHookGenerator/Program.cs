using System;
using System.IO;
using System.Linq;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Rocks;

using Terraria.ModLoader;

using ModuleDefinition = Mono.Cecil.ModuleDefinition;

namespace DaybreakHookGenerator;

internal static class Program
{
    private readonly record struct TypeHookDefinition(Type Type, string[] ExcludedHooks);

    private const string the_namespace = "Daybreak.Common.Features.Hooks";

    public static void Main()
    {
        // Run this from the repository root.
        var path = Path.Combine("src", "libs", "Daybreak", "Common", "Features", "Hooks", "_TML");

        // TODO: Can we include GlobalBlockType?
        // TODO: GlobalBuilderToggle
        // TODO: Mod hooks?
        var definitions = new[]
        {
            new TypeHookDefinition(typeof(GlobalBossBar), []),
            new TypeHookDefinition(typeof(GlobalBuff), []),
            new TypeHookDefinition(typeof(GlobalEmoteBubble), []),
            new TypeHookDefinition(typeof(GlobalInfoDisplay), []),
            new TypeHookDefinition(typeof(GlobalItem), []),
            new TypeHookDefinition(typeof(GlobalNPC), []),
            new TypeHookDefinition(typeof(GlobalProjectile), []),
            new TypeHookDefinition(typeof(GlobalPylon), []),
            new TypeHookDefinition(typeof(GlobalTile), []),
            new TypeHookDefinition(typeof(GlobalWall), []),
            new TypeHookDefinition(
                typeof(ModSystem),
                [
                    nameof(ModSystem.OnModLoad),
                    nameof(ModSystem.OnModUnload),
                    nameof(ModSystem.SetupContent),
                ]
            ),
            new TypeHookDefinition(typeof(ModPlayer), []),
        };

        var modDef = ModuleDefinition.ReadModule(typeof(ModLoader).Assembly.Location);

        Console.WriteLine("Requested generation for the following hook definitions:");
        foreach (var definition in definitions)
        {
            Console.WriteLine($"    {definition.Type.Name}");

            foreach (var exclusion in definition.ExcludedHooks)
            {
                Console.WriteLine("        excluded: " + exclusion);
            }
        }

        foreach (var definition in definitions)
        {
            GenerateHookDefinition(path, definition, modDef);
        }
    }

    private static void GenerateHookDefinition(string path, TypeHookDefinition definition, ModuleDefinition modDef)
    {
        Console.WriteLine("GENERATING HOOK DEFINITION FOR " + definition.Type.Name);

        var className = definition.Type.Name + "Hooks";
        var fileName = Path.Combine(path, className + ".cs");

        Console.WriteLine($"    {className} @ {fileName}");
        var contents = BuildType(definition, className, modDef);

        File.WriteAllText(fileName, contents);
    }

    private static string BuildType(TypeHookDefinition definition, string className, ModuleDefinition modDef)
    {
        var sb = new StringBuilder();

        var typeDef = modDef.GetType(definition.Type.FullName);

        var hooks = ResolveHooksFromType(typeDef, definition.ExcludedHooks);

        sb.AppendLine($"namespace {the_namespace};");
        sb.AppendLine();
        sb.AppendLine($"// Hooks to generate for '{typeDef.FullName}':");
        foreach (var hook in hooks)
        {
            sb.AppendLine($"//     {hook}");
        }
        sb.AppendLine($"public static partial class {className}");
        sb.AppendLine("{");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string BuildHook()
    {
        return "";
    }

    private static MethodDefinition[] ResolveHooksFromType(TypeDefinition typeDef, string[] excludedHooks)
    {
        var methods = typeDef.GetMethods().Where(
            x => x is
            {
                IsPublic: true, // Is accessible (ignore protected, too)
                IsVirtual: true, // Is overridable
                IsFinal: false, // Is not sealed
            } && !excludedHooks.Contains(x.Name)
        );

        return methods.ToArray();
    }
}