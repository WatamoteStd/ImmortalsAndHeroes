using System;
using System.Data.Common;
using System.Net;
using Server.Pools;
using Server.Pools.Session;

namespace Server.Network;

public class SessionManager
{
    
    private Stack<ushort> guestIds = new(); 
    Dictionary<IPEndPoint, UserSession> ipToSession = new();
    public SessionPool GuestPool {get; private set;} 
    public SessionPool MainPool {get; private set;} 

    private byte MainPoolDeleted, GuestPoolDeleted;

    public SessionManager()
    {

        GuestPool = new SessionPool(300, 300);
        MainPool = new SessionPool(2000, 6000);
        
        for (ushort i = 0; i < 300; i++)
        {
            
            guestIds.Push(i);

        }



    }

    public void PacketGateway(IPEndPoint endPoint)
    {
        
        if (ipToSession.TryGetValue(endPoint, out UserSession? session))
        {
                
            if (session.State == UserSession.SessionState.Guest)
            {

                session.LastPacketTime = Environment.TickCount64;
                Console.WriteLine("[Server] Received new packet from 'GUEST' user.");

            }
            else if (session.State == UserSession.SessionState.Active)
            {
                
                session.LastPacketTime = Environment.TickCount64;
                Console.WriteLine("[Server] Received new packet from 'ACTIVE' user.");

            }

        }
        else
            {
            if (guestIds.Count == 0)
            {
                Console.WriteLine("[WARNING] GuestPool is full!");
                return;
            }

            UserSession newSession = new UserSession(endPoint, guestIds.Pop());

            GuestPool.AddSession(newSession);
            ipToSession[endPoint] = newSession;

        }

    }

    public void Cleaner(long timeoutTimeMs)
    {
        
        long now = Environment.TickCount64;

        for (int i = (int)GuestPool.Count - 1; i >= 0; i--)
        {

            var session = GuestPool.Dense[i];
            
            if (now - session.LastPacketTime > timeoutTimeMs)
            {
                
                ipToSession.Remove(session.IpEnd);
                guestIds.Push((ushort)session.UserId);
                GuestPool.DeleteSession(session);

                GuestPoolDeleted++;
                Console.WriteLine($"[CLEANER DEBUG] Deleted Users statistics. Total: {MainPoolDeleted + GuestPoolDeleted}. MainPool:{MainPoolDeleted}. GuestPool{GuestPoolDeleted}");
                Console.WriteLine($"[CLEANER DEBUG] Free guest ID'S:{guestIds.Count} | Total Players:{MainPool.Count}. | Total Guest:{GuestPool.Count}");

            }

        }
        for (int i = (int)MainPool.Count - 1; i >= 0;  i--)
        {
            
            var session = MainPool.Dense[i];
            
            if (now - session.LastPacketTime > timeoutTimeMs)
            {
                
                ipToSession.Remove(session.IpEnd);
                MainPool.DeleteSession(session);

                MainPoolDeleted++;
                Console.WriteLine($"[CLEANER DEBUG] Deleted Users statistics. Total: {MainPoolDeleted + GuestPoolDeleted}. MainPool:{MainPoolDeleted}. GuestPool{GuestPoolDeleted}");
                Console.WriteLine($"[CLEANER DEBUG] Free guest ID'S:{guestIds.Count} | Total Players:{MainPool.Count}. | Total Guest:{GuestPool.Count}");

            }

        }
        

    }


    public void HandshakeRequest(string ticket, IPEndPoint iPEnd)
    {
        
        

    }


}
