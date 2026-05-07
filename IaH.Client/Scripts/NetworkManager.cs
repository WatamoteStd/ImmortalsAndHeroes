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
		EventBus.OnPlayerRMB += (cords) =>
		{
			short x = (short)(cords.X * 100);
			short y = (short)(cords.Y * 100);
			short z = (short)(cords.Z * 100);

			SendMoveRequest(x, y, z);
		};

		// MENU | LOBBY
		EventBus.OnJoinTheQueue += SendJoinQueue;
		EventBus.OnSendMessageToLobby += SendMessageLobby;
		EventBus.OnNicknameChanged += SendNickname;
	}

	private void PacketReceived(NetPeer peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod )
	{
		
		
		PacketType rawPacket = (PacketType)reader.GetByte();

		switch (rawPacket)
		{

			case PacketType.ChatMessage:

				string sender = reader.GetString();
				string message = reader.GetString();
				EventBus.PublishOnMessageReceived(sender, message);

			break;

			case PacketType.LobbyJoined:
				GD.Print("[NetworkManager] Connected to lobby!");
				GetTree().ChangeSceneToFile("res://Scenes/PickStage/Lobby.tscn");

			break;
			
			case PacketType.PlayerJoined:

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

			case PacketType.EntityStats:
				
				EntityStatsPacket newPacket = new EntityStatsPacket();
				newPacket.Deserialize(reader);
				EventBus.PublishStatsPacketReceived(newPacket);
			break;

			case PacketType.EntityRemove:
				{
					
					ushort EntityId = reader.GetUShort();
					EventBus.PublishDisconnectedPacketReceived(EntityId);

				}
			break;
		}
	}

	// MENU

	private void SendNickname(string nick)
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.ChangeNickname);
		_writer.Put(nick);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
	}


	// GAMEPLAY

	private void SendMoveRequest(short x, short y, short z)
	{
	
		_writer.Reset();
		_writer.Put((byte)PacketType.MoveRequest);
		_writer.Put(x);
		_writer.Put(y);
		_writer.Put(z);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
		

	}
	// LOBBY
	private void SendJoinQueue()
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.JoinQueue);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
	}
	private void SendMessageLobby(string text)
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.ChatMessage);
		_writer.Put(text);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
	}


	public override void _Process(double delta)
	{
		
		_netManager.PollEvents();

	}

	
}
