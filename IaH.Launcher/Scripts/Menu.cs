using Godot;
using System;
using System.Diagnostics;

public partial class Menu : Node
{
	
	[Export] private Button _loginButton;
	[Export] private LineEdit _loginEdit;
	[Export] private LineEdit _passwordEdit;

	public override void _Ready()
	{
		
		_loginButton.Pressed += _LoginRequest;

	}

	private void _LoginRequest()
	{
		Process.Start("IaH_Client.exe", "--auth-token 1990");

		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
