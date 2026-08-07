
using Shared.Udp.Packets;
using System.Buffers.Binary;
using Shared.Udp.Packets.Category;
using System.Net;

namespace Server.Network;

public class PacketReader
{
    
    private readonly SessionManager _sessionManager;

    public PacketReader(SessionManager manager)
    {
        _sessionManager = manager;
    }

    public void PacketDeserialize(ReadOnlySpan<byte> buffer, IPEndPoint clientIp)
    {
        
        PacketTypes packetType = (PacketTypes)BinaryPrimitives.ReadUInt16LittleEndian(buffer);

        switch (packetType)
        {
            
            case PacketTypes.C2S_Handshake:
                {
                    
                    var packet = PacketSerialier.Deserialize<C2S_HandshakePacket>(buffer[2..]);

                    _ = _sessionManager.HandshakeRequest(packet.Ticket, clientIp);

                }
            break;

        }

    }

}