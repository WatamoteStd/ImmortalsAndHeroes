using Godot;
using System;

public partial class LobbyManager : Control
{
	[Export] private Label LobbyTimerLabel;
	[Export] private Timer LobbyTimer;
	[Export] private LineEdit InputText;
	[Export] private RichTextLabel ChatHistrory;
	
	public override void _Ready()
	{
		InputText.TextSubmitted += SendMessageToAll;
		EventBus.OnMessageReceived += UpdateChat;
		
	}
	private void UpdateChat(string message)
	{
		ChatHistrory.AppendText(message + "\n");
	}
	public void SendMessageToAll(string message)
	{
		EventBus.PublishSendMessageToLobby(message);
		InputText.Clear();
		InputText.GrabFocus();
	}

	public override void _ExitTree()
{
	EventBus.OnMessageReceived -= UpdateChat;
}

}
