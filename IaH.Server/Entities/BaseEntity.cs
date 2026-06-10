using System;
using System.Numerics;
using Iah.Shared.Entities;
using Iah.Shared.Packets;
using IaH.Server.Entities.Interfaces;
using IaH.Server.PlayerClasses;
using IaH.Server.World;

namespace IaH.Server.Entities
{
    
    public class BaseEntity : IDamagable
    {

        public float HitboxHeight;
        public float HitboxRadius;
        
        public EntityStats Stats;
        public ushort ID {get; set;}
        public WorldMatch _match;
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
        protected float _currentAttackCooldown;
        protected float _attackCooldown;
        protected float _attackRange;
        protected float _projectileSpeed;
        protected AttackTypes _attackType;

        // STATES
        public enum State {Idle, Move, Chase, Attack, Respawning, Casting};
        public State CurrentState {get; set;} = State.Idle;
        protected Vector3 _moveTarget;
        public Vector3 CurrentDirection { get; set; }
        protected BaseEntity? _attackTarget = null;


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

        // STATE MACHINE =======================================

        public virtual void Update(float deltaTime)
        {
            if (_currentAttackCooldown > 0)
            {
                _currentAttackCooldown -= deltaTime;
            }
            
            switch (CurrentState)
            {
                
                case State.Idle:

                break;

                case State.Move:

                    Move(deltaTime);

                break;
                
                case State.Chase:

                    if (_attackTarget == null) return;

                    Vector3 TargetPos = _attackTarget.GetWorldPosition();
                    Vector3 MyPosition = GetWorldPosition();

                    if (Vector3.Distance(TargetPos, MyPosition) > _attackRange)
                    {
                        _moveTarget = TargetPos;
                        Move(deltaTime);
                    }
                    else if (Vector3.Distance(TargetPos, MyPosition) <= _attackRange)
                    {
                        
                        CurrentState = State.Attack;

                    }

                break;

                case State.Attack:

                    {
                        
                        if (_attackTarget == null) return;
                        if (_attackTarget != null && _attackTarget.IsDead == true)
                        {
                            _attackTarget = null;
                            CurrentState = State.Idle;
                            return;
                        }
                        Vector3 targetPos = _attackTarget.GetWorldPosition();
                        Vector3 myPos = GetWorldPosition();
                        if (Vector3.Distance(targetPos, myPos) > _attackRange) 
                        {
                            CurrentState = State.Chase;
                            return;
                        }
                        

                        // ATTACK
                        if (_currentAttackCooldown <= 0)
                        {
                            _attackTarget.TakeDamage(_damage, DamageType.Physsical, this); 
                            _currentAttackCooldown = _attackCooldown;
                            _match.BroadcastDamage((int)_damage, _attackTarget.ID);
                        }

                    }

                break;

                
            }

        }

        //=============================== BASE SYSTEMS ============================================

        public virtual void Move(float deltaTime)
        {
            
            Vector3 currentPosition = new Vector3(X / 100f, Y / 100f, Z / 100f);
            float step = _baseSpeed * deltaTime;
            if (Vector3.Distance(_moveTarget, currentPosition) <= step)
            {
                X = (short)(_moveTarget.X * 100);
                Y = (short)(_moveTarget.Y * 100);
                Z = (short)(_moveTarget.Z * 100);

                return;
            }

            var direction = Vector3.Normalize(_moveTarget - currentPosition);
            CurrentDirection = direction;
            var velocity = direction * _baseSpeed * deltaTime;
           
            currentPosition += velocity;

            X = (short)(currentPosition.X * 100);
            Y = (short)(currentPosition.Y * 100);
            Z = (short)(currentPosition.Z * 100);
            

        }

        public virtual void TakeDamage(float dmg, DamageType type, BaseEntity attacker)
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
        public void SetMoveTarget(int posX, int posY, int posZ)
        {
            
             _moveTarget = new Vector3(posX / 100f, posY / 100f, posZ / 100f);
             CurrentState = State.Move;

        }
        public void SetAttackTarget(BaseEntity enemy)
        {
            _attackTarget = enemy;
        }

        



        // SYSTEM FUCNTIONS =====================================================================

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


    }

}