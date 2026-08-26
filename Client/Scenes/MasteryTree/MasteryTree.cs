using Godot;
using Shared.MasteryTree;
using System;
using Shared.MasteryTree.Rewards;
using Shared.Ability;
using System.Collections.Generic;
public partial class MasteryTree : Control
{
	[Export] private Label _playerExp;
	[Export] public PathInfoPanel BranchInfoPanel;

	[Export] private Button _darkPathButton;
	[Export] private Button _bodyButton;
	[Export] private Button _footAgilityButton;

	private MasteryNodeId _currentBranchId = MasteryNodeId.None; 

	public override void _Ready()
	{

		VisibilityChanged += () =>
		{
			_playerExp.Text = GameSession.Instance.PlayerExpCache.ToString();
		};

		// BUTTON SUBS
		_darkPathButton.Pressed += () => { BranchInfoPanel.OpenBranch(MasteryNodeId.DarkPath);};
		_bodyButton.Pressed += () => {BranchInfoPanel.OpenBranch(MasteryNodeId.Body);};
		_footAgilityButton.Pressed += () => {BranchInfoPanel.OpenBranch(MasteryNodeId.FootAgility);};		

	}

		

	public void InitTree(uint exp)
	{
		_playerExp.Text = exp.ToString();
	}


}
