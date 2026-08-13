using Server.Pools.Session;
using Server.World.Zone;
using Shared.DataTransferObjects;
using Shared.Udp.Packets.Category;
using Shared.Udp.Packets.Category.Game;

namespace Server.Network.Interfaces;

public interface IWorldHolder
{

    void EnqueueCommand(NetworkCommand data);
    void InitiateNewPlayer(HandshakeResponseDto characterData);
    void SM_RemovePlayer(UserSession session);

}