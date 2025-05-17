using System;
using System.Reflection;

using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Daybreak.Common.Features.Hooks;

/// <summary>
///     Automatically subscribes the decorated method to the hook of type
///     <typeparamref name="T"/> if applicable.
///     <br />
///     If this decorates an instance method, the hook will be subscribed when
///     <see cref="Mod.AddContent"/> is called on the instance.  This means
///     instance hooks can only automatically be subscribed if the parent class
///     is an <see cref="ILoadable"/> and the instance has actually been loaded.
///     <br />
///     If this decorates a static method, the hook will be subscribed so long
///     as the parent type is visible under
///     <see cref="AssemblyManager.GetLoadableTypes(Assembly)"/> and the type
///     does not have any generic parameters (technical limitation).
/// </summary>
/// <typeparam name="T">The hook type to subscribe the method to.</typeparam>
[AttributeUsage(AttributeTargets.Method)]
public sealed class HookAttribute<T> : Attribute;