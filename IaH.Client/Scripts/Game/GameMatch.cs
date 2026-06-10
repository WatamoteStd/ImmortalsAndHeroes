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

	public Godot.Collections.Dictionary<ushort, ushort> EntityIdToPlayer {get; private set;} = new();

	private Team CachedLocalTeam = ClientNetworkManager.Instance.CachedPlayersTeam[ClientNetworkManager.Instance.LocalID];
	
	public override void _Ready()
	{
		
		ClientNetworkManager.Instance.RegisterMatch(this);
	}

	public void RefreshAllEntityTeamColors() 
	{

		foreach (var entity in IdToEntity.Values)
		{
			if (entity.EntityTeam == CachedLocalTeam) entity.UpdateHealthBarVisual(true);
			else entity.UpdateHealthBarVisual(false);
		}

	}
	public void ColorizeEntity(BaseEntityClient entity)
	{
		if (entity.EntityTeam == CachedLocalTeam) entity.UpdateHealthBarVisual(true);
		else entity.UpdateHealthBarVisual(false);
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
			entity.CurrentState = BaseEntityClient.State.Move;
			
		}
	}	



	public void AddEntity(ushort id, UnitList unitType, short x, short y, short z, Team team, ushort playerID)
	{
		// SAFE CHECK
		var unitModel = _entityRegistor.GetModel(unitType);
		if (unitModel == null) return;

		// INSTANCE AND INIT'S

		var entity = unitModel.Instantiate<BaseEntityClient>();
		entity.NetID = id;

		// UNIT TYPE & TEAM
		entity.UnitType = unitType;
		
		entity.EntityTeam = team;

		entity.InitStats(EntityRegistry.GetStats(unitType));

		Vector3 targetPosition = new Vector3(x / 100f, y / 100f, z / 100f);
		entity.Position = targetPosition;

		IdToEntity[id] = entity;
		_entitiesContainer.CallDeferred("add_child", entity);
		Callable.From(() => ColorizeEntity(entity)).CallDeferred();


		// PLAYER CONTROL & LOCAL PLAYER SELECT
		if (entity is HeroEntityClient hero)
		{
			
			if (playerID == ClientNetworkManager.Instance.LocalID) {
				hero.IsLocalPlayer = true;
				_playerController.LocalPlayer = hero;
			}
			
		}
		EntityIdToPlayer[id] = playerID;

		
	}
	
}
