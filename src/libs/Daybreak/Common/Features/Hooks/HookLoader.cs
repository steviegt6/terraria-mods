using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Daybreak.Common.Features.Hooks;

internal static class HookLoader
{
#pragma warning disable CA2255
    [ModuleInitializer]
    public static void HookIntoContentLoadingRoutine()
    {
        // Hijack the content loading process to allow us to handle every added
        // loadable.  For every loadable, resolve instanced hooks and apply
        // them.
        MonoModHooks.Add(
            typeof(Mod).GetMethods().Single(x => x.Name.Equals(nameof(Mod.AddContent)) && !x.IsGenericMethod),
            AddContent_ResolveInstancedHooks
        );

        MonoModHooks.Add(
            typeof(Mod).GetMethod(nameof(Mod.Autoload), BindingFlags.NonPublic | BindingFlags.Instance)!,
            Autoload_ResolveStaticHooks
        );
    }
#pragma warning restore CA2255

    private static bool AddContent_ResolveInstancedHooks(Func<Mod, ILoadable, bool> orig, Mod self, ILoadable instance)
    {
        // Only attempt to resolve and apply hooks if the instance actually
        // loaded...
        if (!orig(self, instance))
        {
            return false;
        }

        ResolveInstancedHooks(instance);
        return true;
    }

    private static void Autoload_ResolveStaticHooks(Action<Mod> orig, Mod self)
    {
        orig(self);

        if (self.Code is null)
        {
            return;
        }

        var loadableTypes = AssemblyManager.GetLoadableTypes(self.Code)
                                           .OrderBy(x => x.FullName, StringComparer.InvariantCulture);

        LoaderUtils.ForEachAndAggregateExceptions(loadableTypes, ResolveStaticHooks);
    }

    private static void ResolveInstancedHooks(ILoadable instance)
    {
        SubscribeToHooks(
            instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
            instance
        );
    }

    private static void ResolveStaticHooks(Type type)
    {
        SubscribeToHooks(
            type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static),
            null
        );
    }

    private static void SubscribeToHooks(MethodInfo[] methods, object? instance)
    {
        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes(typeof(SubscribesToAttribute<>), true);
            if (attributes.Length == 0)
            {
                continue;
            }

            foreach (var attribute in attributes)
            {
                var hookType = attribute.GetType().GetGenericArguments()[0];
                Subscribe(hookType, method, instance);
            }
        }
    }

    private static void Subscribe(Type hookType, MethodInfo method, object? instance)
    {
        var eventInfo = hookType.GetEvent("Event");
        if (eventInfo is null)
        {
            throw new InvalidOperationException($"The type {hookType} does not have an event named Event.");
        }

        var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, instance, method);
        if (handler is null)
        {
            throw new InvalidOperationException($"The method {method} is not compatible with the event {eventInfo}.");
        }
        
        eventInfo.AddEventHandler(null, handler);
    }
}