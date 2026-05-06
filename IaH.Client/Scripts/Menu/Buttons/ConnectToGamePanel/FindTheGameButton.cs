using Godot;
using System;

public partial class FindTheGameButton : Button
{

	public override void _Ready()
	{
		Pressed += EventBus.PublishOnJoinQueue;
	}

	private void ConnectToLobby()
	{
		
	}
}
