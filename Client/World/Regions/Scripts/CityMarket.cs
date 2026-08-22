using Godot;
using System;

public partial class CityMarket : Area3D
{
	
	[Export] private uint _regionId;
	[Export] private MarketWindow _marketWindow;

	public override void _Ready()
	{
		
		BodyEntered += LocalPlayerEntered;

	}

	private void LocalPlayerEntered(Node3D body)
	{
		
		if (body is LocalPlayerEntity player)
		{
			
			SceneManager.Instance.SwitchVisiblityCityMarket();

		}

	}


}
