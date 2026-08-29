
using System.Buffers.Binary;
using System.Text;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category;

public struct C2S_AdminConsoleCommandPacket : INetworkPacket
{
    
    public int Length {get; private set;} 
    public string Payload;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        
        ushort stringBytesCount = (ushort)Encoding.UTF8.GetByteCount(Payload);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, stringBytesCount);
        Length += 2;

        int writtenBytes = Encoding.UTF8.GetBytes(Payload, buffer.Slice(Length));
        Length += writtenBytes;

        return Length;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;
        ushort stringBytesCount = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        Length += 2;
        
        Payload = Encoding.UTF8.GetString(buffer.Slice(Length, stringBytesCount));
        Length += stringBytesCount;

    }

}