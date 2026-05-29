using Godot;
using Iah.Shared.Packets;
using System;

public partial class BaseEntityClient : CharacterBody3D
{
	private HealthBar _healthBar;

	public ushort NetID {get; set;}
	public UnitList UnitType;
	protected Vector3 _lastPosition;
	public Vector3 TargetPosition;
	public Vector3 LookDirection;

	
	public override void _Ready()
	{
		_healthBar = GetNode<HealthBar>("HealthBar3D/SubViewport");
	}

	public void UpdateHealthBar (ushort currentHp)
	{
		_healthBar?.UpdateHealth(currentHp);
	}
	public void InitHealthBar (ushort maxHp)
	{
		_healthBar?.Init(maxHp);
	}

	
}
