
using Shared.MasteryTree.Rewards;

namespace Shared.MasteryTree;

public struct MasteryBranch
{
    
    public required MasteryNodeId BranchId {get; init;}
    public required ushort MaxLvl {get; init;}
    public required string Description {get; init;}
    public required string Title {get; init;}
    public required uint BaseExp { get; init; }
    public required float ExpMultiplier { get; init; }
    public required BranchReward[] Rewards {get; init;}

    public uint GetRequiredExpForLevel(ushort level)
    {
        
        if (level > MaxLvl) return uint.MaxValue;

        return (uint)(BaseExp *  Math.Pow(level, ExpMultiplier));

    }

}