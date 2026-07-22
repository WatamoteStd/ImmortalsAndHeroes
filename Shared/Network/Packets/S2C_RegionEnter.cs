using System;
using System.Buffers.Binary;

namespace Shared.Network.Packets;

public struct S2C_RegionEnter : INetworkPacket
{
    
    public uint MyNetworkId;
    public ushort EntityCount;
    public EntitySnapshotData[] Entities;
    public int Length  {get; set;}

    public struct EntitySnapshotData
    {
        
        public uint NetworkId;
        public byte EntityType;
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public ushort Health;

    }

    public void Serialize(Span<byte> buffer)
    {
        int offset = 0;
        
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, MyNetworkId);
        offset += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], EntityCount);
        offset += 2;

        for (int i = 0; i < EntityCount; i++)
        {
            
            BinaryPrimitives.WriteUInt32LittleEndian(buffer[offset..], Entities[i].NetworkId);
            offset += 4;
            buffer[offset] = Entities[i].EntityType;
            offset += 1;

            BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], Entities[i].PositionX);
            offset += 4;
            BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], Entities[i].PositionY);
            offset += 4;
            BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], Entities[i].PositionZ);
            offset += 4;
            
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], Entities[i].Health);
            offset += 2;

        }
        Length = offset;


    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        int offset = 0;

        MyNetworkId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        offset += 4;
        EntityCount = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += 2;

        Entities = new EntitySnapshotData[EntityCount];

        for (int i = 0; i < EntityCount; i++)
        {
            
            Entities[i].NetworkId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[offset..]);
            offset += 4;
            Entities[i].EntityType = buffer[offset];
            offset += 1;
            Entities[i].PositionX = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
            offset += 4;
            Entities[i].PositionY = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
            offset += 4;
            Entities[i].PositionZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
            offset += 4;
            Entities[i].Health = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
            offset += 2;

        }



    }

    

}