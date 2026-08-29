using Godot;
using System;

public partial class PlayerController : Node
{
	
	[Export] private Camera3D _camera;
	[Export] private RayCast3D _raycast;
	[Export] private LocalPlayerEntity _player;
	[Export] private PlayerAbilityController _abilityController;

	private Entity _selectedEntity = null;

	public static Action<Vector3> OnMoveRequest;

	public static Action OnInventoryAction;

	private void SendMoveToServer(Vector3 targetPos)
	{
		OnMoveRequest?.Invoke(targetPos);
	}

	public override void _Ready()
	{
		if (_player != null && _raycast != null)
		{
			_raycast.AddException(_player);
		}
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
				OnMoveRequest?.Invoke(clickPoint.Value);
			}
			else GD.Print("[RAW CLICK] Null click, i get it");
		}

		if (mouseAction.Pressed && mouseAction.ButtonIndex == MouseButton.Left)
			{
				
				GodotObject obj = GetClickCollision();

				if (obj is Entity entity && entity is not LocalPlayerEntity)
				{

					bool wasAlreadySelected = (_selectedEntity == entity);
					if (IsInstanceValid(_selectedEntity)) _selectedEntity.DeselectEntity();

					_selectedEntity = entity;
					SceneManager.Instance.ShowSelectedEntityWindow(_selectedEntity);
					entity.SelectEntity();

					if (SettingsManager.Instance.AttackOnFirstLmb || wasAlreadySelected)
					{
						ServerMaster.Instance.LP_AttackRequest(_selectedEntity.Id);
					}
				
				}
				else
				{
					if (IsInstanceValid(_selectedEntity))
					{
						_selectedEntity.DeselectEntity();
					}
					_selectedEntity = null;
					SceneManager.Instance.HideSelectedEntityWindow();
				}

			}
	}

	for(byte i = 0; i < 6; i++)
			{
				
				if (@event.IsActionPressed($"Skill_{i}"))
				{
					
					Vector3 point = GetClickPoint() ?? Vector3.Zero;
					Entity target = IsInstanceValid(_selectedEntity) ? _selectedEntity : null;

					_abilityController.ExecuteSkill(i,point,target);

				}

			}


	if (@event.IsActionPressed("Inventory"))
	{
		OnInventoryAction?.Invoke();
	}

	if (@event.IsActionPressed("MastryTree"))
	{
		SceneManager.Instance.SwitchVisiblityMasteryTree();
	}

	
	if (@event.IsActionPressed("Console"))
	{
		SceneManager.Instance.ConsoleWindow.ChangeVisible();
	}

}


	private Vector3? GetClickPoint()
	{
		
		var mousePos = GetViewport().GetMousePosition();

		Vector3 origin = _camera.ProjectRayOrigin(mousePos);
		Vector3 normal = _camera.ProjectRayNormal(mousePos);

		_raycast.GlobalPosition = origin;

		_raycast.TargetPosition = normal * 1000;

		_raycast.ForceRaycastUpdate();

		if (_raycast.IsColliding())
		{
			
			Vector3 clickPoint = _raycast.GetCollisionPoint();
			return clickPoint;

		}
		else return null;

	}
	private GodotObject GetClickCollision()
	{
		
		var mousePos = GetViewport().GetMousePosition();

		Vector3 origin = _camera.ProjectRayOrigin(mousePos);
		Vector3 normal = _camera.ProjectRayNormal(mousePos);

		_raycast.GlobalPosition = origin;

		_raycast.TargetPosition = normal * 1000;

		_raycast.ForceRaycastUpdate();

		if (_raycast.IsColliding())
		{
			
			GodotObject body = _raycast.GetCollider();
			return body;

		}
		else return null;


	}


}
