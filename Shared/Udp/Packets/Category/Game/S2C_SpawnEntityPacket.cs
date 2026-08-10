

using System.Buffers.Binary;
using System.Text;
using Shared.Characters;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game;

public struct S2C_SpawnEntityPacket : INetworkPacket
{
    
    public int Length {get; set;} 

    public uint Id;
    public int Health;
    public string Name;
    public float PosX, PosY, PosZ;
    public EntityType Type;

    public int Serialize(Span<byte> buffer)
    {
        

        Length = 0;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Id);
        Length += 4;
        BinaryPrimitives.WriteInt32LittleEndian(buffer[Length..], Health);
        Length += 4;
        ushort nameLength = (ushort)Encoding.UTF8.GetByteCount(Name);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], nameLength);
        Length += 2;
        int written = Encoding.UTF8.GetBytes(Name, buffer[Length..]);
        Length += written;
        
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosY);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosZ);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], (uint)Type);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;

        Id = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;

        Health = BinaryPrimitives.ReadInt32LittleEndian(buffer[Length..]);
        Length += 4;

        ushort nameLength = BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
        Length += 2;

        Name = Encoding.UTF8.GetString(buffer.Slice(Length, nameLength));
        Length += nameLength;

        PosX = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        PosY = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        PosZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        Type = (EntityType)BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
    }


}