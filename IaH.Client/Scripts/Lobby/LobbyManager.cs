using Godot;
using Iah.Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LobbyManager : Control
{
	// UI
	[Export] private Label _player1;
	[Export] private Label _selectedHeroP1;
	[Export] private Label _player2;
	[Export] private Label _selectedHeroP2;
	[Export] private Label _lobbyTimer;
	[Export] private Label _lobbyId;
	[Export] private Button _pickVoidlessStar;
	[Export] private Button _pickFrozen;

	private List<Label> _playersLabel;
	private List<Label> _heroesLabels;

	//TECH
	private int lobbyID;
	private List<string> nicknamesList;
	private int LocalId = ClientNetworkManager.Instance.LocalID;

	public override void _Ready()
	{

		// UI PREFAB
		_player1.Text = "Waiting...";
		_player2.Text = "Waiting...";
		_player1.SelfModulate = Colors.White;
		_player2.SelfModulate = Colors.White;
		_selectedHeroP1.Text = "Picking...";
		_selectedHeroP2.Text = "Picking...";

		// TECH --------------------------------------------
		nicknamesList = new();
		lobbyID = ClientNetworkManager.Instance.CachedLobbyId;
		_lobbyId.Text = lobbyID.ToString();
		nicknamesList = ClientNetworkManager.Instance.CachedPlayersInLobby.Values.ToList();

		_playersLabel = new List<Label> {_player1, _player2};
		_heroesLabels = new List<Label> {_selectedHeroP1, _selectedHeroP2};

		// SIGNALS--------------------------------------------
		_pickVoidlessStar.Pressed += () =>
		{
			EventBus.Instance.PublishLobbyHeroSelected((byte)UnitList.VoidlessStar);
		};
		_pickFrozen.Pressed += () =>
		{
			EventBus.Instance.PublishLobbyHeroSelected((byte)UnitList.Frozen);
		};

		// EVENT BUS ---------------------------------------------------------------------

		EventBus.Instance.LobbyHeroChanged += UpdateUI;
		EventBus.Instance.LobbyTimer += (time) =>
		{
			_lobbyTimer.Text = $"0:{time:D2}";
		};
		UpdateUI();

		
	}

	private void UpdateUI()
	{
		var playersIds = ClientNetworkManager.Instance.CachedPlayersInLobby.Keys.ToList();
		
		for (int i = 0; i < playersIds.Count; i++)
		{
			
			if (i < playersIds.Count)
			{
				
				int currentId = playersIds[i];
				_playersLabel[i].Text = ClientNetworkManager.Instance.CachedPlayersInLobby[currentId];

				if (currentId == LocalId)
				{
					_playersLabel[i].Modulate = Colors.Green;
				}
				else
				{
					_playersLabel[i].Modulate = Colors.White;
				}

				if (ClientNetworkManager.Instance.CachedPlayersHeroes.ContainsKey(currentId))
				_heroesLabels[i].Text = ClientNetworkManager.Instance.CachedPlayersHeroes[currentId].ToString();
			else
				_heroesLabels[i].Text = "Picking...";

			}
			else
			{
				_playersLabel[i].Text = "Waiting...";
		   	 	_playersLabel[i].SelfModulate = Colors.White;
				_heroesLabels[i].Text = "Picking...";
			}

		}

	}
}
