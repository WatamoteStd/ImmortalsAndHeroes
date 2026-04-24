using Godot;
using System;
using IaH.Shared.Networking;
using LiteNetLib;
using LiteNetLib.Utils;

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
		EventBus.OnHeroSelected += SendHeroSelectedToServer;
		EventBus.OnPlayerConnectedToWorld += SendPlayerConnectedToWorld;

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


 // DELETE TOMORROW
private Vector3? GetMouseCoords(Camera3D cam)
{
	var mousePos = GetViewport().GetMousePosition();
	var from = cam.ProjectRayOrigin(mousePos);
	var to = from + cam.ProjectRayNormal(mousePos) * 1000f;
	var query = PhysicsRayQueryParameters3D.Create(from, to);
	var result = cam.GetWorld3D().DirectSpaceState.IntersectRay(query);

	return result.Count > 0 ? (Vector3)result["position"] : null;
}
public override void _Input(InputEvent @event)
{
  
	if (@event is InputEventMouseButton m && m.Pressed && m.ButtonIndex == MouseButton.Right)
	{
		var cam = GetTree().Root.GetCamera3D(); 
		if (cam == null) return;

		Vector3? clickPoint = GetMouseCoords(cam);

		if (clickPoint.HasValue)
		{
			// Сразу пакуем и отправляем
			short x = (short)(clickPoint.Value.X * 100);
			short y = (short)(clickPoint.Value.Y * 100);
			short z = (short)(clickPoint.Value.Z * 100);

			SendMoveRequest(x, y, z);
			GD.Print($"[NET] Отправляем: {x}, {y}, {z}");
		}
	}
}
}
