using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking.Events;
using IaH.Server.Core;

namespace IaH.Server.Entities
{
    public class BaseEntity : IDamable
    {

        

        public ushort Id;
        public short X, Y, Z;
        public float Healths;
        public float Mana;
        public CharacterType SelectedHero;

        public BaseEntity(ushort id,  short x, short y, short z, CharacterType hero)
        {

            Id = id;
            X = x;
            Y = y;
            Z = z;
            Healths = 100;
            Mana = 50;
            SelectedHero = hero;

        }

        public virtual void TakeDamage(DamageType type, float damage)
        {
            Healths -= damage;
            var message = new EntityHpChangedEvent(Id, Healths, Mana);
            EventBus.PublishHp(message);
        }

    }
}
