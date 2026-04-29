
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
                case PacketType.ConnectedToGame:
                    {
                        if (!_peerToEntity.TryGetValue(peer.Id, out ushort targetId)) break;

                        BaseEntity _entity = _entityManager.GetEntity(targetId);

                        // SEND ALL !INFO! ABOUT NEW ENTITY
                        _writer.Reset();
                        _writer.Put((byte)PacketType.PlayerJoined);
                        _writer.Put((ushort)_entity.Id);
                        ;_writer.Put((byte)_entity.SelectedHero); // CharacterType
                        _writer.Put((short)_entity.X);
                        _writer.Put((short)_entity.Y);
                        _writer.Put((short)_entity.Z);
                        _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);                 
                        Console.WriteLine($"[SERVER] ConnectedToGame: PeerId:{peer.Id} | EntityId:{_entity.Id}");

                        // SEND ALL PLAYER STATS TO CLIENT 
                        if (_entity is Hero hero)
                        {
                            var stats = hero.GetStatsPacket(7); // all stats
                            _writer.Reset();
                            _writer.Put((byte)PacketType.EntityStats);
                            stats.Serialize(_writer);
                            _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);

                        }


                        // SEND NEW PLAYER INFORMATION ABOUT OLD PLAYERS
                        foreach (BaseEntity _curEntity in _entityManager.GetActiveEntities())
                        {
                            if (_curEntity.Id == targetId) continue;

                            _writer.Reset();
                            _writer.Put((byte)PacketType.PlayerJoined);
                            _writer.Put((ushort)_curEntity.Id);
                            _writer.Put((byte)_curEntity.SelectedHero); // CharacterType
                            _writer.Put((short)_curEntity.X);
                            _writer.Put((short)_curEntity.Y);
                            _writer.Put((short)_curEntity.Z);
                            peer.Send(_writer, DeliveryMethod.ReliableOrdered);

                            if (_curEntity is Hero curHero)
                            {
                                _writer.Reset();
                                _writer.Put((byte)PacketType.EntityStats);
                                var stats = curHero.GetStatsPacket(7);
                                stats.Serialize(_writer);
                                peer.Send(_writer, DeliveryMethod.ReliableOrdered);
                            }

                        }

                        break;

                    }
                case PacketType.AttackRequest:
                    {
                        Console.WriteLine("[SERVER] AttackRequestPacket received!");

                        ushort _cId = reader.GetUShort();
                        ushort _tId = reader.GetUShort();

                        // ВЫВЕДИ ВСЕ АКТИВНЫЕ ID
                        var allEntities = _entityManager.GetActiveEntities();
                        Console.WriteLine($"[DEBUG] Entities in world: {string.Join(", ", allEntities.Select(e => e.Id))}");

                        // 1. Читаем данные из пакета (клиент прислал ID того, кого хочет ударить)
                        // ВАЖНО: Если клиент шлет и свой ID, и чужой, сначала вычитай их оба
                        ushort _clientSaidMyId = reader.GetUShort(); // Мы это выкинем, но прочитать надо, чтобы сдвинуть указатель
                        ushort _targetEntityId = reader.GetUShort();

                        // 2. Достаем ИСТИННЫЙ ID атакующего по его соединению (peer.Id)
                        if (_peerToEntity.TryGetValue(peer.Id, out ushort myActualHeroId))
                        {
                            BaseEntity attacker = _entityManager.GetEntity(myActualHeroId);
                            BaseEntity target = _entityManager.GetEntity(_targetEntityId);

                            // 3. ПРОВЕРКА НА NULL (Чтобы сервер не падал!)
                            if (attacker == null || target == null)
                            {
                                Console.WriteLine($"[SERVER] Attack failed: One of entities is null! Attacker: {attacker != null}, Target: {target != null}");
                                break;
                            }

                            // 4. Логика дистанции
                            Vector3 targetPos = new Vector3(target.X / 100.0f, target.Y / 100.0f, target.Z / 100.0f);

                            if (attacker is Hero hero)
                            {
                                if (Vector3.Distance(hero.GlobalPos, targetPos) <= hero.AttackRange)
                                {
                                    hero._currentTarget = target; // НЕ ЗАБУДЬ ПРИСВОИТЬ ЦЕЛЬ
                                    hero.CurrentState = Hero.StateMachine.Attack;
                                    Console.WriteLine($"[SERVER] Hero {myActualHeroId} started attacking {target.Id}");
                                }
                                else
                                {
                                    Console.WriteLine("[SERVER] Too far to attack!");
                                    // Тут можно переключить в стейт Chase (преследование)
                                }
                            }
                        }
                    }
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
