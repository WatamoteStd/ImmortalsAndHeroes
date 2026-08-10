namespace Shared.Udp.Packets;

public enum PacketTypes : ushort
{
    
    C2S_Handshake = 0,
    S2C_HandshakeSuccess = 1,
    S2C_HandshakeFailed = 2,

    S2C_SpawnEntity = 3,
    S2C_MoveEntity = 4,
    C2S_MoveRequest = 5

}