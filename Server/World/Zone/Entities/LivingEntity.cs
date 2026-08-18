
using System.Numerics;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class LivingEntity : EntityBase
{
    protected Vector3 _moveTarget;
    public uint BaseDamage {get; protected set;}
    public float AttackRange {get; protected set;}
    public int AttackSpeed {get; protected set;}
    public int Armor {get; protected set;}
    public int MagicResistance {get; protected set;}


    public LivingEntity(uint entityId, Vector3 pos, EntityType type, uint regionId) : base(entityId, pos, type, regionId)
    {
        
        BaseDamage = _dllData.BaseDamage;
        AttackRange = _dllData.AttackRange;
        AttackSpeed = _dllData.AttackSpeed;
        Armor = _dllData.Armor;
        MagicResistance = _dllData.MagicResistance;

    }


    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

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

        }

    }

    public void MoveToPosition(Vector3 pos)
    {
        
        _moveTarget = pos;
        CurrentState = State.Move;

    }

    private void Move(float deltaTime)
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


}