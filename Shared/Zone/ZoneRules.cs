
namespace Shared.Zone;

[Flags]
public enum ZoneRules : byte
{
    
    None = 0,
    AllowPvP = 1 << 0,
    AllowPvE = 1 << 1,
    FullLoot = 1 << 2

}