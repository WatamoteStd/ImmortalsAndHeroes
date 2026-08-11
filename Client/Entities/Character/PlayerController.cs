using Godot;
using System;

public partial class PlayerController : Node
{
	
	[Export] private Camera3D _camera;
	[Export] private RayCast3D _raycast;
	[Export] private LocalPlayerEntity _player;

	public static Action<Vector3> OnMoveRequest;

	public static Action OnInventoryAction;

	private void SendMoveToServer(Vector3 targetPos)
	{
		OnMoveRequest?.Invoke(targetPos);
	}


	public override void _UnhandledInput(InputEvent @event)
{
	if (@event is InputEventMouseButton mouseAction)
	{
		
		if (mouseAction.Pressed && mouseAction.ButtonIndex == MouseButton.Right)
		{
			Vector3? clickPoint = GetClickPoint();

			if (clickPoint.HasValue)
			{
				GD.Print($"[RAW CLICK] X={clickPoint.Value.X:F2}, Z={clickPoint.Value.Z:F2}");
				
				
				OnMoveRequest?.Invoke(clickPoint.Value);
			}
			else GD.Print("[RAW CLICK] Null click, i get it");
		}
	}

	if (@event.IsActionPressed("Inventory"))
		{
			OnInventoryAction?.Invoke();
		}


}


	private Vector3? GetClickPoint()
	{
		
		var mousePos = GetViewport().GetMousePosition();

		Vector3 origin = _camera.ProjectRayOrigin(mousePos);
		Vector3 normal = _camera.ProjectRayNormal(mousePos);

		_raycast.GlobalPosition = origin;

		_raycast.TargetPosition = normal * 100;

		_raycast.ForceRaycastUpdate();

		if (_raycast.IsColliding())
		{
			
			Vector3 clickPoint = _raycast.GetCollisionPoint();
			return clickPoint;

		}
		else return null;

	}


}
