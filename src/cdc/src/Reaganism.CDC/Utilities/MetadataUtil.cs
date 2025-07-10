using System.IO;
using System.Reflection.PortableExecutable;

using ICSharpCode.Decompiler.Metadata;

namespace Reaganism.CDC.Utilities;

internal static class MetadataUtil
{
    public static PEFile ReadModule(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        using var fs = File.OpenRead(path);
        {
            var module = new PEFile(path, fs, PEStreamOptions.PrefetchEntireImage);
            return module;
        }
    }
}