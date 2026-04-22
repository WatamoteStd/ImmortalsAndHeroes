using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Shared.Networking
{
   
    public struct EntityPositionPacket
    {

        public ushort EntityId; // Any entity ID

        public short X;
        public short Z;

    }

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
        EntityPosition,
        SpawnEntity
    }

    public struct WelcomePacket
    {
        public ushort MyEntityId;
    }

}
