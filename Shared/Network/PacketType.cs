namespace Shared.Network;

public enum PacketType : ushort
{
    // SYSTEM AUTHORIZATION (UDP)
    C2S_Handshake = 1,
    S2C_HandshakeResponse = 2,

    // GAME UDP
    C2S_PlayerInput = 100,
    S2C_WorldState = 101

}