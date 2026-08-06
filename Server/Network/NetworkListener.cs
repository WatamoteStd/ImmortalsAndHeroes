using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Network;

public class NetworkListener
{
    
    private readonly Socket _socket;
    EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    private readonly SessionManager _sessionManager;
    private readonly PacketReader _packetReader;
    byte[] buffer = new byte[4096];

    bool isRunning;


    public NetworkListener(Socket socket, SessionManager _manager, PacketReader packetReader)
    {
        _socket = socket;
        _sessionManager = _manager;
        _packetReader = packetReader;
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
            
            int count = _socket.ReceiveFrom(buffer, SocketFlags.None, ref remoteEndPoint);

            if (count >= 2 && remoteEndPoint is IPEndPoint clientIp)
            {
                _sessionManager.PacketGateway(clientIp);
                _packetReader.PacketDeserialize(buffer[..count], clientIp);
            }

        }

    }

}