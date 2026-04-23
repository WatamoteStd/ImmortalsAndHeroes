using Godot;
using System;

public partial class World : Node3D
{
	
	public override void _Ready()
	{
		
		EventBus.PublishPlayerConnectedToWorld();

	}

	
	public override void _Process(double delta)
	{
	}
}
