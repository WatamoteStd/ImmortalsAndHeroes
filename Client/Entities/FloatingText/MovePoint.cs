using Godot;
using System;

public partial class MovePoint : Node3D
{
	
	[Export] private AnimationPlayer _animator;

	public void OnClickRMB()
	{
		
		_animator.Play("Click");

	}
	
}
