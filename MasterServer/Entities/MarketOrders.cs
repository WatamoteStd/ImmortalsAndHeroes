
namespace MasterServer.Entities;

public class MarketOrders
{
    
    public long Id {get; set;}
    public long LocationId {get; set;}
    public long SellerCharacterId {get; set;}
    public string SellerName {get; set;} = string.Empty;
    public uint ItemType {get; set;}
    public byte Quality {get; set;}
    public uint Count {get; set;}
    public uint PricePerUnit {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public bool IsCityOrder {get; set;}
    public bool IsSold {get; set;}

}