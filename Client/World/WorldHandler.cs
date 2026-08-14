using Godot;
using Shared.Characters;
using Shared.Udp.Packets.Category;
using Shared.Udp.Packets.Category.Game;
using System;
using System.Collections.Generic;

public partial class WorldHandler : Node3D
{
	
	public uint RegionId {get; private set;} = 0;
	[Export] private PackedScene _remotePlayerScene;
	[Export] private PackedScene _localPlayerScene;

	public Dictionary<uint, Entity> RegionEntities {get; private set; }= new Dictionary<uint, Entity>();

	public override void _Ready()
	{
		ServerMaster.Instance.WorldManager = this;
	}

	public void AddEntity(S2C_SpawnEntityPacket data)
	{
		if (RegionEntities.ContainsKey(data.Id)) return;
		
		if (data.Type.IsPlayer())
		{
			SpawnRemotePlayer(data);
		}

	}
	public void RemoveEntity(uint entityId)
	{
		
		if (RegionEntities.TryGetValue(entityId, out Entity entity))
		{
			RegionEntities.Remove(entityId);
			entity.QueueFree();

		}

	}


	private void SpawnRemotePlayer(S2C_SpawnEntityPacket data)
	{
		
		PackedScene playerModelScene = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(data.Type).ScenePath);
		var playerModel = playerModelScene.Instantiate<Node3D>();

		var newPlayer = _remotePlayerScene.Instantiate<PlayerEntity>();
		AddChild(newPlayer);
		newPlayer.GetNode<Node3D>("Model").AddChild(playerModel);

		newPlayer.InitEntity(data.Id, data.Health, data.Health, data.Name, data.Type);
		Vector3 pos = new Vector3(data.PosX, data.PosY, data.PosZ);
		newPlayer.GlobalPosition = pos;

		RegionEntities.Add(newPlayer.Id, newPlayer);

	}

	public void SpawnLocalPlayer(S2C_HandshakeSuccessPacket playerPacket)
	{
		
		PackedScene model = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(playerPacket.Type).ScenePath);
		var localPlayer = _localPlayerScene.Instantiate<LocalPlayerEntity>();
		var locPlayerModel = model.Instantiate<Node3D>();
		
		AddChild(localPlayer);
		localPlayer.GetNode<Node3D>("Model").AddChild(locPlayerModel);

		Vector3 dataPos = new Vector3(playerPacket.PosX + 2, playerPacket.PosY, playerPacket.PosZ + 2);
		localPlayer.InitEntity(playerPacket.Id, playerPacket.CurrentHp, playerPacket.CurrentHp, playerPacket.Name, playerPacket.Type, playerPacket.UserId, playerPacket.CurrentMp, playerPacket.CurrentMp);
		localPlayer.GlobalPosition = dataPos;

		RegionEntities.Add(localPlayer.Id, localPlayer);

		SceneManager.Instance.InitPlayerHud((uint)playerPacket.CurrentHp, (uint)playerPacket.CurrentMp, playerPacket.Silver, (uint)playerPacket.Lvl, playerPacket.Name);
		


	}

	public void MoveEntity(uint id, float x, float y, float z)
	{
		
		if (RegionEntities.TryGetValue(id, out Entity entity))
		{
			Vector3 movePos = new Vector3(x, y, z);
			
			entity.Move(movePos);

		}

	}

	public override void _ExitTree()
	{
		
		if (ServerMaster.Instance?.WorldManager == this)
		{
			ServerMaster.Instance.WorldManager = null;
		}

	}



}
