using Godot;
using System;

public partial class RegionChangeArea : Area3D
{
	
	[Export] private uint EnterZoneId;

	public override void _Ready()
	{
		
		BodyEntered += ChangeRegionRequest;

	}

	private void ChangeRegionRequest(Node body)
	{
		if (body is LocalPlayerEntity)
		{
			ServerMaster.Instance.LocalPlayerChangeRegionRequest(EnterZoneId);
		}

	}


}
