using System;
using System.Buffers.Binary;

namespace Shared.Network.Packets;

public struct C2S_HandshakePacket : INetworkPacket
{
    public long PlayerId;
    public int Length => 8;

    public void Serialize(Span<byte> buffer)
    {
        
        BinaryPrimitives.WriteInt64LittleEndian(buffer, PlayerId);

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        PlayerId = BinaryPrimitives.ReadInt64LittleEndian(buffer);

    }

}
