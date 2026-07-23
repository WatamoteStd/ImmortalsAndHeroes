using Godot;
using Shared.Network.Packets;
using Shared.Network.Packets.GamePackets;
using System;

public partial class RegionClient : Node3D
{

	public readonly Godot.Collections.Dictionary<uint, EntityClient> _regionEntities = new();
	[Export] private PackedScene _modelPrefab;
 
	public override void _Ready()
	{

		NetworkPacketManager.Instance.OnServerEnterResponse += EnterRegion;

		NetworkPacketManager.Instance.OnMovePacketReceived += OnEntityMove;

		NetworkUdpClient.Instance.SendEnterTheWorld(); // UDP REQUEST TO SERVER

	}
	

	public void AddEntity(uint id, Vector3 startPosition, ushort health)
	{

		if (_regionEntities.ContainsKey(id)) return;
		
		var entity = _modelPrefab.Instantiate<EntityClient>();
		entity.NetworkId = id;
		entity.GlobalPosition = startPosition;
		AddChild(entity);

		_regionEntities.Add(id, entity);

	}

	private void EnterRegion(S2C_RegionEnter regionPacket)
	{
		
		for (int i = 0; i < regionPacket.EntityCount; i++)
		{
			var pos = new Vector3(regionPacket.Entities[i].PositionX, regionPacket.Entities[i].PositionY, regionPacket.Entities[i].PositionZ);
			AddEntity(regionPacket.Entities[i].NetworkId, pos, regionPacket.Entities[i].Health);

			if (regionPacket.Entities[i].NetworkId == GameSession.Instance.NetworkId)
			{
				
				if ( _regionEntities.TryGetValue(regionPacket.Entities[i].NetworkId, out EntityClient character))
				{
					
					GameSession.Instance.Character = character;
					character.MakeLocalPlayer();
					

				}
				

			}

		}
		GameSession.Instance.CurrentSessionState = GameSession.State.InGame;
	}

	private void OnEntityMove(S2C_MoveEntityPacket movePacket)
	{
		
		if (_regionEntities.TryGetValue(movePacket.NetworkEntityId, out EntityClient entity))
		{
			
			var newPosition = new Vector3(movePacket.PositionX, movePacket.PositionY, movePacket.PositionZ);
			entity.MoveToPosition(newPosition);

		}

	}

	public override void _ExitTree()
	{
		if (NetworkPacketManager.Instance != null)
		{
			NetworkPacketManager.Instance.OnServerEnterResponse -= EnterRegion;
			NetworkPacketManager.Instance.OnMovePacketReceived -= OnEntityMove;
		}
	}



}
