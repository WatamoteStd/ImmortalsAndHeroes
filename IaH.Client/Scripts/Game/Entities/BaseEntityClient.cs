using Godot;
using Iah.Shared.Packets;
using Iah.Shared.Entities;
using System;

public partial class BaseEntityClient : CharacterBody3D
{
	private HealthBar _healthBar;
	private EntityStats Stats;

	// NETWORK INFO
	public ushort NetID {get; set;}
	public UnitList UnitType;
	public Team EntityTeam;
	protected Vector3 _lastPosition;
	public Vector3 TargetPosition;
	public Vector3 LookDirection;

	
	public override void _Ready()
	{
		_healthBar = GetNode<HealthBar>("HealthBar3D/SubViewport");
		
		Callable.From(() => InitHealthBar((ushort)Stats.MaxHealth)).CallDeferred();
	}



	// HEALTH BAR & STATS LOGIC

	public void UpdateHealthBarVisual(bool isAlly)
	{

		if (isAlly)
		{
			_healthBar.UpdateColor(true);
		}
		else _healthBar.UpdateColor(false);
		
	}
	public void UpdateHealthBar (ushort currentHp)
	{
		_healthBar?.UpdateHealth(currentHp);
	}
	public void InitHealthBar (ushort maxHp)
	{
		_healthBar?.Init(maxHp);
		
	}

	public void InitStats(EntityStats stats)
	{
		Stats = stats;
	}

	
}
