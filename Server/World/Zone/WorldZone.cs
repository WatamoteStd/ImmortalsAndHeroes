using Server.World.Zone.Entities;
using Shared.Udp.Packets.Category.Game;
using Shared.Udp.Packets;
using System.Numerics;
using Shared.Items;
using Shared.Udp.Packets.Category;

namespace Server.World.Zone;

public class WorldZone
{
    public WorldHolder.ZoneType Type {get; private set;}
    public uint Id {get; private set;}
    private readonly WorldHolder _worldHolder;

    public Dictionary<uint, PlayerEntity> _players {get; private set;} = new();
    private Dictionary<uint, EntityBase> _entities = new();

    private float latensy;
    private int iterationCount;

    public WorldZone(WorldHolder world, WorldHolder.ZoneType type, uint id)
    {
        
        _worldHolder = world;
        Type = type;
        Id = id;
        LivingEntity wolfWeak = new LivingEntity(999, new Vector3(15,1,15), Shared.Characters.EntityType.WolfWeak, Id);
        _entities[wolfWeak.EntityId] = wolfWeak;

    }

    public void Update(float deltaTime)
    {
        
        // DEBUG ===============
        latensy += deltaTime;
        if (latensy >= 45.0f)
        {
            Console.WriteLine($"[45s Debug| N:{iterationCount}] RegionId:{Id}. Players: {_players.Count} Entities:{_entities.Count}");
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
        }

        _players[player.PlayerId] = player;
        _entities[player.EntityId] = player;

        foreach (var entity in _entities.Values)
        {
            if (entity.EntityId == player.EntityId) continue;
            
            var oldEntityPacket = new S2C_SpawnEntityPacket
            {
                Id = entity.EntityId,
                Health = entity.Health,
                Name = entity.Name,
                PosX = entity.Position.X,
                PosY  = entity.Position.Y,
                PosZ = entity.Position.Z,
                Type = entity.ModelType
            };

            _worldHolder.Broadcaster.SendToPlayer(player.PlayerId, PacketTypes.S2C_SpawnEntity, oldEntityPacket);

        }

         player.OnInventoryChanged += (slotIndex, item, count) =>
        {
            
            var diffPacket = new S2C_ItemDiffPacket
            {
                CharacterId = player.EntityId,
                SlotIndex = slotIndex,
                Item = item,
                Count = count
            };

            _worldHolder.SlotUpdatePlayer(player.PlayerId, diffPacket);

        };

    }


    // =========================== FROM PLAYER TO REGION REQUESTS ======================
    public void MovePlayer(PlayerEntity player, float x, float y, float z)
    {
        player.MoveToPosition(new Vector3(x,1,z));

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

    public void RemovePlayer(uint playerId, bool notifySelf = false)
    {
        
        if (_players.TryGetValue(playerId, out PlayerEntity? player))
        {

            player.ClearAllSubscriptions();
            _players.Remove(player.PlayerId);
            _entities.Remove(player.EntityId);
            
            var packet = new S2C_RemoveEntityPacket { Id = player.EntityId};
        
            foreach (var p in _players.Values)
            {
                _worldHolder.Broadcaster.SendToPlayer(p.PlayerId, PacketTypes.S2C_RemoveEntity, packet);
            }

            if (notifySelf)
            {
                _worldHolder.Broadcaster.SendToPlayer(player.PlayerId, PacketTypes.S2C_RemoveEntity, packet);
            }

            Console.WriteLine($"[REGION {Id}] Character:{player.Name} has been removed from region!");
            return;

            }

        Console.WriteLine($"[REGION {Id}] Can't find PlayerId:{playerId} at this region");
        
    }

    public void PlayerAttackRequest(PlayerEntity player, uint entityId)
    {
        
        if (_entities.TryGetValue(entityId, out EntityBase? entity))
        {
            
            

        }
        else
        {
            Console.WriteLine($"[Region:{Id}] Attack request declined. Can't find some entity.");
        }

    }

}