
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

public struct S2C_EntityMoveSpeedChangedPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint EntityId;
    public float Speed;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, EntityId);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Speed);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        EntityId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        

    }

}