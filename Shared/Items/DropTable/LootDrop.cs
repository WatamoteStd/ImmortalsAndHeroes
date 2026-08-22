

namespace Shared.Items.DropTable;

public readonly struct LootDrop
{
    
    public readonly ItemType Item;
    public readonly float DropChance;
    public readonly ushort MinCount;
    public readonly ushort MaxCount;

    public LootDrop(ItemType item, float dropChance, ushort minCount, ushort maxCount)
    {
        
        Item = item;
        DropChance = dropChance;
        MinCount = minCount;
        MaxCount = maxCount;

    }

}