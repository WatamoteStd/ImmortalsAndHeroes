

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct S2C_PlayerExpSyncPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public int ExpDelta;
    public uint TotalExp;

    public int Serialize(Span<byte> buffer)
    {
        Length = 0;
        BinaryPrimitives.WriteInt32LittleEndian(buffer, ExpDelta);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], TotalExp);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        ExpDelta = BinaryPrimitives.ReadInt32LittleEndian(buffer);
        Length += 4;
        TotalExp = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;

    }


}