using System;
using System.Numerics;

namespace UDPServer.World.Entities;

public class Entity
{

    public enum EntityState { Idle, Move, Chase, Attack, Cast, Respawn };
    public EntityState CurrentState = EntityState.Idle;
    public bool IsDirty {get; set;} = false;
    
    public  uint NetworkId {get; set;}
    public long GlobalId { get; set; }
    public long RegionId {get; set;}

    // GAME INFO & STATS
    public ushort Health {get; set;}
    public float Speed = 1.0f;
    public byte Type {get; set;} = 0;
    public Vector3 Position {get; set;} = new Vector3(0, 0, 0);
    public Vector3 MoveTarget {get; set;} = new();

    public Entity(uint networkId, long regionId, long globalId)
    {
        
        NetworkId = networkId;
        RegionId = regionId;
        GlobalId = globalId;

    }


    public virtual void Update(float deltaTime)
    {
        
        switch (CurrentState)
        {
            
            case EntityState.Idle:
                
            break;

            case EntityState.Move:
                {
                    
                    if (Vector3.DistanceSquared(MoveTarget, Position) < 0.1f)
                    {
                        Position = MoveTarget;
                        CurrentState = EntityState.Idle;
                        Console.WriteLine($"EntityID:{NetworkId} stop move. Now 'IDLE'");
                        IsDirty = true;
                        return;
                    }

                    Vector3 direction = Vector3.Normalize(MoveTarget - Position);
                    Position += direction * (Speed * deltaTime);
                    IsDirty = true;

                }
            break;

        }

    }

    public void SetMoveTarget(float x, float y, float z)
    {
        MoveTarget = new Vector3(x, y, z);
        IsDirty = true;

        if (CurrentState != EntityState.Respawn) CurrentState = EntityState.Move;

    }


}
