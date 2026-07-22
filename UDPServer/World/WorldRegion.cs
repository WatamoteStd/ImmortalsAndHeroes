using System;
using System.Collections.Concurrent;
using UDPServer.Network.Client;
using UDPServer.World.Entities;

namespace UDPServer.World;

public class WorldRegion
{

    // DATA
    ConcurrentDictionary<uint, Entity> _entities;
    ConcurrentDictionary<long, PlayerClient> _players;


    public readonly long RegionId;

    public WorldRegion(long id)
    {
        
        RegionId = id;
        _entities = new ConcurrentDictionary<uint, Entity>();
        _players = new ConcurrentDictionary<long, PlayerClient>();

    }

    public void AddPlayer(PlayerClient player)
    {
        
        _players.TryAdd(player.PlayerId, player);
        if (player.Character != null)
        {
            _entities.TryAdd(player.Character.NetworkId, player.Character);
        }
        else
        {
            Console.WriteLine($"[WorldHolder: ERROR] Player:{player.PlayerId} character is null.");
        }

    }

    public void Update(float deltaTime)
    {
        


    }

}