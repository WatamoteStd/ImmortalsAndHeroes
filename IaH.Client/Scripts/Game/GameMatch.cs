using Godot;
using Iah.Shared.Packets;
using System;
using System.Collections.Generic;

public partial class GameMatch : Node
{
	private readonly Dictionary<ushort, BaseEntityClient> IdToEntity = new();
	[Export] private EntityRegistor _entityRegistor;
	[Export] private Node3D _entitiesContainer;
	[Export] private PlayerController _playerController;
	
	public override void _Ready()
	{
		
		ClientNetworkManager.Instance.RegisterMatch(this);
	}


	public void UpdateHealth(ushort actualHp, ushort targetId)
	{
		var entity = IdToEntity[targetId];
		entity.UpdateHealthBar(actualHp);
	}


	public void UpdateEntityPosition(ushort id, float x, float y, float z, Vector3 lookDir)
	{
	
		if (IdToEntity.TryGetValue(id, out var entity))
		{
			
			entity.TargetPosition = new Vector3(x,y,z);
			entity.LookDirection = lookDir;
			
		}
	}	



	public void AddEntity(ushort id, byte unitType, short x, short y, short z, ushort health)
	{

		var config = _entityRegistor.GetConfig((UnitList)unitType);
		if (config == null) return;

		GD.Print($"Trying to spawn entity {id}, type {unitType}. Config found: {config != null}");

		var entity = config.Model.Instantiate<BaseEntityClient>();
		entity.NetID = id;
		entity.UnitType = (UnitList)unitType;



		Vector3 targetPosition = new Vector3(x / 100f, y / 100f, z / 100f);
		entity.Position = targetPosition;

		IdToEntity[id] = entity;
		_entitiesContainer.CallDeferred("add_child", entity);
		entity.CallDeferred(nameof(BaseEntityClient.InitHealthBar), health);

		if (entity is HeroEntityClient hero)
		{
			hero.InitHero(config);
			if (id == ClientNetworkManager.Instance.LocalID) hero.IsLocalPlayer = true;

			_playerController.LocalPlayer = hero;
		}
		
		

	}
	
}
