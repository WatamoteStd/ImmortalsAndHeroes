

namespace Shared.Items;

public enum QualityType : byte
    {
        
        Horrible = 0,
        Bad = 1,
        BelowNormal = 2,
        Normal = 3,
        Good = 4,
        Great = 5,
        Excellent = 6,
        Primordial = 7


}

public readonly struct ItemData
{
    
    public readonly QualityType Quality;
    public readonly ushort MaxStack;
    public readonly string ItemName;
    public readonly string IconPath;

    public ItemData(QualityType quality, ushort stackCount, string name, string iconPath)
    {
        Quality = quality;
        MaxStack = stackCount;
        ItemName = name;
        IconPath = iconPath;
    }


}

public struct ItemSlot
{
    
    public ItemType ItemId;
    public ushort Count;
    public readonly bool IsEmpty => ItemId == ItemType.None;

    public ItemSlot(ItemType type, ushort count)
    {
        ItemId = type;
        Count = count;
    }

}
public static class ItemRegistry
{
    
    private static readonly Dictionary<ItemType, ItemData> _items = new Dictionary<ItemType, ItemData>()
    {
        
        {ItemType.IronOre_Horrible, new ItemData(QualityType.Horrible, 999, "Iron Ore", "res://Scenes/Inventory/Items/Resource/IronOre/IronOre_Horrible.png")},


        {ItemType.WolfSkin_Horrible, new ItemData(QualityType.Horrible, 999, "Wolf Skin", "res://Scenes/Inventory/Items/Resource/WolfSkin/horrible.png")},
        {ItemType.WolfSkin_Primordial, new ItemData(QualityType.Primordial, 10, "Wolf Skin", "res://Scenes/Inventory/Items/Resource/WolfSkin/Primordial.png")},
        {ItemType.WolfSkin_Normal, new ItemData(QualityType.Normal, 999, "Wolf Skin", "res://Scenes/Inventory/Items/Resource/WolfSkin/Normal.png")}

    };

    private static readonly ItemData defaultData = new ItemData(QualityType.Primordial, 5, "Unexpcted", "res://Scenes/Inventory/Items/Resource/tomato.png");

    public static ItemData GetItemData(ItemType item)
    {
        
        if (_items.TryGetValue(item, out ItemData data))
        {
            return data;
        }
        else return defaultData;

    }

}