using System;
using System.Net;

namespace Server.Pools.Session;

public class UserSession
{
    
     public enum SessionState { Guest, Active, Disconnect };
    public SessionState State = SessionState.Guest;
    public bool IsAuthorazing {get; set;} = false;
    public IPEndPoint IpEnd;
        // MASTER SERVER DATA
    public uint UserId {get; set;}
    public long CharacterId {get; set;}
    public DateTime CreatedTime = DateTime.UtcNow;
    public long LastPacketTime = Environment.TickCount64;

    public UserSession(IPEndPoint ip, uint id)
    {
        IpEnd = ip;
        UserId = id;
    }


}