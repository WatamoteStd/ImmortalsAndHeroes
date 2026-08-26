using Godot;
using Shared.MasteryTree;
using System;

public partial class ProgressionBlock : VBoxContainer
{
	
	[Export] private ProgressBar _progressBar;
	[Export] private Label _curExp;
	[Export] private Label _reqExp;
	[Export] private Label _curLvl;
	[Export] private Label _maxLvl;

	public void SetProgress(int curExp, int curLvl, MasteryBranch branch)
	{
		
		var requiredExp = branch.GetRequiredExpForLevel((ushort)(curLvl + 1));

		if (requiredExp == uint.MaxValue)
		{
			_reqExp.Text = "MAX";
			_curExp.Text = "MAX";
			_progressBar.Value = _progressBar.MaxValue;
		}
		else
		{
			_reqExp.Text = requiredExp.ToString();
			_curExp.Text = curExp.ToString();
			_progressBar.MaxValue = requiredExp;
			_progressBar.Value = curExp;
		}

		_curLvl.Text = curLvl.ToString();
		_maxLvl.Text = branch.MaxLvl.ToString();

	}



}
