using Godot;
using System;

public partial class LobbyManager : Control
{
	[Export] private Label LobbyTimerLabel;
	[Export] private Timer LobbyTimer;
	
	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}
}
