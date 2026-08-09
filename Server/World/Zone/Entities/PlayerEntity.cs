
using System.Numerics;
using Server.World.Zone.Entities;

namespace Server.World;

public class PlayerEntity : LivingEntity
{
    
    public string Name {get; private set;} = null!;
    public uint PlayerId {get; private set;}

    public PlayerEntity(uint playerId, uint entityId, Vector3 pos, string name) : base(entityId, pos)
    {
        Name = name;
        PlayerId = playerId;

    }

}