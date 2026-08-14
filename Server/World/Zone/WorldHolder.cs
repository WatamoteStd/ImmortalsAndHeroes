using System;
using System.Numerics;
using Server.Network.Interfaces;
using Server.World;
using Server.World.Zone.Entities;
using Shared.DataTransferObjects;
using Shared.Udp.Packets.Category;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category.Game;
using System.Collections.Concurrent;
using Server.Pools.Session;
using System.Buffers.Binary;
using System.Buffers;

namespace Server.World.Zone;

public class WorldHolder : IWorldHolder
{

    public enum ZoneType { World, City, Dungeon}
    public Dictionary<uint, PlayerEntity> idToPlayer = new Dictionary<uint, PlayerEntity>();
    public Dictionary<uint, WorldZone> idToZone = new Dictionary<uint, WorldZone>();



    private ConcurrentQueue<NetworkCommand> CommandsQueue = new ConcurrentQueue<NetworkCommand>();
    private ConcurrentQueue<HandshakeResponseDto> InitPlayerQueue = new();

    
    private readonly IWorldBroadcaster _broadcaster;
    public IWorldBroadcaster Broadcaster => _broadcaster;

    public WorldHolder(IWorldBroadcaster broadcaster)
    {
        _broadcaster = broadcaster;
        WorldZone startZone = new WorldZone(this, ZoneType.World, 0);
        WorldZone cityZone = new WorldZone(this, ZoneType.City, 1);
        idToZone[0] = startZone;
        idToZone[1] = cityZone;
    }

    public void Update(float deltaTime)
    {

        while(InitPlayerQueue.TryDequeue(out var newData))
        {
            AddPlayer((uint)newData.RegionId, newData);
        }

        while(CommandsQueue.TryDequeue(out var cmd))
        {

            switch (cmd.PacketType)
            {
                
                case PacketTypes.C2S_MoveRequest:
                    {
                        var packet = PacketSerialier.Deserialize<C2S_MoveRequestPacket>(cmd.Data[2..]);
                        MovePlayer(cmd.Session.UserId, packet);

                        ArrayPool<byte>.Shared.Return(cmd.Data);
                    }
                break;
                case PacketTypes.S2C_RemoveEntity: // INTERNAL PACKET UNIQUE LOGIC!!!
                    {
                        if (idToPlayer.TryGetValue(cmd.Session.UserId, out PlayerEntity? player))
                        {
                            RemovePlayer(player.RegionId, player.PlayerId);
                            SuccessfulDeletePlayer(cmd.Session);
                            
                        }
                        else
                        {
                            Console.WriteLine($"[WORLD HOLDER] Can't delete Userd:{cmd.Session.UserId}");
                        }                       
                    }
                break;

                case PacketTypes.C2S_ChangeRegionRequest:
                    {
                        
                        var packet = PacketSerialier.Deserialize<C2S_ChangeRegionRequestPacket>(cmd.Data[2..]);
                        ChangePlayerRegion(cmd.Session.UserId, packet);

                        ArrayPool<byte>.Shared.Return(cmd.Data);

                    }
                break;

            }

        }
        
        foreach (var zone in idToZone.Values)
        {
            zone.Update(deltaTime);
        }

    }

    public void AddPlayer(uint zoneId, HandshakeResponseDto character)
    {
        
        if (idToZone.TryGetValue(zoneId, out WorldZone? zone))
        {

            Vector3 startPos = new Vector3(character.PosX, character.PosY, character.PosZ);
            PlayerEntity newPlayer = new PlayerEntity((uint)character.Id, startPos, character.Type, character.Name,(uint)character.UserId, (uint)character.RegionId, (uint)character.Silver, (uint)character.Lvl);
            
            zone.AddPlayer(newPlayer);
            idToPlayer[(uint)character.UserId] = newPlayer;
            Console.WriteLine($"[WORLD] Added new player to zoneId{zoneId}");
            return;

        }
        Console.WriteLine($"[WORLD] Can't add player to ZoneId:{zoneId}. Doest exists.");

    }

