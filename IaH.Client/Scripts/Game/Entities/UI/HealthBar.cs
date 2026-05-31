using Godot;
using System;

public partial class HealthBar : SubViewport
{
	private TextureProgressBar _healthBar;
	public override void _Ready()
	{
		_healthBar = GetNode<TextureProgressBar>("TextureProgressBar");
	}

	public void UpdateColor(bool isAlly)
	{
		
		if (isAlly) _healthBar.Modulate = Colors.DarkGreen;
		else _healthBar.Modulate = Colors.DarkRed;

	}

	public void Init(ushort maxHp)
	{
		_healthBar.MaxValue = maxHp;
		_healthBar.Value = maxHp;
	}
	public void UpdateHealth(ushort currentHp)
	{
		_healthBar.Value = currentHp;
	}
}
