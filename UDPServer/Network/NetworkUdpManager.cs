using System;
using System.Buffers.Binary;
using System.Net;
using Shared.Network;
using Shared.Network.Packets;

namespace UDPServer.Network;

public class NetworkUdpManager
{
    
    public void OnPacketReceived(ReadOnlySpan<byte> packetData, EndPoint remoteEndPoint)
    {
        
        //Console.WriteLine($"[NetworkManager] Packet received.");

        if (packetData.Length < 2) return;


        PacketType packetType = (PacketType)BinaryPrimitives.ReadUInt16LittleEndian(packetData);

        switch (packetType)
        {
            
            case PacketType.C2S_Handshake:
                {
                    
                    C2S_HandshakePacket handshake = PacketSerializer.Deserialize<C2S_HandshakePacket>(packetData);
                    Console.WriteLine($"[NetworkManager] Player:{handshake.PlayerId} requests handshake from: {remoteEndPoint}");

                }
            break;

            default:

            break;

        }

    }

}