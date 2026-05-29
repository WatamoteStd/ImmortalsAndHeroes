using Godot;
using System;

public partial class SearchTimePanel : MarginContainer
{
	[Export] private Label _timeLabel;
	private float _timer = 0.0f;
	
	public override void _Ready()
	{
	}

	
	public override void _PhysicsProcess(double delta)
	{
		if (!Visible) return;

		_timer += (float)delta;
		int minutes = (int)(_timer / 60);
		int secs = (int)(_timer % 60);
		
		_timeLabel.Text = $"{minutes:00}:{secs:00}";
	}
}
