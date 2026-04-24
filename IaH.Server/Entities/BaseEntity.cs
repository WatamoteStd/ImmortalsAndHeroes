using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Server.Entities
{
    public class BaseEntity
    {

        public ushort Id;
        public short X, Y, Z;
        public int Health;
        public CharacterType SelectedHero;

        public BaseEntity(ushort id,  short x, short y, short z, CharacterType hero)
        {

            Id = id;
            X = x;
            Y = y;
            Z = z;
            Health = 100;
            SelectedHero = hero;

        }

    }
}
