
using System.Numerics;
using Server.World.Effects;
using Server.World.Zone.Intefaces;
using Shared.Characters;
using Shared.MasteryTree.Rewards;
using Shared.Udp.Packets.Category.Game;

namespace Server.World.Zone.Entities;

public class LivingEntity : EntityBase, IDamageable
{

    public event Action<LivingEntity, Vector3>? OnMoved;
    public event Action<LivingEntity, int, LivingEntity>? OnDamageTaked; // entity(this) | damage | attacker
    public event Action<LivingEntity, LivingEntity>? OnDead; // this, attacker
    public event Action<S2C_StatsSyncPacket>? OnStatsUpdated; 
    protected Vector3 _lastSnapshotCords;
    protected float _timeFromLastPacket = 0.0f;


    protected Vector3 _moveTarget;
    public float BaseDamage {get; protected set;}
    public float AttackRange {get; protected set;}
    protected float _attackSpeed;
    public float AttackSpeed {
        
        get => _attackSpeed;
        protected set
        {
            _attackSpeed = value;
            RecalculateAttackCooldown();
        }
        
    }
    public float Armor {get; protected set;}
    public float MagicResistance {get; protected set;}
    public float BasicAttackTime {get; protected set;}

    protected float _attackCooldown = 0.0f;
    protected float _currentAttackCooldown = 0.0f;

    protected float _healthRegenBuffer = 0f;
    protected float _manaRegenBuffer = 0f;

    protected List<StatusEffectBase> _statusEffects = new ();


