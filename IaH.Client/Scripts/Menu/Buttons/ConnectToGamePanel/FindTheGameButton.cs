using Godot;
using System;

public partial class FindTheGameButton : Button
{

	public override void _Ready()
	{
		Pressed += ConnectToLobby;
	}

	private void ConnectToLobby()
	{
		GetTree().ChangeSceneToFile("res://Scenes/PickStage/Lobby.tscn");
	}
}
