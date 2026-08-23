using Client.NO_NODE;
using Godot;
using Shared.Udp.Interfaces;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category;
using Shared.Udp.Packets.Category.Game;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public partial class ServerMaster : Node
{
    
    private Socket _socket;
    private IPEndPoint _serverEndPoint;
    public static ServerMaster Instance {get; private set;}
    private PacketReaderClient _packetReader;


    // FOR WORLD MAnAGER
    private List<INetworkPacket> _regionPacketsBuffer = new();

    

    private WorldHandler _worldManager;
    public WorldHandler WorldManager
    {
        
        get => _worldManager;
        set
        {
            
            _worldManager = value;
            if (_worldManager != null && _regionPacketsBuffer.Count > 0)
            {
                
                foreach(var packet in _regionPacketsBuffer)
                {
                    HandlePacket(packet);
                }
                _regionPacketsBuffer.Clear();

            }

        }

    }

    private ConcurrentQueue<INetworkPacket> _packetQeueue = new ConcurrentQueue<INetworkPacket>();

    public override void _Ready()
    {

        if (Instance != null)
        {
            QueueFree();
            return;
        }
        else Instance = this;

        PlayerController.OnMoveRequest -= LocalPlayerMoveRequest;
        PlayerController.OnMoveRequest += LocalPlayerMoveRequest;

    }


    public void ConnectToServer()
    {

        if (string.IsNullOrEmpty(GameSession.Instance.UdpToken) || string.IsNullOrEmpty(GameSession.Instance.UdpIp))
        {
            GD.PrintErr("[SESSION FAULT] EMPTY UDP TOKEN OR SERVER END POINT");
            return;
        }

        _socket?.Close();
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        _serverEndPoint = new IPEndPoint(IPAddress.Parse(GameSession.Instance.UdpIp), GameSession.Instance.UdpPort);
        _socket.Connect(_serverEndPoint);

        _packetReader = new PacketReaderClient(_socket, _packetQeueue);
        _packetReader.Start();

        ValidateConnection();

    }

    public void ValidateConnection()
    {
        
        Span<byte> buffer = stackalloc byte[34];

        var data = new C2S_HandshakePacket
        {
            Ticket = GameSession.Instance.UdpToken
        };

        int count = PacketSerialier.Serialize<C2S_HandshakePacket>(buffer, PacketTypes.C2S_Handshake, data);

        _socket.Send(buffer);

    }

    public override void _ExitTree()
    {
        base._ExitTree();
        PlayerController.OnMoveRequest -= LocalPlayerMoveRequest;
        _packetReader.Stop();
    }


    public override void _Process(double delta)
    {
        
        while(_packetQeueue.TryDequeue(out INetworkPacket packet))
        {

            if (packet is S2C_HandshakeSuccessPacket data)
            {

                WorldManager = null; 
                _ = SceneManager.Instance.LoadRegion(data.RegionId);
                _regionPacketsBuffer.Add(packet);
                continue;
            }
            if (packet is S2C_ChangeRegionPacket dataPacket)
            {
                WorldManager = null; 
                _ = SceneManager.Instance.LoadRegion(dataPacket.RegionId);
                continue;     
            }
            
            
            if (WorldManager == null)
            {
                _regionPacketsBuffer.Add(packet);
            }
            else
            {
                HandlePacket(packet);
            }


        }

    }

    private void HandlePacket(INetworkPacket packet)
    {
        
        switch(packet)
        {
            
            case S2C_HandshakeSuccessPacket handshake:
                {
                    WorldManager?.SpawnLocalPlayer(handshake);
                    GameSession.Instance.NetworkId = handshake.Id;
                    GameSession.Instance.PlayerCache = handshake;

                }
            break;

            case S2C_SpawnEntityPacket entityPacket:
                {
                    WorldManager?.AddEntity(entityPacket);
                }
            break;

            case S2C_MoveEntityPacket move:
                {
                    WorldManager?.MoveEntity(move.Id, move.PosX, move.PosY, move.PosZ);
                }
            break;

            case S2C_ItemDiffPacket itemDiff:
                {
                    SceneManager.Instance.UpdateInventoryCell(itemDiff.SlotIndex, itemDiff.Item, itemDiff.Count);
                }
            break;
            
            case S2C_RemoveEntityPacket remove:
                {
                    _worldManager?.RemoveEntity(remove.Id);
                    if (remove.Id == GameSession.Instance.PlayerCache.Id)
                    {
                        SceneManager.Instance.ConnectionLostScren();
                        _packetReader?.Stop();
                        _socket?.Close();
                        _socket = null!;   
                        GameSession.Instance.CurrentSessionState = GameSession.State.Disconnected;

                    }
                }
            break;

            case S2C_EntityDamageTakedPacket takeDamage:
                {
                    
                    _worldManager?.EntityTakeDamage(takeDamage.Id, takeDamage.Damage, takeDamage.AttackerId, takeDamage.ActualHealth);

                }
            break;    

            case S2C_PlayerExpSyncPacket expUpd:
                {
                    GameSession.Instance.PlayerExpCache = expUpd.TotalExp;
                }
            break;

        }

    }

    private void LocalPlayerMoveRequest(Vector3 pos)
    {
        
        Span<byte> buffer = stackalloc byte[14]; // 4 + 4 + 4 (cords) + 2 (packetYType)

        var posPacket = new C2S_MoveRequestPacket
        {
            X = pos.X,
            Y = pos.Y,
            Z = pos.Z
        };
        int length = PacketSerialier.Serialize<C2S_MoveRequestPacket>(buffer, PacketTypes.C2S_MoveRequest, posPacket);

        _socket.Send(buffer); 

    }

    public void LocalPlayerChangeRegionRequest(uint regionId)
    {
        
        Span<byte> buffer = stackalloc byte[6];

        var packet = new C2S_ChangeRegionRequestPacket
        {
            RegionId = regionId
        };
        int length = PacketSerialier.Serialize<C2S_ChangeRegionRequestPacket>(buffer, PacketTypes.C2S_ChangeRegionRequest, packet);
        _socket.Send(buffer);


    }
    public void LP_AttackRequest(uint entityId)
    {
        Span<byte> buffer = stackalloc byte[6];
        var packet = new C2S_AttackRequestPacket {Id = entityId};
        int length = PacketSerialier.Serialize<C2S_AttackRequestPacket>(buffer, PacketTypes.C2S_AttackRequest, packet);
        _socket.Send(buffer);
    }




}
