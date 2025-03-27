using System;

using Terraria.Localization;

namespace Tomat.TML.Lib.RevisedConfiguration.API;

/// <summary>
///     The default implementation of a config entry.
/// </summary>
/// <typeparam name="T">The type this entry stores.</typeparam>
public class ConfigEntry<T> : IConfigEntry<T>
{
#region Identity
    public string UniqueKey { get; }

    public Mod? Mod { get; }

    public ConfigSide Side { get; }
#endregion

#region Display
    public LocalizedText DisplayName { get; }

    public LocalizedText Description { get; }

    public ReadOnlySpan<string> Categories { get; }
#endregion

#region Values
    public T? Value { get; set; }

    public T? LocalValue { get; set; }

    public T? RemoteValue { get; set; }

    public T? DefaultValue { get; }
#endregion
}