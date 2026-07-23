using Godot;
using System;

public partial class EntityClient : CharacterBody3D
{

	private Camera3D _camera;
	private RayCast3D _cameraRay;
	private PlayerController _playerContoller;
	
	public enum EntityState { Idle, Move, Chase, Attack, Cast, Dead}
	public EntityState CurrentState = EntityState.Idle;

	public uint NetworkId {get; set;}
	public int Health {get; set;}
	public float CurrentSpeed = 1.0f;

	public Vector3 _targetPosition {get; private set;}

	public override void _Ready()
	{
		
		_camera = GetNode<Camera3D>("Camera3D");
		_cameraRay = GetNode<RayCast3D>("Camera3D/RayCast3D");

	}


	public override void _PhysicsProcess(double delta)
	{
		
		switch (CurrentState)
		{
			
			case EntityState.Idle:

			break;

			case EntityState.Move:
				{
					
					GlobalPosition = GlobalPosition.Lerp(_targetPosition, (float)delta * 8.0f);

					if (GlobalPosition.DistanceTo(_targetPosition) < 0.1f)
					{
						
						GlobalPosition = _targetPosition;
						CurrentState = EntityState.Idle;

					}

				}
			break;

		}

	}

	public void MakeLocalPlayer()
	{
		
		if (_camera != null || _cameraRay != null) 
		{

			_camera.Current = true;

			_playerContoller = new PlayerController();
			_playerContoller.Initialize(_camera, _cameraRay);
			AddChild(_playerContoller);

		}

	}

	public virtual void MoveToPosition(Vector3 position)
	{
		
		CurrentState = EntityState.Move;
		_targetPosition = position;

	}



}
