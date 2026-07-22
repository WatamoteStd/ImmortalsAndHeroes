namespace Shared.Network;

public enum PacketType : ushort
{
    // SYSTEM AUTHORIZATION (UDP)
    C2S_Handshake = 1,
    S2C_HandshakeResponse = 2,
    C2S_EnterTheWorld = 3,
    S2C_RegionEnter = 4,

    // GAME UDP
    C2S_PlayerInput = 100,
    S2C_WorldState = 101

}