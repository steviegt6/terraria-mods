using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tomat.TML.Lib.Constructor.Common.Format._Calamity;

/// <summary>
///     Shared implementation for Calamity schematics.
/// </summary>
internal static class CalamitySchematicImpl
{
    public enum FormatVersion
    {
        Tml13,
        Tml14,
        Infernum14,
        Tml144,
    }

    private static readonly Dictionary<FormatVersion, byte[]> magic_headers = new()
    {
        { FormatVersion.Tml13, [0xCA, 0x1A, 0x5C] },
        { FormatVersion.Tml14, [0xCA, 0x14, 0x5C] },
        { FormatVersion.Infernum14, [0x1F, 0x14, 0x5C] },
        { FormatVersion.Tml144, [0xCA, 0x44, 0x5C] },
    };

    public static bool Accepts(Stream stream, FormatVersion version)
    {
        var header = magic_headers[version];
        var buffer = new byte[header.Length];

        return stream.Read(buffer, 0, buffer.Length) == buffer.Length
            && header.SequenceEqual(buffer);
    }
}