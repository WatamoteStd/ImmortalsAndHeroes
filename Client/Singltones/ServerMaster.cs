using Godot;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public partial class ServerMaster : Node
{
    
    private Socket _socket;
    private IPEndPoint _serverEndPoint;
    public static ServerMaster Instance {get; private set;}

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


}
