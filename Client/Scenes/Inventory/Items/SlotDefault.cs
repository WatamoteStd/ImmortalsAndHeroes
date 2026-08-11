using Godot;
using Shared.Items;
using System;

public partial class SlotDefault : PanelContainer
{
	
	[Export] private TextureRect _icon;
	[Export] private Label _count;

	public void UpdateVisual(ItemType item, ushort count)
	{
		
		if (item == ItemType.None || count == 0)
		{
			
			_icon.Texture = null;
			_count.Visible = false;
			return;

		}

		ItemData itemData = ItemRegistry.GetItemData(item);

		_icon.Texture = GD.Load<Texture2D>(itemData.IconPath);
		_count.Visible = true;
		_count.Text = count.ToString();

	}

}
