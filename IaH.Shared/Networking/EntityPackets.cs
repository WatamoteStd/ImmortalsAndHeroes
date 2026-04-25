using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;

namespace IaH.Shared.Networking
{
   

    public enum PacketType : byte
    {
        Welcome,
        MoveRequest,
        SpawnEntity,
        PlayerJoined,
        HeroSelected,
        ConnectedToGame,
        EntityPosition,
        BatchEntityPositions,
        EntityStats
    }

    public struct EntityStatsPacket
    {

        public ushort EntityId;
        public byte UpdateMask;

        public EntityAttributesStats Attributes;
        public EntityVitalsStats Vitals;
        public EntityProgressionStats Progress;
        public EntityAttackType AttackType;

        public void Serialize(NetDataWriter writer)
        {

            writer.Put(EntityId);
            writer.Put(UpdateMask);

            if ((UpdateMask & 1) != 0) Vitals.Serialize(writer);
            if ((UpdateMask & 2) != 0) Attributes.Serialize(writer);
            if ((UpdateMask & 4) != 0) Progress.Serialize(writer);


        }

        public void Deserialize(NetDataReader reader)
        {

            EntityId = reader.GetUShort();
            UpdateMask = reader.GetByte();

            if ((UpdateMask & 1) != 0) Vitals.Deserialize(reader);
            if ((UpdateMask & 2) != 0) Attributes.Deserialize(reader);
            if ((UpdateMask & 4) != 0) Progress.Deserialize(reader);

        }

    }


    public struct EntityAttributesStats // health, mana, armor, speed
    {
        public short MaxHealth;
        public short MaxMana;
        public short Armor;
        public short MagicResist;
        public short Speed;
        public short Gold;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(MaxHealth);
            writer.Put(MaxMana);
            writer.Put(Armor);
            writer.Put(MagicResist);
            writer.Put(Speed);
            writer.Put(Gold);

        }
        public void Deserialize(NetDataReader reader)
        {
           MaxHealth = reader.GetShort();
           MaxMana = reader.GetShort();
           Armor = reader.GetShort();
           MagicResist = reader.GetShort();
           Speed = reader.GetShort();
           Gold = reader.GetShort();
        }
    }
    public struct EntityVitalsStats // or dynamic 
    {
        public short CurrentHp;
        public short CurrentMana;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(CurrentHp);
            writer.Put(CurrentMana);
        }

        public void Deserialize(NetDataReader reader)
        {
           CurrentHp = reader.GetShort();
           CurrentMana = reader.GetShort();
        }
    }
    public struct EntityProgressionStats  // exp, lvl
    {
        public short Lvl;
        public short CurrentExp;
        public short ExpToLvl;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Lvl);
            writer.Put(CurrentExp);
            writer.Put(ExpToLvl);
        }
        public void Deserialize(NetDataReader reader)
        {
            Lvl = reader.GetShort();
            CurrentExp = reader.GetShort();
            ExpToLvl = reader.GetShort();
        }
    }
    public enum EntityAttackType
    {
        Melee,
        Ranged
    }


    public enum CharacterType : byte
    {
       Frozen,
       VoidlessStar
    }
    


    public struct EntityPositionPacket
    {
        public ushort EntityId;
        public short x;
        public short y;
        public short z;

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
