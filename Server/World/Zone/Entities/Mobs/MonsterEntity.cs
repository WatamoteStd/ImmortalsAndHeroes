using System.Numerics;
using Server.World.Zone.Intefaces;
using Shared.Characters;

namespace Server.World.Zone.Entities.Mobs;

public class MonsterEntity : LivingEntity
{


    public enum State{ Idle, Patrol, Chase, Attack, ReturnToSpawn }
    public State CurrentMobState = State.Idle;

    public Vector3 _spawnPosition {get; protected set;}
    public float _aggroRadius {get; protected set;} = 10.0f;
    public float _leashRadius {get; protected set;} = 18.0f;
    private float _leashRadiusSq;
    public float _patrolRadius {get; protected set;} = 5.0f;
    public float _aiCheckTimer {get; protected set;} = 0.0f;
    protected float _aiCheckCooldown = 1.5f;

    public float RespawnTimer;


    protected EntityBase _currentEnemy = null!;


    protected WorldZone _region;
    protected byte _searchCyclesCount;
    
    public MonsterEntity(uint entityId, Vector3 pos, EntityType type, uint regionId, WorldZone zone) : base(entityId, pos, type, regionId)
    {
        _spawnPosition = pos;
        _region = zone;

        _leashRadiusSq = _leashRadius * _leashRadius;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);


        if (!IsAlive) return;

        if (_currentEnemy == null)
        {
            _aiCheckTimer += deltaTime;
            if (_aiCheckTimer > _aiCheckCooldown)
            {
                CheckAggroRadius();
                if (_currentEnemy == null)
                {
                    _aiCheckTimer = 0.0f;
                }
            }
        }


        switch (CurrentMobState)
        {
            
            case State.Idle:
                {
                    
                    

                }
            break;

            case State.Patrol:
                {
                    
                    if (Vector3.DistanceSquared(_moveTarget, Position) < 0.25f)
                    {
                        CurrentMobState = State.Idle;
                        _aiCheckCooldown = 1.0f;
                        return;
                    }

                    Move(deltaTime);

                }
            break;

            case State.Chase:
                {
                    
                    if (!_currentEnemy.IsValidEntity())
                    {
                        _currentEnemy = null!;
                        CurrentMobState = State.Idle;
                        return;
                    }

                    if (Vector3.DistanceSquared(_spawnPosition, Position) >= _leashRadiusSq)
                    {
                        
                        CurrentMobState = State.ReturnToSpawn;
                        return;

                    }

                    if (!IsInAttackRadius(_currentEnemy!))
                    {
                        
                        _moveTarget = _currentEnemy!.Position;
                        Move(deltaTime);
                        return;

                    }
                    
                    _moveTarget = Position;
                    CurrentMobState = State.Attack;

                }
            break;

            case State.Attack:
                {
                    
                    if (!_currentEnemy.IsValidEntity()) 
                    {
                        CheckAggroRadius();
                        if (_currentEnemy == null)
                        {
                            CurrentMobState = State.ReturnToSpawn;
                            return;
                        }
                    }

                    if (!IsInAttackRadius(_currentEnemy!))
                    {
                        CurrentMobState = State.Chase;
                        return;
                    }

                    if (_currentAttackCooldown > 0) return;

                    if (_currentEnemy is IDamageable damageable)
                    {
                        damageable.TakeDamage(DamageTypes.Physical, (int)BaseDamage, this);
                        _currentAttackCooldown = _attackCooldown;
                    }

                }
            break;

            case State.ReturnToSpawn:
                {
                    
                    _moveTarget = _spawnPosition;
                    Move(deltaTime);

                    if (Vector3.DistanceSquared(_spawnPosition, Position) < 0.35f)
                    {
                        Position = _spawnPosition;
                        _currentEnemy = null!;
                        CurrentMobState = State.Idle;
                    }

                }
            break;

        }

    }

    private void CheckAggroRadius()
    {
        
        PlayerEntity? player = _region.TryFindNearestPlayer(Position,_aggroRadius);

        if (player != null)
        {
            _searchCyclesCount = 0;
            
            _currentEnemy = player;
            if (IsInAttackRadius(player))
            {
                CurrentMobState = State.Attack;
                return;
            }
            else
            {
                CurrentMobState = State.Chase;
                return;
            }

        }
        
        if (_searchCyclesCount > 10)
        {
            
            Patrol();
            _searchCyclesCount = 0;
            return;

        }

        _searchCyclesCount++;

    }

    private void Patrol()
    {

        float randX = (Random.Shared.NextSingle() * 2f - 1f) * _patrolRadius;
        float randZ = (Random.Shared.NextSingle() * 2f - 1f) * _patrolRadius;

        Vector3 patroolPos = _spawnPosition + new Vector3(randX, 0, randZ);

        _moveTarget = patroolPos;
        _aiCheckCooldown = 1.0f;
        CurrentMobState = State.Patrol;

    }

    public override void TakeDamage(DamageTypes type, float damage, LivingEntity attacker)
    {
        base.TakeDamage(type, damage, attacker);

        if (_currentEnemy == null)
        {
            CheckAggroRadius();
            if (_currentEnemy == null)
            {
                _currentEnemy = attacker;
                CurrentMobState = State.Chase;
            }
        }
    }

    public void Respawn(Vector3 pos)
    {
        _health = (int)_dllData.BaseHealth;
        Position = pos;
        _spawnPosition = pos;
        IsAlive = true;

    }

    

}