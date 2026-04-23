
using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;
using IaH.Shared.Networking;

namespace IaH.Server.Core
{
    public class NetworkManager
    {
        private readonly Dictionary<int, ushort> _peerToEntity = new Dictionary<int, ushort>();
        private ushort _entityId = 0;

        private EventBasedNetListener _listener;
        private NetManager _netManager;

        private EntityManager _entityManager;
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

                _entityManager.EntityCount();
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

                case PacketType.EntityPosition:

                    ushort id = reader.GetUShort();
                    short x = reader.GetShort();
                    short y = reader.GetShort();
                    short z = reader.GetShort();
                break;

                case PacketType.HeroSelected:

                    CharacterType Hero = (CharacterType)reader.GetByte();

                    _entityManager.AddEntity(_entityId, 0, 0, 0, Hero);
                    _peerToEntity.Add(peer.Id, _entityId);

                    _writer.Reset();
                    _writer.Put((byte)PacketType.PlayerJoined);
                    _writer.Put((ushort)_entityId);
                    _writer.Put((byte)Hero);
                    _writer.Put((short)0);
                    _writer.Put((short)0);
                    _writer.Put((short)0);
                    _netManager.SendToAll(_writer, DeliveryMethod.ReliableOrdered);
                    Console.WriteLine($"Данные о ServerID:{peer.Id}, GameID:{_entityId} были отправлены всем игрокам!");

                break;

            }

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
