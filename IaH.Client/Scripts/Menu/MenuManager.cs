using Godot;
using System;
using System.Collections.Generic;

public partial class MenuManager : Node
{
	public enum WindowType
	{
		Profile,
		HeroList,
		HeroDetailed,
		Ladder,
		Settings,
		Glossarium,

	}
	[Export] private Control _proflePanel;
	[Export] private Control _heroListPanel;
	[Export] private Control _heroDetailedPanel;
	[Export] private Control _ladderPanel;
	[Export] private Control _settingsPanel;
	[Export] private Control _glossariumPanel;

	// BUTTONS
	[Export] private Button _ladderButton;
	[Export] private Button _heroesButton;
	[Export] private Button _settingsButton;
	[Export] private Button _settingsLeaveButton;
	[Export] public Button BecomeTester;

	// NICKNAME
	[Export] public Label NicknameText;
	[Export] public LineEdit NicknameEdit;
	[Export] public TextureButton ChangeNickname;

	private Dictionary<WindowType, Control> _window;
	private WindowType _currentWindow;
	
	public override void _Ready()
	{
		
		_window = new Dictionary<WindowType, Control>
		{
			{WindowType.HeroList, _heroListPanel},
			{WindowType.Settings, _settingsPanel}
		};
		_heroesButton.Pressed += () => SwitchWindow(WindowType.HeroList);
		_settingsButton.Pressed += () => SwitchWindow(WindowType.Settings);
		_settingsLeaveButton.Pressed += () => SwitchToMain();

		ChangeNickname.Pressed += EditNickname;
		NicknameEdit.FocusExited += EditNicknameEnd;
		NicknameEdit.TextSubmitted += (string text) => EditNicknameEnd();	

	}
	public void SwitchToMain()
	{
		foreach (var window in _window.Values)
		{
			window.Visible = false;
		}
	}

	public void SwitchWindow(WindowType newWindow)
	{
		foreach (var window in _window.Values)
		{
			window.Visible = false;
		}

		if (_window.ContainsKey(newWindow))
		{
			_window[newWindow].Visible = true;
			_currentWindow = newWindow;
		}
	}

	// NICKNAME
	private void EditNickname()
	{

		NicknameText.Visible = false;
		NicknameEdit.Visible = true;
		NicknameEdit.GrabFocus();
		
	}
	private void EditNicknameEnd()
	{
		
		NicknameEdit.Visible = false;
		NicknameText.Text = NicknameEdit.Text;
		NicknameText.Visible = true;

	}
	public override void _Input(InputEvent @event)
	{
		
		if (@event is InputEventMouseButton mouseAction && mouseAction.Pressed)
		{
			
			if (NicknameEdit.HasFocus() && !NicknameEdit.GetGlobalRect().HasPoint(mouseAction.GlobalPosition))
			{
				NicknameEdit.ReleaseFocus();
			}

		}

	}


}
