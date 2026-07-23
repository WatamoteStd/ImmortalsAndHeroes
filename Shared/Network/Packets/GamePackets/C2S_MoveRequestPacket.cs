using System;
using System.Buffers.Binary;

namespace Shared.Network.Packets.GamePackets;

public struct C2S_MoveRequestPacket : INetworkPacket
{
    

    public int Length {get; private set;}
    public float PositionX;
    public float PositionY;
    public float PositionZ;

    public void Serialize(Span<Byte> buffer)
    {
        
        Length = 0;

        BinaryPrimitives.WriteSingleLittleEndian(buffer, PositionX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PositionY);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PositionZ);
        Length += 4;


    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        int offset = 0;

        PositionX = BinaryPrimitives.ReadSingleLittleEndian(buffer);
        offset += 4;
        PositionY = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        PositionZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        
        Length = offset;

    }


}