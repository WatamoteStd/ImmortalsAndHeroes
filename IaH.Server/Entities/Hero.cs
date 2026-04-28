using IaH.Shared.Networking;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Data;

namespace IaH.Server.Entities
{
    public class Hero : BaseEntity
    {

        public BaseEntity _currentTarget;

        private ushort _lvl = 1;
        private float _expToLvl = 100.0f;
        private float _currentExp = 0.0f;

        private float _speed;
        private float _health;
        private float _maxHealth;
        private float _healthRegen;
        public float Health
        {
            get { return _health; }
            set
            {
                if (value > _maxHealth) _health = _maxHealth;
                else if (value <= 0) _health = MathF.Max(0, value);
                else _health = value;
            }
        }
        private float _mana;
        private float _maxMana;
        private float _manaRegen;
        private float _armor;
        private float _magicResist;

        private float _damage;

        public int AttackRange;

        private short _gold = 1;

        public float _floatX, _floatY, _floatZ;
        public Vector3 TargetPosition;
        public HeroConfig Config;

        public Vector3 GlobalPos => new Vector3(_floatX, _floatY, _floatZ);

        public enum StateMachine
        {
            Idle,
            Chase,
            Move,
            Attack,
            Cast,
            Respawning
        }
        public StateMachine CurrentState = StateMachine.Idle;

        public Hero(ushort id, short x, short y, short z, CharacterType hero, HeroConfig config) : base(id, x, y, z, hero)
        {
            Config = config;

            _floatX = x / 100.0f;
            _floatY = y / 100.0f;
            _floatZ = z / 100.0f;

            _speed = config.Stats.MoveSpeed;
            _maxHealth = config.Stats.BaseHealth;
            _health = config.Stats.BaseHealth;
            _healthRegen = config.Stats.HealthRegen;
            _mana = config.Stats.BaseMana;
            _maxMana = config.Stats.BaseMana;
            _manaRegen = config.Stats.ManaRegen;
            _damage = config.Combat.AttackDamage;
            _magicResist = config.Stats.MagicResist;
            AttackRange = config.Combat.AttackRange;

        }

        public override void TakeDamage(DamageType type, float damage)
        {
            
            switch (type)
            {

                case DamageType.Physical:

                    var damageBlock = _armor * 2;
                    Health -= MathF.Max(0, damage - damageBlock);

                    break;

                case DamageType.Magical:

                    if (_magicResist <= 0)
                    {
                        Health -= damage;
                        return;
                    }

                    Health -= damage * (1 - _magicResist / 100);

                    break;

                case DamageType.Pure:

                    Health -= damage;

                    break;
            }

        }

        public EntityStatsPacket GetStatsPacket(byte mask)
        {

            EntityStatsPacket packet = new EntityStatsPacket();
            packet.EntityId = Id;
            packet.UpdateMask = mask;

            if ((mask & 1) != 0)
            {
                packet.Vitals = new EntityVitalsStats
                {
                    CurrentHp = (short)(_health * 10),
                    CurrentMana = (short)(_mana * 10)
                };

            }
            if ((mask & 2) != 0)
            {
                packet.Attributes = new EntityAttributesStats
                {
                    MaxHealth = (short)(_maxHealth * 10),
                    MaxMana = (short)(_maxMana * 10),
                    Armor = (short)(_armor * 10),
                    MagicResist = (short)(_magicResist * 10),
                    Speed = (short)(_speed * 10),
                    Gold = _gold
                };
            }
            if ((mask & 4) != 0)
            {
                packet.Progress = new EntityProgressionStats
                {
                    Lvl = (short)_lvl,
                    ExpToLvl = (short)(_expToLvl * 10),
                    CurrentExp = (short)(_currentExp * 10),

                };
            }
            return packet;

        }

        public void Update(float deltaTime) // stateMachine
        {
       
            switch (CurrentState)
            {

                case StateMachine.Move:

                    MoveLogic(deltaTime);
                    
                    break;

                case StateMachine.Idle:

                    

                    break;

                case StateMachine.Attack:
                    {
                        float windUpTimeStatic = 0.2f;
                        float curWindUpTime = windUpTimeStatic - deltaTime;
                        Vector3 TargetPos = new Vector3(_currentTarget.X, _currentTarget.Y, _currentTarget.Z);
                        var DistanceToTarget = Vector3.Distance(GlobalPos, TargetPos);
                        if (curWindUpTime <= 0 &&DistanceToTarget < AttackRange)
                        {
                            _currentTarget.TakeDamage(DamageType.Physical, _damage);
                        }
                        else CurrentState = StateMachine.Chase;
                    }
                break;

                case StateMachine.Chase:
                    {

                        CurrentState = StateMachine.Idle; // TEMP

                    }
                    break;

            }



            X = (short)(_floatX * 100);
            Y = (short)(_floatY * 100);
            Z = (short)(_floatZ * 100);



        }

        public void MoveLogic(float deltaTime)
        {
            Vector3 MyPosition = new Vector3(_floatX, _floatY, _floatZ);

            Vector3 rawDirection = (TargetPosition - MyPosition);
            rawDirection.Y = 0;

            float Distance = rawDirection.Length();
            Console.WriteLine($"[MOVE] ID: {Id}, Dist: {Distance:F2}, Pos: {MyPosition}, Target: {TargetPosition}");

            if (Distance > 0.1f)
            {

                Vector3 direction = Vector3.Normalize(rawDirection);

                Vector3 velocity = direction * _speed * deltaTime;

                _floatX += velocity.X;
                _floatY += velocity.Y;
                _floatZ += velocity.Z;

                X = (short)(_floatX * 100);
                Y = (short)(_floatY * 100);
                Z = (short)(_floatZ * 100);

            }
            else
            {
                Console.WriteLine($"[MOVE] Прибыли в точку или слишком близко. Смена на Idle.");
                CurrentState = StateMachine.Idle;
            }
        }


    }
}
