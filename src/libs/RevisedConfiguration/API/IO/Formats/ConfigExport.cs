using System;
using System.Collections.Generic;

namespace Tomat.TML.Lib.RevisedConfiguration.API.IO.Formats;

/// <summary>
///     A data export containing an intermediary representation of a config
///     (format-agnostic).
/// </summary>
/// <param name="ModVersion">The version of the mod, if present.</param>
/// <param name="Entries">
///     A mapping of config entries and their values by name.
/// </param>
public readonly record struct ConfigExport(Version? ModVersion, Dictionary<string, object> Entries);