using System;
using System.Collections.Concurrent;
using System.Net;

namespace UDPServer.World;

public class WorldHolder
{
    
    private ConcurrentDictionary<long, WorldRegion> _regions;

    public WorldHolder()
    {
        
        _regions = new ConcurrentDictionary<long, WorldRegion>();
        _regions.TryAdd(0, new WorldRegion(0));

    }

    public void Update(float deltaTime)
    {
        
        foreach (var region in _regions.Values)
        {
            region.Update(deltaTime);
        }

    }

    public WorldRegion? GetRegion(long id)
    {
        
        if (_regions.TryGetValue(id, out WorldRegion? region))
        {
            return region;
        }
        else return null;

    }


}