using Godot;
using Iah.Shared.Packets;
using Iah.Shared.Entities;
using System;

public partial class BaseEntityClient : CharacterBody3D
{
	// STATES ===========================
	
	public enum State {Idle, Move, Attack, Cast};
	public State CurrentState = State.Idle;



	// STATS==========================
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

	// STATE MACHINE ========================================
	public override void _Process(double delta)
	{

		switch (CurrentState)
		{
			
			case State.Idle:
				{
					
				}
				break;

			case State.Move:
				{
					
					if (Position.DistanceTo(TargetPosition) > 0.001f)
					{
						Position = Position.Lerp(TargetPosition, (float)delta * 15.0f);
					}
					else
					{
						CurrentState = State.Idle;
					}
					if (LookDirection.Length() > 0.1f)
					{
			
						var targetTransform = Transform.LookingAt(Position + LookDirection, Vector3.Up);

						GlobalTransform = GlobalTransform.InterpolateWith(targetTransform, (float)delta * 10.0f);

					}

				}	
				break;



		}

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
