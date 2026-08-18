
using System.Numerics;
using Server.World.Zone.Intefaces;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class LivingEntity : EntityBase, IDamageable
{
    protected Vector3 _moveTarget;
    protected EntityBase _currentEnemy = null!;
    public uint BaseDamage {get; protected set;}
    public float AttackRange {get; protected set;}
    public int AttackSpeed {get; protected set;}
    public int Armor {get; protected set;}
    public int MagicResistance {get; protected set;}
    public const float BASIC_ATTACK_TIME = 1.6f;

    protected float _attackCooldown = 0.0f;
    protected float _currentAttackCooldown = 0.0f;


    public LivingEntity(uint entityId, Vector3 pos, EntityType type, uint regionId) : base(entityId, pos, type, regionId)
    {
        
        BaseDamage = _dllData.BaseDamage;
        AttackRange = _dllData.AttackRange;
        AttackSpeed = _dllData.AttackSpeed;
        Armor = _dllData.Armor;
        MagicResistance = _dllData.MagicResistance;

        // ATTACK COOLDOWN CALCULATE
        _attackCooldown = BASIC_ATTACK_TIME * 100f / AttackSpeed;


    }


    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_currentAttackCooldown > 0.0f)
        {
            _currentAttackCooldown -= deltaTime;
        }

        switch (CurrentState)
        {

            case State.Idle:
                {

                }
            break;
            
            case State.Move:
                {
                    
                    Move(deltaTime);

                }
            break;
            case State.Chase:
                {
                    
                    if (_currentEnemy == null) { CurrentState = State.Idle; return;}

                    if (IsInAttackRadius(_currentEnemy))
                    {
                        
                        _moveTarget = Position;
                        CurrentState = State.Attack;
                        return;
                    }
                    
                    Vector3 direction = _currentEnemy.Position - Position;
                    if (direction.LengthSquared() > 0.0001f)
                    {
                        Vector3 dirNormalized = Vector3.Normalize(direction);
                        Position += dirNormalized * BaseSpeed * deltaTime;
                    }

                }
            break;

            case State.Attack:
                {
                    
                    if (_currentEnemy == null || _currentEnemy.CurrentState == State.Dead) {CurrentState = State.Idle; return;}
                    if (!IsInAttackRadius(_currentEnemy)) { CurrentState = State.Chase; return;}

                    if (_currentAttackCooldown > 0.0f) return;

                    if (_currentEnemy is IDamageable damageable)
                    {
                        damageable.TakeDamage(DamageTypes.Physical, (int)BaseDamage, this);
                        _currentAttackCooldown = _attackCooldown;
                    }
                    else
                    {
                        CurrentState = State.Idle;
                        _currentEnemy = null!;
                        return;
                    }


                }
                break;

        }

    }

    public void MoveToPosition(Vector3 pos)
    {
        
        _moveTarget = pos;
        CurrentState = State.Move;

    }

    protected virtual void Move(float deltaTime)
    {

        float distanceSquared = Vector3.DistanceSquared(_moveTarget, Position);
        
        if (distanceSquared < 0.05f)
        {
            Position = _moveTarget;
            CurrentState = State.Idle;
            return;
        }

        Vector3 direction = (_moveTarget - Position);
        Vector3 direcionNormalized = Vector3.Normalize(direction);

        Vector3 velocity = direcionNormalized * BaseSpeed * deltaTime;

        if (velocity.LengthSquared() > distanceSquared)
        {
            
            Position = _moveTarget;
            CurrentState = State.Idle;
            return;

        }

        Position += velocity;

    }

    public virtual void SetAttackTarget(LivingEntity entity)
    {
    
        _currentEnemy = entity;
        CurrentState = State.Attack;

    }
    public virtual void PerformAttack()
    {
        
        if (_currentEnemy == null) { CurrentState = State.Idle; return; }
        if (!IsInAttackRadius(_currentEnemy)) { CurrentState = State.Chase; return;}

        

    }

    public virtual void TakeDamage(DamageTypes type, int damage, EntityBase attacker)
    {

        switch (type)
        {
            
            case DamageTypes.Physical:
                {
                    if (Armor >= 0)
                    {
                        float realDmg = (float)damage * (100f / (100f + (float)Armor));
                        Health -= (int)MathF.Round(realDmg);
                    }
                    else
                    {
                        float finalDmg = (float)damage * (1.0f + 2.0f * (1.0f - (100f / (100f - (float)Armor))));
                        Health -= (int)MathF.Round(finalDmg);
                    }

                }
            break;

            case DamageTypes.Magic:
                {
                    
                    if (MagicResistance >= 0)
                    {
                        float realDmg = (float)damage * (70f / (70f + (float)MagicResistance));
                        Health -= (int)MathF.Round(realDmg);
                    }
                    else
                    {
                        float finalDmg = (float)damage * (1.0f + 2f * (1.0f - (70f / (70f - (float)MagicResistance))));
                        Health -= (int)MathF.Round(finalDmg);
                    }

                }
            break;

            case DamageTypes.Pure:
                {
                    Health -= damage;
                }
            break;

        }

    }




    public void RecalculateAttackCooldown()
    {
        int safeSpeed = Math.Max(1, AttackSpeed);
        _attackCooldown = (BASIC_ATTACK_TIME * 100f) / safeSpeed;

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


}