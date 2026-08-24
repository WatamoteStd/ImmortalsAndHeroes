
using System.Buffers.Binary;
using Shared.MasteryTree;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.MasteryTree;

public struct C2S_MasteryTreeLearnRequestPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public MasteryNodeId BranchId;

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)BranchId);
        Length = 2;
        return Length;
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        BranchId = (MasteryNodeId)BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        Length = 2;

    }

}