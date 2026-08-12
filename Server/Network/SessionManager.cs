using System;
using System.Buffers;
using System.Data.Common;
using System.Net;
using Server.Network.Interfaces;
using Server.Pools;
using Server.Pools.Session;
using Server.World.Zone;
using Shared.DataTransferObjects;
using Shared.Udp.Interfaces;
using Shared.Udp.Packets;
using Shared.Udp.Packets.Category;
using Shared.Udp.Packets.Category.Game;

namespace Server.Network;

public class SessionManager : IWorldBroadcaster
{
    
    private Stack<ushort> guestIds = new(); 
    Dictionary<IPEndPoint, UserSession> ipToSession = new();
    public SessionPool GuestPool {get; private set;} 
    public SessionPool MainPool {get; private set;} 

    private readonly PacketReader _packetReader;

    // HTTP
    private readonly HttpMaster _httpMaster = new HttpMaster();
    private readonly PacketSender _packetSender;
    private IWorldHolder? _worldApi;

    private byte MainPoolDeleted, GuestPoolDeleted;

    public SessionManager(PacketSender packetSender)
    {
        _packetReader = new PacketReader(this);
        _packetSender = packetSender;

        GuestPool = new SessionPool(300, 300);
        MainPool = new SessionPool(2000, 6000);
        
        for (ushort i = 0; i < 300; i++)
        {
            
            guestIds.Push(i);

        }



    }

    public void InitializeWorld(IWorldHolder holder)
    {
        _worldApi = holder;
    }

    public void PacketGateway(IPEndPoint endPoint, byte[] data, ushort dataLength)
    {
        
        if (ipToSession.TryGetValue(endPoint, out UserSession? session))
        {
            if (session.State == UserSession.SessionState.Active)
            {
                
                session.LastPacketTime = Environment.TickCount64;
                Console.WriteLine($"[Server] Received new packet from Player:{session.UserId}.");

                var localPacket = new NetworkCommand
                {
                    Session = session,
                    Data = data,
                    Length = dataLength
                };
                _worldApi?.EnqueueCommand(localPacket);

            }
            else if (session.State == UserSession.SessionState.Guest)
            {
                
                session.LastPacketTime = Environment.TickCount64;

                if (session.IsAuthorazing) return;
                
                if (_packetReader.ReadPacketType(data) == PacketTypes.C2S_Handshake)
                {
                    _packetReader.PacketDeserialize(data, session);
                    session.IsAuthorazing = true;
                }

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

            if (_packetReader.ReadPacketType(data) == PacketTypes.C2S_Handshake)
            {
                
                _packetReader.PacketDeserialize(data, newSession);
                newSession.IsAuthorazing = true;

            }


        }

    }

    public void Cleaner(long timeoutTimeMs, long mainTimeouteMs)
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
                Console.WriteLine($"[CLEANER DEBUG] Cleared-Users| MainPool:{MainPoolDeleted}. GuestPool{GuestPoolDeleted}");
                Console.WriteLine();
                Console.WriteLine($"[CLEANER DEBUG] Free guest ID'S:{guestIds.Count} | Total Players:{MainPool.Count}. | Total Guest:{GuestPool.Count}");

            }

        }
        for (int i = (int)MainPool.Count - 1; i >= 0;  i--)
        {
            
            var session = MainPool.Dense[i];
            
            if (now - session.LastPacketTime > mainTimeouteMs)
            {
                
                ipToSession.Remove(session.IpEnd);
                MainPool.DeleteSession(session);

                MainPoolDeleted++;
                Console.WriteLine($"[CLEANER DEBUG] Deleted Users statistics. | MainPool:{MainPoolDeleted}. GuestPool{GuestPoolDeleted}");
                Console.WriteLine();
                Console.WriteLine($"[CLEANER DEBUG] Free guest ID'S:{guestIds.Count} | Total Players:{MainPool.Count}. | Total Guest:{GuestPool.Count}");

            }

        }
        

    }

    public void AuthorizeSession(IPEndPoint userIp, HandshakeResponseDto characterData)
    {
        
        if (ipToSession.TryGetValue(userIp, out UserSession? session))
        {
            
            // DELETE FROM GUEST
            GuestPool.DeleteSession(session);
            guestIds.Push((ushort)session.UserId);
    
            session.State = UserSession.SessionState.Active;
            session.IsAuthorazing = false;
            session.UserId = (uint)characterData.UserId;
            session.LastPacketTime = Environment.TickCount64;
            MainPool.AddSession(session);

            Console.WriteLine($"[SESSIONS] New active session created! UserId:{session.UserId}");

            _packetSender.SM_SendHandhsakeResult(true, characterData, userIp);
            if (_worldApi != null)
            {
                _worldApi.AddPlayer((uint)characterData.RegionId, characterData);
            }
            else
            {
                Console.WriteLine($"[ERROR] Cannot add new player. WorldHolder is null!");
            }

        }

    }
    public async Task HandshakeRequest(string ticket, IPEndPoint iPEnd)
    {
        

        var (isValid, characterData, message) = await _httpMaster.ValidateSessionAsync(ticket);

        if (!isValid || characterData == null)
        {
            
            Console.WriteLine($"[HTTP] Handshake for {iPEnd} failed. Message{message}");
            _packetSender.SM_SendHandhsakeResult(false, null, iPEnd);

            return;

        }
        AuthorizeSession(iPEnd, characterData);

    }

    // =========== API METHODS ======================================

    public void SendToPlayer<T>(uint userId, PacketTypes packetType, T packet) 
        where T : struct, INetworkPacket
    {

        var session = MainPool.GetSession(userId);
        if (session == null) return;
        
        _packetSender.SendPacket(session.IpEnd, packetType, packet);

    }


}
