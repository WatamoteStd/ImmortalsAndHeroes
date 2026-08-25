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
	[Export] private Label[] _statName;
	[Export] private Label[] _statValue;
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

		int rewardsCount = dllData.Rewards?.Length ?? 0;

		for (int i = 0; i < _statName.Length; i++)
		{
			if (i < rewardsCount)
			{
				var curReward = dllData.Rewards[i];

				if (curReward.Type == RewardType.Stat)
				{
					_statName[i].Text = curReward.StatId.ToString();
					_statValue[i].Text = $"+{curReward.Value}";
					_statName[i].Visible = true;
					_statValue[i].Visible = true;
					
					switch (curReward.StatId)
					{
						
						case StatType.Health:
							{
								_statName[i].Modulate = new Color(0.988f, 0.0f, 0.141f);
							}
						break;
						default:
							{
								_statName[i].Modulate = new Color(0.816f, 0.863f, 0.859f);
							}
						break;

					}

				}
				else
				{
					_statName[i].Text = curReward.Type.ToString();
					_statValue[i].Text = string.Empty;
					_statName[i].Visible = true;
					_statValue[i].Visible = true;
				}
			}
			else
			{
				_statName[i].Visible = false;
				_statValue[i].Visible = false;
			}
		}
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
