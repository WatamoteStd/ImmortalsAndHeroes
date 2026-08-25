
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

public struct C2S_ActivateAbilityPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public int Code;

    public int Serialize(Span<byte> buffer)
    {
        Length = 0;
        BinaryPrimitives.WriteInt32LittleEndian(buffer, Code);
        Length += 4;
        return Length;
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;
        Code = BinaryPrimitives.ReadInt32LittleEndian(buffer);
        Length += 4;
    }

}