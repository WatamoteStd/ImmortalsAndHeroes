using Godot;
using Shared.MasteryTree;
using System;
using Shared.MasteryTree.Rewards;
public partial class MasteryTree : Control
{
	
	[Export] private Button _darkPathButton;
	[Export] private Button _bodyPathButton;
	[Export] private Label _playerExp;

	// INFO PANEl
	[Export] private PanelContainer _pathInfoWindow;
	[Export] private Button _learnPathButton;
	[Export] private Label _title;
	[Export] private Label _description;
	[Export] private Label _requiredExp;
	[Export] private Label _currentExp;
	[Export] private Label _statName;
	[Export] private Label _statValue;
	[Export] private Label _curLvl;
	[Export] private Label _maxlvl;


	private BranchCache[] _branchesCache; 

	private MasteryNodeId _currentBranchId = MasteryNodeId.None; // default alt null

	public override void _Ready()
	{

		_branchesCache = new BranchCache[MasteryBranchesRegistry.Count];



		VisibilityChanged += () =>
		{
			_playerExp.Text = GameSession.Instance.PlayerExpCache.ToString();
		};
		_pathInfoWindow.Visible = false;
		_darkPathButton.Pressed += () => OpenPathInfo(MasteryNodeId.DarkPath);
		_bodyPathButton.Pressed += () => OpenPathInfo(MasteryNodeId.Body);




		_learnPathButton.Pressed += () =>
		{
			
			if (_currentBranchId == MasteryNodeId.None) return;
			LearnBranchRequest();

		};

	}


	public void OpenPathInfo(MasteryNodeId id)
	{
		if (!MasteryBranchesRegistry.TryGetBranch(id, out var dllData))
			return;

		var cache = _branchesCache[(ushort)id];
		
		_pathInfoWindow.Visible = true;
		_currentBranchId = id;
		_title.Text = dllData.Title;
		_description.Text = dllData.Description;
		_currentExp.Text = cache.CurrentExp.ToString();

		var required = dllData.GetRequiredExpForLevel((ushort)(cache.CurrentLvl + 1));
		if (required == uint.MaxValue)
		{
			_requiredExp.Text = "MAX";
		}
		else
		{
			_requiredExp.Text = required.ToString();
		}

		_curLvl.Text = cache.CurrentLvl.ToString();
		_maxlvl.Text = dllData.MaxLvl.ToString();

		if (dllData.Rewards.Length > 0)
		{
			
				for (int i = 0; i < dllData.Rewards.Length; i++)
				{
			
					var curReward = dllData.Rewards[i];

					if (curReward.Type == RewardType.Stat)
					{
					
						_statName.Text = curReward.StatId.ToString();
						_statValue.Text = curReward.Value.ToString();

					}
					else
				{
					_statName.Text = string.Empty;
					_statValue.Text = string.Empty;
				}

			}

		}
		else
		{
			_statName.Text = string.Empty;
			_statValue.Text = string.Empty;
		}

		//_requiredExp.Text = dllData.GetRequiredExpForLevel()

	}

	public void InitTree(uint exp)
	{
		_playerExp.Text = exp.ToString();
	}

	private void LearnBranchRequest()
	{
		ServerMaster.Instance.LP_MasteryTreeLearnRequest(_currentBranchId);
	}

	#region NETWORK -> THIS

	public void UpdateBranch(MasteryNodeId branchid, uint currentExp, ushort currentLvl)
	{
		
		int index = (int)branchid;
		if (index < 0 || index >= _branchesCache.Length) return;

		ref var cache = ref _branchesCache[index];
		cache.CurrentExp = currentExp;
		cache.CurrentLvl = currentLvl;

		if (_currentBranchId == branchid && _pathInfoWindow.Visible)
		{
			OpenPathInfo(branchid);
		}

	}

	#endregion

	private struct BranchCache
	{
		
		public MasteryNodeId BranchId;
		public uint CurrentExp; 
		public ushort CurrentLvl;

	}

}
