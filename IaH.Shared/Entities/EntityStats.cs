using System;
using Iah.Shared.Packets;

namespace Iah.Shared.Entities
{
    
    public readonly struct EntityStats
    {
        
        public readonly float MaxHealth;
        public readonly float MoveSpeed;
        public readonly float Damage;
        public readonly float Mana;

        public EntityStats(float maxHp, float moveSpeed, float dmg, float mp)
        {
            
            MaxHealth = maxHp;
            MoveSpeed = moveSpeed;
            Damage = dmg;
            Mana = mp;

        }

    }

}