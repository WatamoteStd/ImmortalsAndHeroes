using Godot;
using System;

public partial class PanelsController : Control
{
	
	[Export] private TabContainer _tabWindow;
	[Export] private FadePanel _topTab;

	public override void _Ready()
	{
		
		_tabWindow.TabChanged += OnTabChanged;

	}

	private void OnTabChanged(long tabId)
	{
		
		if (tabId == 0)
		{
			_topTab.ShowSmooth();
			
		}
		else
		{
			_topTab.HideSmooth();
		}

	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _tabWindow != null)
		{
			_tabWindow.TabChanged -= OnTabChanged;
		}
		base.Dispose(disposing);
	}



}
