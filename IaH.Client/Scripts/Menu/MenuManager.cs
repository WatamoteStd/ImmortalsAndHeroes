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

	private Dictionary<WindowType, Control> _window;
	private WindowType _currentWindow;
	
	public override void _Ready()
	{
		
		_window = new Dictionary<WindowType, Control>
		{
			{ WindowType.Profile, _proflePanel},
			{WindowType.HeroList, _heroListPanel},
			{WindowType.Settings, _settingsPanel}
		};
		_heroesButton.Pressed += () => SwitchWindow(WindowType.HeroList);

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

	
	public override void _Process(double delta)
	{
	}
}
