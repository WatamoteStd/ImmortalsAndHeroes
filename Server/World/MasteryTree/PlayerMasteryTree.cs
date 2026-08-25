using Shared.MasteryTree.Rewards;
using Shared.MasteryTree;
using Shared.Ability;

namespace Server.World.MasteryTree;

public class PlayerMasteryTree
{

    public event Action<MasteryNodeId, uint, ushort>? OnBranchUpdate; // BrenchId, exp, lvl
    private PlayerMasteryBranchProgress[] _branches = new PlayerMasteryBranchProgress[MasteryBranchesRegistry.Count];
    private PlayerEntity _player;

    public PlayerMasteryTree(PlayerEntity player)
    {
        _player = player;
    }

    public void AddExp(MasteryNodeId branchId)
    {
        
        if (!MasteryBranchesRegistry.TryGetBranch(branchId, out var branch))
            return;

        ref var progress = ref _branches[(int)branchId];
        
        if (progress.CurrentLevel == branch.MaxLvl)
        {
            Console.WriteLine($"[BrachSystem] Branch:{branch.Title} Level is maximum!");
            return;
        }
        if (_player.Exp <= 0) return;

        var dif =  branch.GetRequiredExpForLevel((ushort)(progress.CurrentLevel + 1)) - progress.CurrentExp;
        if (dif <= _player.Exp)
        {
            
            progress.CurrentLevel++;
            progress.CurrentExp = 0;
            _player.AddExp((int)-dif);

            LevelUp(branchId, progress.CurrentLevel);

        }
        else
        {
            progress.CurrentExp += (uint)_player.Exp;
            _player.AddExp(-_player.Exp);
        }

        OnBranchUpdate?.Invoke(branchId, progress.CurrentExp, progress.CurrentLevel);


    }

    private void LevelUp(MasteryNodeId branchId, ushort lvl)
    {
        Console.WriteLine($"LevelUp trigerred by branch:{branchId.ToString()}");
        
        if (!MasteryBranchesRegistry.TryGetBranch(branchId, out var data))
            return;
        if (data.Rewards.Length == 0) return;


        for (int i = 0; i < data.Rewards.Length; i++)
        {
            
            var reward = data.Rewards[i];

            if (reward.Context == RewardContextType.SingleLevel && reward.TargetLevel == lvl)
            {
                GiveReward(reward);
            }

            if (reward.Context == RewardContextType.PerLevel)
            {
                GiveReward(reward);
            }

        }

    }

    private void GiveReward(BranchReward reward)
    {
        
        switch (reward.Type)
        {
            
            case RewardType.Stat:
                {
                    _player.UpdateStat(reward.StatId, reward.Value);
                }
            break;
            case RewardType.ActiveSkill:
                {
                    _player.AddAbility((AbilityTypes)reward.Value);
                }
            break;

        }

    }



}