
using System.Numerics;
using Server.World.Zone.Intefaces;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class LivingEntity : EntityBase, IDamageable
{

    public event Action<LivingEntity, Vector3>? OnMoved;
    public event Action<LivingEntity, int, LivingEntity>? OnDamageTaked; // entity(this) | damage | attacker
    public event Action<LivingEntity>? OnDead;
    protected Vector3 _lastSnapshotCords;
    protected float _timeFromLastPacket = 0.0f;


    protected Vector3 _moveTarget;
    public uint BaseDamage {get; protected set;}
    public float AttackRange {get; protected set;}
    public int AttackSpeed {get; protected set;}
    public int Armor {get; protected set;}
    public int MagicResistance {get; protected set;}
    public float BasicAttackTime {get; protected set;}

    protected float _attackCooldown = 0.0f;
    protected float _currentAttackCooldown = 0.0f;


    public LivingEntity(uint entityId, Vector3 pos, EntityType type, uint regionId) : base(entityId, pos, type, regionId)
    {
        
        BaseDamage = _dllData.BaseDamage;
        AttackRange = _dllData.AttackRange;
        AttackSpeed = _dllData.AttackSpeed;
        Armor = _dllData.Armor;
        MagicResistance = _dllData.MagicResistance;
        BasicAttackTime = _dllData.BasicAttackTime;

        // ATTACK COOLDOWN CALCULATE
        _attackCooldown = BasicAttackTime * 100f / AttackSpeed;


    }


    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_currentAttackCooldown > 0.0f)
        {
            _currentAttackCooldown -= deltaTime;
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
            OnDead?.Invoke(this);
        }

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


    public void RecalculateAttackCooldown()
    {
        int safeSpeed = Math.Max(1, AttackSpeed);
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
    }  


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