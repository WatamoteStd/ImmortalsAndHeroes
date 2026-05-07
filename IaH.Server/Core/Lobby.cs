using IaH.Server.Core;
using IaH.Server.Entities;
using IaH.Shared.Networking;
using LiteNetLib;
using LiteNetLib.Layers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Server.Core
{
    public class Lobby
    {
        public enum LobbyStatus
        {
            Waiting,
            Picking,
            InGame
        }
        public LobbyStatus State = LobbyStatus.Waiting;
        public List<Player> playersList = new List<Player>();
        private EntityManager _entityManager = new EntityManager();
        private NetworkManager _netManager;
        private NetDataWriter _writer;

        public Lobby(NetworkManager managerFromMain)
        {
            _netManager = managerFromMain;
        }

        public bool TryAddPlayer(Player player)
        {
            if (playersList.Count >= 10 || State != LobbyStatus.Waiting) return false;

            playersList.Add(player);
            return true;
        }

        public void Update(float deltaTime)
        {
            if (State != LobbyStatus.InGame) return; 

            var entities = _entityManager.GetActiveEntities();
            foreach (var entity in entities)
            {
                if (entity is Hero _hero)
                {
                    _hero.Update(deltaTime);

                }
            }
           
            _netManager.SendToLobby(playersList, entities);
            
        }
        

    }
}
