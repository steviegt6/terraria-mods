using System.IO;

using Nightshade.Common.Features;

using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace Nightshade;

partial class ModImpl
{
    internal const int PACKET_CURSOR_SLOTS = 0;

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        base.HandlePacket(reader, whoAmI);

        var packetKind = reader.ReadByte();
        switch (packetKind)
        {
            case PACKET_CURSOR_SLOTS:
                VanityCursorPlayer.NetHandler.HandlePacket(reader, whoAmI);
                break;
        }
    }

    internal static ModPacket GetPacket(byte packetType)
    {
        var packet = ModContent.GetInstance<ModLoaderMod>().GetPacket();
        {
            packet.Write(packetType);
        }
        return packet;
    }
}