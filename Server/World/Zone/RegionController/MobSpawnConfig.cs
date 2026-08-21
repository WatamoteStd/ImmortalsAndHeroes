

using Shared.Characters;

namespace Server.World.Zone.RegionController;

public struct MobSpawnConfig
{
    
    public EntityType Type;
    public int Count;

    public MobSpawnConfig(EntityType type, int count)
    {
        Type = type;
        Count = count;
    }

}