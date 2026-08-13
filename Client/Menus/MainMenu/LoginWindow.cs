using Godot;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Text.Json;

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
	[Export] private StatusWindow _statusWindow;
	[Export] private Label _tempText;
	[Export] private PanelContainer _tempWindow;
	[Export] private Button _tempButton;

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
		_tempText.MouseEntered += () =>
		{
			_tempText.SelfModulate = new Color(0.279f, 0.629f, 0.86f);
		};
		_tempText.MouseEntered += () =>
		{
			_registerText.SelfModulate = new Color(0.165f, 0.498f, 0.659f);
		};

		// DELETE
		_tempText.GuiInput += (InputEvent @event) =>
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				
				_tempWindow.Visible = true;

			}
		};
		_tempButton.Pressed += () =>
		{
			_tempWindow.Visible = false;
		};

		_registerButton.Pressed += () =>
		{
			_ = RegisterAsync();
		};
		_loginButton.Pressed += () =>
		{
			_ = LoginAsync();
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

		if (!DontSpamFilter() || !NullFilter(1)) return;
		
		try
		{
			
			var response = await HttpsMasterClient.Instanсe.RegisterRequestAsync(_loginLine.Text, _passwordLine.Text, _emailLine.Text);

			if (response.isSuccess) _statusWindow.ShowMessage("Success!", response.message);
			else _statusWindow.ShowMessage("Fault!", response.message);
		}
		catch (Exception e)
		{
			GD.PrintErr($"[LoginWindow] Registration error. {e.Message}");
			_statusWindow.ShowMessage("Client error!", "Something went wrong on the client side.");

		}



	}
 
	private async Task LoginAsync()
	{
		
		if (!DontSpamFilter() || !NullFilter(0)) return;

		try
		{
			
			var response = await HttpsMasterClient.Instanсe.LoginRequestAsync(_loginLine.Text, _passwordLine.Text);

			if (response.isSuccess) 
			{

				_statusWindow.ShowMessage("Successful login!", response.message);
				GetTree().CreateTimer(1.25f).Timeout += () =>
				{
					_ = SceneManager.Instance.AuthToMainMenu();
				};

			}

			else _statusWindow.ShowMessage("Failure!", response.message);

		}
		catch (Exception e)
		{
			
			GD.Print($"[Login Window] Server error. {e.Message}");
			_statusWindow.ShowMessage("Server error!", "Please try again");

		}
		
	}

	private bool DontSpamFilter()
	{
		
		if (_timeFromLastRequestToServer < 1.0f)
		{
			
			_statusWindow.ShowMessage("Slow down!", "Wait at least secound before trying again");
			return false;

		}
		_timeFromLastRequestToServer = 0.0f;
		return true;

	}
	private bool NullFilter(byte mode) // 0 pass and login check. other - full check
	{
		if (mode == 0)
		{
			
			if (string.IsNullOrWhiteSpace(_loginLine.Text) || string.IsNullOrWhiteSpace(_passwordLine.Text))
			{
				
				_statusWindow.ShowMessage("Invalid at player", "Fill all data");
				return false;

			}
			return true;

		}
		else
		{
			
			if (string.IsNullOrWhiteSpace(_loginLine.Text) || string.IsNullOrWhiteSpace(_passwordLine.Text) || string.IsNullOrWhiteSpace(_emailLine.Text))
			{
			
				_statusWindow.ShowMessage("Invalid at player", "Fill all data");
				return false;
			}
			return true;

		}
		

	}

	

}
