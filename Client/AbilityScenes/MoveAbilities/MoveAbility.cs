using Godot;
using System;

public partial class MoveAbility : AbilityBase
{

	private float _speed;
	private Entity _targetEntity;
	private Vector3 _targetPosition;


	public override void Setup(float lifeTIme, float speed = 0, Vector3 targetPos = default, Entity targetEntity = null)
	{
		base.Setup(lifeTIme, speed, targetPos, targetEntity);

		_speed = speed;
		_targetPosition = targetPos;
		_targetEntity = targetEntity;


	}

	public override void _Process(double delta)
	{
		
		if (_speed <= 0) return;

		Vector3 destination = IsInstanceValid(_targetEntity) ? _targetEntity.GlobalPosition : _targetPosition;

		GlobalPosition = GlobalPosition.MoveToward(destination, _speed * (float)delta);

	}


	

}
