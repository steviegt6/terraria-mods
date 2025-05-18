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
            new TypeHookDefinition(typeof(GlobalBossBar))
               .WithInvokeStrategy(nameof(GlobalBossBar.PreDraw), new BoolCombinerStrategy(true, "&=")),
            new TypeHookDefinition(typeof(GlobalBuff))
               .WithInvokeStrategy("ReApply_int_Player_int_int", new EarlyReturnOnTrueStrategy())
               .WithInvokeStrategy("ReApply_int_NPC_int_int", new EarlyReturnOnTrueStrategy())
               .WithInvokeStrategy(nameof(GlobalBuff.PreDraw), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalBuff.RightClick), new BoolCombinerStrategy(true, "&=")),
            new TypeHookDefinition(typeof(GlobalEmoteBubble))
               .WithInvokeStrategy(nameof(GlobalEmoteBubble.UpdateFrame), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalEmoteBubble.UpdateFrameInEmoteMenu), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalEmoteBubble.PreDraw), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalEmoteBubble.PreDrawInEmoteMenu), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalEmoteBubble.GetFrame), new NullableValueMayBeOverriddenStrategy("Microsoft.Xna.Framework.Rectangle"))
               .WithInvokeStrategy(nameof(GlobalEmoteBubble.GetFrameInEmoteMenu), new NullableValueMayBeOverriddenStrategy("Microsoft.Xna.Framework.Rectangle")),
            new TypeHookDefinition(typeof(GlobalInfoDisplay))
               .WithInvokeStrategy(nameof(GlobalInfoDisplay.Active), new NullableBooleanCombinerStrategy()),
            new TypeHookDefinition(typeof(GlobalItem)),
            new TypeHookDefinition(typeof(GlobalNPC)),
            new TypeHookDefinition(typeof(GlobalProjectile))
               .WithExclusions(
                    nameof(GlobalProjectile.CanCutTiles),
                    nameof(GlobalProjectile.CanDamage),
                    nameof(GlobalProjectile.CanHitNPC),
                    nameof(GlobalProjectile.CanHitPvp),
                    nameof(GlobalProjectile.CanHitPlayer),
                    nameof(GlobalProjectile.Colliding),
                    nameof(GlobalProjectile.GetAlpha),
                    nameof(GlobalProjectile.CanUseGrapple),
                    nameof(GlobalProjectile.GrappleCanLatchOnTo)
                ) // lazy
               .WithInvokeStrategy(nameof(GlobalProjectile.PreAI), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalProjectile.ShouldUpdatePosition), new EarlyReturnOnFalseStrategy())
               .WithInvokeStrategy(nameof(GlobalProjectile.TileCollideStyle), new EarlyReturnOnFalseStrategy())
               .WithInvokeStrategy(nameof(GlobalProjectile.OnTileCollide), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalProjectile.PreKill), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalProjectile.MinionContactDamage), new EarlyReturnOnTrueStrategy())
               .WithInvokeStrategy(nameof(GlobalProjectile.PreDrawExtras), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalProjectile.PreDraw), new BoolCombinerStrategy(true, "&=")),
            new TypeHookDefinition(typeof(GlobalPylon))
               .WithInvokeStrategy(nameof(GlobalPylon.PreDrawMapIcon), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalPylon.PreCanPlacePylon), new NullableBooleanEarlyReturnStrategy())
               .WithInvokeStrategy(nameof(GlobalPylon.ValidTeleportCheck_PreNPCCount), new NullableBooleanEarlyReturnStrategy())
               .WithInvokeStrategy(nameof(GlobalPylon.ValidTeleportCheck_PreAnyDanger), new NullableBooleanEarlyReturnStrategy())
               .WithInvokeStrategy(nameof(GlobalPylon.ValidTeleportCheck_PreBiomeRequirements), new NullableBooleanEarlyReturnStrategy()),
            new TypeHookDefinition(typeof(GlobalTile))
               .WithExclusions(nameof(GlobalTile.AutoSelect), nameof(GlobalTile.Slope)) // lazy
               .WithInvokeStrategy(nameof(GlobalTile.CanDrop), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalTile.CanKillTile), new EarlyReturnOnFalseStrategy())
               .WithInvokeStrategy(nameof(GlobalTile.IsTileDangerous), new NullableBooleanCombinerStrategy())
               .WithInvokeStrategy(nameof(GlobalTile.IsTileBiomeSightable), new NullableBooleanCombinerStrategy())
               .WithInvokeStrategy(nameof(GlobalTile.IsTileSpelunkable), new NullableBooleanCombinerStrategy())
               .WithInvokeStrategy(nameof(GlobalTile.TileFrame), new BoolCombinerStrategy(true, "&="))
               .WithInvokeStrategy(nameof(GlobalTile.AdjTiles), new ArrayCombinerStrategy("int"))
               .WithInvokeStrategy(nameof(GlobalTile.PreHitWire), new EarlyReturnOnFalseStrategy())
               .WithInvokeStrategy(nameof(GlobalTile.CanReplace), new EarlyReturnOnFalseStrategy())
               .WithInvokeStrategy(nameof(GlobalTile.ShakeTree), new EarlyReturnOnTrueStrategy()),
            new TypeHookDefinition(typeof(GlobalWall))
               .WithInvokeStrategy(nameof(GlobalWall.Drop), new EarlyReturnOnFalseStrategy())
               .WithInvokeStrategy(nameof(GlobalWall.WallFrame), new EarlyReturnOnFalseStrategy()),
            new TypeHookDefinition(typeof(ModSystem))
               .WithExclusions(
                    nameof(ModSystem.OnModLoad),
                    nameof(ModSystem.OnModUnload),
                    nameof(ModSystem.SetupContent),
                    nameof(ModSystem.CanWorldBePlayed),
                    nameof(ModSystem.WorldCanBePlayedRejectionMessage),
                    nameof(ModSystem.HijackGetData),
                    nameof(ModSystem.HijackSendData)
                )
            /*.WithInvokeStrategy(nameof(ModSystem.CanWorldBePlayed), new EarlyReturnOnFalseStrategy())*/,
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