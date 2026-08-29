using Godot;
using System;

public partial class AbilitySceneBase : Node3D
{

	public override void _Ready()
	{
		
		GetTree().CreateTimer(5f).Timeout += () => {QueueFree();};

	}

	
}
