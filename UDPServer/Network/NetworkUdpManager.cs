using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using Shared.Network;
using Shared.Network.Packets;

namespace UDPServer.Network;

public class NetworkUdpManager
{
    
    public void OnPacketReceived(ReadOnlySpan<byte> packetData, EndPoint remoteEndPoint, Socket serverSocket)
    {

        if (packetData.Length < 2) return;


        PacketType packetType = (PacketType)BinaryPrimitives.ReadUInt16LittleEndian(packetData);

        switch (packetType)
        {
            
            case PacketType.C2S_Handshake:
                {
                    
                    C2S_HandshakePacket handshake = PacketSerializer.Deserialize<C2S_HandshakePacket>(packetData);
                    Console.WriteLine($"[Server] New connection! Ticket:{handshake.Ticket}");
                    
                    S2C_HandshakeResponse response = new S2C_HandshakeResponse()
                    {
                        Status = 1 
                    };
                    Span<byte> cache = stackalloc byte[3];

                    int packetLenght = PacketSerializer.Serialize<S2C_HandshakeResponse>(cache, PacketType.S2C_HandshakeResponse, response);

                    serverSocket.SendTo(cache[..packetLenght], SocketFlags.None, remoteEndPoint);

                }
            break;

            default:

            break;

        }

    }

}