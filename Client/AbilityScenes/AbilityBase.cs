using Godot;
using System;

public partial class AbilityBase : Node3D
{

	protected float _lifeTime;

	public override void _Ready()
	{
		
		
	}

	public virtual void Setup(float lifeTIme, float speed = 0f, Vector3 targetPos = default, Entity targetEntity = null)
	{
		
		_lifeTime = lifeTIme;
		GetTree().CreateTimer(lifeTIme).Timeout += () =>
		{
			if (IsInstanceValid(this)) QueueFree();
		};

	}

	
}
