

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct C2S_AttackRequestPacket : INetworkPacket
{
    
    public int Length {get; private set;}
    public uint Id;

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Id);
        Length = 4;
        return 4;
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Id = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        Length = 4;

    }

}