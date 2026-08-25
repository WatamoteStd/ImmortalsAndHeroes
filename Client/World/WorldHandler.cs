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
	[Export] private PackedScene _entityScene;
	[Export] private PackedScene _floatDamageScene;

	public Dictionary<uint, Entity> RegionEntities {get; private set; } = new Dictionary<uint, Entity>();

	public override void _Ready()
	{
		ServerMaster.Instance.WorldManager = this;
	}


	public void LOC_StateSync(in S2C_StatsSyncPacket packet)
	{
		
		if (RegionEntities.TryGetValue(GameSession.Instance.NetworkId, out Entity entity))
		{
			
			if (entity is LocalPlayerEntity player)
			{
				player.UpdateStats(packet);
			}
		}

	}



	#region  Entities interaction

	public void AddEntity(S2C_SpawnEntityPacket data)
	{
		if (RegionEntities.ContainsKey(data.Id)) return;
		
		if (data.Type.IsPlayer())
		{
			SpawnRemotePlayer(data);
		}
		else if (data.Type.IsMob())
		{
			SpawnEntity(data);
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

	private void SpawnEntity(S2C_SpawnEntityPacket data)
	{
		
		PackedScene entityModelScene = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(data.Type).ScenePath);
		var entityModel = entityModelScene.Instantiate<Node3D>();

		var newEntity = _entityScene.Instantiate<Entity>();
		AddChild(newEntity);
		newEntity.GetNode<Node3D>("Model").AddChild(entityModel);

		GD.Print($"[SPAWN ENTITY] ID: {data.Id}, Type: {data.Type}, POS: {data.PosX}, {data.PosY}, {data.PosZ}");
		Vector3 pos = new Vector3(data.PosX, data.PosY, data.PosZ);
		newEntity.InitEntity(data.Id, data.Health, data.Health, data.Name, data.Type, pos);
		newEntity.Move(pos);


		RegionEntities.Add(newEntity.Id, newEntity);

	}


	private void SpawnRemotePlayer(S2C_SpawnEntityPacket data)
	{
		
		PackedScene playerModelScene = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(data.Type).ScenePath);
		var playerModel = playerModelScene.Instantiate<Node3D>();

		var newPlayer = _remotePlayerScene.Instantiate<PlayerEntity>();
		AddChild(newPlayer);
		newPlayer.GetNode<Node3D>("Model").AddChild(playerModel);

		Vector3 pos = new Vector3(data.PosX, data.PosY, data.PosZ);
		newPlayer.InitEntity(data.Id, data.Health, data.Health, data.Name, data.Type, pos);
		newPlayer.Move(pos);


		RegionEntities.Add(newPlayer.Id, newPlayer);

	}

	public void SpawnLocalPlayer(S2C_HandshakeSuccessPacket playerPacket)
	{
		
		PackedScene model = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(playerPacket.Type).ScenePath);
		var localPlayer = _localPlayerScene.Instantiate<LocalPlayerEntity>();
		var locPlayerModel = model.Instantiate<Node3D>();
		
		AddChild(localPlayer);
		localPlayer.GetNode<Node3D>("Model").AddChild(locPlayerModel);

		Vector3 dataPos = new Vector3(playerPacket.PosX, playerPacket.PosY, playerPacket.PosZ);
		localPlayer.InitEntity(playerPacket.Id, playerPacket.CurrentHp, playerPacket.CurrentHp, playerPacket.Name, playerPacket.Type, dataPos, playerPacket.UserId, playerPacket.CurrentMp, playerPacket.CurrentMp);
		localPlayer.Move(dataPos);

		RegionEntities.Add(localPlayer.Id, localPlayer);

		SceneManager.Instance.InitPlayerHud((uint)playerPacket.CurrentHp, (uint)playerPacket.CurrentMp, playerPacket.Silver, playerPacket.Name);
		


	}

	public void MoveEntity(uint id, float x, float y, float z)
	{
		Vector3 movePos = new Vector3(x, y, z);
		
		if (RegionEntities.TryGetValue(id, out Entity entity))
		{
			if (entity.GlobalPosition.DistanceSquaredTo(movePos) > 2.25f)
			{
				entity.Move(movePos);
			}
		}

	}

	public void EntityTakeDamage(uint entityId, float damage, uint attackerId, uint actualHealth)
	{
		
		if (RegionEntities.TryGetValue(entityId, out Entity entity))
		{
			
			if (entity is LocalPlayerEntity localPlayer)
			{
				SceneManager.Instance.PlayerHud.UpdateHealth(actualHealth);
			}

			entity.TakeDamage(damage, (int)actualHealth);

			// FLOATING TEXT
			var damageText = _floatDamageScene.Instantiate<DamageNumber>();

			AddChild(damageText);
			damageText.Setup(damage, entity.GlobalPosition + new Vector3(0, 1.5f, 0));

		}

	}

	#endregion

	public override void _ExitTree()
	{
		
		if (ServerMaster.Instance?.WorldManager == this)
		{
			ServerMaster.Instance.WorldManager = null;
		}

	}



}
