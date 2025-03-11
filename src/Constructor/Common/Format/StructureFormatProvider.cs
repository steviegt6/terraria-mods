using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Tomat.TML.Lib.Constructor.Common.Format._Calamity;

namespace Tomat.TML.Lib.Constructor.Common.Format;

/// <summary>
///     Provides <see cref="IStructureFormat"/>s.
/// </summary>
public static class StructureFormatProvider
{
    private static readonly IStructureFormat[] formats =
    [
        // Structure Helper
        new StructureHelperV1(),
        new StructureHelperV2(),
        new StructureHelperV3(),

        // Calamity Mod (Calamity Schematic IO)
        new CalamityTml13(),
        new CalamityTml14(),
        new CalamityInfernum14(),
        new CalamityTml144(),
    ];

    /// <summary>
    ///     Attempts to find the most appropriate <paramref name="format"/> to
    ///     use for the data within the <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The stream to find a formatter for.</param>
    /// <param name="format">The resolved format.</param>
    /// <returns>Whether a format was found.</returns>
    public static bool TryFindBestFit(
        Stream                                             stream,
        [NotNullWhen(returnValue: true)] IStructureFormat? format
    )
    {
        Debug.Assert(stream.CanRead);
        Debug.Assert(stream.CanSeek);

        var startPos = stream.Position;

        try
        {
            foreach (var candidate in formats)
            {
                if (!candidate.Accepts(stream))
                {
                    stream.Position = startPos;
                    continue;
                }

                format = candidate;
                return true;
            }

            format = null;
            return false;
        }
        finally
        {
            stream.Position = startPos;
        }
    }
}