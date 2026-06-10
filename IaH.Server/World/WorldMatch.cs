using System;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using IaH.Server.Managers;
using IaH.Server.PlayerClasses;
using IaH.Server.Entities;
using Iah.Shared.Packets;

namespace IaH.Server.World
{
    
    public class WorldMatch
    {
        public enum MatchState { Loading, Game, End}
        public MatchState CurrentState {get; private set;} = MatchState.Loading;
        public ushort MatchID;

        private NetDataWriter _writer;

        // PLAYERS & ENTITY WORK
        private List<Player> playersList;
        public Dictionary<Player, HeroEntity> _playerToHero {get; private set;}
        private EntityManager _entityManager;


        public WorldMatch(List<Player> players, ushort id)
        {
            _playerToHero = new Dictionary<Player, HeroEntity>();
            _entityManager = new EntityManager(); 
            _writer = new NetDataWriter();
            MatchID = id;
            playersList = players;
            foreach (var player in playersList)
            {
                player.CurrentMatchID = this.MatchID;
            }
        }

        public void UpdateMatch(float deltaTime)
        {
            
            switch (CurrentState)
            {
                
                case MatchState.Loading:

                break;

                case MatchState.Game: 

                    foreach (var hero in _playerToHero.Values)
                    {
                        hero.Update(deltaTime);

                        if (hero.CurrentState == HeroEntity.State.Move || hero.CurrentState == HeroEntity.State.Chase)
                        {
                            
                            _writer.Reset();
                            _writer.Put((byte)PacketType.EntityMove);

                            _writer.Put((ushort)hero.ID);
                            _writer.Put(hero.X);
                            _writer.Put(hero.Y);
                            _writer.Put(hero.Z);
                            _writer.Put((short)(hero.CurrentDirection.X * 100));
                            _writer.Put((short)(hero.CurrentDirection.Y * 100));
                            _writer.Put((short)(hero.CurrentDirection.Z * 100));

                            SendToAll(_writer, DeliveryMethod.Unreliable);

                        }

                    }

                break;

                case MatchState.End:

                break;

            }

        }

        // NETWORK WORKING
        public void UpdatePlayerReady(Player player)
        {
            player.isLoadOnMatch = true;

            foreach (var p in playersList)
            {
                if (p.isLoadOnMatch == false) return;
            }
            CurrentState = MatchState.Game;

            foreach (var p in playersList)
            {
                HeroEntity newHero = new HeroEntity( 0, p.SelectedHero, p.TeamID, p, this); // ID 0 BCS ENTITY MANAGER REWRITE IT
                _entityManager.AddEntity(newHero);
                _playerToHero[p] = newHero;

            }
            // SEND INFO ABOUT ALL ENTITY TO PLAYERS
            // SEND PLAYERS ENTITY
            _writer.Reset();
            foreach (var p in playersList)
            {
                _writer.Reset();
                _writer.Put((byte)PacketType.SpawnEntity);

                if (_playerToHero.TryGetValue(p, out HeroEntity? curHero))
                {

                    _writer.Put((ushort)curHero.ID);
                    _writer.Put((ushort)p.ID);
                    _writer.Put((byte)curHero.Unit);
                    _writer.Put((byte)curHero.EntityTeam);
                    _writer.Put((short)curHero.X);
                    _writer.Put((short)curHero.Y);
                    _writer.Put((short)curHero.Z);
                    SendToAll(_writer, DeliveryMethod.ReliableOrdered);
                    
                } 
                
            }

        }



        // FOR ENTITY -> NETWORK COMMUNICATION
        public void BroadcastDamage(int dmg, ushort targetId)
        { 

            _writer.Reset();
            _writer.Put((byte)PacketType.AttackInfo);
            _writer.Put((ushort)targetId);
            _writer.Put((ushort)dmg);
            _writer.Put((ushort)_entityManager.EntityById(targetId).Health);
            SendToAll(_writer, DeliveryMethod.ReliableOrdered);
        }




        public BaseEntity? EntityByID(ushort id)
        {
            return _entityManager.EntityById(id);
        }
        public void SendToAll(NetDataWriter writer, DeliveryMethod method)
        {
            foreach (var pl in playersList)
            {

                pl.PlayerPeer.Send(writer, method);
            }
        }

        // FOR CONSOLE DEBUG
        public int TotalPlayersCount()
        {
            return playersList.Count;
        }
        public List<Player> MatchPlayers()
        {
            return playersList;
        }

    }

}