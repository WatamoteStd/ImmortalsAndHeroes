using Godot;
using System;
using LiteNetLib;
using LiteNetLib.Utils;
using Iah.Shared.Packets;
using System.Collections.Generic;
public partial class ClientNetworkManager : Node
{

	public GameMatch CurrentMatch {get; private set;}

	// CASH FOR LOBBY
	public int CachedLobbyId {get; private set;} = new();
	public Godot.Collections.Dictionary<int, string> CachedPlayersInLobby {get; private set;} = new();

	public Godot.Collections.Dictionary<int, UnitList> CachedPlayersHeroes { get; private set; } = new();
	public ushort LocalID {get; private set;}
	public static ClientNetworkManager Instance {get; private set;}

	// SERVER PART
	private NetManager _netManager;
	private EventBasedNetListener _listener;
	private NetDataWriter _writer;
	private NetPeer _serverPeer;
	
	public override void _Ready()
	{	
		Instance = this;
		
		// INIT AND LISTENER SUBS
		_listener = new EventBasedNetListener();
		_listener.NetworkReceiveEvent += OnPacketReceived;
		_netManager = new NetManager(_listener);
		_writer = new NetDataWriter();

		// MANAGER + OTHER SUBS ===========
		_netManager.Start();
		_netManager.Connect("localhost", 9050, "");

		// EVENT BUS ==================================
		EventBus.Instance.JoinQueueRequested += StandInQueue;
		EventBus.Instance.LoginRequest += (type, name) =>
		{
			_writer.Reset();
			_writer.Put((byte)type);
			_writer.Put(name);
			_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
		};
		EventBus.Instance.LobbyHeroSelected += (hero) =>
		{
			_writer.Reset();
			_writer.Put((byte)PacketType.HeroSelected);
			_writer.Put((byte)hero);
			_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
		};

		// GAME EVENT BUS==========================================
		EventBus.Instance.ReadyToGame += SendReadyToGame;

	}

	private void OnPacketReceived (NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
	{
		
		PacketType Type = (PacketType)reader.GetByte();
		
		// SYSTEM | MENU & LOBBY PACKETS
		switch (Type)
		{
			
			case PacketType.FirstConnect:
				_serverPeer = peer;
			break;

			case PacketType.LoginResponse:
				LocalID = reader.GetUShort();
			break;

			case PacketType.LobbyFind:
				GetTree().ChangeSceneToFile("res://Scenes/Lobby/Lobby.tscn");
			break;
			
			case PacketType.ConnectToLobby:

				ushort lobbyId = reader.GetUShort();
				byte count = reader.GetByte();
				CachedLobbyId = lobbyId;

				for (int i = 0; i < count; i++)
				{
					
					ushort pID = reader.GetUShort();
					string pName = reader.GetString();

					CachedPlayersInLobby[pID] = pName;

				}

			break;
			case PacketType.HeroSelected:

				ushort playerID = reader.GetUShort();
				UnitList playerHero = (UnitList)reader.GetByte();
				CachedPlayersHeroes[playerID] = playerHero;
				EventBus.Instance.PublishLobbyHeroChanged();
			break;
			case PacketType.LobbyTimer:

				ushort time = reader.GetByte();
				EventBus.Instance.PublishLobbyTimer(time);

			break;
			case PacketType.ConnectedToGame:
			GetTree().ChangeSceneToFile("res://Scenes/Game/Map.tscn");
			break;
			
			// GAME PACKETS

			case PacketType.SpawnEntity:
			{
				ushort entityID = reader.GetUShort();
				byte unitType = reader.GetByte();
				ushort maxHealth = reader.GetUShort();
				short cordX = reader.GetShort();
				short cordY = reader.GetShort();
				short cordZ = reader.GetShort();
				CurrentMatch.AddEntity(entityID, unitType, cordX, cordY, cordZ, maxHealth);
				
			}
				break;

			case PacketType.EntityMove:
				{
					
					ushort entityID = reader.GetUShort();
					int x = reader.GetInt();
					int y = reader.GetInt();
					int z = reader.GetInt();
					short dirX = reader.GetShort();
					short dirY = reader.GetShort();
					short dirZ = reader.GetShort();

					Vector3 lookDirection = new Vector3(dirX / 100f, dirY / 100f, dirZ / 100f);

					float posX = x / 100f;
					float posY = y / 100f;
					float posZ = z / 100f;

					CurrentMatch.UpdateEntityPosition(entityID, posX, posY, posZ, lookDirection);

				}
			break;

			case PacketType.AttackInfo:
				{
					
					ushort targetId = reader.GetUShort();
					short damage = reader.GetShort();
					ushort actualHp = reader.GetUShort();
					CurrentMatch.UpdateHealth(actualHp, targetId);

				}
			break;
			

		}
		
		reader.Recycle();


	}

	// FOR NO DATA PACKETS
	private void StandInQueue(byte packet)
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.JoinQueue);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);

	}

	// GAME PACKETS SEND
	private void SendReadyToGame()
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.ReadyToGame);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
	}
	public void SendMovePacket(float x, float y, float z)
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.EntityMove);
		_writer.Put((int)(x * 100f));
		_writer.Put((int)(y * 100f));
		_writer.Put((int)(z * 100f));
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);

	}
	public void SendAttackPacket(ushort id)
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.AutoAttack);
		_writer.Put((ushort)id);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
	}

	// SKILLS 
	public void SendSkillExecute(byte index)
	{
		_writer.Reset();
		_writer.Put((byte)PacketType.SkillExecuteSelf);
		_writer.Put(index);
		_serverPeer.Send(_writer, DeliveryMethod.ReliableOrdered);
	}


	// GAME
	public void RegisterMatch(GameMatch match)
	{
		CurrentMatch = match;
	}
	public void UnregisterMatch()
	{
		CurrentMatch = null;
	}

	
	public override void _Process(double delta)
	{
		_netManager.PollEvents();
	}
}
