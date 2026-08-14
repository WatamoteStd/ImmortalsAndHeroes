
using System.Numerics;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class LivingEntity : EntityBase
{
    protected Vector3 _moveTarget;

    public LivingEntity(uint entityId, Vector3 pos, EntityType type, uint regionId) : base(entityId, pos, type, regionId)
    {
        

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
        
        if (distanceSquared < 0.05)
        {
            Position = _moveTarget;
            CurrentState = State.Idle;
            return;
        }

        Vector3 direction = (_moveTarget - Position);
        Vector3 direcionNormalized = Vector3.Normalize(direction);

        Vector3 velocity = direcionNormalized * Speed * deltaTime;

        if (velocity.LengthSquared() > distanceSquared)
        {
            
            Position = _moveTarget;
            CurrentState = State.Idle;
            return;

        }

        Position += velocity;

    }


}