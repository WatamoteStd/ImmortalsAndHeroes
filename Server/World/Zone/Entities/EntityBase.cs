
using System.Numerics;
using Shared.Characters;

namespace Server.World.Zone.Entities;

public class EntityBase
{
    
    public uint EntityId {get; protected set;}
    public Vector3 Position {get; protected set;}

    public EntityType ModelType {get; protected set;}

    public float Radius {get; protected set;} = 0.5f;
    public float Height {get; protected set;} = 1.8f;

    public EntityBase(uint entityId, Vector3 pos, EntityType type, float radius = 0.5f, float height = 1.8f)
    {
        
        EntityId = entityId;
        Position = pos;

        Radius = radius;
        Height = height;
        ModelType = type;

    }

}