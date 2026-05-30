using Godot;
using Iah.Shared.Packets;
using System;
using System.Collections.Generic;

public partial class LobbyManagerClient : Control
{
	// HERO CHOOSE
	private UnitList _localPlayerHero = UnitList.None;
	[Export] private Button _frozenButton;
	[Export] private Button _voidlessStarButton;
	[Export] private Button _whipButton;
	[Export] private Button _ozonidButton;
	[Export] private Button _lilithButton;
	[Export] private Button _chooseHeroButton;



	[Export] private Label _timer;
	// TEAM WHITE STUFF
	[Export] private Label _nicknameWhite1;
	[Export] private Label _nicknameWhite2;
	[Export] private Label _heroWhite1;
	[Export] private Label _heroWhite2;

	private Godot.Collections.Array<Label> _whiteTeamNicknames;
	private Godot.Collections.Array<Label> _whiteTeamHeroes;

	// BLACK TEAM STUFF

	[Export] private Label _nicknameBlack1;
	[Export] private Label _nicknameBlack2;
	[Export] private Label _heroBlack1;
	[Export] private Label _heroBlack2;

	private Godot.Collections.Array<Label> _blackTeamNicknames;
	private Godot.Collections.Array<Label> _blackTeamHeroes;

	
	public override void _Ready()
	{
		_chooseHeroButton.Pressed += () =>
		{
			
			if (_localPlayerHero == UnitList.None) return;
			EventBus.Instance.PublishLobbyHeroSelected((byte)_localPlayerHero);

		};

		_frozenButton.Pressed += () =>
		{
			_localPlayerHero = UnitList.Frozen;
		};
		_voidlessStarButton.Pressed += () =>
		{
			_localPlayerHero = UnitList.VoidlessStar;
		};
		_whipButton.Pressed += () =>
		{
			_localPlayerHero = UnitList.Whip;
		};
		_lilithButton.Pressed += () =>
		{
			_localPlayerHero = UnitList.Lilith;
		};
		_ozonidButton.Pressed += () =>
		{
			_localPlayerHero = UnitList.Ozonid;
		};
		

		_whiteTeamNicknames = new Godot.Collections.Array<Label>()
		{
			_nicknameWhite1,
			_nicknameWhite2
		};
		_whiteTeamHeroes = new Godot.Collections.Array<Label>()
		{
			_heroWhite1,
			_heroWhite2	
		};

		// BLACk
		_blackTeamNicknames = new Godot.Collections.Array<Label>()
		{
			_nicknameBlack1,
			_nicknameBlack2
		};
		_blackTeamHeroes = new Godot.Collections.Array<Label>()
		{
			_heroBlack1,
			_heroBlack2	
		};

		// EVENT BUS
		EventBus.Instance.LobbyTimer += UpdateTimer;
		EventBus.Instance.LobbyHeroChanged += UpdateInfo;
		EventBus.Instance.PlayerConnectedToLobby += (lobbyID) =>
		{
			if (lobbyID != ClientNetworkManager.Instance.CachedLobbyId) return;
			UpdateInfo();

		};
		UpdateInfo();
	}

	private void UpdateTimer(ushort time)
	{
		_timer.Text = $"{time:D2}";
	}
	private void UpdateInfo()
	{

		foreach (var l in _whiteTeamNicknames) l.Text = "";
		foreach (var l in _blackTeamNicknames) l.Text = "";
		foreach (var l in _whiteTeamHeroes) l.Text = "Waiting...";
		foreach (var l in _blackTeamHeroes) l.Text = "Waiting...";

		int whiteIdx = 0;
		int blackIdx = 0;
		
		foreach (var p in ClientNetworkManager.Instance.CachedPlayersInLobby)
		{
			
			int pID = p.Key;
			string name = p.Value;
			Team team = ClientNetworkManager.Instance.CachedPlayersTeam[pID];

			string heroName = ClientNetworkManager.Instance.CachedPlayersHeroes.TryGetValue(pID, out var hero) 
				? hero.ToString() 
				: "Waiting...";

			if (team == Team.White && whiteIdx < _whiteTeamNicknames.Count)
			{
				
				_whiteTeamNicknames[whiteIdx].Text = name;
				if (pID == ClientNetworkManager.Instance.LocalID) _whiteTeamNicknames[whiteIdx].Modulate = Colors.SeaGreen;
				else _whiteTeamNicknames[whiteIdx].Modulate = Colors.White;
				_whiteTeamHeroes[whiteIdx].Text = heroName;
				whiteIdx++;

			}
			else if (team == Team.Black && blackIdx < _blackTeamNicknames.Count)
			{
				_blackTeamNicknames[blackIdx].Text = name;
				_blackTeamHeroes[blackIdx].Text = heroName;
				if (pID == ClientNetworkManager.Instance.LocalID) _blackTeamNicknames[blackIdx].Modulate = Colors.SeaGreen;
				else _blackTeamNicknames[blackIdx].Modulate = Colors.White;
				blackIdx++;
			}

		}

	}

	
	
}
