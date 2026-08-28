
using System.Buffers.Binary;
using Shared.Ability;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

public struct S2C_AbilityCastedPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint CasterEntityId;
    public AbilityTypes AbilityId; // 4b
    public float PosX, PosY, PosZ;
    public uint TargetEntityId;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, CasterEntityId);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], (uint)AbilityId);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosY);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosZ);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], TargetEntityId);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        CasterEntityId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        AbilityId = (AbilityTypes)BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

        PosX = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        PosY = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        PosZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        TargetEntityId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);

    }

}