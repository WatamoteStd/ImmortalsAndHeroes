

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_RemoveEntityPacket : INetworkPacket
{
    
    public int Length => 4;

    public uint Id;

    public int Serialize(Span<byte> buffer)
    {
        
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Id);
        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Id = BinaryPrimitives.ReadUInt32LittleEndian(buffer);

    }

}