using System;
using System.Net;
using System.Net.Sockets;

namespace Server.Network;

public class NetworkManager
{
    
    private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    public SessionManager sessionManager { get; } = new SessionManager();
    public NetworkListener networkListener { get; }
    
    public NetworkManager(int port)
    {
        
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        _socket.Bind(endPoint);
        networkListener = new NetworkListener(_socket, sessionManager);

    }

    public void NetStart()
    {
        
        networkListener.Start();

    }




}