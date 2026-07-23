using System;
using System.Buffers.Binary;

namespace Shared.Network.Packets.GamePackets;

public struct S2C_MoveEntityPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public uint NetworkEntityId;
    public float PositionX;
    public float PositionY;
    public float PositionZ;

    public void Serialize(Span<byte> buffer)
    {
        Length = 0;
        
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, NetworkEntityId);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PositionX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..],PositionY);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PositionZ);
        Length += 4;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        int offset = 0;

        NetworkEntityId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        offset += 4;
        PositionX = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        PositionY = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        PositionZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;

        Length = offset;

    }

}