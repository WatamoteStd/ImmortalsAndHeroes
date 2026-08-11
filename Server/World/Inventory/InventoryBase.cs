

using Shared.Items;

namespace Server.World.Inventory;

public class InventoryBase
{
    
    public event Action<ushort, ItemType, ushort>? OnItemChanged;

    private readonly ItemSlot[] _slots;
    public ref readonly ItemSlot this[int index] => ref _slots[index];
    public int Capacity => _slots.Length;
    private uint _capacity;

    public InventoryBase(uint capacity)
    {
        
        _slots = new ItemSlot[capacity];
        _capacity = capacity;

    }

    public ushort AddItem(ItemType item, ushort count)
    {
        ItemData itemData = ItemRegistry.GetItemData(item);
        
        for (int i = 0; i < _slots.Length; i++) // search the same item 
        {
            ref ItemSlot curSlot = ref _slots[i];
            
            if (curSlot.ItemId == item)
            {
                ushort freeSpace = (ushort)(itemData.MaxStack - curSlot.Count);
                if (freeSpace == 0) continue;
                
                if (freeSpace >= count) // if have ALL free space
                {
                    curSlot.Count += count;
                    OnItemChanged?.Invoke((ushort)i, curSlot.ItemId, curSlot.Count);
                    return 0;
                }

                curSlot.Count += freeSpace;
                count -= freeSpace;
                OnItemChanged?.Invoke((ushort)i, curSlot.ItemId, curSlot.Count);
                

            }
            
        }

        if (count == 0) return 0;

        for (int i = 0; i < _slots.Length; i++)
        {
            
            ref ItemSlot curSlot = ref _slots[i];

            if (curSlot.IsEmpty)
            {
                

                if (count >= itemData.MaxStack)
                {
                    curSlot = new ItemSlot(item, itemData.MaxStack);
                    count -= itemData.MaxStack;
                    OnItemChanged?.Invoke((ushort)i, curSlot.ItemId, curSlot.Count);
                }
                else
                {
                    curSlot = new ItemSlot(item, count);
                    OnItemChanged?.Invoke((ushort)i, curSlot.ItemId, curSlot.Count);
                    return 0;
                }
                
            }

        }
        return count;
        

    }

    public uint GetItemCount(ItemType item)
    {
        ushort count = 0;
        
        for (int i = 0; i < _slots.Length; i++)
        {
            
            ref readonly ItemSlot curSlot = ref _slots[i];

            if (curSlot.ItemId == item)
            {
                
                count += curSlot.Count;

            }

        }
        return count;

    }

    public bool RemoveItem(ItemType item, ushort count)
    {
        
        uint readCount = GetItemCount(item);

        if (readCount < count) return false;
        
            
        for(int i = _slots.Length - 1; i >= 0; i--)
        {
                
            ref ItemSlot curSlot = ref _slots[i];

            if (curSlot.ItemId == item)
            {
                    
                if (curSlot.Count >= count)
                {
                    curSlot.Count -= count;
                    OnItemChanged?.Invoke((ushort)i, curSlot.ItemId, curSlot.Count);
                    return true;
                }
                count -= curSlot.Count;
                curSlot = new ItemSlot(ItemType.None, 0);
                OnItemChanged?.Invoke((ushort)i, curSlot.ItemId, curSlot.Count);


            }

        }
        return true;


    }

}