    public void RemovePlayer(uint zoneId, uint playerId)
    {
        
        if (idToZone.TryGetValue(zoneId, out WorldZone? zone))
        {
            zone.RemovePlayer(playerId, notifySelf: true);
            idToPlayer.Remove(playerId);
            Console.WriteLine($"[WORLD HOLDER] PlayerID{playerId} leave from the world.");
        }

    }


    // ================================ FROM REGION TO PLAYER ==============================
    public void SlotUpdatePlayer(uint userId, S2C_ItemDiffPacket packet)
    {
        _broadcaster.SendToPlayer(userId, PacketTypes.S2C_ItemDiff, packet);
    }

    // =============================== FROM PLAYER TO REGION PACKETS ==================

    public void MovePlayer(uint userId, C2S_MoveRequestPacket packet)
    {
        if (idToPlayer.TryGetValue(userId, out PlayerEntity? player))
        {

            idToZone[player.RegionId].MovePlayer(player, packet.X, 1, packet.Z);
        }
    }

    public void ChangePlayerRegion(uint userId, C2S_ChangeRegionRequestPacket packet)
    {
        
        if (idToPlayer.TryGetValue(userId, out PlayerEntity? player))
        {
            
            if (player.RegionId != packet.RegionId) // WARNING! NOW THERE IS NO LEGIT CHECK 
            {
                
                if(idToZone.TryGetValue(player.RegionId, out WorldZone? oldRegion) && idToZone.TryGetValue(packet.RegionId, out WorldZone? newRegion))
                {
                    
                    oldRegion.RemovePlayer(player.PlayerId, notifySelf: false);
                    player.RegionId = newRegion.Id;

                    var changeRegPacket = new S2C_ChangeRegionPacket
                    {
                        CharacterId = player.EntityId,
                        RegionId = packet.RegionId
                    };
                    player.SetPosition(0,1,0);
                    player.MoveToPosition(new Vector3(0,1,0));
                    var characterDataPacket = new S2C_HandshakeSuccessPacket
                    {
                        Id = player.EntityId,
                        RegionId = player.RegionId,
                        Name = player.Name,
                        PosX = player.Position.X,
                        PosY = player.Position.Y,
                        PosZ = player.Position.Z,
                        UserId = player.PlayerId,
                        Type = player.ModelType,
                        CurrentHp = player.Health,
                        CurrentMp = player.Health,
                        Lvl = (int)player.Lvl,
                        Silver = player.Silver
                    };

                    _broadcaster.SendToPlayer<S2C_ChangeRegionPacket>(player.PlayerId, PacketTypes.S2C_ChangeRegion, changeRegPacket);
                    _broadcaster.SendToPlayer<S2C_HandshakeSuccessPacket>(player.PlayerId, PacketTypes.S2C_HandshakeSuccess, characterDataPacket);
                    newRegion.AddPlayer(player);


                    return;
                }

            }
            Console.WriteLine($"[CHANGE REGION] Invalid region params!");
            return;

        }
        Console.WriteLine($"[CHANGE REGION] Unknown player try to change region");

    }

    public void InitiateNewPlayer(HandshakeResponseDto data)
    {
        InitPlayerQueue.Enqueue(data);
    }


    public void EnqueueCommand(NetworkCommand cmd)
    {
        CommandsQueue.Enqueue(cmd);
    }
    public void SM_RemovePlayer(UserSession session)
    {
        
        var cmd = new NetworkCommand
        {
            Session = session,
            Data = null!,
            PacketType = PacketTypes.S2C_RemoveEntity,
            Length = 0
        };
        CommandsQueue.Enqueue(cmd);

    }
    public void SuccessfulDeletePlayer(UserSession session)
    {
        _broadcaster.API_RemoveSession(session);
    }

    

}