    public LivingEntity(uint entityId, Vector3 pos, EntityType type, uint regionId) : base(entityId, pos, type, regionId)
    {
        
        BaseDamage = _dllData.BaseDamage;
        AttackRange = _dllData.AttackRange;
        Armor = _dllData.Armor;
        MagicResistance = _dllData.MagicResistance;
        BasicAttackTime = _dllData.BasicAttackTime;

        AttackSpeed = _dllData.AttackSpeed;


    }


    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_currentAttackCooldown > 0.0f)
        {
            _currentAttackCooldown -= deltaTime;
        }

        Regenerate(deltaTime);

        if (_statusEffects.Count > 0)
        {
            
            for (int i = _statusEffects.Count -1; i >= 0; i--)
            {
            
                var effect = _statusEffects[i];

                effect.OnUpdate(deltaTime);

                if (effect.Duration <= 0)
                {
                
                    effect.OnLeave();
                    _statusEffects.RemoveAt(i);

                }

            }

        }


    }

    protected virtual void Move(float deltaTime)
    {

        float distanceSquared = Vector3.DistanceSquared(_moveTarget, Position);
        
        if (distanceSquared < 0.05f)
        {

            Position = _moveTarget;

            OnMoved?.Invoke(this, Position);
            _timeFromLastPacket = 0.0f;
            _lastSnapshotCords = Position;
            return;
        }

        Vector3 direction = (_moveTarget - Position);
        Vector3 direcionNormalized = Vector3.Normalize(direction);

        Vector3 velocity = direcionNormalized * BaseSpeed * deltaTime;

        if (velocity.LengthSquared() > distanceSquared)
        {
            
            Position = _moveTarget;

            OnMoved?.Invoke(this, Position);
            _timeFromLastPacket = 0.0f;
            _lastSnapshotCords = Position;
            return;

        }

        Position += velocity;
        CheckMoveSynchronization(deltaTime);

    }

    public virtual void TakeDamage(DamageTypes type, int damage, LivingEntity attacker)
    {

        float finalDamage = 0.0f;

        switch (type)
        {
            
            case DamageTypes.Physical:
                {
                    if (Armor >= 0)
                    {
                        finalDamage = (float)damage * (100f / (100f + (float)Armor));
                    }
                    else
                    {
                        finalDamage = (float)damage * (1.0f + 2.0f * (1.0f - (100f / (100f - (float)Armor))));
                    }

                }
            break;

            case DamageTypes.Magic:
                {
                    
                    if (MagicResistance >= 0)
                    {
                        finalDamage = (float)damage * (70f / (70f + (float)MagicResistance));
                    }
                    else
                    {
                        finalDamage = (float)damage * (1.0f + 2f * (1.0f - (70f / (70f - (float)MagicResistance))));
                    }

                }
            break;

            case DamageTypes.Pure:
                {
                    finalDamage = damage;
                }
            break;

        }

        int actualDamage = (int)MathF.Round(finalDamage);
        Health -= actualDamage;

        Console.WriteLine($"[Entity:{EntityId}] Take {finalDamage} damage! CurrentHealth:{Health}. Attacker:{attacker.Name}");
        OnDamageTaked?.Invoke(this, actualDamage, attacker);
        
        if (Health == 0) 
        {
            IsAlive = false;
            OnDead?.Invoke(this, attacker);
        }

    }

    public void UpdateStat(StatType stat, float value)
    {
        
        switch (stat)
        {
            
            case StatType.Armor:
                {
                    Armor += value;
                }
            break;
            case StatType.Health:
                {
                    MaxHealth += value;
                }
            break;
            case StatType.Mana:
                {
                    MaxMana += value;
                }
            break;
            case StatType.PhysicalDamage:
                {
                    BaseDamage += value;
                }
            break;
            case StatType.AttackSpeed:
                {
                    AttackSpeed += value;
                }
            break;
            case StatType.HealthRegen:
                {
                    HealthRegeneration += value;
                }
            break;
            case StatType.ManaRegen:
                {
                    ManaRegeneration += value;
                }
            break;
            case StatType.MagicResistance:
                {
                    MagicResistance += value;
                }
            break;
            case StatType.MoveSpeed:
                {
                    BaseSpeed += value;
                }
            break;

            default:

            break;

        }

        var packet = new S2C_StatsSyncPacket
        {
            Health = Health,
            Mana = Mana,
            HealthRegen = HealthRegeneration,
            ManaRegen = ManaRegeneration,
            Damage = BaseDamage,
            Armor = Armor,
            MagicResistance = MagicResistance,
            AttackSpeed = AttackSpeed,
            MaxHealth = MaxHealth,
            MaxMana = MaxMana,
            Speed = BaseSpeed
        };
        OnStatsUpdated?.Invoke(packet);
        Console.WriteLine($"[Entity:{Name}] Stat update trigerred! CurrentHealth:{Health}");

    }


    protected void CheckMoveSynchronization(float deltaTime)
    {
        _timeFromLastPacket += deltaTime;

        if (_timeFromLastPacket > 0.1f || Vector3.DistanceSquared(Position, _lastSnapshotCords) >= 0.5f)
        {
            
            OnMoved?.Invoke(this, Position);
            _timeFromLastPacket = 0.0f;
            _lastSnapshotCords = Position;

        }

    }

    public virtual void Regenerate(float deltaTime)
    {
        
        if (_health < MaxHealth)
        {
            
            _healthRegenBuffer += HealthRegeneration * deltaTime;

            if (_healthRegenBuffer >= 1.0f)
            {
                int amount = (int)_healthRegenBuffer;
                Health += amount;
                _healthRegenBuffer -= (float)amount;

            }

        }

    }

    public void ApplyStatusEffect(StatusEffectBase effect, LivingEntity caster)
    {
        Console.WriteLine($"[Entity:{Name}] Status effect apply! effect:{effect.Name}");
        effect.OnApply(caster, this, null);
        _statusEffects.Add(effect);

    }



    #region  INTERNAL


    public void RecalculateAttackCooldown()
    {
        float safeSpeed = Math.Max(1, AttackSpeed);
        _attackCooldown = (BasicAttackTime * 100f) / safeSpeed;

    }
    public bool IsInAttackRadius(EntityBase entity)
    {
        if (entity == null) return false;
        return IsInRadius(Position.X, Position.Z, Radius, entity.Position.X, entity.Position.Z, entity.Radius, AttackRange);
    }

    public bool IsInRadius(float x, float z, float radius, float enemyX, float enemyZ, float enemyRadius, float attackRange)
    {
        
        float dX = enemyX - x;
        float dZ = enemyZ - z;

        float distanceSquared = (dX * dX) + (dZ * dZ);

        float maxDistance = attackRange + radius + enemyRadius;

        return distanceSquared <= (maxDistance * maxDistance);

    }


    public virtual void ClearAllSubscriptions()
    {
        OnMoved = null!;
        OnDamageTaked = null!;
        OnDead = null!;
        OnStatsUpdated = null;
    }  

    #endregion


}
    public static class EntityExtensions
    {
        
        public static bool IsValidEntity(this EntityBase? entity)
    {
        
        if (entity == null) return false;
        if (!entity.IsAlive) return false;

        return true;

    }

    }
    