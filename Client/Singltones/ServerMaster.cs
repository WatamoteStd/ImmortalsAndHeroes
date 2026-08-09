using Client.NO_NODE;
using Godot;
using Shared.Udp.Interfaces;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category;
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
        _packetReader.Stop();
    }


    public override void _Process(double delta)
    {
        
        while(_packetQeueue.TryDequeue(out INetworkPacket packet))
        {

            if (packet is S2C_HandshakeSuccessPacket handshake)
            {
            _ = SceneManager.Instance.LoadRegion(handshake.RegionId);
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
                }
            break;

        }

    }




}
