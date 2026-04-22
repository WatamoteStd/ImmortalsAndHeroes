
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

        public NetworkManager() // constructor
        {
            _listener = new EventBasedNetListener();
            _netManager = new NetManager(_listener);

            _entityManager = new EntityManager();

            _listener.ConnectionRequestEvent += (request) =>
            {
                Console.WriteLine($"Connection request by: {request}");
                request.Accept();
            };

            _listener.PeerConnectedEvent += (peer) =>
            {
                Console.WriteLine($"ID: {peer.Id} connected to the server!");
                _entityManager.AddEntity(_entityId, 0, 0);
                _peerToEntity.Add(peer.Id, _entityId);

                // SEND THE CONNECTION EVENT PACKET
                NetDataWriter _writer = new NetDataWriter();
                _writer.Put((byte)PacketType.Welcome);
                _writer.Put(_entityId);
                peer.Send(_writer, DeliveryMethod.ReliableOrdered);

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



            // FUNCTIONS | ФУНКЦИИ

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
