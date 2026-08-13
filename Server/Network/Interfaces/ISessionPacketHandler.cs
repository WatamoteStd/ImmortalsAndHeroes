

using System.Net;

namespace Server.Network.Interfaces;

public interface ISessionPacketHandler
{
    
    Task HandshakeRequest(string ticket, IPEndPoint ip);

}