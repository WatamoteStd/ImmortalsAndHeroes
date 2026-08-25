using System.Net.Sockets;
using System.Net;
using Shared.DataTransferObjects;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category;
using System.Security;
using Shared.Udp.Interfaces;

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
                Exp = data.Exp,
                Silver = (uint)data.Silver
            };

            int length = PacketSerialier.Serialize<S2C_HandshakeSuccessPacket>(buffer, PacketTypes.S2C_HandshakeSuccess, packet);

            _socket.SendTo(buffer[..length], SocketFlags.None, clientIp);
            return;

        }
        Console.WriteLine($"[HANDSHAKE FAILED] Reason: isOk={isOk}, dataIsNull={data == null} for IP {clientIp}");
        int bytes = PacketSerialier.Serialize<S2C_HandshakeFailedPacket>(buffer, PacketTypes.S2C_HandshakeFailed, new S2C_HandshakeFailedPacket{});
        _socket.SendTo(buffer[..bytes], SocketFlags.None, clientIp);



    }


    public void SendPacket<T>(IPEndPoint clientIp, PacketTypes packetType, T packet) 
        where T: struct, INetworkPacket
    {
        
        Span<byte> buffer = stackalloc byte[512];

        int length = PacketSerialier.Serialize<T>(buffer, packetType, packet);

        _socket.SendTo(buffer[..length], SocketFlags.None, clientIp);

    }

}