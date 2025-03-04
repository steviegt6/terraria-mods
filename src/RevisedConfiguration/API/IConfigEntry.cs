using System;

using Terraria.Localization;

namespace Tomat.TML.Lib.RevisedConfiguration.API;

/// <summary>
///     An un-typed config entry with basic definitions for displaying and
///     handling.
/// </summary>
public interface IConfigEntry
{
#region Identity
    /// <summary>
    ///     A unique key to identify this entry with (paired with the
    ///     <see cref="Mod"/>).
    /// </summary>
    /// <remarks>
    ///     The key should <b>not</b> contain the mod name.  The key needs to
    ///     only be unique when compared against other keys in the same mod.
    /// </remarks>
    string UniqueKey { get; }

    /// <summary>
    ///     The mod this entry belongs to.  If the mod is <see langword="null"/>
    ///     then the entry is considered belonging to vanilla.
    /// </summary>
    Mod? Mod { get; }

    /// <summary>
    ///     This entry's side, which controls syncing and which version of the
    ///     config to display to.
    /// </summary>
    ConfigSide Side { get; }

    /// <summary>
    ///     The main category, derived from the first item in
    ///     <see cref="Categories"/>.
    /// </summary>
    string MainCategory => Categories[0];

    /// <summary>
    ///     The type this entry stores.
    /// </summary>
    Type ValueType { get; }
#endregion

#region Display
    /// <summary>
    ///     The display name of this config entry.
    /// </summary>
    LocalizedText DisplayName { get; }

    /// <summary>
    ///     The description of this entry.
    /// </summary>
    LocalizedText Description { get; }

    /// <summary>
    ///     The categories this entry belongs to.  The first category is the
    ///     &quot;main&quot; category to which this entry canonically belongs
    ///     to.
    /// </summary>
    /// <remarks>
    ///     There must be <b>at least 1</b> category.
    /// </remarks>
    ReadOnlySpan<string> Categories { get; }
#endregion
}

/// <summary>
///     A typed config entry containing typed values.
/// </summary>
/// <typeparam name="T">The type this entry stores.</typeparam>
public interface IConfigEntry<T> : IConfigEntry
{
    Type IConfigEntry.ValueType => typeof(T);

#region Values
    /// <summary>
    ///     The actual value being worked with.  Depending on the game context,
    ///     way either use <see cref="LocalValue"/> or <see cref="RemoteValue"/>
    ///     as its backing property.  Mutating this value will modify either
    ///     the local or remote value according to the context.
    /// </summary>
    T? Value { get; set; }

    /// <summary>
    ///     The local (client-sided) value.
    /// </summary>
    T? LocalValue { get; set; }

    /// <summary>
    ///     The remote (server-sided) value.
    /// </summary>
    T? RemoteValue { get; set; }

    /// <summary>
    ///     The default value.
    /// </summary>
    T? DefaultValue { get; }
#endregion
}