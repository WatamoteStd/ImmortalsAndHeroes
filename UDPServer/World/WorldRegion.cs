using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using UDPServer.Network.Client;
using UDPServer.World.Entities;

namespace UDPServer.World;

public class WorldRegion
{

    // DATA
    public readonly Dictionary<uint, Entity> Entities = new();
    public readonly Dictionary<long, PlayerClient> Players = new();

    private Entity[] _entitiesArray = Array.Empty<Entity>();

    private List<Entity> _changedEntities = new();


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
    public PlayerClient[] GetPlayers()
    {
        
        return Players.Values.ToArray();

    }

    public void Update(float deltaTime)
    {
        
        for (int i = 0; i < _entitiesArray.Length; i++)
        {
            

            var entity = _entitiesArray[i];
            entity.Update(deltaTime);

            if (entity.IsDirty)
            {
                if (!_changedEntities.Contains(entity))
                {
                    _changedEntities.Add(entity);
                }
                entity.IsDirty = false;
            }

        }

    }

    // [[======================== WORLD HOLDER FUNC ==================================]]


    public ReadOnlySpan<Entity> GetEntityToUpdate()
        => CollectionsMarshal.AsSpan(_changedEntities);

    public void ClearChangedEntities()
    {
        _changedEntities.Clear();
    }


    // ======================== NETWORK UDP MAAGER FUNC ============================ \\

    public void MoveEntity(long playerId, float x, float y, float z)
    {
        
        if (Players.TryGetValue(playerId, out PlayerClient? client) && client != null)
        {
            
            if (client.Character != null)
            {
                
                client.Character.SetMoveTarget(x, y, z);

            }
            else Console.WriteLine($"[SERVER] ClientId{client.PlayerId} don't have a character.");

        }

    }

}