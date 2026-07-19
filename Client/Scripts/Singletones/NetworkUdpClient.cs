using Godot;
using System;
using System.Net.Sockets;
using System.Net;
using Shared.Network;
using Shared.Network.Packets;

public partial class NetworkUdpClient : Node
{
	Socket socket;
	public static NetworkUdpClient Instance {get; private set;}
	private IPEndPoint serverEndPoint;
	private IPAddress serverIp;


	public override void _Ready()
	{
		
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		else
		{
			Instance = this;
		}

		socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		// WINDOWS BUG FIX
		const int SIO_UDP_CONNRESET = -1744830452;
		socket.IOControl(SIO_UDP_CONNRESET, new byte[] { 0 }, null);

		serverIp = IPAddress.Parse("127.0.0.1");
		serverEndPoint = new IPEndPoint(serverIp, 29555);
		

	}

	public void Connect(long ticket)
	{
		
		Span<byte> buffer = stackalloc byte[10];

		C2S_HandshakePacket packet = new C2S_HandshakePacket()
		{
			Ticket = ticket
		};

		int packetLenght = PacketSerializer.Serialize<C2S_HandshakePacket>(buffer, PacketType.C2S_Handshake, packet);
		
		socket.SendTo(buffer[..packetLenght], SocketFlags.None, serverEndPoint);

	}


}
