using Server.World.Zone.Entities;
using Shared.Udp.Packets.Category.Game;
using Shared.Udp.Packets;
using System.Numerics;
using Shared.Items;

namespace Server.World.Zone;

public class WorldZone
{
    public WorldHolder.ZoneType Type {get; private set;}
    public uint Id {get; private set;}
    private readonly WorldHolder _worldHolder;

    private readonly Dictionary<uint, PlayerEntity> _players = new();
    private Dictionary<uint, EntityBase> _entities = new();

    private float latensy;
    private int iterationCount;

    public WorldZone(WorldHolder world, WorldHolder.ZoneType type, uint id)
    {
        
        _worldHolder = world;
        Type = type;
        Id = id;

    }

    public void Update(float deltaTime)
    {
        
        // DEBUG ===============
        latensy += deltaTime;
        if (latensy >= 45.0f)
        {
            Console.WriteLine($"[45s Debug| N:{iterationCount}] RegionId:{Id}. Players: {_players.Count}");
            latensy = 0f;
            iterationCount++;
        }

        foreach (var entity in _entities.Values)
        {
            entity.Update(deltaTime);    
        }


    }

    public void AddPlayer(PlayerEntity player)
    {

        var newPlayerPacket = new S2C_SpawnEntityPacket
        {
            Id = player.EntityId,
            Health = player.Health,
            Name = player.Name,
            PosX = player.Position.X,
            PosY = player.Position.Y,
            PosZ = player.Position.Z,
            Type = player.ModelType
        };

        foreach (var oldPlayer in _players.Values)
        {
            
            _worldHolder.Broadcaster.SendToPlayer(oldPlayer.PlayerId, PacketTypes.S2C_SpawnEntity, newPlayerPacket);

            var oldPlayerPacket = new S2C_SpawnEntityPacket
            {
                Id = oldPlayer.EntityId,
                Health = oldPlayer.Health,
                Name = oldPlayer.Name,
                PosX = oldPlayer.Position.X,
                PosY  = oldPlayer.Position.Y,
                PosZ = oldPlayer.Position.Z,
                Type = oldPlayer.ModelType
            };

            _worldHolder.Broadcaster.SendToPlayer(player.PlayerId, PacketTypes.S2C_SpawnEntity, oldPlayerPacket);

        }
        
        _players[player.PlayerId] = player;
        _entities[player.EntityId] = player;

    }

    public void MovePlayer(PlayerEntity player, float x, float y, float z)
    {
        
        Console.WriteLine($"[REGION:{Id}] Take move task for player:{player.Name}");
        player.MoveToPosition(new Vector3(x,y,z));

        foreach (var curPlayer in _players.Values)
        {
            
            var movePacket = new S2C_MoveEntityPacket
            {
                Id = player.EntityId,
                PosX = x,
                PosY = y,
                PosZ = z
            };

            _worldHolder.Broadcaster.SendToPlayer(curPlayer.PlayerId, PacketTypes.S2C_MoveEntity, movePacket);

        }

    }

    public void AddItemToPlayer()
    {

        
       

    }
    public void RemoveItemPlayer()
    {
        
       

    }

}