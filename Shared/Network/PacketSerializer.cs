using System;
using System.Buffers.Binary;
using Shared.Network.Packets;

namespace Shared.Network;

public static class PacketSerializer
{
    
    public static int Serialize<T>(Span<byte> buffer, PacketType type, T networkPacket)
        where T : struct, INetworkPacket
    {
        
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        
        networkPacket.Serialize(buffer[2..]);

        return 2 + networkPacket.Length;

    }

    public static T Deserialize<T>(ReadOnlySpan<byte> buffer)
        where T : struct, INetworkPacket
    {
        
        T packet = default;

        packet.Deserialize(buffer[2..]);

        return packet;

    }

}