using Godot;
using System;

public partial class CameraControl : Camera3D
{
	[Export] public float Sensitivity = 0.05f; // Чувствительность мыши
	private bool _isDragging = false;

	public override void _Input(InputEvent @event)
	{
		// Проверяем нажатие колесика мыши
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Middle)
			{
				_isDragging = mouseButton.Pressed;
			}
		}

		// Если зажато и мышь движется
		if (@event is InputEventMouseMotion mouseMotion && _isDragging)
		{
			// Двигаем "хаб" камеры
			// Инвертируем значения, чтобы движение было естественным (тянем карту)
			Vector3 offset = new Vector3(-mouseMotion.Relative.X, 0, -mouseMotion.Relative.Y) * Sensitivity;
			
			// Чтобы камера двигалась относительно своего поворота, а не глобальных осей
		   	Basis basis = Transform.Basis;
			Vector3 direction = (basis.X * offset.X) + (new Vector3(basis.Z.X, 0, basis.Z.Z).Normalized() * offset.Z);
			
			GlobalPosition += direction;
		}
	}
	/*private const float RayLength = 1000.0f;

	public Vector3? GetMouseClickCoords()
	{
		// 1. Получаем позицию мыши на экране
		var mousePos = GetViewport().GetMousePosition();
		var from = ProjectRayOrigin(mousePos);
		var to = from + ProjectRayNormal(mousePos) * RayLength;

		// 3. Создаем параметры запроса
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
	}*/

	
}
