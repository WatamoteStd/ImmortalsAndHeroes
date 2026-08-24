
using System.Buffers.Binary;
using Shared.MasteryTree;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.MasteryTree;

public struct S2C_BranchUpdatePacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public MasteryNodeId BranchId;
    public uint CurrentExp;
    public ushort CurrentLevel;
    
    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)BranchId);
        Length += 2;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], CurrentExp);
        Length += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], CurrentLevel);
        Length += 2;

        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Length = 0;
        BranchId = (MasteryNodeId)BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        Length += 2;
        CurrentExp = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        CurrentLevel = BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
        Length += 2;


    }

}