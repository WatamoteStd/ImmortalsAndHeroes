

using Shared.Items;

namespace Shared.DataTransferObjects.Market;

public record MarketItemDto
{
    
    public long OrderId {get; init;}
    public uint ItemType {get; init;}
    public byte Quality {get; init;}
    public string SellerName {get; init;} = string.Empty;
    public uint PricePerUnit {get; init;}
    public uint Count {get; init;}
    public bool IsCityOrder {get; init;}

}