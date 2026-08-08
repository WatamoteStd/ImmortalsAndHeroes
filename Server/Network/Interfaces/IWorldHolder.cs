

using Shared.DataTransferObjects;

namespace Server.Network.Interfaces;

public interface IWorldHolder
{

    void AddPlayer(uint regionId, HandshakeResponseDto characterData);

}