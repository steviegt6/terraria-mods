using System.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format._Calamity;

internal sealed class CalamityTml13 : IStructureFormat
{
    private const CalamitySchematicImpl.FormatVersion version = CalamitySchematicImpl.FormatVersion.Tml13;
    
    bool IStructureFormat.Accepts(Stream stream)
    {
        return CalamitySchematicImpl.Accepts(stream, version);
    }
}