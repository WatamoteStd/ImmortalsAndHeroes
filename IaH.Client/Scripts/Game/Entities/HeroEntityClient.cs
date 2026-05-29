using Godot;
using Iah.Shared.Packets;
using System;

public partial class HeroEntityClient : BaseEntityClient
{

	public enum State {Idle, Move, Attack, Cast};
	public State CurrentState = State.Idle;
	private State _lastState = State.Idle;

	public HeroData Config {get; private set;}
	private PackedScene _projectile;
	public bool IsLocalPlayer = false;
	public Godot.Collections.Array<AbilityType> AbilityList;

	// ANIMATION
	private AnimationPlayer _animation;
	// DELETE AFTER REFAC
	private float _castTimeElapsed = 0.0f;
	private const float MAX_CAST_DURATION = 6.0f;

	public override void _Ready()
	{
		base._Ready();
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void InitHero(HeroData config)
	{
		Config = config;

		if (Config != null)
		{
			AbilityList = Config.Abilities;
		}
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

		if (CurrentState == State.Cast)
		{
			_castTimeElapsed += (float)delta;

			float progress = Mathf.Clamp(_castTimeElapsed / MAX_CAST_DURATION, 0.0f, 1.0f);

			float newSpeed = 0.5f + (progress * 2.0f);
			_animation.SpeedScale = newSpeed;
		}

        
		State newState = Position.DistanceTo(TargetPosition) > 0.05f
		? State.Move
		: State.Idle;

		if (newState != _lastState)
		{
			_lastState = newState;
			CurrentState = newState;
			UpdateAnimation(newState);

		}

    }

	// SKILLS & ANIMATIONS


	private void UpdateAnimation(State state)
	{
		if (CurrentState == State.Cast) return;

		if (state == State.Idle)  _animation.Play("Idle");

		if (state == State.Move) _animation.Play("move");

	}



}

