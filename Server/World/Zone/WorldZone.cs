using Server.World.Zone.Entities;

namespace Server.World.Zone;

public class WorldZone
{
    public WorldHolder.ZoneType Type {get; private set;}
    public uint Id {get; private set;}
    private readonly WorldHolder _worldHolder;

    private readonly Dictionary<uint, PlayerEntity> _players = new();
    private Dictionary<uint, EntityBase> _entities = new();

    private float latensy;
    private int iterationCount;

    public WorldZone(WorldHolder world, WorldHolder.ZoneType type, uint id)
    {
        
        _worldHolder = world;
        Type = type;
        Id = id;

    }

    public void Update(float deltaTime)
    {
        
        latensy += deltaTime;
        if (latensy >= 10.0f)
        {
            Console.WriteLine($"[10s Debug| N:{iterationCount}] RegionId:{Id}. Players: {_players.Count}");
            latensy = 0f;
            iterationCount++;
        }

    }

    public void AddPlayer(PlayerEntity player)
    {
        
        _players[player.PlayerId] = player;

    }

}