using Godot;
using System;

public partial class HeroListPanel : MarginContainer
{
	[Export] public MarginContainer HeroList;
	[Export] public MarginContainer DetailedInfo;

	private bool isDetailedInfoOpem = false;

	
	public override void _Ready()
	{
		
		foreach (var node in GetTree().GetNodesInGroup("HeroButton"))
		{
			if (node is HeroCard card)
			{
				card.HeroSelected += OnHeroSelected;
			}
		}

	}
	private void OnHeroSelected(HeroResource data)
	{
		
		HeroList.Visible = false;
		DetailedInfo.Visible = true;


	}

	
	public override void _Process(double delta)
	{
	}
}
