
using System.Collections.Frozen;
using Shared.MasteryTree.Rewards;

namespace Shared.MasteryTree;

public static class MasteryBranchesRegistry
{
    
    private static FrozenDictionary<MasteryNodeId, MasteryBranch> _branches = FrozenDictionary<MasteryNodeId, MasteryBranch>.Empty;
    public readonly static ushort Count;

    static MasteryBranchesRegistry()
    {
        
        var list = new MasteryBranch[]
        {
            
            new MasteryBranch
            {
                BranchId = MasteryNodeId.DarkPath,
                Title = "Dark Path",
                Description = "On the way of darkness",
                MaxLvl = 1,
                BaseExp = 100,
                ExpMultiplier = 1.0f,

                Rewards = new BranchReward[]
                {
                    new BranchReward
                    {
                         Context = RewardContextType.SingleLevel,
                        Type = RewardType.HidenSkill,
                        StatId = StatType.Armor,
                        Value = 2
                    }
                }
              
            }

        };

        _branches = list.ToFrozenDictionary(b => b.BranchId);
        Count = (ushort)list.Length;

    }

    public static bool TryGetBranch(MasteryNodeId id, out MasteryBranch branch)
    {
        return _branches.TryGetValue(id, out branch);
    }

}