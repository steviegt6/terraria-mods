using System.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format;

/// <summary>
///     A structure format.
/// </summary>
public interface IStructureFormat
{
    bool Accepts(Stream stream);
}