using Godot;
using System;
using System.Net.Sockets;
using System.Net;
using Shared.Network;
using Shared.Network.Packets;
using System.Threading.Tasks;
using System.Buffers.Binary;
using System.Collections.Concurrent;

public partial class NetworkUdpClient : Node
{
	Socket socket;
	public static NetworkUdpClient Instance {get; private set;}
	private IPEndPoint serverEndPoint;
	private IPAddress serverIp;

	// PACKET REСEIVE ==============================================
	byte[] _buffer;
	Memory<byte> _bufferMemory;
	bool _isRunning = false;

	public ConcurrentQueue<INetworkPacket> PacketQueue { get; } = new();



	// FUNCTIONS START ============================================

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
		socket.Connect(serverEndPoint); // PROTeCTION BY OS

		// PACKET RECEIVE ==================================
		_buffer = new byte[4096];
		_bufferMemory = new Memory<byte>(_buffer);

		// START PACKET RECEIVING =============================

		_isRunning = true;
		_ = ReceiveListenerAsync();
		

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

	private async Task ReceiveListenerAsync()
	{
		
		while(_isRunning)
		{	

			int byteCount = await socket.ReceiveAsync(_bufferMemory, SocketFlags.None);

			if (byteCount < 2) continue;

			PacketType packetType = (PacketType)BinaryPrimitives.ReadUInt16LittleEndian(_bufferMemory.Span);

			switch (packetType)
			{
				
				case PacketType.S2C_HandshakeResponse:
					{
						
						S2C_HandshakeResponse packet = PacketSerializer.Deserialize<S2C_HandshakeResponse>(_bufferMemory.Span[..byteCount]);

						PacketQueue.Enqueue(packet);
					}
				break;

				default:

					GD.Print("Some shit packet");

				break;

			}
			

		}

	}


}
