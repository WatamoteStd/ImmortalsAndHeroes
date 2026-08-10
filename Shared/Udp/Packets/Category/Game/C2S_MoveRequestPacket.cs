

using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game;

public struct C2S_MoveRequestPacket : INetworkPacket
{
    
    public int Length {get; private set;}

    public float X, Y, Z;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;

        BinaryPrimitives.WriteSingleLittleEndian(buffer, X);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Y);
        Length += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Z);
        Length += 4;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;

        X = BinaryPrimitives.ReadSingleLittleEndian(buffer);
        Length += 4;
        Y = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;
        Z = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        Length += 4;

    }

}