
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

public struct C2S_CastAbilityRequestPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public byte Slot;
    public float PosX, PosY, PosZ;
    public uint TargetEntityId;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        buffer[Length] = Slot;
        Length++;
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
        Slot = buffer[0];
        Length++;
        PosX = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        PosY = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        PosZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        TargetEntityId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;


    }

}