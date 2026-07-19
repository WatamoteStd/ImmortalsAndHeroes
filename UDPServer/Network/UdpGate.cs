using System;
using System.Net.Sockets;
using System.Net;

namespace UDPServer.Network;

public class UdpGate
{

    public delegate void PacketReceiveHandler(ReadOnlySpan<byte> packetData, EndPoint remoteEndPoint); // новый тип данных
    private readonly PacketReceiveHandler _onPacketReceived;

    private bool _isRunning = false;
    private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private readonly byte[] _buffer = new byte[65507];
    private readonly Memory<byte> _bufferMemory;

    public UdpGate(PacketReceiveHandler onPacketReceived)
    {
        
        _bufferMemory = new Memory<byte>(_buffer);
        _onPacketReceived = onPacketReceived;

    }

    public void Start(int port)
    {
        
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        _socket.Bind(endPoint);
        _isRunning = true;

        Task.Run(ListenerAsync);

    }

    private async Task ListenerAsync()
    {
        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        
        while(_isRunning)
        {
            
            SocketReceiveFromResult request = await _socket.ReceiveFromAsync(_bufferMemory, SocketFlags.None, remoteEndPoint);

            ReadOnlySpan<byte> packetSpan = _bufferMemory.Span[..request.ReceivedBytes];

            //Console.WriteLine($"[Server] Received new connection from: {request.RemoteEndPoint}");
            //Console.WriteLine($"[Server] Data:{request.ReceivedBytes}");

            _onPacketReceived(packetSpan, request.RemoteEndPoint);
           

        };

    }

    

}