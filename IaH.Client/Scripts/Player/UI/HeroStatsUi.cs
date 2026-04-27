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
		EventBus.OnStatsPacketReceived += UpdateStats;
	}

	public void UpdateStats(EntityStatsPacket packet)
	{
		
		_nameLabel.Text = $"Hero: {packet.EntityId}";

		if ((packet.UpdateMask & 1) != 0) // vitals
		{
			_healthLabel.Text = $"HP: {packet.Vitals.CurrentHp / 10}";
			_manaLabel.Text = $"MP: {packet.Vitals.CurrentMana / 10}";
		}
		if ((packet.UpdateMask & 2) != 0) // attribut
		{
			_healthLabel.Text += $" / {packet.Attributes.MaxHealth / 10}";
			_manaLabel.Text += $" / {packet.Attributes.MaxMana / 10}";
			_speedLabel.Text = $"MoveSpeed: {packet.Attributes.Speed / 10.0f}";
		}

	}
}
