using Godot;
using System;

public partial class CameraHolder : Node3D
{
	[Export] public float _speed;
	private bool _isDragging;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Middle)
			{
				_isDragging = mouseButton.Pressed;
			}
		}
		if (_isDragging && @event is InputEventMouseMotion mouseMotion)
		{
			Vector3 offset = new Vector3(-mouseMotion.Relative.X, 0, -mouseMotion.Relative.Y) * _speed * 0.01f;
			GlobalPosition += offset;
		}
	}


	
	public override void _Process(double delta)
	{
	}
}
