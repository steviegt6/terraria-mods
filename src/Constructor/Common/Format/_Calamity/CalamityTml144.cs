using System.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format._Calamity;

internal sealed class CalamityTml144 : IStructureFormat
{
    private const CalamitySchematicImpl.FormatVersion version = CalamitySchematicImpl.FormatVersion.Tml144;

    bool IStructureFormat.Accepts(Stream stream)
    {
        return CalamitySchematicImpl.Accepts(stream, version);
    }
}