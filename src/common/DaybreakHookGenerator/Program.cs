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

        var typeDef = modDef.GetType(definition.Type.FullName);
        var generator = new Generator(modDef, typeDef);
        var contents = generator.BuildType(the_namespace, className, definition.ExcludedHooks);

        File.WriteAllText(fileName, contents);
    }
}