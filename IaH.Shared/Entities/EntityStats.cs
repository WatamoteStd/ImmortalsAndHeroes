using System;
using Iah.Shared.Packets;

namespace Iah.Shared.Entities
{

    public enum AttackTypes : byte
    {
        Melee,
        Ranged
    }
    
    public readonly struct EntityStats
    {
        
        public readonly float HitboxRadius;
        public readonly float HitboxHeight;
        public readonly float MaxHealth;
        public readonly float RegenHealh;
        public readonly float MoveSpeed;
        public readonly float Damage;
        public readonly float Mana;
        public readonly float RegenMana;
        public readonly float AttackSpeed;
        public readonly float ProjectileSpeed;
        public readonly float Armor;
        public readonly float AttackRange;
        public readonly AttackTypes AttackType;


        public EntityStats(float hitboxSize, float hitboxHeight, float maxHp, float regenHealh, float moveSpeed, float dmg, float mp, float regenMana,
        float attackSpeed, float projectileSpeed, float armor, float attackRange, AttackTypes attackType)
        {
            HitboxRadius = hitboxSize;
            HitboxHeight = hitboxHeight;
            MaxHealth = maxHp;
            RegenHealh = regenHealh;
            MoveSpeed = moveSpeed;
            Damage = dmg;
            Mana = mp;
            RegenMana = regenMana;
            AttackSpeed = attackSpeed;
            ProjectileSpeed = projectileSpeed;
            Armor = armor;
            AttackRange = attackRange;
            AttackType = attackType;


        }

    }

}