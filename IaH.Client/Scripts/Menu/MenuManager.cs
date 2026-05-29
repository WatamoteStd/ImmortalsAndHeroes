using Godot;
using System;
using Iah.Shared.Packets;
public partial class MenuManager : Control
{

			//EventBus.Instance.PublishJoinQueueRequested(PacketType.JoinQueue); FOR JOIN QUEUE
	// AUTH WINDOW

	public enum Window {None, SearchGame}
	public Window CurrentWindow = Window.None;
	private  Godot.Collections.Dictionary<Window, PanelContainer> _windowToPanel;
	[Export] private Button _loginButton;
	[Export] private PanelContainer _loginWindow;
	[Export] private TextEdit _nicknameEdit;

	// MENU FUCNTIONAL
	[Export] private Button _searchGameButton;
	[Export] private PanelContainer _searchGameContainer;

	public override void _Ready()
	{
		_loginWindow.Visible = true;
		_windowToPanel = new Godot.Collections.Dictionary<Window, PanelContainer>()
		{
			{Window.SearchGame, _searchGameContainer}
		};
		foreach (var panel in _windowToPanel.Values)
		{
			panel.Visible = false;
		}
		

		// AUTH
		_loginButton.Pressed += () => {
			_loginWindow.Visible = false;
			string playerNickname = _nicknameEdit.Text;
			EventBus.Instance.PublishLoginRequest(PacketType.LoginRequest, playerNickname);
		};

		// MENU FUCNTIONAL
		
		_searchGameButton.Pressed += () =>
		{
			
			_searchGameButton.Visible = false;
			_searchGameContainer.Visible = true;

		};

		// EVENT BUS FITCH
		EventBus.Instance.JoinQueueRequested += (packet) =>
		{
			_searchGameButton.Visible = true;
		};
		

	}
	
}
