
namespace Shared.MasteryTree.Rewards;

public readonly struct BranchReward
{
    
    public required RewardType Type {get; init;}
    public required StatType StatId {get; init;}
    public required RewardContextType Context {get; init;}
    public ushort TargetLevel {get; init;}
    public required uint Value {get; init;}

}
public enum RewardContextType : byte
{
    SingleLevel = 0,  // Выдается строго на одном уровне
    PerLevel = 1,     // Выдается каждый уровень
}