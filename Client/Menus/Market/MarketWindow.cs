using Godot;
using Shared.DataTransferObjects.Market;
using Shared.Items;
using System;

public partial class MarketWindow : Control
{
	
	[Export] private VBoxContainer _itemList;
	[Export] private PackedScene _item;

	public void MARKET_AddItem(MarketItemDto dto)
	{
		
		var newItem = _item.Instantiate<ItemCard>();
		_itemList.AddChild(newItem);
		newItem.CreateItem((ItemType)dto.ItemType, dto.SellerName, dto.PricePerUnit, dto.Quality);


	}

}
