using Godot;
using Shared.Items;
using System;

public partial class ItemCard : Button
{

	[Export] private Label _name;
	[Export] private Label _description;
	[Export] private Label _cost;
	[Export] private TextureRect _icon;
	[Export] private Label _seller;
	[Export] private Label _rare;
	
	[Export] private Button _ItemBuy;
	[Export] private Button _ItemSale;

	public override void _Ready()
	{
		
		

	}

	public void CreateItem(ItemType item, string sellerName, uint price, uint quality)
	{
		var data = ItemRegistry.GetItemData(item);
		
		_name.Text = data.ItemName;
		_cost.Text = price.ToString();
		_icon.Texture = GD.Load<Texture2D>(data.IconPath);
		_seller.Text = sellerName;
		_rare.Text = ((QualityType)quality).ToString();
		

	}



}
