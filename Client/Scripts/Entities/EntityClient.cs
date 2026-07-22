using Godot;
using System;

public partial class EntityClient : CharacterBody3D
{

	[Export] private Camera3D _camera;
	
	public enum EntityState { Idle, Move, Chase, Attack, Cast, Dead}
	public EntityState CurrentState = EntityState.Idle;

	public uint NetworkId {get; set;}
	public int Health {get; set;}
	public float CurrentSpeed {get; set;}

	public Vector3 TargetPosition {get; set;}

	public override void _PhysicsProcess(double delta)
	{
		
		switch (CurrentState)
		{
			
			case EntityState.Idle:

			break;

			case EntityState.Move:
				{
					


				}
			break;

		}

	}

	public void MakeLocalPlayer()
	{
		
		if (_camera != null)
		{
			_camera.Current = true;
		}

	}

	public virtual void MoveToPosition(Vector3 position)
	{
		
		

	}



}
