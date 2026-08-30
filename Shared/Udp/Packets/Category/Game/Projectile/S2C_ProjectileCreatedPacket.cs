
using System.Buffers.Binary;
using Shared.ProjectilesData;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Projectile;

public struct S2C_ProjectileCreatedPacket : INetworkPacket
{
    
    public int Length {get; private set;}


    public ushort Id;
    public uint CasterId;
    public uint TargetId;
    public ProjectileType Type;
    public float Speed;

    public int Serialize(Span<byte> buffer)
    {
        
        Length = 0;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, Id);
        Length += 2;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], CasterId);
        Length += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[Length..], TargetId);
        Length += 4;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[Length..], (ushort)Type);
        Length += 2;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[Length..], Speed);
        Length += 4;

        return Length;
        

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Length = 0;

        Id = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        Length += 2;
        CasterId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        TargetId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[Length..]);
        Length += 4;
        Type = (ProjectileType)BinaryPrimitives.ReadUInt16LittleEndian(buffer[Length..]);
        Length += 2;
        Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[Length..]);
        

    }

}