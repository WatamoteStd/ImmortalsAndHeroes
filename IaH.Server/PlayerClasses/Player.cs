using System;
using Iah.Shared.Packets;
using IaH.Server.Managers;
using IaH.Server.World;
using LiteNetLib;

namespace IaH.Server.PlayerClasses
{
    
    public class Player
    {
        public enum State {Auth, InMenu, SearchTheGame, InLobby, InGame};
        public ushort ID {get; private set;}
        public Lobby? CurrentLobby {get; set;}

        public ushort CurrentMatchID {get; set;} = 0;
        public bool isLoadOnMatch = false;
        public NetPeer PlayerPeer {get; private set;}
        public string Nickname {get; private set;}
        public State CurrentState = State.Auth;
        public UnitList SelectedHero = (byte)UnitList.None;
        
        public Player(ushort id, NetPeer netPeer, string nickname)
        {
            
            ID = id;
            PlayerPeer = netPeer;
            Nickname = nickname;

        }

    }

}