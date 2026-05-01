using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking.Events;
using IaH.Server.Core;

namespace IaH.Server.Entities
{
    public class BaseEntity : IDamable
    {

        

        public ushort Id;
        public short X, Y, Z;
        protected float _health;
        protected float _maxHealth;
        protected float _healthRegen;

        protected float _speed;
        protected float _mana;
        protected float _maxMana;
        protected float _manaRegen;
        protected float _armor;
        protected float _magicResist;

        protected float _damage;
        protected float _reloadTime = 2.0f;
        protected float _preAttack = 0.2f;

        public int AttackRange;
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
        public float Mana;
        public CharacterType SelectedHero;

        public BaseEntity(ushort id,  short x, short y, short z, CharacterType hero)
        {

            Id = id;
            X = x;
            Y = y;
            Z = z;
            _maxHealth = 100;
            _health = 100;
            Mana = 50;
            SelectedHero = hero;

        }

        public virtual void TakeDamage(DamageType type, float damage)
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
            var message = new EntityHpChangedEvent(Id, Health, Mana);
            Console.WriteLine($"BaseEntity: TakeDamage! {damage}");
            EventBus.PublishHp(message);

        }

    }
}
