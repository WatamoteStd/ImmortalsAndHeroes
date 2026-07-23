using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Shared.Network;
using Shared.Network.Packets;
using Shared.Network.Packets.GamePackets;
using UDPServer.Network.Client;
using UDPServer.World;
using UDPServer.World.Entities;

namespace UDPServer.Network;

public class NetworkUdpManager
{

    private ConcurrentDictionary<IPEndPoint, PlayerClient> _players = new();
    private readonly WorldHolder _world;

    private Socket? _serverSocket;

    public NetworkUdpManager(WorldHolder world)
    {
        
        _world = world;

    }
    
    public void OnPacketReceived(ReadOnlySpan<byte> packetData, EndPoint remoteEndPoint, Socket serverSocket)
    {

        if (packetData.Length < 2) return;

        if (_serverSocket == null)
        {
            _serverSocket = serverSocket;
        }

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
                    
                    Console.WriteLine($"[Server] PlayerId:{newPlayerConnection.PlayerId} connected to the server! IP:{newPlayerConnection.RemoteIpEndPoint}");

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

                    if (_players.TryGetValue(ipEndPoint, out PlayerClient? client) && client != null)
                    {
                        
                        Console.WriteLine($"[Server] PlayerID:{client.PlayerId} enter the world. Searching region...");

                        var region = _world.GetRegion(0);

                        if (region != null )
                        {
                            
                            client.RegionId = region.RegionId;

                            // CREATING CHARACTER FOR PLAYER
                            Entity newEntity = new Entity((uint)Random.Shared.Next(), region.RegionId, Random.Shared.NextInt64());
                            client.Character = newEntity;
                            client.NetworkId = newEntity.NetworkId;
                            Console.WriteLine($"[Server] PlayerId{client.PlayerId} new character created.");


                            region.AddPlayer(client);
                            Console.WriteLine($"[Server] PlayerId:{client.PlayerId} added to regionID:{region.RegionId}");

                            // CREATING REGION PACKET FOR CLIENT ==============================

                            var regionEntites = region.Entities;
                            var snapshots = new S2C_RegionEnter.EntitySnapshotData[regionEntites.Count];

                           int i = 0;

                            foreach (var kvp in regionEntites)
                            {
                                Entity ent = kvp.Value;
                                
                                snapshots[i] = new S2C_RegionEnter.EntitySnapshotData
                                {
                                    NetworkId = ent.NetworkId,
                                    EntityType = ent.Type,
                                    PositionX = ent.Position.X,
                                    PositionY = ent.Position.Y,
                                    PositionZ = ent.Position.Z,
                                    Health = ent.Health
                                };

                                i++;

                            }

                            S2C_RegionEnter snapshotPacket = new S2C_RegionEnter()
                            {
                                
                                MyNetworkId = client.NetworkId,
                                EntityCount = (ushort)snapshots.Length,
                                Entities = snapshots

                            };

                            Span<byte> buffer = stackalloc byte[10 + (snapshots.Length * 19)];
                            int packetLenght = PacketSerializer.Serialize<S2C_RegionEnter>(buffer, PacketType.S2C_RegionEnter, snapshotPacket);

                            serverSocket.SendTo(buffer[..packetLenght], SocketFlags.None, remoteEndPoint);

                        }


                    }

                }
            break;

            case PacketType.C2S_MoveRequest:
                {
                    
                    C2S_MoveRequestPacket packet = PacketSerializer.Deserialize<C2S_MoveRequestPacket>(packetData);

                    if (_players.TryGetValue(ipEndPoint, out PlayerClient? client) && client != null)
                    {
                        
                        WorldRegion? region = GetClientRegion(client);

                        if (region == null)
                        {
                            Console.WriteLine($"[Server] PlayerId:{client.PlayerId} can't send move packet. It's region id is null.");
                        }
                        else
                        {
                            
                            region.MoveEntity(client.PlayerId, packet.PositionX, packet.PositionY, packet.PositionZ);

                        }

                    }
                    else
                    {
                        Console.WriteLine("[Server] Unregistred account move request declined.");
                    }

                }
            break;

            default:

            break;

        }

    }

    private WorldRegion? GetClientRegion(PlayerClient playerClient)
    {
        
        WorldRegion? region = _world.GetRegion(playerClient.RegionId);

        return region;

    }

    // ============================== WORLD HOLDER PACKETS =================== \\

    public void WHSendRegionPacket(WorldRegion region, ReadOnlySpan<Entity> entities)
    {
        
        var players = region.GetPlayers();
        if (players.Length == 0) return;

        Span<byte> buffer = stackalloc byte[64];

        for (int i = 0; i < entities.Length; i++)
        {
            
            var entity = entities[i];

            var movePacket = new S2C_MoveEntityPacket
            {
                NetworkEntityId = entity.NetworkId,
                PositionX = entity.Position.X,
                PositionY = entity.Position.Y,
                PositionZ = entity.Position.Z
            };

            int packetSize = PacketSerializer.Serialize<S2C_MoveEntityPacket>(buffer, PacketType.S2C_MoveEntity, movePacket);

            ReadOnlySpan<byte> packetBytes = buffer[..packetSize];

            for (int p = 0; p < players.Length; p++)
            {
                
                _serverSocket!.SendTo(packetBytes, SocketFlags.None, players[p].RemoteIpEndPoint);

            }

        }

    }

}