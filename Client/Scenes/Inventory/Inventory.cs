using Godot;
using Shared.Items;
using System;

public partial class Inventory : PanelContainer
{
	
	private SlotDefault[] _slots;

	public override void _Ready()
	{
	
		var grid = GetNode<Control>("VBoxContainer/Slots");

		int childCount = grid.GetChildCount();
		_slots = new SlotDefault[childCount];

		for (int i = 0; i < childCount; i ++)
		{   
			_slots[i] = grid.GetChild<SlotDefault>(i);
		}

	}

	public void UpdateCell(ushort slotIndex, ItemType item, ushort count)
	{
		
		_slots[slotIndex].UpdateVisual(item, count);

	}


}
