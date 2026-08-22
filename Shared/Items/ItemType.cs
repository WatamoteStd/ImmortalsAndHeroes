

namespace Shared.Items;

public enum ItemType : uint
{
    None = 0,
    // 1 - 1.000.000 Equipment


    // 1.000.001 - 2.500.000 Resources

    IronOre_Horrible = 1000001,
    IronOre_Bad = 1000002,
    IronOre_BelowNormal = 1000003,
    IronOre_Normal = 1000004,
    IronOre_Good = 1000005,
    IronOre_Great = 1000006,
    IronOre_Excellent = 1000007,
    IronOre_Primordial = 1000008,

    
    WolfSkin_Horrible = 1_000_009,
    WolfSkin_Bad = 1_000_010,
    WolfSkin_BelowNormal = 1_000_011,
    WolfSkin_Normal = 1_000_012,
    WolfSkin_Good = 1_000_013,
    WolfSkin_Great = 1_000_014,
    WolfSkin_Excellent = 1_000_015,
    WolfSkin_Primordial = 1_000_016


    // 2.500.001 - 4.200.000 Other



}

public static class ItemTypeExtensions
{
    
    public static bool IsEquipment(this ItemType type)
        => type is > ItemType.None and <= (ItemType)1000000;
    
    public static bool IsResource(this ItemType type) 
        => type is >= ItemType.IronOre_Horrible and <= (ItemType)2500000;

    public static bool IsOther(this ItemType type) 
        => type is >= (ItemType)2500001 and <= (ItemType)4200000;

}