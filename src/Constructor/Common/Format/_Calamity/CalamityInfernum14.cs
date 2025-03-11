using System.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format._Calamity;

internal sealed class CalamityInfernum14 : IStructureFormat
{
    private const CalamitySchematicImpl.FormatVersion version = CalamitySchematicImpl.FormatVersion.Infernum14;

    bool IStructureFormat.Accepts(Stream stream)
    {
        return CalamitySchematicImpl.Accepts(stream, version);
    }
}