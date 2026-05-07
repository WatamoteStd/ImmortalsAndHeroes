using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Server.Entities
{
    public class Player
    {
        // Net data
        public int PeerId { get; set; }
        public string Name { get; set; }

        // states
        public enum PlayerStates
        {
            InMenu,
            InLobby,
            Picking,
            Disconnected,
            LobbyDisconnected,
            InGame
        }
        public PlayerStates CurrentState { get; set; } = PlayerStates.InMenu;

        public ushort EntityId { get; set; } = ushort.MaxValue;
        public CharacterType SelectedHero { get; set; }

        public Player()
        {
            Name = "UnknownPlayer"; //default value
            CurrentState = PlayerStates.InMenu;
        }

    }
}
