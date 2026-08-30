
using System.Buffers.Binary;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Projectile;

public struct S2C_ProjectileDeletedPacket : INetworkPacket
{
    
    public int Length => 2;
    public ushort Id;

    public int Serialize(Span<byte> buffer)
    {
        
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, Id);
        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Id = BinaryPrimitives.ReadUInt16LittleEndian(buffer);

    }

}