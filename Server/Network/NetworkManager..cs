using System;
using System.Net;
using System.Net.Sockets;
using Server.Network.Interfaces;

namespace Server.Network;

public class NetworkManager
{
    
    private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 29555);
    public SessionManager sessionManager { get; }
    public NetworkListener networkListener { get; }
    public PacketSender packetSender { get; }
    public PacketReader packetReader { get;}
    
    public NetworkManager(int port)
    {
        
        packetReader = new PacketReader(new Lazy<ISessionPacketHandler>(() => sessionManager!));
        packetSender = new PacketSender(_socket);
        sessionManager = new SessionManager(packetSender, packetReader);
        networkListener = new NetworkListener(_socket, sessionManager);

    }

    public void NetStart()
    {
        _socket.Bind(localEndPoint);
        networkListener.Start();

    }




}