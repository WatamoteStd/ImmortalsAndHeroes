
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game;

 public struct S2C_StatsSyncPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public float Health;
    public float Mana;
    public float HealthRegen; 
    public float ManaRegen; 
    public float Damage; 
    public float Armor;
    public float MagicResistance;
    public float AttackSpeed;
    public float MaxHealth;
    public float MaxMana;
    public float Speed;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        BinaryPrimitives.WriteSingleLittleEndian(buffer, Health);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Mana);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], HealthRegen);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], ManaRegen);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Damage);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Armor);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], MagicResistance);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], AttackSpeed);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], MaxHealth);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], MaxMana);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Speed);
        Length += 4;


        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;

        Health = BinaryPrimitives.ReadSingleLittleEndian(buffer);
        Length += 4;
        Mana = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        HealthRegen = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        ManaRegen = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        Damage = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        Armor = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        MagicResistance = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        AttackSpeed = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        MaxHealth = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        MaxMana = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

    }
     

}