using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Network;

public class NetworkListener
{
    
    Socket socket;
    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    SessionManager sessionManager;
    byte[] buffer = new byte[4096];

    bool isRunning;


    public NetworkListener(Socket _socket, SessionManager _manager)
    {
        socket = _socket;
        sessionManager = _manager;
    }

    public void Start()
    {
        isRunning = true;
        
        Thread listenerThread = new Thread(Listen);
        listenerThread.IsBackground = true;
        listenerThread.Start();

    }

    public void Listen()
    {
        
        while(isRunning)
        {
            
            int count = socket.ReceiveFrom(buffer, SocketFlags.None, ref remoteEndPoint);

            if (count >= 2 && remoteEndPoint is IPEndPoint clientIp)
            {
                sessionManager.PacketGateway(clientIp);
            }

        }

    }

}