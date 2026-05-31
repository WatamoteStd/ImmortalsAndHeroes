using Godot;
using Iah.Shared.Packets;
using System;

public partial class HeroEntityClient : BaseEntityClient
{

	public enum State {Idle, Move, Attack, Cast};
	public State CurrentState = State.Idle;
	private State _lastState = State.Idle;

	private PackedScene _projectile;
	public bool IsLocalPlayer = false;


	public override void _Ready()
	{
		base._Ready();
		
	}

	public override void _Process(double delta)
	{
		

		if (Position.DistanceTo(TargetPosition) > 0.001f)
		{
			Position = Position.Lerp(TargetPosition, (float)delta * 15.0f);
			
		}
		if (LookDirection.Length() > 0.1f)
		{
			
			var targetTransform = Transform.LookingAt(Position + LookDirection, Vector3.Up);

			GlobalTransform = GlobalTransform.InterpolateWith(targetTransform, (float)delta * 10.0f);

		}

		
		State newState = Position.DistanceTo(TargetPosition) > 0.05f
		? State.Move
		: State.Idle;

		if (newState != _lastState)
		{
			_lastState = newState;
			CurrentState = newState;

		}

	}

	// SKILLS & ANIMATIONS


}
