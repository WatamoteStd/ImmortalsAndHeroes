
namespace Shared.Ability;

public enum AbilityTypes : uint
{
    
    None = 0,

    // 1 - 200.000 ACTIVE SKILL

    DefaulthRun = 1,
    ZoneOfBlood = 2,
    Sharp = 3,
    Poke = 4

    // 200.001 - 600.000 PASSIVE SKILL

    // 600.001 - UINTMAXVALUE - HIDDEN SKILLS

    

}
public static class AbilityTypeExtension
{
    
    public static bool IsActive(this AbilityTypes type) 
        => type is > AbilityTypes.None and <= (AbilityTypes)200_000;
    
    public static bool IsPassive(this AbilityTypes type)
        => type is >= (AbilityTypes)200_001 and <= (AbilityTypes)600_000;

    public static bool IsHidden(this AbilityTypes type)
        => type >= (AbilityTypes)600_001;

}