using System.Numerics;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class EntityBase
{

    public enum State { Idle, Move, Chase, Attack, Controlled, Cast, ProtectedCast, Dead}
    public State CurrentState = State.Idle;
    
    public uint EntityId {get; protected set;}
    public uint RegionId {get; set;}
    public Vector3 Position {get; protected set;}
    protected readonly EntityData _data;

    public EntityType ModelType {get; protected set;}

    public float Radius => _data.Radius;
    public float Height => _data.Height;



    public EntityBase(uint entityId, Vector3 pos, EntityType type, uint regionId)
    {
        
        EntityId = entityId;
        Position = pos;
        ModelType = type;
        RegionId = regionId;
        _data = EntityRegistry.GetEntityData(type);

    }

    public virtual void Update(float deltaTime)
    {
        


    }

    public virtual void SetPosition(float x, float y, float z)
    {
        Position = new Vector3(x,y,z);
    }

}