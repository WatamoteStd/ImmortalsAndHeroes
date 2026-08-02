using Godot;
using System;

public partial class StatusWindow : PanelContainer
{
	
	[Export] private Label _statusName;
	[Export] private Label _statusMessage;

	public void ShowMessage(string header, string message)
	{
		
		Visible = true;

		_statusName.Text = header;
		_statusMessage.Text = message;

	}

}
