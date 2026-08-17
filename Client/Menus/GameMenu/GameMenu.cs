using Godot;
using System;

public partial class GameMenu : Control
{
	
	[Export] private Button _openCreateMenu;
	[Export] private CharacterCreateWindow _createMenu;
	[Export] private Label _userId;
	[Export] private Label _username;
	[Export] private StatusWindow _statusWindow;
	
	// CHARACTER WINDOW

	public override void _Ready()
	{
		
		_openCreateMenu.Pressed += () =>
		{
			_createMenu.ChangeVisiblity();
		};

		_userId.Text = GameSession.Instance.GlobalId.ToString();
		_username.Text = GameSession.Instance.Username;

	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("HideMenu"))
		{
			
			Visible = !Visible;

		}
    }



}
