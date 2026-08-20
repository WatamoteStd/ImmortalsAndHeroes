using Server.World.Zone.Entities;
using Shared.Udp.Packets.Category.Game;
using Shared.Udp.Packets;
using System.Numerics;
using Shared.Items;
using Shared.Udp.Packets.Category;
using Shared.Characters;

namespace Server.World.Zone;

public class WorldZone
{
    public WorldHolder.ZoneType Type {get; private set;}
    public uint Id {get; private set;}
    private readonly WorldHolder _worldHolder;

    public Dictionary<uint, PlayerEntity> _players {get; private set;} = new();
    private Dictionary<uint, EntityBase> _entities = new();
    private static uint _currentMobId = 2_000_000;
    

    private float latensy;
    private int iterationCount;

    public WorldZone(WorldHolder world, WorldHolder.ZoneType type, uint id)
    {
        
        _worldHolder = world;
        Type = type;
        Id = id;

        CreateEntity(EntityType.WolfWeak, new Vector3(15, 1, 15));

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
        player.OnMoved += (character, pos) =>
        {
            
            var movePacket = new S2C_MoveEntityPacket
            {
                Id = character.EntityId,
                PosX = character.Position.X,
                PosY = character.Position.Y,
                PosZ = character.Position.Z
            };

            foreach (var p in _players.Values)
            {
                _worldHolder.Broadcaster.SendToPlayer<S2C_MoveEntityPacket>(p.PlayerId, PacketTypes.S2C_MoveEntity, movePacket);
            }

        };
        player.OnDamageTaked += (character, dmg, attacker) =>
        {
            
            var packet = new S2C_EntityDamageTakedPacket
            {
                Id = character.EntityId,
                Damage = dmg,
                AttackerId = attacker.EntityId,
                ActualHealth = (uint)character.Health
            };

            foreach (var p in _players.Values)
            {
                _worldHolder.Broadcaster.SendToPlayer(p.PlayerId, PacketTypes.S2C_EntityDamageTaked, packet);
            }

        };

    }

    public void CreateEntity(EntityType type, Vector3 spawnPosition)
    {
        
        var entityData = EntityRegistry.GetEntityData(type);

        LivingEntity newEntity = new LivingEntity(GenerateNextMobId(), spawnPosition, type, Id);
        _entities[newEntity.EntityId] = newEntity;

        var packet = new S2C_SpawnEntityPacket
        {
            Id = newEntity.EntityId,
            Health = (int)entityData.BaseHealth,
            Name = entityData.Name,
            PosX = spawnPosition.X,
            PosY = spawnPosition.Y,
            PosZ = spawnPosition.Z,
            Type = type
        };

        foreach (var p in _players.Values)
        {
            _worldHolder.Broadcaster.SendToPlayer<S2C_SpawnEntityPacket>(p.PlayerId, PacketTypes.S2C_SpawnEntity, packet);
        }

        newEntity.OnDamageTaked += (entity, damage, attacker) =>
        {
            var packet = new S2C_EntityDamageTakedPacket
            {
                Id = entity.EntityId,
                Damage = damage,
                AttackerId = attacker.EntityId,
                ActualHealth = (uint)entity.Health
            };

            foreach (var p in _players.Values)
            {
                _worldHolder.Broadcaster.SendToPlayer(p.PlayerId, PacketTypes.S2C_EntityDamageTaked, packet);
            }
        };
        newEntity.OnMoved += (entity, pos) =>
        {
            var movePacket = new S2C_MoveEntityPacket
            {
                Id = entity.EntityId,
                PosX = entity.Position.X,
                PosY = entity.Position.Y,
                PosZ = entity.Position.Z
            };

            foreach (var p in _players.Values)
            {
                _worldHolder.Broadcaster.SendToPlayer<S2C_MoveEntityPacket>(p.PlayerId, PacketTypes.S2C_MoveEntity, movePacket);
            }
        };

    }


    public static uint GenerateNextMobId()
    {
        return Interlocked.Increment(ref _currentMobId);
    }


    
    #region Player -> World || Server -> World
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
            
            player.SetAttackTarget(entity);

        }
        else
        {
            Console.WriteLine($"[Region:{Id}] Attack request declined. Can't find some entity.");
        }

    }

    #endregion

}