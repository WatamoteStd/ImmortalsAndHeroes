using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Shared.Network;
using Shared.Network.Packets;
using UDPServer.Network.Client;
using UDPServer.World;
using UDPServer.World.Entities;

namespace UDPServer.Network;

public class NetworkUdpManager
{

    private ConcurrentDictionary<IPEndPoint, PlayerClient> _players = new();
    private readonly WorldHolder _world;

    public NetworkUdpManager(WorldHolder world)
    {
        
        _world = world;

    }
    
    public void OnPacketReceived(ReadOnlySpan<byte> packetData, EndPoint remoteEndPoint, Socket serverSocket)
    {

        if (packetData.Length < 2) return;

        IPEndPoint ipEndPoint = (IPEndPoint)remoteEndPoint;

        PacketType packetType = (PacketType)BinaryPrimitives.ReadUInt16LittleEndian(packetData);

        switch (packetType)
        {
            
            case PacketType.C2S_Handshake:
                {
                    
                    C2S_HandshakePacket handshake = PacketSerializer.Deserialize<C2S_HandshakePacket>(packetData);
                    Console.WriteLine($"[Server] New connection! Ticket:{handshake.Ticket}");
                    
                    PlayerClient newPlayerConnection = new PlayerClient(ipEndPoint, Random.Shared.NextInt64());
                    _players.TryAdd(ipEndPoint, newPlayerConnection);
                    
                    Console.WriteLine($"[Server] PlayerId:{newPlayerConnection.PlayerId} connected to the server! IP:{newPlayerConnection.RemoveEndPoint}");

                    // RESPONSE TO CLIENT ============================================================
                    S2C_HandshakeResponse response = new S2C_HandshakeResponse()
                    {
                        Status = 1 
                    };
                    Span<byte> cache = stackalloc byte[3];

                    int packetLenght = PacketSerializer.Serialize<S2C_HandshakeResponse>(cache, PacketType.S2C_HandshakeResponse, response);

                    serverSocket.SendTo(cache[..packetLenght], SocketFlags.None, remoteEndPoint);

                }
            break;

            case PacketType.C2S_EnterTheWorld:
                {

                    if (_players.TryGetValue(ipEndPoint, out PlayerClient client))
                    {
                        
                        Console.WriteLine($"PlayerID:{client.PlayerId} enter the world. Searching region...");

                        var region = _world.GetRegion(0);

                        if (region != null )
                        {
                            
                            client.Region = region;

                            // CREATING CHARACTER FOR PLAYER
                            Entity newEntity = new Entity(Random.Shared.NextInt64(), region.RegionId);
                            client.Character = newEntity;
                            Console.WriteLine($"[Server] PlayerId{client.PlayerId} new character created.");


                            region.AddPlayer(client);
                            Console.WriteLine($"[Server] PlayerId:{client.PlayerId} added to regionID:{region.RegionId}");

                        }


                    }

                }
            break;

            default:

            break;

        }

    }

}