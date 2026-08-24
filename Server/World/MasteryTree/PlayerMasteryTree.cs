
using Shared.MasteryTree;

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

        }
        else
        {
            progress.CurrentExp += (uint)_player.Exp;
            _player.AddExp(-_player.Exp);
        }

        OnBranchUpdate?.Invoke(branchId, progress.CurrentExp, progress.CurrentLevel);

        Console.WriteLine($"[MasteryTree] Branch:{branch.Title} Upgraded! Lvl:{progress.CurrentLevel}, Exp:{progress.CurrentExp}, PlayerExpLeft:{_player.Exp}");
        Console.WriteLine($"[MasteryTree] Branch:{branch.Title} Required! MaxLvl:{branch.MaxLvl}, ExpToLvl:{branch.GetRequiredExpForLevel((ushort)(progress.CurrentLevel + 1))}");

    }



}