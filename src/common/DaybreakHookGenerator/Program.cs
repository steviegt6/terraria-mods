using System;
using System.Collections.Generic;
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
    private class TypeHookDefinition(Type type)
    {
        public Type Type { get; } = type;

        public List<string> Exclusions { get; } = [];

        public Dictionary<string, InvokeStrategy> InvokeStrategies { get; } = [];

        public TypeHookDefinition WithExclusions(params string[] exclusions)
        {
            Exclusions.AddRange(exclusions);
            return this;
        }

        public TypeHookDefinition WithInvokeStrategy(string methodName, InvokeStrategy strategy)
        {
            InvokeStrategies[methodName] = strategy;
            return this;
        }
    }

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
            new TypeHookDefinition(typeof(GlobalBossBar)),
            new TypeHookDefinition(typeof(GlobalBuff)),
            new TypeHookDefinition(typeof(GlobalEmoteBubble)),
            new TypeHookDefinition(typeof(GlobalInfoDisplay)),
            new TypeHookDefinition(typeof(GlobalItem)),
            new TypeHookDefinition(typeof(GlobalNPC)),
            new TypeHookDefinition(typeof(GlobalProjectile)),
            new TypeHookDefinition(typeof(GlobalPylon)),
            new TypeHookDefinition(typeof(GlobalTile)),
            new TypeHookDefinition(typeof(GlobalWall)),
            new TypeHookDefinition(typeof(ModSystem))
               .WithExclusions(
                    nameof(ModSystem.OnModLoad),
                    nameof(ModSystem.OnModUnload),
                    nameof(ModSystem.SetupContent)
                ),
            new TypeHookDefinition(typeof(ModPlayer)),
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
        var contents = generator.BuildType(the_namespace, className, definition.Exclusions, definition.InvokeStrategies);

        File.WriteAllText(fileName, contents);
    }
}