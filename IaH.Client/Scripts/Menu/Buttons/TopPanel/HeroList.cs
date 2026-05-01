using Godot;
using System;

public partial class HeroList : Button
{
	[Export] public MarginContainer HeroesPanel;
	private bool isPanelActive = false;
	public override void _Ready()
	{
		Pressed += PanelManager;
	}

	private void PanelManager()
	{
		
		if (isPanelActive == false)
		{
			HeroesPanel.Visible = true;
			isPanelActive = true;
		}
		else if (isPanelActive == true)
		{
			HeroesPanel.Visible = false;
			isPanelActive = false;
		}

	}
	public override void _Process(double delta)
	{
	}
}
