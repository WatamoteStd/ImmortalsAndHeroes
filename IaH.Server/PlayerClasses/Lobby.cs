using System;
using IaH.Server.PlayerClasses;
using IaH.Server.Managers;
using LiteNetLib;
using LiteNetLib.Utils;
using Iah.Shared.Packets;

namespace IaH.Server.PlayerClasses
{
    
    public class Lobby
    {
        public ushort ID {get; private set;}
        private float _lobbyTimer = 30.0f;
        private int _lastSentSec = 31;

        // PLAYERS AND TEAMS
        private List<Player> _lobbyPlayers;
        private List<Player> _teamWhite;
        private List<Player> _teamDark;

        // ONLINE INFO
        public NetManager _netManager;
        private NetDataWriter _writer;
        public enum State {WaitingForPlayers, WaitingForAllReady, Picking, Waiting, InGame};
        public State CurrentState {get; private set;}
        
        public Lobby(NetManager serverNetManager, ushort id)
        {
            _teamDark = new();
            _teamWhite = new();
            ID = id;
            _lobbyPlayers = new List<Player>();
            _netManager = serverNetManager;
            _writer = new NetDataWriter();
            CurrentState = State.WaitingForPlayers;

        }

        private void UpdateLobbyState()
        {
            
            switch (CurrentState)
            {
                
                case State.WaitingForPlayers:
                    if (_lobbyPlayers.Count == 4) CurrentState = State.Picking;
                    Console.WriteLine($"[LOBBY: {ID}] Waiting for players. Players: {_lobbyPlayers.Count} / 4");
                break;
            }

        }
        public void UpdateTimer(float deltaTime) // PRE GAME TIMER
        {
            if (CurrentState != State.Picking) return;
            
            _lobbyTimer -= deltaTime;
            int currentSecound = (int)Math.Ceiling(_lobbyTimer);
            if (currentSecound != _lastSentSec) {

                _lastSentSec = currentSecound;
                _writer.Reset();
                _writer.Put((byte)PacketType.LobbyTimer);
                _writer.Put((ushort)_lastSentSec);
                SendToLobby(DeliveryMethod.ReliableOrdered);

            }   
            
            if (_lobbyTimer <= 0) StartGame();
        }
        private void StartGame()
        {
            CurrentState = State.InGame;
            _writer.Reset();
            _writer.Put((byte)PacketType.ConnectedToGame);
            SendToLobby(DeliveryMethod.ReliableOrdered);   

        }

        public List<Player> GetPlayersList()
        {
           return _lobbyPlayers;
        }


        // PRE GAME FUNC
        public void AddPlayer(Player player)
        {
            
            if (_lobbyPlayers.Contains(player)) return;
            if (_lobbyPlayers.Count >= 4) return;
            
            _lobbyPlayers.Add(player);
            player.CurrentState = Player.State.InLobby;

            if (_teamWhite.Count < _teamDark.Count) {
                _teamWhite.Add(player);
                player.TeamID = Team.White;
            }
            else {
                _teamDark.Add(player);
                player.TeamID = Team.Black;
            }
            
            UpdateLobbyState();

        }
        public byte PlayersCount()
        {
            return (byte)_lobbyPlayers.Count;
        }
        public void SendToLobby(DeliveryMethod method)
        {
            if (_writer == null) return;
            
            foreach (var curPlayer in _lobbyPlayers)
            {
                curPlayer.PlayerPeer.Send(_writer, method);
            }

        }



        public void HeroSelected(Player player, UnitList hero)
        {
            _writer.Reset();
            _writer.Put((byte)PacketType.HeroSelected);
            _writer.Put((ushort)player.ID);
            _writer.Put((byte)hero);
            SendToLobby(DeliveryMethod.ReliableOrdered);
        }
        public void LobbyCreated()
        {
            _writer.Reset();
            _writer.Put((byte)PacketType.LobbyFind);
            SendToLobby(DeliveryMethod.ReliableOrdered);

        }
        public void ConnectBroadcast()
        {
            _writer.Reset();
            _writer.Put((byte)PacketType.ConnectToLobby);
            _writer.Put((ushort)ID); // lobby id
            _writer.Put(PlayersCount());
            foreach (var p in _lobbyPlayers) {
                _writer.Put((ushort)p.ID);
                _writer.Put((string)p.Nickname);
                _writer.Put((byte)p.TeamID);
            }
            SendToLobby(DeliveryMethod.ReliableOrdered);
        }

    }

}