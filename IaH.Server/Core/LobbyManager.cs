using System;
using System.Collections.Generic;
using System.Text;
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

        public void HandleJoinQueue(NetPeer peer)
        {
            foreach (var lobby in _allLobies)
            {
                if (lobby.TryAddPlayer(peer))
                {
                    Console.WriteLine($"[LobbyManager] ClientPeer:{peer.Id}, succesfully added to lobby!");
                    return;
                }
            }
            Lobby newLobby = new Lobby(_netManager);
            newLobby.TryAddPlayer(peer);
            _allLobies.Add(newLobby);
            Console.WriteLine($"[LobbyManager] Created new lobby for ClientPeer:{peer.Id}!");
        }

    }
}
