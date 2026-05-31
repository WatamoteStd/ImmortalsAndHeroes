using System;
using System.Numerics;
using Iah.Shared.Entities;
using Iah.Shared.Packets;
using IaH.Server.Entities.Interfaces;

namespace IaH.Server.Entities
{
    
    public class BaseEntity : IDamagable
    {
        
        public EntityStats Stats;
        public ushort ID {get; set;}
        public UnitList Unit;
        public int X {get; set;} = 0;
        public int Y {get; set;} = 0;
        public int Z {get; set;} = 0;
        public bool IsDead = false;
        protected float _attackCooldown = 1.7f;

        // BATTLE SYSTEM
        protected float _health;
        protected float _maxHealth;
        public float Health
        {
            get => _health;
            set
            {
                _health = Math.Clamp(value, 0f, _maxHealth);
            }

        }
        public float AttackRange = 5.0f;
        protected float _baseSpeed;
        protected float _manna;
        protected float _maxManna;
        public float Manna
        {
            get => _manna;
            set
            {
                _manna = Math.Clamp(value, 0, _maxManna);
            }
        }
        public float Damage;

        public BaseEntity(ushort id, UnitList type)
        {
            ID = id;
            Unit = type;
            Stats = EntityRegistry.GetStats(Unit);
            _health = Stats.MaxHealth;
            _maxHealth = Stats.MaxHealth;
            _manna = Stats.Mana;
            _maxManna = Stats.Mana;
            _baseSpeed = Stats.MoveSpeed;
            Damage = Stats.Damage;

        }


        // SYSTEM FUCNTIONS

        public ushort GetMaxHealth()
        {
            return (ushort)_maxHealth;
        }

         public Vector3 GetNetworkPosition()
        {
            Vector3 pos = new Vector3(X, Y, Z); // always short for server-client
            return pos;
        }
        public Vector3 GetWorldPosition()
        {
            Vector3 pos = new Vector3(X / 100f, Y / 100f, Z / 100f);
            return pos;
        }

        public virtual void Update(float deltaTime)
        {
            
        }
        public virtual void TakeDamage(float dmg, HeroEntity attacker)
        {
            Health -= dmg;
        }
        

    }

}