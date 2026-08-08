using System;
using Server.Network.Interfaces;
using Server.World;
using Server.World.Zone.Entities;
using Shared.DataTransferObjects;

namespace Server.World.Zone;

public class WorldHolder : IWorldHolder
{

    public enum ZoneType { World, City, Dungeon}
    public Dictionary<uint, PlayerEntity> idToPlayer = new Dictionary<uint, PlayerEntity>();
    public Dictionary<uint, WorldZone> idToZone = new Dictionary<uint, WorldZone>();
    
    private readonly IWorldBroadcaster _broadcaster;

    public WorldHolder(IWorldBroadcaster broadcaster)
    {
        _broadcaster = broadcaster;
        WorldZone startZone = new WorldZone(this, ZoneType.World, 0);
        idToZone[0] = startZone;
    }

    public void Update(float deltaTime)
    {
        
        foreach (var zone in idToZone.Values)
        {
            zone.Update(deltaTime);
        }

    }

    public void AddPlayer(uint zoneId, HandshakeResponseDto character)
    {
        
        PlayerEntity newPlayer = new PlayerEntity(character.Name, (uint)character.UserId);
        if (idToZone.TryGetValue(zoneId, out WorldZone? zone))
        {
            
            zone.AddPlayer(newPlayer);
            idToPlayer[(uint)character.UserId] = newPlayer;
            Console.WriteLine($"[WORLD] Added new player to zoneId{zoneId}");
            return;

        }
        Console.WriteLine($"[WORLD] Can't add player to ZoneId:{zoneId}. Doest exists.");
            
        

    }

}