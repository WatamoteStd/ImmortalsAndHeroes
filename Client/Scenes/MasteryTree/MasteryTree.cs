using Godot;
using System;

public partial class MasteryTree : Control
{
	
	[Export] private Button _darkPathButton;
	[Export] private Label _playerExp;

	// INFO PANEl
	[Export] private PanelContainer _pathInfoWindow;
	[Export] private Button _learnPathButton;
	[Export] private Label _title;
	[Export] private Label _description;
	[Export] private Label _requiredExp;

	public override void _Ready()
	{
		_pathInfoWindow.Visible = false;
		_darkPathButton.Pressed += () => OpenPathInfo("DarkPath", "You entered to this way. + 1 armor", 10);

	}


	public void OpenPathInfo(string name, string description, uint requiredExp)
	{
		
		_pathInfoWindow.Visible = true;
		_title.Text = name;
		_description.Text = description;
		_requiredExp.Text = requiredExp.ToString();

	}

	public void InitTree(uint exp)
	{
		
		_playerExp.Text = exp.ToString();

	}

}
