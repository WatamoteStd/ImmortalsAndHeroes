
using System.Buffers.Binary;
using Shared.Items;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_InventorySnapshotPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public uint CharacterId;
    public ushort SlotCount;
    public ItemSlot[] Slots;

    public int Serialize(Span<byte> buffer)
    {
        Length = 0;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, CharacterId);
        Length += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], SlotCount);
        Length += 2;

        for (int i = 0; i < Slots.Length; i++)
        {
            
            BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], (uint)Slots[i].ItemId);
            Length += 4;
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], Slots[i].Count);
            Length += 2;

        }

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;

        CharacterId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        SlotCount = BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
        Length += 2;

        for (int i = 0; i < SlotCount; i++)
        {
            
            Slots[i].ItemId = (ItemType)BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
            Length += 4;
            Slots[i].Count = BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
            Length += 2;

        }

    }
    


}