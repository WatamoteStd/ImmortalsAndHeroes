
using Shared.Udp.Packets;
using System.Buffers.Binary;
using Shared.Udp.Packets.Category;
using System.Net;
using Shared.Udp.Packets.Category.Game;
using Server.Pools.Session;

namespace Server.Network;

public class PacketReader
{
    
    private readonly SessionManager _sessionManager;

    public PacketReader(SessionManager manager)
    {
        _sessionManager = manager;
    }

    public void PacketDeserialize(ReadOnlySpan<byte> buffer, UserSession session)
    {
        
        PacketTypes packetType = (PacketTypes)BinaryPrimitives.ReadUInt16LittleEndian(buffer);

        switch (packetType)
        {
            
            case PacketTypes.C2S_Handshake:
                {
                    
                    var packet = PacketSerialier.Deserialize<C2S_HandshakePacket>(buffer[2..]);

                    _ = _sessionManager.HandshakeRequest(packet.Ticket, session.IpEnd);

                }
            break;

            case PacketTypes.C2S_MoveRequest:
                {
                    
                    var packet = PacketSerialier.Deserialize<C2S_MoveRequestPacket>(buffer[2..]);

                    _sessionManager.PlayerMoveRequest(session, packet);

                }
            break;

            case PacketTypes.C2S_ChangeRegionRequest:
                {
                    
                    var packet = PacketSerialier.Deserialize<C2S_ChangeRegionRequestPacket>(buffer[2..]);

                    _sessionManager.WH_PlayerChangeRegionRequest(session, packet);

                }
            break;

        }

    }

    public PacketTypes ReadPacketType(ReadOnlySpan<byte> data)
    {
        
        PacketTypes packetType = (PacketTypes)BinaryPrimitives.ReadUInt16LittleEndian(data);

        return packetType;
    }

}