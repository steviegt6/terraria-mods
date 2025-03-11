using System.IO;

using Terraria.ModLoader.IO;

namespace Tomat.TML.Lib.Constructor.Common.Format;

internal sealed class StructureHelperV2 : IStructureFormat
{
    private readonly record struct TileData(
        string       TileTypeName,
        string       WallTypeName,
        short        TileFrameX,
        short        TileFrameY,
        int          WallWireData,
        short        LiquidData,
        byte         BrightInvisibleData,
        string?      TileEntityTypeName,
        TagCompound? TileEntityData
    );

    bool IStructureFormat.Accepts(Stream stream)
    {
        return false;
    }
}