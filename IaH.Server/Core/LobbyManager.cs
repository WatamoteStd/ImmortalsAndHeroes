using System;
using System.Collections.Generic;
using System.Text;
using IaH.Server.Entities;
using LiteNetLib;
using LiteNetLib.Utils;

namespace IaH.Server.Core
{
    public class LobbyManager
    {

        private List<Lobby> _allLobies;
        private NetworkManager _netManager;

        public LobbyManager(List<Lobby> lobbbies, NetworkManager netManager)
        {
            _allLobies = lobbbies;
            _netManager = netManager;
        }

        public void HandleJoinQueue(Player player )
        {
            if (player.CurrentState == Player.PlayerStates.InLobby) return;

            player.CurrentState = Player.PlayerStates.InLobby;
            foreach (var lobby in _allLobies)
            {
                if (lobby.TryAddPlayer(player))
                {
                    Console.WriteLine($"[LobbyManager] Player:{player.Name}, succesfully added to lobby!");
                    EventBus.PublishPlayerJoinedQueue(player);
                    return;
                }
            }
            Lobby newLobby = new Lobby(_netManager);
            newLobby.TryAddPlayer(player);
            _allLobies.Add(newLobby);
            EventBus.PublishPlayerJoinedQueue(player);
            Console.WriteLine($"[LobbyManager] Created new lobby for Player:{player.Name}!");
        }

    }
}
