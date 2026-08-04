using System.Collections.Concurrent;

namespace MasterServer.Services;

public class TicketService : ITicketService
{

    private readonly ConcurrentDictionary<string, PendingTicket> _tickets = new();
    private record PendingTicket(long UserId, long CharacterId, DateTime ExpiresAt);

    public string IssueTicket(long userId, long characterId)
    {

        var ticket = Guid.NewGuid().ToString("N");

        var pending = new PendingTicket(userId, characterId, DateTime.UtcNow.AddSeconds(20));
        _tickets[ticket] = pending;

        return ticket;

    }

    public ITicketService.TicketData? ConsumeTicket(string ticket)
    {

        if (_tickets.TryRemove(ticket, out var pending))
        {

            if (pending.ExpiresAt > DateTime.UtcNow)
            {
                return new ITicketService.TicketData(pending.UserId, pending.CharacterId);
            }
            
        }

        return null;

    }
    
}