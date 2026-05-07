
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using LiteNetLib;
using LiteNetLib.Utils;

using IaH.Shared.Networking;
using IaH.Server.Entities;
using IaH.Shared.Networking.Events;

namespace IaH.Server.Core
{
    public class NetworkManager
    {

        private EventBasedNetListener _listener;
        private NetManager _netManager;

        public EntityManager _entityManager;
        private NetDataWriter _writer;

        // LOBBY
        private LobbyManager _lobbyManager;
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public NetworkManager() // constructor
        {
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);
            _writer = new NetDataWriter();
            _entityManager = new EntityManager();

            // DICTIONARY

            _listener.ConnectionRequestEvent += (request) =>
            {
                Console.WriteLine($"Connection request by: {request}");
                request.Accept();
            };

            _listener.PeerConnectedEvent += (peer) =>
            {

                var player = new Player
                {
                    PeerId = peer.Id,
                    CurrentState = Player.PlayerStates.InMenu
                };
                _players.Add(peer.Id, player);
                Console.WriteLine($"[SERVER] New Player session created. PeerId: {peer.Id}");


            };
            _listener.PeerDisconnectedEvent += (peer, disconnectedInfo) =>
            {
                

            };

            _listener.NetworkReceiveEvent += OnPacketReceived;

            // EVENTS
            EventBus.OnHpChanged += OnHealthChanged;
            EventBus.OnPlayerJoinedQueue += (peer) =>
            {
                _writer.Reset();
                _writer.Put((byte)PacketType.LobbyJoined);
                peer.Send(_writer, DeliveryMethod.ReliableOrdered);
            };
        }

        // FUNCTIONS | ФУНКЦИИ

        private void OnPacketReceived(NetPeer peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod)
        {

            PacketType rawByte = (PacketType)reader.GetByte();

            switch (rawByte)
            {

                case PacketType.JoinQueue:

                    _lobbyManager.HandleJoinQueue(peer);

                    break;

                case PacketType.ChatMessage:

                    string msg = reader.GetString();
                    _writer.Reset();
                    _writer.Put((byte)PacketType.ChatMessage);
                    _writer.Put(msg);

                    _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);
                    Console.WriteLine($"[CHAT] Broad-casted:{msg}");

                    break;

                case PacketType.MoveRequest:
                    {
                       
                    }
                    break;

                case PacketType.HeroSelected:
                    {
                        

                        break;
                    }
                case PacketType.AttackRequest:
                    
                       
                        break;

            }
        }
        private void OnHealthChanged(EntityHpChangedEvent data)
        {
            Console.WriteLine("EventBus | Event 'HealthChange' received succesfully!");
            _writer.Reset();
            _writer.Put((byte)PacketType.EntityStats);
            EntityStatsPacket packet = new EntityStatsPacket
            {
                EntityId = data.EntityId,
                UpdateMask = 1,
                Vitals = new EntityVitalsStats { CurrentHp = (short)data.NewHp, CurrentMana = (short)data.NewMp }
            };
            packet.Serialize(_writer);
            _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);
            
        }
        public void SendToLobby(List<NetPeer> recipients, IEnumerable<BaseEntity> entities)
        {
            _writer.Reset();
            _writer.Put((byte)PacketType.BatchEntityPositions);
            _writer.Put((short)entities.Count());

            foreach (var entity in entities)
            {
                _writer.Put((ushort)entity.Id);
                _writer.Put((short)entity.X); 
                _writer.Put((short)entity.Y);
                _writer.Put((short)entity.Z);
            }

           
            foreach (var peer in recipients)
            {
                peer.Send(_writer, DeliveryMethod.Unreliable);
            }
        }

        public void SendPacketToLobby(List<NetPeer> recipients, NetDataWriter writer)
        {
            foreach (var peer in recipients)
            {
                peer.Send(writer, DeliveryMethod.ReliableUnordered);
            }
        }

        public void SetLobbyManager(LobbyManager manager)
        {
            _lobbyManager = manager;
        }

        public void Start()
        {

            _netManager.Start(9050);


        }
        public void Update()
        {
            _netManager.PollEvents();
        }

    }
}
