using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Server.Entities
{
    public class BaseEntity : IDamable
    {

        public ushort Id;
        public short X, Y, Z;
        public int Healths;
        public CharacterType SelectedHero;

        public BaseEntity(ushort id,  short x, short y, short z, CharacterType hero)
        {

            Id = id;
            X = x;
            Y = y;
            Z = z;
            Healths = 100;
            SelectedHero = hero;

        }

        public virtual void TakeDamage(DamageType type, float damage)
        {
            Healths -= (int)damage;
        }

    }
}
