using Godot;
using System;
using IaH.Shared.Networking;


public partial class HeroStatsUi : Control
{
	
	
	public static HeroStatsUi Instance { get; private set; }
	public ushort MainHeroId;
	public ushort TargetHeroId;
	[Export] public Label _nameLabel;
	[Export] public Label _healthLabel;
	[Export] public Label _speedLabel;
	[Export] public Label _manaLabel;
	public override void _Ready()
	{
		Instance = this;
		EventBus.OnStatsPacketReceived += UpdateStats;
	}

	public void UpdateStats(EntityStatsPacket packet)
	{
		if (_nameLabel == null || _healthLabel == null || _manaLabel == null || _speedLabel == null) return;
		if (packet.EntityId != MainHeroId) return;
		
		_nameLabel.Text = $"Hero: {packet.EntityId}";

		if ((packet.UpdateMask & 1) != 0) // vitals
		{
			_healthLabel.Text = (packet.Vitals.CurrentHp / 10.0f).ToString() +  " / " + (packet.Attributes.MaxHealth / 10.0f).ToString();
			_manaLabel.Text = $"MP: {packet.Vitals.CurrentMana / 10}";
		}
		if ((packet.UpdateMask & 2) != 0) // attribut
		{
			_healthLabel.Text = (packet.Vitals.CurrentHp / 10.0f).ToString() +  " / " + (packet.Attributes.MaxHealth / 10.0f).ToString();
			_manaLabel.Text += $" / {packet.Attributes.MaxMana / 10}";
			_speedLabel.Text = $"MoveSpeed: {packet.Attributes.Speed / 10.0f}";
		}

	}
}
