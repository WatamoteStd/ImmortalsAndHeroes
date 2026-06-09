using System;
using System.Numerics;
using Iah.Shared.Packets;
using IaH.Server.Ability;
using IaH.Server.Entities;
using IaH.Server.PlayerClasses;
using IaH.Server.World;

namespace IaH.Server.Entities
{
    
    public class HeroEntity : BaseEntity
    {
        public Player MainPlayer;
        public enum State {Idle, Move, Chase, Attack, Respawning, Casting};
        public State CurrentState {get; set;} = State.Idle;
        public float CurrentSpeed;

        // MATCH
        private WorldMatch _match;
        // MOVE 

        private Vector3 _moveTarget;
        public Vector3 CurrentDirection { get; private set; }
        private BaseEntity? _attackTarget = null;

        // ABILITY
        private List<AbilityBase>? _abilityList;
        private AbilityBase? _activeAbility;
        
        public HeroEntity(ushort id, UnitList unitType, Player player, WorldMatch m, Team team) : base (id, unitType, team)
        {
            _abilityList = new();
            _match = m;
            MainPlayer = player;

            if (Unit != UnitList.None)
            {
                
                switch (Unit)
                {
                    
                    case UnitList.VoidlessStar:
                        {
                            
                            _abilityList.Add(new ReleaseOfCosmicEnergy(this, _match));

                        }
                    break;

                }

            }

        }

        public override void Update(float deltaTime)
        {
            if (_attackCooldown > 0)
            {
                _attackCooldown -= deltaTime;
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
                        if (_attackCooldown <= 0)
                        {
                            _attackTarget.TakeDamage(20, DamageType.Physsical, this); // hardcode
                            _attackCooldown = 1.7f;
                            _match.BroadcastDamage(20, _attackTarget.ID);
                        }

                    }

                break;

                case State.Casting:
                    {
                        if (_activeAbility == null)
                        {
                            CurrentState = State.Idle;
                            return;
                        }

                       _activeAbility.OnUpdate(deltaTime);
                        CurrentSpeed = 0;

                    }
                break;

            }

        }


        // MATCH -> HERO COMMUNICATION
        public void ExecuteSkill(byte slotIndex, BaseEntity? target, EntityManager em, short x, short y, short z)
        {
            if (slotIndex >= _abilityList.Count) 
            {
                Console.WriteLine($"[Hero] Ошибка: Попытка каста из недоступного слота {slotIndex}");
                return;
            }
            
            _abilityList[slotIndex].Execute(target, em, x, y, z);
            _activeAbility = _abilityList[slotIndex];
            _match.BroadcastSkillCast(ID, slotIndex);


        }


        private void Move(float deltaTime)
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


        public void SetMoveTarget(int posX, int posY, int posZ)
        {
            
             _moveTarget = new Vector3(posX / 100f, posY / 100f, posZ / 100f);
             CurrentState = State.Move;

        }
        public void SetAttackTarget(BaseEntity enemy)
        {
            _attackTarget = enemy;
        }

        public override void TakeDamage(float dmg, DamageType type, HeroEntity attacker)
        {
            base.TakeDamage(dmg, type,attacker);
            Console.WriteLine($"[{Unit}] Take {dmg} damage. Health: {Health}/{GetMaxHealth()}");


    }

}}