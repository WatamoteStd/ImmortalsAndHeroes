using Godot;
using System;
using IaH.Shared.Networking;
using LiteNetLib;
using LiteNetLib.Utils;

public partial class NetworkManager : Node
{
	private NetManager _netManager;
	private EventBasedNetListener _listener;


	public override void _Ready()
	{
		
		_listener = new EventBasedNetListener();
		_listener.NetworkReceiveEvent += PacketReceived;
		_listener.PeerConnectedEvent += (peer) =>
		{
			GD.Print("Подключение к серверу успешно!");	
		};


		_netManager = new NetManager(_listener);
		_netManager.Start();
		_netManager.Connect("localhost", 9050, "");

	}

	private  void PacketReceived(NetPeer peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod )
	{
		
		
		PacketType rawPacket = (PacketType)reader.GetByte();

		switch (rawPacket)
		{
			
			case PacketType.Welcome:

				ushort id = reader.GetUShort();
				GD.Print($"ID: {id}");

			break;

		}

	}


	public override void _Process(double delta)
	{
		
		_netManager.PollEvents();

	}
}
