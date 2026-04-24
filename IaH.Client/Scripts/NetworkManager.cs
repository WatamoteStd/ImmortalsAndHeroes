using Godot;
using System;
using IaH.Shared.Networking;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;

public partial class NetworkManager : Node
{
	private NetManager _netManager;
	private EventBasedNetListener _listener;
	private NetPeer _serverPeer;
	private NetDataWriter _writer;

	private readonly Dictionary<ushort, Node3D> _entities = new();


	public override void _Ready()
	{
		
		
		_listener = new EventBasedNetListener();
		_listener.NetworkReceiveEvent += PacketReceived;
		_listener.PeerConnectedEvent += (peer) =>
		{
			GD.Print("Подключение к серверу успешно!");	
			_serverPeer = peer;
		};


		_netManager = new NetManager(_listener);
		_netManager.Start();
		_netManager.Connect("localhost", 9050, "");

		_writer = new NetDataWriter();

		// EVENT BUS
		EventBus.OnHeroSelected += SendHeroSelectedToServer;
		EventBus.OnPlayerConnectedToWorld += SendPlayerConnectedToWorld;
		EventBus.OnPlayerRMB += (cords) =>
		{
			short x = (short)(cords.X * 100);
			short y = (short)(cords.Y * 100);
			short z = (short)(cords.Z * 100);

			SendMoveRequest(x, y, z);
		};

	}

	private void PacketReceived(NetPeer peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod )
	{
		
		
		PacketType rawPacket = (PacketType)reader.GetByte();
		GD.Print($"Пришел пакет: {rawPacket}, Размер: {reader.AvailableBytes + 1} байт");

		switch (rawPacket)
		{
			
			case PacketType.Welcome:

				GD.Print("Подключение к серверу успешно!");

			break;

			case PacketType.PlayerJoined:

				ushort _id = reader.GetUShort();
				CharacterType _hero = (CharacterType)reader.GetByte();
				short _x = reader.GetShort();
				short _y = reader.GetShort();
				short _z = reader.GetShort();
				PlayerJoinedPacket packet = new PlayerJoinedPacket()
				{
					
					EntityId = _id,
					SelectedHero = _hero,
					X = _x,
					Y = _y,
					Z = _z

				};
				EventBus.PublishPlayerJoined(packet);


			break;

			case PacketType.BatchEntityPositions:

				short count = reader.GetShort();

				for (short i = 0; i < count; i++)
				{
					
					ushort id = reader.GetUShort();
					short x = reader.GetShort();
					short y = reader.GetShort();
					short z = reader.GetShort();

					Vector3 pos = new Vector3(x / 100.0f, y / 100.0f, z / 100.0f);

					EventBus.PublishPositionsUpdated(id, pos);

				}

			break;

		}

	}

	private void SendHeroSelectedToServer(CharacterType hero)
	{

		_writer.Reset();
		_writer.Put((byte)PacketType.HeroSelected);
		_writer.Put((byte)hero);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
		
	}
	private void SendPlayerConnectedToWorld()
	{

		_writer.Reset();
		_writer.Put((byte)PacketType.ConnectedToGame);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);

	}
	private void SendMoveRequest(short x, short y, short z)
	{
	
		_writer.Reset();
		_writer.Put((byte)PacketType.MoveRequest);
		_writer.Put(x);
		_writer.Put(y);
		_writer.Put(z);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
		

	}


	public override void _Process(double delta)
	{
		
		_netManager.PollEvents();

	}


 
}
