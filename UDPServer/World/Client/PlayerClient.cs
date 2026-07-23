using System;
using System.Net;
using UDPServer.World;
using UDPServer.World.Entities;

namespace UDPServer.Network.Client;

public class PlayerClient
{

    public enum ClientState { LoginMenu, InGame, Loading }

    // ENET INFO ===================================================
    public readonly long PlayerId;
    public uint NetworkId {get; set;}
    public IPEndPoint RemoteIpEndPoint {get; set;} // adress to send packets
    public long RegionId {get; set;}

    // GAME ==========================================================
    public Entity? Character;

    public PlayerClient(IPEndPoint endPoint, long id)
    {
        
        RemoteIpEndPoint = endPoint;
        PlayerId = id;

    }

}