

using System.Buffers.Binary;
using Shared.Items;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_ItemDiffPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint CharacterId;
    public ushort SlotIndex;
    public ItemType Item;
    public ushort Count;

    public int Serialize(Span<byte> buffer)
    {
        Length = 0;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, CharacterId);
        Length += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], SlotIndex);
        Length += 2;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], (uint)Item);
        Length += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], Count);
        Length += 2;

        return Length;


    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        CharacterId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        SlotIndex = BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
        Length += 2;
        Item = (ItemType)BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        Count = BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
        Length += 2;

    }


}