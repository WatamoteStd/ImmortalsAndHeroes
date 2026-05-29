using System;
using System.Numerics;
using IaH.Server.Entities;
using IaH.Server.Entities.Interfaces;

namespace IaH.Server.Entities
{
    
    public class BaseEntity : IDamagable
    {
        
        public ushort ID {get; set;}
        public byte Type {get; set;}
        public int X {get; set;} = 0;
        public int Y {get; set;} = 0;
        public int Z {get; set;} = 0;
        public bool IsDead = false;
        protected float _attackCooldown = 1.7f;

        // BATTLE SYSTEM
        private float _health = 200;
        private float _maxHealth = 200;
        public float Health
        {
            get => _health;
            set
            {
                _health = Math.Clamp(value, 0f, _maxHealth);
            }

        }
        public float AttackRange = 5.0f;

        public BaseEntity(ushort id, byte type)
        {
            ID = id;
            Type = type;
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