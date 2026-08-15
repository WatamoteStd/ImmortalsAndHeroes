using Godot;
using System;

public partial class SelectedEntityWindow : PanelContainer
{
	
	[Export] private Label _name;
	[Export] private Label _health;
	private Entity _selectedEntity;

	public void ShowWindow(Entity entity)
	{

		_name.Text = entity.EntityName;
		_health.Text = entity.Health.ToString();
		Visible = true;

	}
	public void HideWindow()
	{
		
		_selectedEntity = null;
		_name.Text = "";
		_health.Text = "";
		Visible = false;

	}

}
