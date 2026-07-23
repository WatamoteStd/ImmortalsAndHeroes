using System;
using System.Collections.Concurrent;
using System.Net;
using UDPServer.Network;

namespace UDPServer.World;

public class WorldHolder
{

    NetworkUdpManager? NetworkManager;

    private float _timer;
    private float _netTimer;

    private Dictionary<long, WorldRegion> _regions = new(); // USER FRIENDLY ARRAY 
    private WorldRegion[] _regionsArray = Array.Empty<WorldRegion>(); // CACHE FRIENDLY :)
    public WorldHolder()
    {
        
        AddRegion(new WorldRegion(0));

    }

    public void Initialize(NetworkUdpManager netManager)
    {
        NetworkManager = netManager;
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
        // GAME WORLD ===========================================================

        for (int i = 0; i < _regionsArray.Length; i++)
        {
            
            _regionsArray[i].Update(deltaTime);

        }

        // NETWORK ==============================================================

        _netTimer += deltaTime;

        if (_netTimer >= 0.05f) // 20 ticks
        {
            
            _netTimer = 0.0f;

            for (int i = 0; i < _regionsArray.Length; i++)
            {
                
                var entities = _regionsArray[i].GetEntityToUpdate();

                if (entities.Length > 0)
                {
                    
                    

                }

            } 

        }
        

        // FOR DEBUG=============================================================
        _timer += deltaTime;
        if (_timer >= 10.0f)
        {
            long bytes = GC.GetTotalMemory(false);
            double mb = bytes / 1024.0 / 1024.0;

            Console.WriteLine($"[10s Check] GC Heap: {mb:F3} MB");

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