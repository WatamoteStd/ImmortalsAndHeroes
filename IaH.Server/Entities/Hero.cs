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

        private float _damage;

        float _floatX, _floatY, _floatZ;
        public Vector3 TargetPosition;
        public HeroConfig Config;

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

        }

        public void Update(float deltaTime)
        {
       
            switch (CurrentState)
            {

                case StateMachine.Move:

                    MoveLogic(deltaTime);
                    
                    break;

                case StateMachine.Idle:

                    

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
