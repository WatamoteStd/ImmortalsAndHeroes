using Godot;
using Shared.MasteryTree;
using System;
using System.Collections.Generic;
using Shared.MasteryTree.Rewards;
using Shared.Ability;

public partial class PathInfoPanel : PanelContainer
{
	[Export] private PackedScene _statRewardScene;
	[Export] private PackedScene _abilityRewardScene;
	[Export] private VBoxContainer _rewardsBox;
	[Export] private HFlowContainer _abilityRewardBox;
	[Export] private ProgressionBlock _progressionBlock;
	[Export] private Button _learnBranchButton;
	[Export] private Label _headerTitle;
	[Export] private Label _descriptionLabel;

	private Dictionary<MasteryNodeId, BranchCache> _bracnesCache = new ();

	private MasteryNodeId _currentBranchId;



	public override void _Ready()
	{
		
		_learnBranchButton.Pressed += LearnBranchRequest;

	}


	public void OpenBranch(MasteryNodeId branchId)
	{
		
		Visible = true;
		_currentBranchId = branchId;
		MasteryBranchesRegistry.TryGetBranch(branchId, out var dllData);

		_headerTitle.Text = dllData.Title;
		_descriptionLabel.Text = dllData.Description;

		ushort currentLvl = 0;

		if (_bracnesCache.TryGetValue(branchId, out var cache))
		{
			
			currentLvl = cache.CurrentLvl;
			_progressionBlock.SetProgress((int)cache.CurrentExp, cache.CurrentLvl, dllData);

		}
		else
		{
			_progressionBlock.SetProgress(0, 0, dllData);
		}

		bool isMax = currentLvl >= dllData.MaxLvl;
		_learnBranchButton.Disabled = isMax;

		// REWARDS

		foreach (Node child in _rewardsBox.GetChildren())
		{
			if (child is HFlowContainer) continue;
			child.QueueFree();
		}

		foreach(Node child in _abilityRewardBox.GetChildren())
		{
			child.QueueFree();
		}

		if (dllData.Rewards.Length == 0) return;



		for (int i = 0; i < dllData.Rewards.Length; i++) // STAT PER LVL
		{
			
			var reward = dllData.Rewards[i];

			if (reward.Type == RewardType.Stat && reward.Context == RewardContextType.PerLevel)
			{
				var newReward = _statRewardScene.Instantiate<StatReward>();

				_rewardsBox.AddChild(newReward);
				newReward.CreateVisual(reward.StatId, (int)reward.Value, 0);
			}

		}
		
		for (int i = 0; i < dllData.Rewards.Length; i++) // STAT SINGLE LVL
		{
			
			var reward = dllData.Rewards[i];

			if (reward.Type == RewardType.Stat && reward.Context == RewardContextType.SingleLevel) 
			{

				var newReward = _statRewardScene.Instantiate<StatReward>();

				_rewardsBox.AddChild(newReward);
				newReward.CreateVisual(reward.StatId, (int)reward.Value, reward.TargetLevel, true);

			}

		}

		for (int i = 0; i < dllData.Rewards.Length; i++)
		{
			
			var reward = dllData.Rewards[i];

			if (reward.Type == RewardType.ActiveSkill && reward.Context == RewardContextType.SingleLevel)
			{
				AbilityRegistry.TryGetAbility((AbilityTypes)reward.Value, out var data);
				var newReward = _abilityRewardScene.Instantiate<AbilityButton>();


				bool isLocked = false;
				if (_bracnesCache.TryGetValue(branchId, out var branchCache))
				{
					isLocked = _bracnesCache[branchId].CurrentLvl < reward.TargetLevel;
				}

				_abilityRewardBox.AddChild(newReward);
				newReward.Init(data, reward.TargetLevel, isLocked);

			}

		}


	}

	public void UpdateBranch(MasteryNodeId branchid, uint currentExp, ushort currentLvl)
	{
		
		var cache = new BranchCache {BranchId = branchid, CurrentExp = currentExp, CurrentLvl = currentLvl};
		_bracnesCache[branchid] = cache;

		if (_currentBranchId == branchid && Visible == true)
		{
			OpenBranch(branchid);
		}

	}

	private void LearnBranchRequest()
	{
		ServerMaster.Instance.LP_MasteryTreeLearnRequest(_currentBranchId);
	}


	private struct BranchCache
	{
		
		public MasteryNodeId BranchId;
		public uint CurrentExp; 
		public ushort CurrentLvl;

	}

}
