using Godot;
using System;

public partial class ItemCard : Button
{
	// BD INFO
	[Export] private string ItemName;
	[Export] private string ItemDescription;
	[Export] private int ItemCost;
	//[Export] private string ItemIcon;
	[Export] private string ItemSeller;
	[Export] private string ItemRare;

	// UI ELEMENTS

	[Export] private Label _name;
	[Export] private Label _description;
	[Export] private Label _cost;
	//[Export] private TextureRect _icon;
	[Export] private Label _seller;
	[Export] private Label _rare;
	
	private Button _ItemBuy;
	private Button _ItemSale;

	public override void _Ready()
	{
		UpdateData();
		_ItemBuy = GetNode<Button>("MarginContainer/HBoxContainer/ActionMenu/BuyButton");
		_ItemSale = GetNode<Button>("MarginContainer/HBoxContainer/ActionMenu/SaleButton");
	}

	private void UpdateData()
	{
		
		_name.Text = ItemName;
		_description.Text = ItemDescription;
		_cost.Text = ItemCost.ToString();
		_seller.Text = ItemSeller;
		_rare.Text = ItemRare;
		

	}



}
