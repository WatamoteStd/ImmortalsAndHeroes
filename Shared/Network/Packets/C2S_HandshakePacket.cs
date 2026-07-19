using System;
using System.Buffers.Binary;

namespace Shared.Network.Packets;

public struct C2S_HandshakePacket : INetworkPacket
{
    public long Ticket;
    public int Length => 8;

    public void Serialize(Span<byte> buffer)
    {
        
        BinaryPrimitives.WriteInt64LittleEndian(buffer, Ticket);

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Ticket = BinaryPrimitives.ReadInt64LittleEndian(buffer);

    }

}
