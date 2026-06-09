using System;
using System.Numerics;
using Iah.Shared.Entities;
using Iah.Shared.Packets;
using IaH.Server.Entities.Interfaces;

namespace IaH.Server.Entities
{
    
    public class BaseEntity : IDamagable
    {

        public float HitboxHeight;
        public float HitboxRadius;
        
        public EntityStats Stats;
        public ushort ID {get; set;}
        public UnitList Unit;
        public Team EntityTeam;
        public int X {get; set;} = 0;
        public int Y {get; set;} = 0;
        public int Z {get; set;} = 0;
        public bool IsDead = false;

        // BATTLE SYSTEM
        protected float _health;
        protected float _maxHealth;
        protected float _healthRegeneration;
        public float Health
        {
            get => _health;
            set
            {
                _health = Math.Clamp(value, 0f, _maxHealth);
            }

        }

        protected float _baseSpeed;
        protected float _manna;
        protected float _maxManna;
        protected float _mannaRegeneration;
        public float Manna
        {
            get => _manna;
            set
            {
                _manna = Math.Clamp(value, 0, _maxManna);
            }
        }
        protected float _armor;

        protected float _damage;
        protected float _attackCooldown;
        protected float _attackRange;
        protected float _projectileSpeed;
        protected AttackTypes _attackType;


        public BaseEntity(ushort id, UnitList type, Team team)
        {
            EntityTeam = team;
            ID = id;
            Unit = type;

            Stats = EntityRegistry.GetStats(Unit);
            HitboxHeight = Stats.HitboxHeight;
            HitboxRadius = Stats.HitboxRadius;
            _health = Stats.MaxHealth;
            _maxHealth = Stats.MaxHealth;
            _healthRegeneration = Stats.RegenHealh;
            _mannaRegeneration = Stats.RegenMana;
            _manna = Stats.Mana;
            _maxManna = Stats.Mana;
            _baseSpeed = Stats.MoveSpeed;
            _damage = Stats.Damage;
            _attackCooldown = Stats.AttackSpeed;
            _projectileSpeed = Stats.ProjectileSpeed;
            _armor = Stats.Armor;
            _attackType = Stats.AttackType;
            _attackRange = Stats.AttackRange;

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

        // GAME FUNC ===========================================================

        public virtual void Update(float deltaTime)
        {
            
        }
        public virtual void TakeDamage(float dmg, DamageType type,HeroEntity attacker)
        {
            
            switch (type)
            {
                
                case DamageType.Physsical:
                    {
                        
                        float armorDecrease = _armor / (_armor + 30);
                        float reduction = 1f - armorDecrease;
                        Health -= dmg * reduction;

                    }
                break;
                case DamageType.Mage:
                    {
                        
                        Health -= dmg;

                    }
                break;

                 case DamageType.Pure:
                    {
                        
                        Health -= dmg * 1.1f;

                    }
                break;


            }

        }
        

    }

}