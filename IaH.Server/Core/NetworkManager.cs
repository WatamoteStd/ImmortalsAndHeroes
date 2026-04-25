
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using LiteNetLib;
using LiteNetLib.Utils;
using IaH.Shared.Networking;
using IaH.Server.Entities;

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
                peer.Send(_writer, DeliveryMethod.ReliableOrdered);

                Console.WriteLine($"TOTAL CLIENTS CONNECTED:{_netManager.ConnectedPeersCount}");
                _entityId++;


            };
            _listener.PeerDisconnectedEvent += (peer, disconnectedInfo) =>
            {

                if (_peerToEntity.TryGetValue(peer.Id, out ushort entityId))
                {

                    _entityManager.RemoveEntity(entityId);
                    _peerToEntity.Remove(peer.Id);
                    Console.WriteLine($"Entity {entityId} removed from the world!");

                }

            };

            _listener.NetworkReceiveEvent += OnPacketReceived;

        }

        // FUNCTIONS | ФУНКЦИИ

        private void OnPacketReceived(NetPeer peer, NetDataReader reader, byte channel, DeliveryMethod deliveryMethod)
        {

            PacketType rawByte = (PacketType)reader.GetByte();

            switch (rawByte)
            {

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
                        CharacterType _hero = (CharacterType)reader.GetByte();
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

                        _entityManager.AddEntity(_entityId, posX, 200, posZ, _hero);
                        _peerToEntity.Add(peer.Id, _entityId);
                        _entityId++;
                        Console.WriteLine($"[SERVER] Spawning Hero: {_hero} for EntityID: {_entityId} at X: {posX}");

                        break;
                    }
                case PacketType.ConnectedToGame:
                    {
                        if (!_peerToEntity.TryGetValue(peer.Id, out ushort targetId)) break;

                        ushort _targetId = _peerToEntity[peer.Id];
                        BaseEntity _entity = _entityManager.GetEntity(_targetId);

                        // SEND ALL INFO ABOUT NEW PLAYER
                        _writer.Reset();
                        _writer.Put((byte)PacketType.PlayerJoined);
                        _writer.Put((ushort)_entity.Id);
                        _writer.Put((byte)_entity.SelectedHero); // CharacterType
                        _writer.Put((short)_entity.X);
                        _writer.Put((short)_entity.Y);
                        _writer.Put((short)_entity.Z);
                        _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);
                        _entityManager.EntityCount(_netManager.ConnectedPeersCount);
                        Console.WriteLine($"Данные о ServerID:{peer.Id}, GameID:{_entity.Id} были отправлены всем игрокам!");

                        // SEND NEW PLAYER INFORMATION ABOUT OLD PLAYERS
                        foreach (BaseEntity _curEntity in _entityManager.GetActiveEntities())
                        {
                            if (_curEntity.Id == _targetId) continue;

                            _writer.Reset();
                            _writer.Put((byte)PacketType.PlayerJoined);
                            _writer.Put((ushort)_curEntity.Id);
                            _writer.Put((byte)_curEntity.SelectedHero); // CharacterType
                            _writer.Put((short)_curEntity.X);
                            _writer.Put((short)_curEntity.Y);
                            _writer.Put((short)_curEntity.Z);
                            peer.Send(_writer, DeliveryMethod.ReliableOrdered);

                        }

                        break;

                    }

            }

        }

        public void BroadcastPosition(IEnumerable<BaseEntity> entities)
        {

            entities = _entityManager.GetActiveEntities();
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

            _netManager.SendToAll(_writer, DeliveryMethod.Unreliable);

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
