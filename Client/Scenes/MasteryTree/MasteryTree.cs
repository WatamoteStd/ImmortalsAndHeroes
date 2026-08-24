using Godot;
using Shared.MasteryTree;
using System;

public partial class MasteryTree : Control
{
	
	[Export] private Button _darkPathButton;
	[Export] private Label _playerExp;

	// INFO PANEl
	[Export] private PanelContainer _pathInfoWindow;
	[Export] private Button _learnPathButton;
	[Export] private Label _title;
	[Export] private Label _description;
	[Export] private Label _requiredExp;

	private MasteryNodeId _currentBranchId = MasteryNodeId.None; // default alt null

	public override void _Ready()
	{
		VisibilityChanged += () =>
		{
			_playerExp.Text = GameSession.Instance.PlayerExpCache.ToString();
		};
		_pathInfoWindow.Visible = false;
		_darkPathButton.Pressed += () => OpenPathInfo(MasteryNodeId.DarkPath);




		_learnPathButton.Pressed += () =>
		{
			
			if (_currentBranchId == MasteryNodeId.None) return;
			LearnBranchRequest();

		};

	}


	public void OpenPathInfo(MasteryNodeId id)
	{
		
		_pathInfoWindow.Visible = true;
		_currentBranchId = id;
		//_title.Text = name;
		//_description.Text = description;
		//_requiredExp.Text = requiredExp.ToString();

	}

	public void InitTree(uint exp)
	{
		
		_playerExp.Text = exp.ToString();

	}

	private void LearnBranchRequest()
	{
		
		ServerMaster.Instance.LP_MasteryTreeLearnRequest(_currentBranchId);

	}

}
