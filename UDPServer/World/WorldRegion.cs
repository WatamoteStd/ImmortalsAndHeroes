using System;
using System.Collections.Concurrent;
using UDPServer.Network.Client;
using UDPServer.World.Entities;

namespace UDPServer.World;

public class WorldRegion
{

    // DATA
    public readonly Dictionary<uint, Entity> Entities = new();
    public readonly Dictionary<long, PlayerClient> Players = new();

    private Entity[] _entitiesArray = Array.Empty<Entity>();


    public readonly long RegionId;

    public WorldRegion(long id)
    {
        
        RegionId = id;

    }

    public void AddPlayer(PlayerClient player)
    {
        
        Players.TryAdd(player.PlayerId, player);
        if (player.Character != null)
        {
            if (Entities.TryAdd(player.Character.NetworkId, player.Character))
            {
                _entitiesArray = Entities.Values.ToArray();
            }
        }
        else
        {
            Console.WriteLine($"[WorldHolder: ERROR] Player:{player.PlayerId} character is null.");
        }

    }

    public Entity? GetEntity(uint id)
    {
        
        if (Entities.TryGetValue(id, out Entity? entity) && entity != null)
        {
            
            return entity;

        }
        else return null;

    }

    public void Update(float deltaTime)
    {
        
        for (int i = 0; i < _entitiesArray.Length; i++)
        {
            
            _entitiesArray[i].Update(deltaTime);

        }

    }

}