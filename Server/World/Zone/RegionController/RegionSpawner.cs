

namespace Server.World.Zone.RegionController;

public class RegionSpawner
{
    
    public DensityModes Density {get;}
    public  int MaxCapacity {get;}

    public WorldZone Region {get;}
    public IReadOnlyList<MobSpawnConfig> Mobs {get;}

    internal RegionSpawner(DensityModes densityMode, int capacity, WorldZone region, IReadOnlyList<MobSpawnConfig> mobsConfigs)
    {
        
        Density = densityMode;
        MaxCapacity = capacity;
        Region = region;
        Mobs = mobsConfigs;

    }

    public void Update(float deltaTime)
    {
        


    }

}