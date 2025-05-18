using System;

using JetBrains.Annotations;

namespace Daybreak.Common.Features.Hooks;

/// <summary>
///     Decorating an interface with this attribute will enable a source
///     generator to automatically generate relevant members to make this type
///     compliant with the API expected by the hook loader.
///     <br />
///     The source generator expects a public delegate named <c>Definition</c>
///     to be defined.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
public sealed class HookDefinitionAttribute : Attribute;
