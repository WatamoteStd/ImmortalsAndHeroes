using Godot;
using Shared.Udp.Packets.Category;
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

		if (string.IsNullOrEmpty(text)) return;
		
		var packet = new C2S_AdminConsoleCommandPacket { Payload = text};

		ServerMaster.Instance.LP_SendConsoleCommand(packet);
		
		_history.AppendText($"{text}\n");
		_commandLine.Text = string.Empty;

	}

	public void ReceiveAnswer(C2S_AdminConsoleCommandPacket packet)
	{
		
		_history.AppendText($"[color=green]{packet.Payload}[/color]\n");

	}

}
