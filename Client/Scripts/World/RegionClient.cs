using Godot;
using System;

public partial class RegionClient : Node3D
{

	public override void _Ready()
	{
		
		NetworkUdpClient.Instance.SendEnterTheWorld();

	}


}
