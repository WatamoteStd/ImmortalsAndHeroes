using System;
using LiteNetLib.Utils;

namespace Iah.Shared.Packets
{ 
    public enum PacketType : byte
    {
        // 0 - Grab server Peer
        // 1 - 10 MENU | LOBBY
        FirstConnect,
        LoginRequest,
        LoginResponse,
        JoinQueue,
        JoinQueueResponse,
        LobbyFind,
        GameAccept,
        ConnectToLobby,
        HeroSelected,
        LobbyTimer,
        ConnectedToGame,

        // 11 - ??? GAME PACKETS
        ReadyToGame,
        SpawnEntity,
        EntityMove,
        AutoAttack,
        AttackInfo,
        SkillExecuteSelf,
        SkillExecutePosition,
        SkillExecuteTarget,
        SkillRelease

    }
    public struct SpawnEntityPacket {};
    public struct ReadyToGamePacket
    {
        ushort PlayerId;
        ushort MatchID;

    };

    public struct HeroSelectedPacket
    {
        public ushort ID; // PLAYER id, not PeerId or lobbyId
        public UnitList hero;
    }
    public struct LobbyTimerPaclet
    {
        byte lobbyTimer;
    }

    public struct FirstConnectPacket {};
    public struct LoginRequestPacket
    {
        public string Nickname;
    };
    public struct LoginResponsePacket
    {
        public ushort ID;
    };
    public struct JoinQueuePacket {};
    public struct LobbyFindPacket {};
    public struct GameAcceptPacket {};
    public struct ConnectToLobbyPacket
    {
        public ushort LobbyId;
        public byte PlayersCount;
        
    };

}