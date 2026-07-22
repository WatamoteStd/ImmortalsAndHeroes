using System;
using System.Collections.Concurrent;
using UDPServer.Network.Client;
using UDPServer.World.Entities;

namespace UDPServer.World;

public class WorldRegion
{

    // DATA
    public readonly ConcurrentDictionary<uint, Entity> Entities;
    public readonly ConcurrentDictionary<long, PlayerClient> Players;


    public readonly long RegionId;

    public WorldRegion(long id)
    {
        
        RegionId = id;
        Entities = new ConcurrentDictionary<uint, Entity>();
        Players = new ConcurrentDictionary<long, PlayerClient>();

    }

    public void AddPlayer(PlayerClient player)
    {
        
        Players.TryAdd(player.PlayerId, player);
        if (player.Character != null)
        {
            Entities.TryAdd(player.Character.NetworkId, player.Character);
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
        


    }

}