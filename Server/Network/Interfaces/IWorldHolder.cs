

using Shared.DataTransferObjects;
using Shared.Udp.Packets.Category.Game;

namespace Server.Network.Interfaces;

public interface IWorldHolder
{

    void AddPlayer(uint regionId, HandshakeResponseDto characterData);
    void MovePlayer(uint userId, C2S_MoveRequestPacket packet);

}