using System;
using System.Buffers.Binary;

public struct S2C_RegionEnterFM : INetworkPacket
{
    
    public int Length {get; private set;}

    public uint NetworkId;
    public float posX;
    public float posY;
    public float posZ;
    public ushort Health;

    public void Serialize(Span<byte> buffer)
    {
        Length = 0;
        
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, NetworkId);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], posX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], posY);
        Length +=4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], posZ);
        Length += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], Health);
        Length += 2;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        int offset = 0;

        NetworkId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        offset += 4;
        posX = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        posY = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset +=4;
        posZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset +=4;
        Health = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += 2;

        Length = offset;

    }

}