using Godot;
using Shared.MasteryTree.Rewards;
using System;

public partial class StatReward : Control
{
	
	[Export] private Label _statLabel;
	[Export] private Label _valueLabel;
	[Export] private Label _requiredLabel;
	[Export] private PanelContainer _requiredPanel;
	public bool IsLvlRequired {get; private set;}

	public void CreateVisual(StatType stat, int value, int requiredLvl, bool lvlRequired = false)
	{
		

		if (lvlRequired)
		{
			
			IsLvlRequired = true;
			_requiredPanel.Visible = true;
			_requiredLabel.Text = requiredLvl.ToString();

		}
		else _requiredPanel.Visible = false;

		switch (stat)
		{
			
			case StatType.Health:
				{
					_statLabel.Text = "HEALTH";
					_statLabel.Modulate = new Color(1.0f, 0.184f, 0.349f);
				}
			break;
			case StatType.Mana:
				{
					_statLabel.Text = "MANA";
					_statLabel.Modulate = new Color(0.271f, 0.482f, 0.898f);
				}
			break;
			case StatType.Armor:
				{
					_statLabel.Text = "ARMOR";
					_statLabel.Modulate = new Color(0.447f, 0.502f, 0.635f);
				}
			break;
			case StatType.AttackSpeed:
				{
					_statLabel.Text = "ATTACK SPEED";
					_statLabel.Modulate = new Color(0.42f, 1.0f, 0.659f);
				}
			break;
			case StatType.HealthRegen:
				{
					_statLabel.Text = "HEALTH REGEN";
					_statLabel.Modulate = new Color(0.946f, 0.0f, 0.243f);
				}
			break;
			case StatType.MagicResistance:
				{
					_statLabel.Text = "MAGIC RESISTANCE";
					_statLabel.Modulate = new Color(0.435f, 0.533f, 0.992f);
				}
			break;
			case StatType.ManaRegen:
				{
					_statLabel.Text = "MANA REGEN";
					_statLabel.Modulate = new Color(0.271f, 0.482f, 0.898f);
				}
			break;
			case StatType.MoveSpeed:
				{
					_statLabel.Text = "SPEED";
					_statLabel.Modulate = Colors.WhiteSmoke;
				}
			break;
			case StatType.PhysicalDamage:
				{
					_statLabel.Text = "DAMAGE";
					_statLabel.Modulate = Colors.YellowGreen;
				}
			break;

		}
		_valueLabel.Text = "+" + value.ToString();


	}


}
