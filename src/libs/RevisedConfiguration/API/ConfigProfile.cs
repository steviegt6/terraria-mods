using System.Collections.Generic;

namespace Tomat.TML.Lib.RevisedConfiguration.API;

public sealed class ConfigProfile
{
    public IConfigEntry[] Entries { get; }

    public Dictionary<string, IConfigEntry> EntriesByKey { get; }
    
    public Dictionary<string, >
}