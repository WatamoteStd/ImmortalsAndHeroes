using Godot;
using System;

public partial class CameraControl : Camera3D
{
	[Export] public float Sensitivity = 0.05f; // Чувствительность мыши
	private bool _isDragging = false;

	public override void _Input(InputEvent @event)
	{
		
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Middle)
			{
				_isDragging = mouseButton.Pressed;
			}
			if (mouseButton.ButtonIndex == MouseButton.Right && mouseButton.Pressed)
			{
				var cords = GetMouseClickCoords();
				GD.Print($"CameraContorl| MouseRMB: {cords.Value}");
				if (cords.HasValue) {
				EventBus.PublishPlayerRMB(cords.Value);
				GD.Print("CameraControl| Cords succesfully send to NetworkMabager!");
				}

			}
		}
	

		
		if (@event is InputEventMouseMotion mouseMotion && _isDragging)
		{
			
			Vector3 offset = new Vector3(-mouseMotion.Relative.X, 0, -mouseMotion.Relative.Y) * Sensitivity;
			
			
		   	Basis basis = Transform.Basis;
			Vector3 direction = (basis.X * offset.X) + (new Vector3(basis.Z.X, 0, basis.Z.Z).Normalized() * offset.Z);
			
			GlobalPosition += direction;
		}
	}
	private const float RayLength = 1000.0f;

	public Vector3? GetMouseClickCoords()
	{
	
		var mousePos = GetViewport().GetMousePosition();
		var from = ProjectRayOrigin(mousePos);
		var to = from + ProjectRayNormal(mousePos) * RayLength;

		
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(from, to);
		
		// Тут можно указать маску коллизий, чтобы луч бился только в пол
		// query.CollisionMask = 1; 

		var result = spaceState.IntersectRay(query);

	 
		if (result.Count > 0)
		{
			return (Vector3)result["position"];
		}

		return null;
	}

	
}
