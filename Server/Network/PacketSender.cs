using System.Net.Sockets;
using System.Net;
using Shared.DataTransferObjects;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category;

namespace Server.Network;

public class PacketSender
{
    
    private readonly Socket _socket;

    public PacketSender(Socket socket)
    {
        _socket = socket;
    }

    public void SM_SendHandhsakeResult(bool isOk, HandshakeResponseDto? data, IPEndPoint clientIp)
    {
        Span<byte> buffer = stackalloc byte[512];
        
        if (isOk && data != null)
        {

            var packet = new S2C_HandshakeSuccessPacket
            {
                
                Id = (uint)data.Id,
                RegionId = (uint)data.RegionId,
                Name = data.Name,
                PosX = data.PosX,
                PosY = data.PosY,
                PosZ = data.PosZ,
                UserId = (uint)data.UserId,
                Type = data.Type,
                CurrentHp = data.CurrentHp,
                CurrentMp = data.CurrentMp,
                Lvl = data.Lvl,
                Silver = (uint)data.Silver
            };

            int length = PacketSerialier.Serialize<S2C_HandshakeSuccessPacket>(buffer, PacketTypes.S2C_HandshakeSuccess, packet);

            _socket.SendTo(buffer[..length], SocketFlags.None, clientIp);
            return;

        }

        int bytes = PacketSerialier.Serialize<S2C_HandshakeFailedPacket>(buffer, PacketTypes.S2C_HandshakeFailed, new S2C_HandshakeFailedPacket{});
        _socket.SendTo(buffer[..bytes], SocketFlags.None, clientIp);



    }



}