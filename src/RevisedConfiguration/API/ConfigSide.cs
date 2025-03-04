using Terraria.ModLoader;

namespace Tomat.TML.Lib.RevisedConfiguration.API;

/// <summary>
///     The scope for a config entry.
/// </summary>
/// <remarks>
///     The options and their functionality currently more or less mirrors that
///     of <see cref="ModSide"/>.
/// </remarks>
public enum ConfigSide
{
    /// <summary>
    ///     The entry is only client-side and does not need syncing across the
    ///     boundary.
    /// </summary>
    ClientSide,

    /// <summary>
    ///     The entry is only server-side and does not need syncing across the
    ///     boundary.
    /// </summary>
    ServerSide,

    /// <summary>
    ///     The entry applies to both the client and server and needs syncing
    ///     across the boundary.
    /// </summary>
    Both,

    /// <summary>
    ///     The entry applies to both the client and server but does not need
    ///     syncing across the boundary.
    /// </summary>
    /// <remarks>
    ///     This is not particularly useful in most cases, but is important for
    ///     mods that are also no-sync.
    /// </remarks>
    NoSync,
}