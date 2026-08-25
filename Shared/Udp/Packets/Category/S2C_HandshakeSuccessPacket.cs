
using Shared.Udp.Interfaces;
using Shared.Characters;
using System.Buffers.Binary;
using System.Text;

namespace Shared.Udp.Packets.Category;

public struct S2C_HandshakeSuccessPacket() : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint Id {get; set;} 
    public uint RegionId {get; set;}
    public string Name {get; set;} = "";
    public float PosX {get; set;}
    public float PosY {get; set;}
    public float PosZ {get; set;}
    public uint UserId {get; set;}

    // CHARACTER

    public EntityType Type {get; set;} = EntityType.Default;
    public float CurrentHp {get; set;} 
    public float CurrentMp {get; set;} 
    public float Exp {get; set;}
    public uint Silver {get; set;} 

    public int Serialize(Span<byte> buffer)
    {
        Length = 0;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer,Id);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], RegionId);
        Length += 4;

        byte nameLenth = (byte)Encoding.UTF8.GetByteCount(Name);
        buffer[Length] = nameLenth;
        Length += 1;

        Encoding.UTF8.GetBytes(Name, buffer[Length..]);
        Length += nameLenth;

        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosX);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosY);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], PosZ);
        Length += 4;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], UserId);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], (uint)Type);
        Length +=4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], CurrentHp);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], CurrentMp);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Exp);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], Silver);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;

        Id = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;

        RegionId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

        // Читаем 1 байт длины строки
        byte nameLength = buffer[Length];
        Length += 1;

        // Вырезаем срез под имя и десериализуем
        Name = Encoding.UTF8.GetString(buffer[Length..(Length + nameLength)]);
        Length += nameLength;

        PosX = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        PosY = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        PosZ = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        UserId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

        Type = (EntityType)BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

        CurrentHp = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        CurrentMp = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        Exp = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

        Silver = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
    }

}