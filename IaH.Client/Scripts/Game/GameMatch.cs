using Godot;
using Iah.Shared.Entities;
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



	public void AddEntity(ushort id, UnitList unitType, short x, short y, short z)
	{
		// SAFE CHECK
		var unitModel = _entityRegistor.GetModel(unitType);
		if (unitModel == null) return;

		// INSTANCE AND INIT'S

		var entity = unitModel.Instantiate<BaseEntityClient>();
		entity.NetID = id;
		entity.UnitType = unitType;
		entity.InitStats(EntityRegistry.GetStats(unitType));

		Vector3 targetPosition = new Vector3(x / 100f, y / 100f, z / 100f);
		entity.Position = targetPosition;

		IdToEntity[id] = entity;
		_entitiesContainer.CallDeferred("add_child", entity);


		// PLAYER CONTROL & LOCAL PLAYER SELECT
		if (entity is HeroEntityClient hero)
		{
			
			if (id == ClientNetworkManager.Instance.LocalID) hero.IsLocalPlayer = true;

			_playerController.LocalPlayer = hero;
		}
		
		

	}
	
}
