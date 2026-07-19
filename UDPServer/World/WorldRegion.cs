using System;
using System.Collections.Concurrent;
using UDPServer.Network.Client;
using UDPServer.World.Entities;

namespace UDPServer.World;

public class WorldRegion
{

    // DATA
    ConcurrentDictionary<long, Entity> _entities;
    ConcurrentDictionary<long, PlayerClient> _players;


    public readonly long RegionId;

    public WorldRegion(long id)
    {
        
        RegionId = id;
        _entities = new ConcurrentDictionary<long, Entity>();
        _players = new ConcurrentDictionary<long, PlayerClient>();

    }

    public void AddPlayer(PlayerClient player)
    {
        
        _players.TryAdd(player.PlayerId, player);
        _entities.TryAdd(player.Character.Id, player.Character);

    }

    public void Update(float deltaTime)
    {
        


    }

}