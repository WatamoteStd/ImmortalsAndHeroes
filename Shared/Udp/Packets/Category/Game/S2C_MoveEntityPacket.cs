

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game;

public struct S2C_MoveEntityPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public uint Id;
    public float PosX, PosY, PosZ;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Id);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosY);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosZ);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;

        Id = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        PosX = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        PosY = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        PosZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

    }

    

}