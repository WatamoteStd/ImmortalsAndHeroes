using Shared.Characters;
using System.Numerics;
using Server.World.Zone.Entities;
using Server.World.Inventory;
using Shared.Items;

namespace Server.World;

public class PlayerEntity : LivingEntity
{
    
    public string Name {get; private set;} = null!;
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

    }

    public ushort AddItem(ItemType item, ushort count)
    {
        ushort less = Inventory.AddItem(item, count);

        Console.WriteLine($"[Player:{Name}] Inventory Status ========================");
        for(int i = 0; i < Inventory.Capacity; i++)
        {
            ref readonly var slot = ref Inventory[i];
            Console.WriteLine($"[SLOT:{i}] Item:{slot.ItemId} Count:{slot.Count}");
        }

        return less;
    }
    

}