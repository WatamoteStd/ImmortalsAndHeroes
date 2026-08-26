
using System.Collections.Frozen;
using Shared.Ability;
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
                        Context = RewardContextType.PerLevel,
                        Type = RewardType.Stat,
                        StatId = StatType.AttackSpeed,
                        Value = 200
                    },
                    new BranchReward
                    {
                        Context = RewardContextType.PerLevel,
                        Type = RewardType.Stat,
                        StatId = StatType.HealthRegen,
                        Value = 20
                    },
                }
              
            },
            new MasteryBranch
            {
                BranchId = MasteryNodeId.Body,
                Title = "Body",
                Description = "Core of your strength",
                MaxLvl = 20,
                BaseExp = 50,
                ExpMultiplier = 1.35f,

                Rewards = new BranchReward[]
                {
                    new BranchReward
                    {
                        Context = RewardContextType.PerLevel,
                        Type = RewardType.Stat,
                        StatId = StatType.Health,
                        Value = 8
                    },
                    new BranchReward
                    {
                        Context = RewardContextType.PerLevel,
                        Type = RewardType.Stat,
                        StatId = StatType.Armor,
                        Value = 1
                    }
                }
            },
            new MasteryBranch
            {
                BranchId = MasteryNodeId.FootAgility,
                Title = "Foot Agility",
                Description = "Be more flex",
                MaxLvl = 20,
                BaseExp = 75,
                ExpMultiplier = 1.45f,

                Rewards = new BranchReward[]
                {
                    
                    new BranchReward
                    {
                        Context = RewardContextType.SingleLevel,
                        Type = RewardType.ActiveSkill,
                        StatId = StatType.None,
                        TargetLevel = 3,
                        Value = (uint)AbilityTypes.DefaulthRun
                    },
                    new BranchReward
                    {
                        Context = RewardContextType.PerLevel,
                        Type = RewardType.Stat,
                        StatId = StatType.Health,
                        Value = 5
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