using System;
using System.Collections.Concurrent;
using System.Net;

namespace UDPServer.World;

public class WorldHolder
{
    private float _timer;
    private int _frameCount;
    private Dictionary<long, WorldRegion> _regions = new(); // USER FRIENDLY ARRAY 
    private WorldRegion[] _regionsArray = Array.Empty<WorldRegion>(); // CACHE FRIENDLY :)
    public WorldHolder()
    {
        
        AddRegion(new WorldRegion(0));

    }

    public void AddRegion(WorldRegion region)
    {
        
        if (_regions.TryAdd(region.RegionId, region))
        {
            
            _regionsArray = _regions.Values.ToArray();

        }

    }

    public void Update(float deltaTime)
    {

        for (int i = 0; i < _regionsArray.Length; i++)
        {
            
            _regionsArray[i].Update(deltaTime);

        }


        _timer += deltaTime;
        _frameCount++;
        if (_timer >= 10.0f)
        {
            long bytes = GC.GetTotalMemory(false);
            double mb = bytes / 1024.0 / 1024.0;

            Console.WriteLine($"[10s Check] GC Heap: {mb:F3} MB");

            _frameCount = 0;
            _timer = 0;
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