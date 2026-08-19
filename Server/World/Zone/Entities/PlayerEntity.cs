using Shared.Characters;
using System.Numerics;
using Server.World.Zone.Entities;
using Server.World.Inventory;
using Shared.Items;

namespace Server.World;

public class PlayerEntity : LivingEntity
{

    public event Action<ushort, ItemType, ushort>? OnInventoryChanged;
    
    public uint PlayerId {get; private set;}
    public uint Silver {get; private set;}
    public uint Lvl {get; private set;}

    public InventoryBase Inventory = new InventoryBase(10);

    public PlayerEntity(uint entityId, Vector3 pos, EntityType type, string name, uint playerId, uint regionId, uint silver, uint lvl) : base(entityId, pos, type, regionId)
    {
        Name = name;
        PlayerId = playerId;
        Silver = silver;
        Lvl = lvl;

        Inventory.OnItemChanged += (slotIndex, item, count) =>
        {
            OnInventoryChanged?.Invoke(slotIndex, item,count);
        };

    }

    public ushort AddItem(ItemType item, ushort count)
    {
        ushort less = Inventory.AddItem(item, count);

        return less;
    }

    public bool RemoveItem(ItemType item, ushort count)
    {
        
        bool isOk = Inventory.RemoveItem(item, count);
        return isOk;


    }

    public override void ClearAllSubscriptions()
    {
        base.ClearAllSubscriptions();
        OnInventoryChanged = null!; 
    }
    

}