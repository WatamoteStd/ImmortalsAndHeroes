

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_EntityDamageTakedPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public uint Id;
    public int Damage;
    public uint AttackerId;
    public uint ActualHealth;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Id);
        Length += 4;
        BinaryPrimitives.WriteInt32LittleEndian(buffer[Length..], Damage);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], AttackerId);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], ActualHealth);
        Length += 4;

        return Length;
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        Id = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        Damage = BinaryPrimitives.ReadInt32LittleEndian(buffer[Length..]);
        Length += 4;
        AttackerId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        ActualHealth = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

    }

}