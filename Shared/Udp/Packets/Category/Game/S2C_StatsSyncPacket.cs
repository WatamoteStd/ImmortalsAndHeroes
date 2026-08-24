
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game;

 public struct S2C_StatsSyncPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint Health;
    public uint Mana;
    public float HealthRegen; 
    public float ManaRegen; 
    public uint Damage; 
    public int Armor;
    public int MagicResistance;
    public uint AttackSpeed;
    public uint MaxHealth;
    public uint MaxMana;
    public float Speed;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Health);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], Mana);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], HealthRegen);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], ManaRegen);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], Damage);
        Length += 4;
        BinaryPrimitives.WriteInt32LittleEndian(buffer[Length..], Armor);
        Length += 4;
        BinaryPrimitives.WriteInt32LittleEndian(buffer[Length..], MagicResistance);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], AttackSpeed);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], MaxHealth);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], MaxMana);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Speed);
        Length += 4;


        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;

        Health = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        Mana = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        HealthRegen = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        ManaRegen = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        Damage = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        Armor = BinaryPrimitives.ReadInt32LittleEndian(buffer[Length..]);
        Length += 4;
        MagicResistance = BinaryPrimitives.ReadInt32LittleEndian(buffer[Length..]);
        Length += 4;
        AttackSpeed = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        MaxHealth = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        MaxMana = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

    }
     

}