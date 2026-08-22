using Godot;
using System;

public partial class Hud : CanvasLayer
{
	
	[Export] private ProgressBar _healthBar;
	[Export] private Label _healthBarLabel;
	[Export] private ProgressBar _manaBar;
	[Export] private Label _manaBarLabel;
	[Export] private Label _silver;
	[Export] private Label _nickname;
	[Export] private SelectedEntityWindow _selectEntityWindow;

	private uint _maxHealth;
	private uint _maxMana;

	public void InitHud(uint hp, uint mp, uint silver, string name)
	{
		
		_healthBar.MaxValue = hp;
		_healthBar.Value = hp;
		_manaBar.MaxValue = mp;
		_manaBar.Value = mp;

		_maxHealth = hp;
		_maxMana = mp;

		_healthBarLabel.Text = hp.ToString() + " / " + hp.ToString();
		_manaBarLabel.Text = mp.ToString() + " / " + mp.ToString();

		_silver.Text = silver.ToString();
		_nickname.Text = name;


	}
	public void UpdateHealth(uint actualHealth)
	{
		
		_healthBar.Value = actualHealth;
		_healthBarLabel.Text = actualHealth.ToString() + " / " + _maxHealth.ToString();

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
