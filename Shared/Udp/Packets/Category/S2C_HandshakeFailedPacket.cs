
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_HandshakeFailedPacket : INetworkPacket
{
    
    public int Length => 0;

    public int Serialize(Span<byte> buffer)
    {
        return 0;
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
    }

}