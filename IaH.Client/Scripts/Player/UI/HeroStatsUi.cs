using Godot;
using System;
using IaH.Shared.Networking;


public partial class HeroStatsUi : Control
{
	[Export] public Label _nameLabel;
	[Export] public Label _healthLabel;
	[Export] public Label _speedLabel;
	[Export] public Label _manaLabel;
	public override void _Ready()
	{
	}

	public void UpdateStats(EntityStatsPacket packet)
	{
		
		_nameLabel.Text = $"Hero: {packet.EntityId}";

		if ((packet.UpdateMask & 1) != 0) // vitals
		{
			_healthLabel.Text = $"HP: {packet.Vitals.CurrentHp}";
		}
		if ((packet.UpdateMask & 2) != 0) // attribut
		{
			_healthLabel.Text += $" / {packet.Attributes.MaxHealth}";
			_speedLabel.Text = $"MoveSpeed: {packet.Attributes.Speed / 100.0f}";
		}

	}
}
