namespace MasterServer.Services;

public interface ITicketService
{

    string IssueTicket(long userId, long characterId);

    TicketData? ConsumeTicket(string ticket);

    public record TicketData(long UserId, long CharacterId);
}
