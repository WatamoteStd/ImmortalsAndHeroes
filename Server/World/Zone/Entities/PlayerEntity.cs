
using Server.World.Zone.Entities;

namespace Server.World;

public class PlayerEntity : EntityBase
{
    
    public string Name {get; private set;} = null!;
    public uint PlayerId {get; private set;}

    public PlayerEntity(string name, uint playerId)
    {
        Name = name;
        PlayerId = playerId;

    }

}