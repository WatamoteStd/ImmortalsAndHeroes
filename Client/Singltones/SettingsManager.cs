using Godot;
using System;

public partial class SettingsManager : Node
{
	

	public bool IsSpaceToAttack {get; set;}
	public bool AttackOnFirstLmb {get; set;}
	public bool AllowEquipmentOverheat {get; set;}
	public static SettingsManager Instance {get; private set;}

	public override void _Ready()
	{
		
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		else
		{
			Instance = this;
		}

	}


}
