using Shared.Characters;
using System.Numerics;
using Server.World.Zone.Entities;

namespace Server.World;

public class PlayerEntity : LivingEntity
{
    
    public string Name {get; private set;} = null!;
    public uint PlayerId {get; private set;}
    public uint Silver {get; private set;}
    public uint Lvl {get; private set;}

    public PlayerEntity(uint entityId, Vector3 pos, EntityType type, string name, uint playerId, uint regionId, uint silver, uint lvl) : base(entityId, pos, type, regionId)
    {
        Name = name;
        PlayerId = playerId;
        Silver = silver;
        Lvl = lvl;

    }

}