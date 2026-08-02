using Godot;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json;
using Shared.Network.Packets;

public partial class LoginWindow : PanelContainer
{
	
	[Export] private LineEdit _loginLine;
	[Export] private LineEdit _passwordLine;
	[Export] private Button _loginButton;
	[Export] private Label _registerText;
	[Export] private Button _registerButton;
	[Export] private HBoxContainer _emailField;

	private bool isRegisterModeOn = false;

	public override void _Ready()
	{
		
		_registerText.MouseEntered += () =>
		{
			_registerText.SelfModulate = new Color(0.279f, 0.629f, 0.86f);
		};
		_registerText.MouseExited += () =>
		{
			_registerText.SelfModulate = new Color(0.165f, 0.498f, 0.659f);
		};
		_registerText.GuiInput += (InputEvent @event) =>
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				
				if (!isRegisterModeOn)
				{
					
					_registerText.Text = "Already registered? Log in";
					_loginButton.Visible = false;
					_registerButton.Visible = true;
					_emailField.Visible = true;
					isRegisterModeOn = true;

				}
				else
				{
					_registerText.Text = "Don't have an account? Register it";
					_loginButton.Visible = true;
					_registerButton.Visible = false;
					_emailField.Visible = false;
					isRegisterModeOn = false;
				}

			}
		};

		

	}



}
