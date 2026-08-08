

using System.ComponentModel.DataAnnotations;

namespace Shared.Udp.Interfaces;

public interface INetworkPacket
{
    
    public int Length {get;}

    public int Serialize(Span<byte> buffer);
    public void Deserialize(ReadOnlySpan<byte> buffer);


}