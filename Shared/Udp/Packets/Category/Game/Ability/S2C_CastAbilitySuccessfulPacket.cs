
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

public struct S2C_CastAbilitySuccessfulPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public byte Slot;
    public float CurrentCooldown;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;

        buffer[0] = Slot;
        Length++;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], CurrentCooldown);
        Length += 4;
        
        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        Slot = buffer[0];
        Length++;
        CurrentCooldown = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

    }
}