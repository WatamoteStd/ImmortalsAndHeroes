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

namespace Server.World.Zone;

public class WorldHolder : IWorldHolder
{

    public enum ZoneType { World, City, Dungeon}
    public Dictionary<uint, PlayerEntity> idToPlayer = new Dictionary<uint, PlayerEntity>();
    public Dictionary<uint, WorldZone> idToZone = new Dictionary<uint, WorldZone>();

    private ConcurrentQueue<NetworkCommand> CommandsQueue = new ConcurrentQueue<NetworkCommand>();

    
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

        while(CommandsQueue.TryDequeue(out var cmd))
        {
            
            var packetType = (PacketTypes)BinaryPrimitives.ReadUInt16LittleEndian(cmd.Data);

            switch (packetType)
            {
                
                case PacketTypes.C2S_MoveRequest:
                    {
                        var packet = PacketSerialier.Deserialize<C2S_MoveRequestPacket>(cmd.Data[2..]);
                        MovePlayer(cmd.Session.UserId, packet);
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
            zone.NETWORK_RemovePlayer(playerId);
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
            Console.WriteLine($"[Server Receive] Player wants to go to: X={packet.X:F2}, Y={packet.Y:F2}, Z={packet.Z:F2}");
            idToZone[player.RegionId].MovePlayer(player, packet.X, packet.Y + 1, packet.Z);
            Console.WriteLine($"[WORLD HOLDER] Move player task for Region:{player.RegionId}");
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
                    
                    oldRegion.NETWORK_RemovePlayer(player.PlayerId);
                    newRegion.AddPlayer(player);

                    return;
                }

            }
            Console.WriteLine($"[CHANGE REGION] Invalid region params!");
            return;

        }
        Console.WriteLine($"[CHANGE REGION] Unknown player try to change region");

    }


    public void EnqueueCommand(NetworkCommand cmd)
    {
        CommandsQueue.Enqueue(cmd);
    }

    

}