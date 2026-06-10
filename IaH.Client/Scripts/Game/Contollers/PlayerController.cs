using Godot;
using System;
using Iah.Shared.Entities;
using Iah.Shared.Packets;

public partial class PlayerController : Node
{

	public HeroEntityClient LocalPlayer {get; set;}
	
	public override void _Ready()
	{
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{

		if (LocalPlayer == null) return;

		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Right)
		{
			
			var mouseClick = GetClickPosition();

			Node collider = (Node)mouseClick["collider"].AsGodotObject();
			if (collider is BaseEntityClient enemy)
			{
				if (enemy.EntityTeam != LocalPlayer.EntityTeam)
				ClientNetworkManager.Instance.SendAttackPacket(enemy.NetID);
			}
			else
			{
				Vector3 clickPos = mouseClick["position"].AsVector3();
				ClientNetworkManager.Instance.SendMovePacket(clickPos.X, clickPos.Y, clickPos.Z);
			}
			
		}

		if (@event.IsActionPressed("skill1"))
		{
			
		

		}

	}
	private Godot.Collections.Dictionary GetClickPosition()
	{
		
	var camera = GetViewport().GetCamera3D();

	
	Vector2 mousePos = GetViewport().GetMousePosition();

	
	Vector3 origin = camera.ProjectRayOrigin(mousePos);
	Vector3 normal = camera.ProjectRayNormal(mousePos);
	Vector3 target = origin + normal * 1000f;

   
	var spaceState = GetTree().Root.GetWorld3D().DirectSpaceState;
	var query = PhysicsRayQueryParameters3D.Create(origin, target);

	var result = spaceState.IntersectRay(query);

	
	return result;

	}
}
