using Godot;
using System;

public partial class DebugConsole : Control
{
	
	[Export] private LineEdit _commandLine;
	[Export] private RichTextLabel _history;

	public override void _Ready()
	{
		_commandLine.TextSubmitted += SendRequest;
		_commandLine.Text = string.Empty;
	}

	public void ChangeVisible()
	{
		Visible = !Visible;

        if (Visible)
	    {
		    _commandLine.GrabFocus();
	    }
	    else
	    {
	        _commandLine.ReleaseFocus();
	    }
	}


	public void SendRequest(string text)
	{
		
		if (text == "/clear")
		{
			_history.Clear();
			_commandLine.Text = string.Empty;
			return;
		}
		
		_history.AppendText($"{text}\n");
		_commandLine.Text = string.Empty;

	}

}
