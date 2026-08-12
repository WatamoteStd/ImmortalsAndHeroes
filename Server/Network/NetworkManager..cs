using System;
using System.Net;
using System.Net.Sockets;

namespace Server.Network;

public class NetworkManager
{
    
    private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    public SessionManager sessionManager { get; }
    public NetworkListener networkListener { get; }
    public PacketSender packetSender { get; }
    public PacketReader packetReader { get;}
    
    public NetworkManager(int port)
    {
        
        

    }

    public void NetStart()
    {
        
        networkListener.Start();

    }




}