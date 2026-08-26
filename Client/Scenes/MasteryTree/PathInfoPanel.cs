using Godot;
using Shared.MasteryTree;
using System;
using System.Collections.Generic;
using Shared.MasteryTree.Rewards;

public partial class PathInfoPanel : PanelContainer
{
	[Export] private PackedScene _statRewardScene;
	[Export] private VBoxContainer _rewardsBox;
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

		bool isMax = cache.CurrentLvl >= dllData.MaxLvl;
		_learnBranchButton.Disabled = isMax;

		// REWARDS

		foreach (Node child in _rewardsBox.GetChildren())
		{
			child.QueueFree();
		}

		if (dllData.Rewards.Length == 0) return;



		for (int i = 0; i < dllData.Rewards.Length; i++)
		{
			
			var reward = dllData.Rewards[i];

			if (reward.Type == RewardType.Stat)
			{
				
				var newReward = _statRewardScene.Instantiate<StatReward>();
				_rewardsBox.AddChild(newReward);

				if (reward.Context == RewardContextType.PerLevel)
				{
					newReward.CreateVisual(reward.StatId, (int)reward.Value, 0);
				}
				else
				{
					newReward.CreateVisual(reward.StatId, (int)reward.Value, reward.TargetLevel, true );
				}

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
