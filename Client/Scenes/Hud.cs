using Godot;
using System;

public partial class Hud : CanvasLayer
{
	
	[Export] private ProgressBar _healthBar;
	[Export] private ProgressBar _manaBar;
	[Export] private Label _silver;
	[Export] private Label _lvl;
	[Export] private Label _nickname;
	[Export] private SelectedEntityWindow _selectEntityWindow;

	public void InitHud(uint hp, uint mp, uint silver, uint lvl, string name)
	{
		
		_healthBar.MaxValue = hp;
		_healthBar.Value = hp;
		_manaBar.MaxValue = mp;
		_manaBar.Value = mp;

		_silver.Text = silver.ToString();
		_lvl.Text = lvl.ToString();
		_nickname.Text = name;


	}

	public void ShowSelectedEntity(Entity entity)
	{
		_selectEntityWindow.ShowWindow(entity);
	}
	public void HideSelectedEntity()
	{
		_selectEntityWindow.HideWindow();
	}



}
