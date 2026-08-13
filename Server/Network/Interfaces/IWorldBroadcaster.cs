
using Server.Pools.Session;
using Shared.Udp.Interfaces;
using Shared.Udp.Packets;

namespace Server.Network.Interfaces;

public interface IWorldBroadcaster
{
    
    void SendToPlayer<T>(uint userId, PacketTypes packetType, T packet) 
        where T : struct, INetworkPacket;

    void API_RemoveSession(UserSession session);


}