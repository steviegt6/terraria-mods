using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tomat.TML.Lib.RevisedConfiguration.API.IO.Formats;

/// <summary>
///     A config format which supports reading and writing to a file kind.
/// </summary>
/// <param name="Extension">The file extension of this format.</param>
/// <param name="Read">Reads a config from the stream.</param>
/// <param name="Write">Writes a config to the stream.</param>
public readonly record struct ConfigFormat(
    string                   Extension,
    ConfigFormat.ReadConfig  Read,
    ConfigFormat.WriteConfig Write
)
{
    private const string metadata_section = "__metadata";
    private const string mod_version      = "modVersion";

    public static readonly ConfigFormat NBT = new(
        ".nbt",
        (stream, out ConfigExport export) => { },
        (stream, in  ConfigExport export) => { }
    );

    public static readonly ConfigFormat JSON = new(
        ".json",
        (stream, out ConfigExport export) =>
        {
            using var sr = new StreamReader(stream);
            using var jr = new JsonTextReader(sr);

            try
            {
                var obj = JObject.Load(jr);

                if (!obj.TryGetValue(metadata_section, out var meta)
                 || meta is not JObject metaObj
                 || !metaObj.TryGetValue(mod_version, out var modVersion)
                 || modVersion is not JValue { Value: string modVersionString }
                 || !Version.TryParse(modVersionString, out var version))
                {
                    // TODO: log version resolution failure?
                }

                var hadErrors = false;

                foreach (var (key, value) in obj)
                {
                    
                }
            }
            catch
            {
                export = default(ConfigExport);
                return (ResultKind.Error, Result.ErrorFileBroken);
            }
        },
        (stream, in ConfigExport export) => { }
    );

    /// <summary>
    ///     Delegate definition for the <see cref="Read"/> callback.
    /// </summary>
    public delegate (ResultKind Kind, Result Result) ReadConfig(Stream stream, out ConfigExport export);

    /// <summary>
    ///     Delegate definition for the <see cref="Write"/> callback.
    /// </summary>
    public delegate (ResultKind Kind, Result Result) WriteConfig(Stream stream, in ConfigExport export);
}