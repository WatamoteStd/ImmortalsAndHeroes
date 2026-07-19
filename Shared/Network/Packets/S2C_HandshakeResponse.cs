using System;
using System.Buffers.Binary;

namespace Shared.Network.Packets;

public struct S2C_HandshakeResponse : INetworkPacket
{
    public byte Status;
    public int Length => 1;

    public void Serialize(Span<byte> buffer)
    {
        
        buffer[0] = Status;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Status = buffer[0];

    }

}
