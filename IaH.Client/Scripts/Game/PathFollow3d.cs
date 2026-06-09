using Godot;
using System;

public partial class PathFollow3d : PathFollow3D
{
	[Export] public float Speed = 0.03f;
	
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		
		ProgressRatio += Speed * (float)delta;
		
		if (ProgressRatio >= 1.0f)
		{
			ProgressRatio = 1.0f;
			SetProcess(false);
		}
	}
}
