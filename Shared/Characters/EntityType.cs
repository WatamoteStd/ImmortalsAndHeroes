using System;

namespace Shared.Characters;

public enum EntityType : uint
{
    None = 0,
    // 1 - 300 players
    Default = 1,
    Male = 2,
    Female = 3,
    Human = 4,


    // 301 - 20000 MOBS
    WolfWeak = 301,
    ForestBear = 302,
    UnknownMage = 303


    // 20001 - ushortMax OTHER


}

public static class EntityTypeExtensions
{
    
    public static bool IsPlayer(this EntityType type) 
        => type is >= EntityType.Default and <= (EntityType)300;
    
    public static bool IsMob(this EntityType type)
        => type is >= EntityType.WolfWeak and <= (EntityType)20000;

    public static bool IsEnvirmoment(this EntityType type)
        => type >= (EntityType)20001;

}