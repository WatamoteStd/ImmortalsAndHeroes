

using Shared.Characters;

namespace Server.World.Zone.RegionController;

public class RegionSpawnBuilder
{
    private  DensityModes _density = DensityModes.Normal;
    private  int _maxCapacity = 0;
    private WorldZone _region;
    private List<MobSpawnConfig> _mobs = new();
    private bool _groupsAllowed = true;
    
    public RegionSpawnBuilder(WorldZone region)
    {
        _region = region;
    }

    public RegionSpawnBuilder SetDensity(DensityModes density)
    {
        _density = density;
        return this;
    }
    public RegionSpawnBuilder SetCapacity(int capacity)
    {
        _maxCapacity = capacity;
        return this;
    }   
    public RegionSpawnBuilder AddMonster(EntityType type, int count)
    {
        _mobs.Add(new MobSpawnConfig(type, count));
        return this;
    }
    public RegionSpawnBuilder GroupsAllowed(bool isAllowed)
    {
        _groupsAllowed = isAllowed;
        return this;
    }

    public RegionSpawner Build()
    {
        
        int totalMobsCount = 0;

        foreach (var m in _mobs)
        {
            totalMobsCount += m.Count;
        }

        if (totalMobsCount != _maxCapacity)
        {
            throw new InvalidOperationException($"[SpawnerBuilder error] At region:{_region.Id} MaxCapacity:{_maxCapacity}, but total mobs count:{totalMobsCount}");
        }

        return new RegionSpawner(_density, _maxCapacity, _region, _mobs);


    }

}