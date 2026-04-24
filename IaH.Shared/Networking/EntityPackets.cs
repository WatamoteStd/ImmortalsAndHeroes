using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Shared.Networking
{
   

    public enum CharacterType : byte
    {
        Warrior,
        Archer,
        Mage,
        Tank
    }
    public enum PacketType : byte
    {
        Welcome,
        MoveRequest,
        SpawnEntity,
        PlayerJoined,
        HeroSelected,
        ConnectedToGame
    }

    public struct ConnectedToGamePacket
    {

    }

    public struct PlayerJoinedPacket
    {
        public ushort EntityId;
        public CharacterType SelectedHero;
        public short X, Y, Z;

    }

    public struct MoveRequestPacket
    {

        public short X;
        public short y;
        public short Z;

    }

    public struct WelcomePacket
    {
        public ushort MyEntityId;
    }

    public struct EntityData
    {



    }
    public struct HeroSelectedPacket
    {
        public CharacterType SelectedHero;
    }

}
