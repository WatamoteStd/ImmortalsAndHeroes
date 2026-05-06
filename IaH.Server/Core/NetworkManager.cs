
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
        private readonly Dictionary<int, ushort> _peerToEntity = new Dictionary<int, ushort>();
        private ushort _entityId = 0;

        private EventBasedNetListener _listener;
        private NetManager _netManager;

        public EntityManager _entityManager;
        private NetDataWriter _writer;

        // LOBBY
        private LobbyManager _lobbyManager;

        public NetworkManager() // constructor
        {
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);
            _writer = new NetDataWriter();


            _entityManager = new EntityManager();

            _listener.ConnectionRequestEvent += (request) =>
            {
                Console.WriteLine($"Connection request by: {request}");
                request.Accept();
            };

            _listener.PeerConnectedEvent += (peer) =>
            {
                _writer.Reset();
                _writer.Put((byte)PacketType.Welcome);
                _writer.Put((ushort)peer.Id);
                peer.Send(_writer, DeliveryMethod.ReliableOrdered);

                Console.WriteLine($"[SERVER] PeerConnected! PeerId:{peer.Id} | Entity Id: NotOrdered Yet.");


            };
            _listener.PeerDisconnectedEvent += (peer, disconnectedInfo) =>
            {
                Console.WriteLine($"[SERVER] PeerId:{peer.Id} disconnected from the server.");

                if (_peerToEntity.TryGetValue(peer.Id, out ushort entityId))
                {
                    _writer.Reset();
                    _writer.Put((byte)PacketType.EntityRemove);
                    _writer.Put((ushort)entityId);
                    _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);

                    _entityManager.RemoveEntity(entityId);
                    _peerToEntity.Remove(peer.Id);
                    Console.WriteLine($"Entity {entityId} removed from the world!");

                }

            };

            _listener.NetworkReceiveEvent += OnPacketReceived;

            // EVENTS
            EventBus.OnHpChanged += OnHealthChanged;

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

                case PacketType.MoveRequest:
                    {
                        short x = reader.GetShort();
                        short y = reader.GetShort();
                        short z = reader.GetShort();

                        Vector3 _finalCords = new Vector3(x / 100.0f, y / 100.0f, z / 100.0f);

                        Console.WriteLine($"[SERVER] Получен запрос на движение от Peer {peer.Id}: {_finalCords}");

                        if (_peerToEntity.TryGetValue(peer.Id, out ushort myEntityId))
                        {

                            Hero _playerHero = _entityManager.GetEntity(myEntityId) as Hero;

                            if (_playerHero != null)
                            {
                                _playerHero.TargetPosition = _finalCords;
                                _playerHero.CurrentState = Hero.StateMachine.Move;
                                Console.WriteLine($"[SERVER] Установили цель для Entity {myEntityId}: {_finalCords}");
                            }

                        }
                    }
                    break;

                case PacketType.HeroSelected:
                    {
                        if (_peerToEntity.ContainsKey(peer.Id))
                        {
                            Console.WriteLine($"[SERVER] PeerId:{peer.Id} already have entity selected!");
                            return;
                        }
                        CharacterType _selectedHero = (CharacterType)reader.GetByte();
                        short posX, posZ;
                        if (_entityId % 2 == 0)
                        {
                            posX = -2000;
                            posZ = 0;
                        }
                        else
                        {
                            posX = 2000;
                            posZ = 0;
                        }

                        _entityManager.AddEntity(_entityId, posX, 200, posZ, _selectedHero);
                        _peerToEntity.Add(peer.Id, _entityId);
                        Console.WriteLine($"[SERVER] CreatedEntity:{_selectedHero} | EntityID: {_entityId} | PeerID: {peer.Id}");
                        _entityId++;


                        break;
                    }
                case PacketType.AttackRequest:
                    {
                        Console.WriteLine("[SERVER] AttackRequestPacket received!");

                        var attackerId = reader.GetUShort();
                        var enemyId = reader.GetUShort();
                        var attackerEntity = _entityManager.GetEntity(attackerId);
                        var attackerVictim = _entityManager.GetEntity(enemyId);

                        if (attackerEntity != null && attackerEntity is Hero attacker)
                        {

                            if (attackerVictim != null && attackerVictim is BaseEntity victim)
                            {
                                attacker.CurrentState = Hero.StateMachine.Attack;
                                attacker._currentTarget = victim;
                                Console.WriteLine("Attack: target valid!");
                            }

                        }


                        break;

                    }

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
