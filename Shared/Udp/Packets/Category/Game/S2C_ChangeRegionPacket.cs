

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_ChangeRegionPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint CharacterId;
    public uint RegionId;

    public int Serialize(Span<byte> buffer)
    {
        Length = 0;
        
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, CharacterId);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], RegionId);
        Length += 4;

        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;
        CharacterId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length += 4;
        RegionId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

    }

}