
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

public struct S2C_CastAbilityFailedPacket : INetworkPacket
{
    
    public int Length {get; private set;} 
    public byte ResponseCode;

    public int Serialize(Span<byte> buffer)
    {
        
        buffer[0] = ResponseCode;

        Length = 1;
        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 1;
        ResponseCode = buffer[0];

    }

}