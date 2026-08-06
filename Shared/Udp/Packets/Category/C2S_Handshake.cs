using System.Text;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct C2S_HandshakePacket : INetworkPacket
{
    
    public int Length => 32;
    public string Ticket {get; set;}

    public void Serialize(Span<byte> buffer)
    {
        
        Encoding.UTF8.GetBytes(Ticket, buffer);

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Ticket = Encoding.UTF8.GetString(buffer[..32]);

    }

}