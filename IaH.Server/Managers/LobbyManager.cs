using System;
using LiteNetLib;
using LiteNetLib.Utils;
using IaH.Server.Managers;
using IaH.Server.PlayerClasses;
using System.Text.RegularExpressions;
using IaH.Server.World;

namespace IaH.Server.Managers
{

    public class LobbyManager
    {
        private List<Lobby> _lobbyPool;
        private List<WorldMatch> _activeMatches;
        private List<Player> _playersInSearch;
        public NetManager _netManager;
        private float _lobbyTimer = 0.0f;
        private ushort _lobbyID = 1;

        
        public LobbyManager(NetManager serverNetManager)
        {
            _activeMatches = new List<WorldMatch>();
            _lobbyPool = new List<Lobby>();
            _playersInSearch = new List<Player>();
            _netManager = serverNetManager;

        }
        public void AddPlayerToQueue(Player player)
        {
            if (_playersInSearch.Contains(player)) return;
            _playersInSearch.Add(player);
            Console.WriteLine($"[LobbyManager] Player:{player.Nickname} added to queue. Players in queue:{_playersInSearch.Count}");

        }

        public void Matchmake()
        {
            
            while (_playersInSearch.Count >= 4)
            {
                
                Lobby newLobby = new Lobby(_netManager, _lobbyID);
                Console.WriteLine($"[LobbyManager] Lobby created. ID:#{newLobby.ID}");
                _lobbyPool.Add(newLobby);
                for (int i = 0; i < 4; i++)
                {
                    
                    Player p = _playersInSearch[0];
                    _playersInSearch.RemoveAt(0);

                    newLobby.AddPlayer(p);
                    p.CurrentState = Player.State.InLobby;
                    p.CurrentLobby = newLobby;
                    Console.WriteLine($"[LobbyManager] Player:{p.Nickname} joined to lobby:#{newLobby.ID}");

                }
                newLobby.LobbyCreated();
                newLobby.ConnectBroadcast();
                _lobbyID++;

            }

        }
        public void Update(float deltaTime)
        {
            _lobbyTimer += deltaTime;
            if (_lobbyTimer >= 1.0f)
            {
                Matchmake();
                _lobbyTimer = 0.0f;
            }
            
            for (int i = _lobbyPool.Count -1; i >= 0; i--) // LOBBY UPDATE
            {
                
                var lobby = _lobbyPool[i];
                lobby.UpdateTimer(deltaTime);
                if (lobby.CurrentState == Lobby.State.InGame)
                {
                    
                    List<Player> players = lobby.GetPlayersList();
                    WorldMatch m = new WorldMatch(players, lobby.ID);
                    _activeMatches.Add(m);
                    _lobbyPool.RemoveAt(i);

                }

            }

            for (int i = _activeMatches.Count -1; i >= 0; i--)
            {
                
                var match = _activeMatches[i];
                match.UpdateMatch(deltaTime);

            }

        }

        public WorldMatch? MatchByID(ushort id)
        {
            foreach (var match in _activeMatches)
            {
                if (match.MatchID == id)
                {
                    return match;
                }
            }
            return null;
        }

        // FOR DEBUG COMMANDS
        public void LobbyList()
        {
            Console.WriteLine($"TOTAL LOBBY: {_lobbyPool.Count}");
            foreach (var lobby in _lobbyPool)
            {
                Console.WriteLine($"Lobby #{lobby.ID}. Players Count:{lobby.PlayersCount()}");
                Console.WriteLine($"Lobby#{lobby.ID} PlayersList:");
                List<Player> players = lobby.GetPlayersList();
                foreach (var player in players)
                {
                    Console.WriteLine(player.Nickname);
                }
            }
        }
        public void MatchList()
        {
            Console.WriteLine($"TOTAL MATCHES: {_activeMatches.Count}");
            foreach (var match in _activeMatches)
            {
                Console.WriteLine($"Match#{match.MatchID}. Players Count:{match.TotalPlayersCount()}");
                Console.WriteLine($"Match#{match.MatchID} PlayersList:");
                List<Player> players = match.MatchPlayers();
                foreach (var player in players)
                {
                    Console.WriteLine(player.Nickname);
                }
            }
        }
    }
    
}