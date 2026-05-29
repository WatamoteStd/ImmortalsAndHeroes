using Godot;
using System;

public partial class MapManager : Node3D
{
	private int LocalID;
	
	public override void _Ready()
	{
		LocalID = ClientNetworkManager.Instance.LocalID;
		EventBus.Instance.PublishReadyToGame();
	}


	public override void _Process(double delta)
	{
	}
}
