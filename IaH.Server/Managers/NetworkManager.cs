using System;
using Iah.Shared.Packets;
using LiteNetLib;
using LiteNetLib.Utils;
using IaH.Server.PlayerClasses;
using IaH.Server.World;
using System.Text.RegularExpressions;
using IaH.Server.Entities;

namespace IaH.Server.Managers
{
    
    public class NetworkManager
    {
        private EventBasedNetListener _listener;
        private NetManager _netManager;
        private NetDataWriter _writer;
        private LobbyManager _lobbyManager;
        private ushort playerID = 0;

        private readonly Dictionary<int, Player> _peerToPlayer;
        
        public NetworkManager()
        {
            // INIT ==============================================================

            _writer = new NetDataWriter();
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);
            _lobbyManager = new LobbyManager(_netManager);

            _peerToPlayer = new Dictionary<int, Player>();


            // LISTENER SUBSCRIPBES =============================================
            _listener.ConnectionRequestEvent += (request) =>
            {
                request.Accept();
            };

            _listener.PeerConnectedEvent += (peer) =>
            {
                _writer.Reset();
                _writer.Put((byte)PacketType.FirstConnect);
                peer.Send(_writer, DeliveryMethod.ReliableOrdered);
                Console.WriteLine($"[SERVER] PeerId:{peer.Id} conenct to server. Status: Loging-in");
            };

            _listener.PeerDisconnectedEvent += (peer, info) =>
            {
                
                if (_peerToPlayer.TryGetValue(peer.Id, out Player? player))
                {
                    Console.WriteLine($"[SERVER] Player:{player.Nickname} disconnect from the server.");
                    _peerToPlayer.Remove(peer.Id);
                }
                else Console.WriteLine($"[SERVER: Non-logged player disconnecteed. PeerId:{peer.Id}");

            };

            _listener.NetworkReceiveEvent += OnPacketReceived;


        }

        public void Start(int port)
        {
            _netManager.Start(port);
        }

        public void Update(float deltaTime)
        {
            _netManager.PollEvents();

            _lobbyManager.Update(deltaTime);
            
        }

        private void OnPacketReceived(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            
            PacketType type = (PacketType)reader.GetByte();

            switch (type)
            {
                
                case PacketType.JoinQueue:
                    var player = _peerToPlayer[peer.Id];
                    Console.WriteLine($"[SERVER] Player:{player.Nickname} search the game...");
                    player.CurrentState = Player.State.SearchTheGame;
                    _lobbyManager.AddPlayerToQueue(player);

                    // RESPONSE TO CLIENT FOR SEARCH TIMER
                    _writer.Reset();
                    _writer.Put((byte)PacketType.JoinQueueResponse);
                    peer.Send(_writer, DeliveryMethod.ReliableOrdered);

                break;
                case PacketType.LoginRequest:
                    string name = reader.GetString();
                    Player newPlayer = new Player((ushort)playerID, peer, name);
                    _peerToPlayer.Add(peer.Id, newPlayer);
                    Console.WriteLine($"[SERVER] Player: {_peerToPlayer[peer.Id].ID} logged in. Nickname: {name}");
                    _peerToPlayer[peer.Id].CurrentState = Player.State.InMenu;

                    // SEND LOGIN RESPONSE
                    _writer.Reset();
                    _writer.Put((byte)PacketType.LoginResponse);
                    _writer.Put((ushort)playerID);
                    peer.Send(_writer, DeliveryMethod.ReliableOrdered);

                    playerID++;

                break;
                case PacketType.HeroSelected:
                    
                    {
                    UnitList hero = (UnitList)reader.GetByte();
                    Player p = _peerToPlayer[peer.Id];
                    p.SelectedHero = hero;
                    p.CurrentLobby?.HeroSelected(p, hero);

                    }

                break;
                
                // GAME PACKETS
                case PacketType.ReadyToGame:
                    {

                    Player p = _peerToPlayer[peer.Id];
                    ushort matchID = p.CurrentMatchID;
                    WorldMatch? match = _lobbyManager.MatchByID(matchID);
                    match?.UpdatePlayerReady(p);

                    }
                    break;

                case PacketType.EntityMove:
                    {
                        
                        int x = reader.GetInt();
                        int y = reader.GetInt();
                        int z = reader.GetInt();
                        Player p = _peerToPlayer[peer.Id];
                        ushort matchId = p.CurrentMatchID;
                        WorldMatch? m = _lobbyManager.MatchByID(matchId);

                        if (m != null && m._playerToHero.ContainsKey(p)) 
                        {
                        HeroEntity hero = m._playerToHero[p];
                        hero.SetMoveTarget(x, y, z);
                        }


                    }
                break;
                case PacketType.AutoAttack:
                    {
                        
                        ushort targetId = reader.GetUShort();
                        Player p = _peerToPlayer[peer.Id];
                        ushort matchId = p.CurrentMatchID;
                        WorldMatch? m = _lobbyManager.MatchByID(matchId);

                        if (m != null && m._playerToHero.ContainsKey(p))
                        {
                            BaseEntity? target = m.EntityByID(targetId);
                            if (target == null)  return;
                            HeroEntity hero = m._playerToHero[p];

                            hero.SetAttackTarget(target);
                            hero.CurrentState = HeroEntity.State.Chase;
                        }

                    }
                break;

                // SKILLS
                

            }

            reader.Recycle();

        }


        private WorldMatch? GetMatch(Player p)
        {
            ushort mID = p.CurrentMatchID;
            return _lobbyManager.MatchByID(mID);

        }

        // FOR CONSOLE DEBUG
        public void LobbyListRequest()
        {
            _lobbyManager.LobbyList();
            
        }
        public void MatchListRequest()
        {
            _lobbyManager.MatchList();
        }
    }

}