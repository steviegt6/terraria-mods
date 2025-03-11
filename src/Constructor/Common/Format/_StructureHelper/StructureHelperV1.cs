using System.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format;

internal sealed class StructureHelperV1 : IStructureFormat
{
    bool IStructureFormat.Accepts(Stream stream)
    {
        return false;
    }
}