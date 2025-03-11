using System.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format;

internal sealed class StructureHelperV3 : IStructureFormat
{
    bool IStructureFormat.Accepts(Stream stream)
    {
        return false;
    }
}