
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct C2S_ChangeRegionRequestPacket : INetworkPacket
{
    
    public int Length => 4;
    public uint RegionId;

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, RegionId);
        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        RegionId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);

    }

}