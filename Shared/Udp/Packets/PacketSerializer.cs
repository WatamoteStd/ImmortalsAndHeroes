
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets;

public static class PacketSerialier
{
    

    public static int Serialize<T>(Span<byte> buffer, PacketTypes packetType, T data)
        where T : struct, INetworkPacket
    {
        
        int offset = 0;

        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)packetType);
        offset += 2;

        offset += data.Serialize(buffer[offset..]);
        
        return offset;

    }

    public static T Deserialize<T>(ReadOnlySpan<byte> buffer)
        where T : struct, INetworkPacket
    {
        
        var packet = new T();

        packet.Deserialize(buffer);
        return packet;

    }


}