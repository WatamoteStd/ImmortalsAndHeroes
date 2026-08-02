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
	[Export] private LineEdit _emailLine;
	[Export] private Button _loginButton;
	[Export] private Label _registerText;
	[Export] private Button _registerButton;
	[Export] private HBoxContainer _emailField;

	// REGISTER & LOGIN STATUS LOG WINDOW
	[Export] private Label _statusCode;
	[Export] private Label _serverMessage;
	[Export] private PanelContainer _statusWindow;

	private float _timeFromLastRequestToServer = 0.0f;

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

		_registerButton.Pressed += () =>
		{
			_ = RegisterAsync();
		};

	}

	public override void _Process(double delta)
	{
		if (_timeFromLastRequestToServer < 1.0f)
		{
			_timeFromLastRequestToServer += (float)delta;
		}
	}


	private async Task RegisterAsync()
	{

		if (_timeFromLastRequestToServer < 1.0f)
		{
			
			_statusWindow.Visible = true;
			_statusCode.Text = "Slow down";
			_serverMessage.Text = "Wait at least secound before trying again";
			return;

		}
		_timeFromLastRequestToServer = 0.0f;
		
		// PROTECTION FROM EMPTY REQUESTS
		if (string.IsNullOrWhiteSpace(_loginLine.Text) || string.IsNullOrWhiteSpace(_passwordLine.Text) || string.IsNullOrWhiteSpace(_emailLine.Text))
		{
			_statusWindow.Visible = true;
			_statusCode.Text = "Invalid at player";
			_serverMessage.Text = "Fill all data.";
			return;
		}

		try
		{
			
			var response = await HttpsMasterClient.Instanсe.RegisterRequestAsync(_loginLine.Text, _passwordLine.Text, _emailLine.Text);

			_statusWindow.Visible = true;
			_statusCode.Text = response.isSuccess ? "Success" : "Error";
			_serverMessage.Text = response.message;

		}
		catch (Exception e)
		{
			GD.PrintErr($"[LoginWindow] Error when try register. {e.Message}");
			_statusWindow.Visible = true;
			_statusCode.Text = "Client Error";
			_serverMessage.Text = "Something went wrong on the client.";
		}



	}
 


}
