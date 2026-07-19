using System;
using Shared.Network.Packets;
public interface INetworkPacket
{
    
    int Length { get; }

    void Serialize(Span<byte> buffer);
    void Deserialize(ReadOnlySpan<byte> buffer);

}